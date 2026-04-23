using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace MyApi.Models
{
    /// <summary>
    /// 装備品を表す（マスターデータ）
    /// </summary>
    public class EquipmentItem : SellableCatalogItem, IHasCustomData
    {
        public override bool IsStackable => false; // 装備品はスタック不可で固定

        public ItemRarity Rarity { get; set; } = ItemRarity.COMMON;

        public string? CustomData { get; set; }

    }
    public class EquipmentItemConfiguration : SellableCatalogItemConfiguration<EquipmentItem>, IEntityTypeConfiguration<EquipmentItem>
    {
        public override void Configure(EntityTypeBuilder<EquipmentItem> builder)
        {
            // 1. 基底クラスの設定を適用
            base.Configure(builder);

            // 2. テーブル名の明示
            builder.ToTable("equipment_items");

            // 3. スタック不可の強制 (DBデフォルト値でも担保)
            builder.Property(e => e.IsStackable)
                   .HasDefaultValue(false)
                   .ValueGeneratedNever(); // アプリ側での制御を優先

            builder.Property(e => e.Rarity)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(e => e.CustomData)
                   .HasColumnType("jsonb");

            // 4. Django の Meta constraints 相当の設定

            // UQ: カタログUUIDとリビジョンの一意性
            builder.HasIndex(e => new { e.Uuid, e.Revision })
                   .IsUnique()
                   .HasDatabaseName("UQ_Equipment_Catalog_Revision");

            // Partial Index: 現在有効なバージョンはカタログごとに1つだけ
            builder.HasIndex(e => e.Uuid)
                   .IsUnique()
                   .HasFilter("\"IsCurrentVersion\" = TRUE")
                   .HasDatabaseName("UQ_Equipment_CurrentVersion");
        }
    }
}