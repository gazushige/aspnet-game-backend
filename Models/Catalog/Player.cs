using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyApi.Models
{
    /// <summary>
    /// 経験値テーブル（マスターデータ）
    /// 例: "普通タイプ", "早熟タイプ", "晩成タイプ" など
    /// </summary>
    public class ExpTable
    {
        public int Id { get; set; }

        // 基本的なレベルキャップ設定
        public int MinLevel { get; set; } = 1;
        public int MaxLevel { get; set; } = 99;

        // 成長ロジック（例: 各レベルごとの必要累計EXPリストや係数）
        // PlayFabへ送る際やサーバー計算時に使用
        public JsonDocument? Logic { get; set; }

        // 逆参照ナビゲーション
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }

    /// <summary>
    /// ガチャ景品としてのプレイヤーキャラクター
    /// </summary>
    public class Player : CatalogItemBase // CharacterCatalogItem の代わり
    {
        public ItemRarity Rarity { get; set; } = ItemRarity.COMMON;

        // 経験値テーブルへの外部キー
        public int ExpTableId { get; set; }
        public ExpTable ExpTable { get; set; } = null!;
        public JsonDocument? CustomData { get; set; }

        // Django の __str__ 相当（デバッグやログ用）
        public override string ToString() => DisplayName;
    }
    // --- ExpTable Configuration ---
    public class ExpTableConfiguration : IEntityTypeConfiguration<ExpTable>
    {
        public void Configure(EntityTypeBuilder<ExpTable> builder)
        {
            builder.ToTable("exp_tables");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Logic).HasColumnType("jsonb");
            builder.Property(e => e.MinLevel).HasDefaultValue(1);
            builder.Property(e => e.MaxLevel).HasDefaultValue(99);
        }
    }

    // --- Player Configuration ---
    public class PlayerConfiguration : CatalogItemBaseConfiguration<Player>
    {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            base.Configure(builder); // 基底のインデックスやDisplayName設定を継承

            builder.ToTable("players");

            builder.Property(e => e.Rarity)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // リレーション設定 (Django の PROTECT 相当)
            builder.HasOne(e => e.ExpTable)
                   .WithMany(t => t.Players)
                   .HasForeignKey(e => e.ExpTableId)
                   .OnDelete(DeleteBehavior.Restrict); // 勝手にテーブルが消されないよう保護

            // Django の Meta constraints 相当
            builder.HasIndex(e => new { e.CatalogUuid, e.Revision })
                   .IsUnique()
                   .HasDatabaseName("UQ_Player_Catalog_Revision");

            builder.HasIndex(e => e.CatalogUuid)
                   .IsUnique()
                   .HasFilter("\"IsCurrentVersion\" = TRUE")
                   .HasDatabaseName("UQ_Player_CurrentVersion");

            builder.Property(e => e.CustomData).HasColumnType("jsonb");
        }
    }
}