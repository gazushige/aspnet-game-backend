using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyApi.Models
{
    /// <summary>
    /// 消費アイテムを表す（マスターデータ）
    /// </summary>
    public class ConsumableItem : SellableCatalogItem, IHasCustomData
    {
        public int UsageCount { get; set; } = 1;    // 消費アイテムの使用回数。1回使い切りのアイテムは1、複数回使えるアイテムは2以上の値を設定する。例：回復薬（UsageCount=1）、スタミナドリンク（UsageCount=3）など
        public int? UsagePeriodSeconds { get; set; }    // 消費アイテムの使用期間（秒）。時間制の消費アイテムの場合は、使用開始からこの期間が経過するとアイテムが消費される。例：経験値ブースト（UsagePeriodSeconds=3600）など。回数制のアイテムの場合はnullを設定する。
        public ItemRarity Rarity { get; set; } = ItemRarity.COMMON;

        public int MaxStack { get; set; } = 99;
        public override bool IsStackable => MaxStack > 1; // MaxStackが1より大きい場合はスタック可能とみなす
        public string? CustomData { get; set; }

        public bool IsConsumableByTime { get; set; }
        public bool IsConsumableByCount { get; set; } = true;
    }
    public class ConsumableItemConfiguration : SellableCatalogItemConfiguration<ConsumableItem>, IEntityTypeConfiguration<ConsumableItem>
    {
        public override void Configure(EntityTypeBuilder<ConsumableItem> builder)
        {
            // 1. 基底クラス(CatalogItemBase, SellableCatalogItem)の設定を適用
            base.Configure(builder);

            // 2. テーブル名の明示
            builder.ToTable("consumable_items");

            // 3. 基本プロパティの設定
            builder.Property(e => e.UsageCount).IsRequired().HasDefaultValue(1);
            builder.Property(e => e.MaxStack).IsRequired().HasDefaultValue(99);

            builder.Property(e => e.Rarity)
                   .HasConversion<string>() // 監査ログ用に文字列保存を推奨
                   .HasMaxLength(20);

            builder.Property(e => e.CustomData);
            //    .HasColumnType("jsonb");

            // 4. 制約 (Django の Meta constraints 相当)

            // UQ: カタログIDとリビジョンの組み合わせを一意にする
            builder.HasIndex(e => new { e.Uuid, e.Revision })
                   .IsUnique()
                   .HasDatabaseName("UQ_Consumable_Catalog_Revision");

            // フィルター付きユニークインデックス (Partial Index)
            // 「IsCurrentVersion が true のものは、1つの CatalogUuid に対して1つだけ」という制約
            // Django の condition=models.Q(is_current_version=True) 相当
            builder.HasIndex(e => e.Uuid)
                   .IsUnique()
                   .HasFilter("\"IsCurrentVersion\" = TRUE") // PostgreSQL 構文
                   .HasDatabaseName("UQ_Consumable_CurrentVersion");
        }
    }
}