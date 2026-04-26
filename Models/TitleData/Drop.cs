using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyApi.Models
{
    /// <summary>
    /// ドロップテーブル本体（PlayFabのRandomResultTableに相当）
    /// </summary>
    public class DropTable : ICacheableEntity, IHasCustomData
    {
        public int Id { get; set; }
        public string KeyCode { get; set; } = string.Empty; // PlayFab側のTableId
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CustomData { get; set; }


        // 多対多のナビゲーション
        public ICollection<DropItem> DropItems { get; set; } = new List<DropItem>();
    }

    /// <summary>
    /// 中間クラス（Throughクラス）: ドロップテーブルとアイテムの紐付け
    /// </summary>
    public class DropItem : ICacheableEntity
    {
        public int Id { get; set; }
        public int DropTableId { get; set; }
        public DropTable DropTable { get; set; } = null!;

        public Guid CatalogUuid { get; set; }   // CatalogItemBaseのUuidを参照するための外部キー 1対多
        public CatalogCategory CatalogCategory { get; set; } // アイテムのカテゴリを直接保持しておく（クエリの効率化のため）
        public int MinQuantity { get; set; } = 1;
        public int MaxQuantity { get; set; } = 1;
        public int Weight { get; set; } = 1;    // ドロップの重み（確率）を表す。PlayFabの仕様に合わせて整数で管理。例: 1000なら全体の中で10%の確率。-1なら確定ドロップ。
        public bool IsGuaranteed => Weight == -1; // 重みが-1なら確定ドロップとみなす
    }
    public class DropTableConfiguration : IEntityTypeConfiguration<DropTable>
    {
        public void Configure(EntityTypeBuilder<DropTable> builder)
        {
            builder.ToTable("drop_tables");
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.KeyCode).IsUnique();
            builder.Property(e => e.KeyCode).IsRequired().HasMaxLength(100);
        }
    }

    public class DropItemConfiguration : IEntityTypeConfiguration<DropItem>
    {
        public void Configure(EntityTypeBuilder<DropItem> builder)
        {
            builder.ToTable("drop_items");

            // 1. 複合主キーの設定 (Many-to-Manyの基本)
            // これにより、同じテーブルに同じアイテムが重複登録されるのを防ぐ
            builder.HasKey(e => new { e.DropTableId, e.CatalogUuid });

            // 2. リレーションシップの明示
            builder.HasOne(e => e.DropTable)
                   .WithMany(t => t.DropItems)
                   .HasForeignKey(e => e.DropTableId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 4. パフォーマンス用インデックス
            // 主キーで {DropTableId, CatalogUuid} はカバーされているが、
            // 逆引き（このアイテムはどのテーブルに含まれるか）が必要なら以下を追加
            builder.HasIndex(e => e.CatalogUuid);

            // 5. チェック制約 (Django の CheckConstraint 相当)
            // PostgreSQL/SQLite 両対応の SQL で記述
            builder.ToTable(t => t.HasCheckConstraint(
                "CK_DropTableItem_QuantityRange",
                "\"MinQuantity\" <= \"MaxQuantity\""
            ));

            builder.ToTable(t => t.HasCheckConstraint(
                "CK_DropTableItem_PositiveWeight",
                "\"Weight\" > 0"
            ));
        }
    }
}