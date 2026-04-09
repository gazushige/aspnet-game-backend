using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    /// <summary>
    /// 素材（石、鉄鋼、木材など）の定義マスター
    /// </summary>
    public class Material : CatalogItemBase
    {
        public int MinQuantity { get; set; } = 1;

        // PlayFabの制限やDBのint範囲を考慮した最大値
        public int MaxQuantity { get; set; } = 2100000000;

        public bool IsStackable { get; set; } = true;
        public bool IsTradable { get; set; } = false;

        public override string ToString() => DisplayName;
    }
    public class MaterialConfiguration : CatalogItemBaseConfiguration<Material>
    {
        public override void Configure(EntityTypeBuilder<Material> builder)
        {
            // 基底クラス（CatalogUuid, Revision, IsCurrentVersion等）の設定を適用
            base.Configure(builder);

            builder.ToTable("materials");

            // デフォルト値の設定
            builder.Property(e => e.MinQuantity).HasDefaultValue(1);
            builder.Property(e => e.MaxQuantity).HasDefaultValue(2100000000);
            builder.Property(e => e.IsStackable).HasDefaultValue(true);
            builder.Property(e => e.IsTradable).HasDefaultValue(false);

            // --- 制約 (Constraints) ---

            // 1. カタログUUID + リビジョンのユニーク制約
            builder.HasIndex(e => new { e.CatalogUuid, e.Revision })
                   .IsUnique()
                   .HasDatabaseName("UQ_VC_Catalog_Revision");

            // 2. 現在のバージョンはカタログごとに1つ（Partial Index）
            builder.HasIndex(e => e.CatalogUuid)
                   .IsUnique()
                   .HasFilter("\"IsCurrentVersion\" = TRUE")
                   .HasDatabaseName("UQ_VC_CurrentVersion");

            // 3. 数値範囲のチェック制約 (DjangoのPositiveIntegerField相当 + α)
            builder.ToTable(t => t.HasCheckConstraint(
                "CK_VC_QuantityRange",
                "\"MinQuantity\" >= 0 AND \"MinQuantity\" <= \"MaxQuantity\""
            ));
        }
    }

}