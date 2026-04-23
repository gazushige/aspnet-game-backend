
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApi.Models
{
    /// <summary>
    /// VIP効果のマスターデータ
    /// </summary>
    public class VipMaster : IEntity, IHasCustomData, IHasRequirement
    {
        public int Id { get; set; }

        /// <summary>VIPレベル (0=非VIP)</summary>
        public int Level { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>このレベルに必要な累計VIPポイント</summary>
        public long RequiredPoint { get; set; }

        /// <summary>課金時VIPポイント獲得倍率 (e.g. 1.5 = 1.5倍)</summary>
        public decimal PointMultiplier { get; set; } = 1.0m;

        /// <summary>UI表示順 (Levelと同値運用なら省略可)</summary>
        public int SortOrder { get; set; }

        /// <summary>VIPバッジアセットのFK (null=バッジなし)</summary>
        public int? IconAssetId { get; set; }

        /// <summary>ナビゲーションプロパティ (必要時にInclude)</summary>
        public AddressableAsset? IconAsset { get; set; }

        /// <summary>
        /// 解放条件 (JSON / AND評価)
        /// e.g. {"vip_point":1000,"player_level":10}
        /// </summary>
        public string? Requirement { get; set; }

        /// <summary>拡張用自由JSON</summary>
        public string? CustomData { get; set; }
    }
    public class VipMasterConfiguration : IEntityTypeConfiguration<VipMaster>
    {
        public void Configure(EntityTypeBuilder<VipMaster> builder)
        {
            builder.ToTable("vip_masters");

            // ── PK ──────────────────────────────────────────
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            // ── ランク定義 ────────────────────────────────────
            builder.Property(x => x.Level)
                .HasColumnName("level")
                .IsRequired();

            builder.Property(x => x.SortOrder)
                .HasColumnName("sort_order")
                .IsRequired();

            // ── 基本情報 ─────────────────────────────────────
            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(512)
                .IsRequired();

            // ── ポイント ──────────────────────────────────────
            builder.Property(x => x.RequiredPoint)
                .HasColumnName("required_point")
                .IsRequired();

            builder.Property(x => x.PointMultiplier)
                .HasColumnName("point_multiplier")
                .HasPrecision(5, 2)
                .IsRequired();

            // ── アセット参照 ──────────────────────────────────
            builder.Property(x => x.IconAssetId)
                .HasColumnName("icon_asset_id");

            builder.HasOne(x => x.IconAsset)
                .WithMany()                          // AddressableAsset側に逆ナビは不要
                .HasForeignKey(x => x.IconAssetId)
                .IsRequired(false)                   // nullable FK
                .OnDelete(DeleteBehavior.Restrict);  // アセット削除でVipMasterを巻き込まない

            // ── JSON ──────────────────────────────────────────
            builder.Property(x => x.Requirement)
                .HasColumnName("requirement")
                .HasColumnType("jsonb");

            builder.Property(x => x.CustomData)
                .HasColumnName("custom_data")
                .HasColumnType("jsonb");

            // ── インデックス ──────────────────────────────────
            builder.HasIndex(x => x.Level)
                .HasDatabaseName("ix_vip_masters_level")
                .IsUnique();

            builder.HasIndex(x => x.SortOrder)
                .HasDatabaseName("ix_vip_masters_sort_order");

            builder.HasIndex(x => x.IconAssetId)
                .HasDatabaseName("ix_vip_masters_icon_asset_id");
        }
    }
    /// <summary>
    /// VipMaster.Requirement のスキーマ定義
    /// 全条件はAND評価。未指定キーは条件なしとみなす。
    /// </summary>
    public sealed record VipRequirement
    {
        /// <summary>累計VIPポイント (課金額連動)</summary>
        [JsonPropertyName("vip_point")]
        public long? VipPoint { get; init; }

        /// <summary>プレイヤーレベル</summary>
        [JsonPropertyName("player_level")]
        public int? PlayerLevel { get; init; }

        /// <summary>前提VIPレベル (直前レベルを指定するのが原則)</summary>
        [JsonPropertyName("prev_vip_level")]
        public int? PrevVipLevel { get; init; }

        /// <summary>特定実績ID (PlayFabのStatistics等で管理)</summary>
        [JsonPropertyName("achievement_id")]
        public int? AchievementId { get; init; }

        // 将来拡張用: 新条件はここにプロパティ追加するだけ
    }
}