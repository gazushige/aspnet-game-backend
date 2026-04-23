using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace MyApi.Models
{
    /// <summary>
    /// カタログアイテムのカテゴリ。大分類。アイテム以外にも存在する。例：キャラクター、武器、防具、敵、ガチャなど
    /// </summary>
    public enum CatalogCategory
    {
        WEAPON,
        ARMOR,
        CONSUMABLE,
        CHARACTER,
        ENEMY,
        GACHA,
        OTHER
    }
    /// <summary>
    /// カタログアイテムのprefixを管理するためのもの。より細分化されたカテゴリ。jsonシリアライズ時のkeyにする文字列のprefixにする。例：weapon001
    /// </summary> 
    public class CatalogPrefix : IEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public CatalogCategory Category { get; set; } = CatalogCategory.OTHER;

        public string Prefix { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int LastNumber { get; set; }
    }

    /// <summary>
    /// カタログアイテムの基本クラス。シリーズごとにアイテムの内容は異なるが、共通の属性をここで定義する。シリーズごとに個別のテーブルを作る場合は、これを継承して具体的なアイテムクラスを作成する。
    /// </summary>
    public abstract class CatalogItemBase : IHasTimestamps, IEntity, IHasUuid
    {
        public int Id { get; set; }
        [Key]
        public Guid Uuid { get; init; } = Guid.CreateVersion7();
        public int PrefixId { get; set; }
        public CatalogPrefix Prefix { get; set; } = null!;
        public int NumberInPrefix { get; set; }     //同じprefixの中で何番目か
        public string Code => Prefix.Prefix + string.Format("{0:0>3}", NumberInPrefix);     //例：weapon001     jsonのkeyに使用する
        public int Revision { get; set; } = 1;
        public string DisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCurrentVersion { get; set; }
        public string? Tags { get; set; }
        public int? IconAssetId { get; set; }
        public AddressableAsset? IconAsset { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.DRAFT;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}

namespace MyApi.Models
{
    public class CatalogPrefixConfiguration : IEntityTypeConfiguration<CatalogPrefix>
    {
        public void Configure(EntityTypeBuilder<CatalogPrefix> builder)
        {
            builder.ToTable("catalog_prefixes");
            builder.HasKey(e => e.Id);

            // ここに制約をすべて書く
            builder.HasIndex(e => e.Prefix).IsUnique();

            builder.HasIndex(e => new { e.CategoryId, e.Prefix })
                   .IsUnique()
                   .HasDatabaseName("UQ_CatalogPrefix_Category_Prefix");

            builder.HasIndex(e => e.Category);

        }
    }
    public abstract class CatalogItemBaseConfiguration<TEntity>
    where TEntity : CatalogItemBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Uuid);

            // 1. インデックス設定 (監査・検索の高速化)
            builder.HasIndex(e => e.Uuid);
            builder.HasIndex(e => e.IsCurrentVersion);
            builder.HasIndex(e => e.Status);

            // 2. プロパティ制約
            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(e => e.Description)
                .HasMaxLength(1000); // 任意ですが、DB保護のため上限を推奨

            builder.HasOne(e => e.IconAsset)
                .WithMany()
                .HasForeignKey(e => e.IconAssetId)
                .OnDelete(DeleteBehavior.SetNull); // アイコンが削除されたら参照をnullにする

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

    /// <summary>
    /// EF CoreでDB管理するクラスに付ける。マイグレーションに含ませるのに必要
    /// </summary> 
    public interface IEntity
    {
        int Id { get; }
    }
    /// <summary>
    /// タイムスタンプを持つエンティティのインターフェース。CreatedAtとUpdatedAtを定義する。これを実装することで、アイテムの作成日時と更新日時を管理できる。
    /// </summary> 
    interface IHasTimestamps
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
    /// <summary>
    /// 有効期限を持つエンティティのインターフェース。StartAtとExpiredAtを定義する。これを実装することで、アイテムの有効期間を管理できる。
    /// </summary>
    interface IHasExpiry
    {
        DateTimeOffset? StartAt { get; set; }
        DateTimeOffset? ExpiredAt { get; set; }
    }
    /// <summary>
    /// 効果を持つエンティティのインターフェース。CustomDataを定義する。これを実装することで、アイテムの効果やパラメータを柔軟に管理できる。
    /// </summary>
    interface IHasCustomData
    {
        string? CustomData { get; set; }
    }
    /// <summary>
    /// 前提条件があるエンティティのインターフェース。
    /// </summary> 
    interface IHasRequirement
    {
        string? Requirement { get; set; }
    }
    public interface IHasUuid
    {
        Guid Uuid { get; }
    }
}
