using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyApi.Models
{
    /// <summary>
    /// カタログアイテムのカテゴリ。大分類。アイテム以外にも存在する。例：キャラクター、武器、敵、ガチャなど
    /// </summary>
    public class CatalogCategory : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // ナビゲーション (virtualを削除)
        public ICollection<CatalogSeries> Series { get; set; } = new List<CatalogSeries>();
    }
    /// <summary>
    /// カタログアイテムのprefixを管理するためのもの。より細分化されたカテゴリ
    /// </summary> 
    public class CatalogSeries : IEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public CatalogCategory Category { get; set; } = null!; // virtualを削除

        public string Prefix { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int LastNumber { get; set; }
    }
    /// <summary>
    /// カタログアイテムのID。UUIDで管理。シリーズIDと通し番号も保持する。例：キャラクターシリーズの001番目のアイテム、武器シリーズの002番目のアイテムなど。アイテムの内容はCatalogItemBaseを継承した具体的なクラスで管理する。
    /// </summary>
    public class Catalog : IEntity
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public int SeriesId { get; set; }
        public CatalogSeries Series { get; set; } = null!;

        public int Number { get; set; }
        public string KeyCode { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
    }

    /// <summary>
    /// カタログアイテムの基本クラス。シリーズごとにアイテムの内容は異なるが、共通の属性をここで定義する。シリーズごとに個別のテーブルを作る場合は、これを継承して具体的なアイテムクラスを作成する。
    /// </summary>
    public abstract class CatalogItemBase : IHasTimestamps, IEntity
    {
        public int Id { get; set; }
        public Guid CatalogUuid { get; set; } // CatalogId.Uuid への外部キー
        public Catalog Catalog { get; set; } = null!;

        public int Revision { get; set; } = 1;
        public string DisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCurrentVersion { get; set; }
        public JsonDocument? Tags { get; set; }
        public string? ItemImageUrl { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.DRAFT;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}

namespace MyApi.Models
{
    public class CatalogSeriesConfiguration : IEntityTypeConfiguration<CatalogSeries>
    {
        public void Configure(EntityTypeBuilder<CatalogSeries> builder)
        {
            // ここに制約をすべて書く
            builder.HasIndex(e => e.Prefix).IsUnique();

            builder.HasIndex(e => new { e.CategoryId, e.Prefix })
                   .IsUnique()
                   .HasDatabaseName("UQ_CatalogSeries_Category_Prefix");

            builder.HasOne(e => e.Category)
                   .WithMany(c => c.Series)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class CatalogCategoryConfiguration : IEntityTypeConfiguration<CatalogCategory>
    {
        public void Configure(EntityTypeBuilder<CatalogCategory> builder)
        {
            // ここに制約をすべて書く
            builder.HasIndex(e => e.Code).IsUnique();
        }
    }

    public abstract class CatalogItemBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : CatalogItemBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // 1. インデックス設定 (監査・検索の高速化)
            builder.HasIndex(e => e.CatalogUuid);
            builder.HasIndex(e => e.IsCurrentVersion);
            builder.HasIndex(e => e.Status);

            // 2. 文字列制約 (Fluent APIで上書き・明示)
            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(e => e.Description)
                .HasMaxLength(1000); // 任意ですが、DB保護のため上限を推奨

            builder.Property(e => e.ItemImageUrl)
                .HasMaxLength(2048); // URLの標準的な最大長

            // 3. Enumの文字列保存 (監査ログとしての可読性を重視)
            // 数値(int)だとDBを直接見た時に意味が不明なため、文字列保存を推奨
            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // 4. JSONB設定 (PostgreSQL専用)
            // Native AOTでは JsonDocument のシリアライズに Source Generator が必要
            builder.Property(e => e.Tags)
                .HasColumnType("jsonb");

            // 5. バージョニング (リビジョン管理)
            builder.Property(e => e.Revision)
                .IsRequired()
                .HasDefaultValue(1);

            // 6. タイムスタンプ設定 (IHasTimestamps)
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP") // DB側で生成
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP") // 初期値
                .ValueGeneratedOnAddOrUpdate(); // 更新時に自動計算 (PostgreSQLはTriggerが必要)
        }
    }
    public class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
    {
        public void Configure(EntityTypeBuilder<Catalog> builder)
        {
            builder.HasKey(e => e.Uuid);
            builder.HasIndex(e => new { e.SeriesId, e.Number }).IsUnique();
            builder.HasIndex(e => e.KeyCode).IsUnique();

            builder.HasOne(e => e.Series)
                   .WithMany()
                   .HasForeignKey(e => e.SeriesId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public enum ItemStatus
    {
        DRAFT,
        ACTIVE,
        ARCHIVED,
        DEPRECATED
    }
    public enum ItemRarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC,
        LEGENDARY,
        MYTHIC,  //通常の最上位レア。ガチャで出すのはここまで。
        THE_ONE //唯一無二の特殊レア。例：イベント優勝者への限定報酬など
    }
    public interface IEntity
    {
        int Id { get; }
    }
}