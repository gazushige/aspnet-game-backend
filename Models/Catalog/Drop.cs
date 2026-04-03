using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyApi.Models
{
    /// <summary>
    /// ドロップテーブル本体（PlayFabのRandomResultTableに相当）
    /// </summary>
    public class DropTable
    {
        public int Id { get; set; }
        public string KeyCode { get; set; } = string.Empty; // PlayFab側のTableId
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // 多対多のナビゲーション
        public ICollection<DropTableItem> DropItems { get; set; } = new List<DropTableItem>();
    }

    /// <summary>
    /// 中間クラス（Throughクラス）: ドロップテーブルとアイテムの紐付け
    /// </summary>
    public class DropTableItem
    {
        public int DropTableId { get; set; }
        public DropTable DropTable { get; set; } = null!;

        public Guid CatalogUuid { get; set; }
        public Catalog Catalog { get; set; } = null!;

        public int MinQuantity { get; set; } = 1;
        public int MaxQuantity { get; set; } = 1;
        public int Weight { get; set; } = 1;
        public bool IsGuaranteed { get; set; } = false;
        public ItemRarity Rarity { get; set; } = ItemRarity.COMMON;
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

    public class DropTableItemConfiguration : IEntityTypeConfiguration<DropTableItem>
    {
        public void Configure(EntityTypeBuilder<DropTableItem> builder)
        {
            builder.ToTable("drop_table_items");

            // 1. 複合主キーの設定 (Many-to-Manyの基本)
            // これにより、同じテーブルに同じアイテムが重複登録されるのを防ぐ
            builder.HasKey(e => new { e.DropTableId, e.CatalogUuid });

            // 2. リレーションシップの明示
            builder.HasOne(e => e.DropTable)
                   .WithMany(t => t.DropItems)
                   .HasForeignKey(e => e.DropTableId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Catalog)
                   .WithMany() // Catalog側からどのDropTableに使われているか追う必要がなければ空でOK
                   .HasForeignKey(e => e.CatalogUuid)
                   .OnDelete(DeleteBehavior.Restrict);

            // 3. Enum設定
            builder.Property(e => e.Rarity)
                   .HasConversion<string>()
                   .HasMaxLength(20);

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