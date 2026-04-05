using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    /// <summary>
    /// ガチャ本体
    /// </summary>
    public class Lottery : CatalogItemBase
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PityNumber { get; set; } = 0; // 天井回数
        public PityType PityType { get; set; } = PityType.NONE;
        public VirtualCurrency SingleCost { get; set; } = new VirtualCurrency();
        public VirtualCurrency TenCost { get; set; } = new VirtualCurrency();

        // ナビゲーションプロパティ
        public ICollection<LotteryRarity> Rarities { get; set; } = new List<LotteryRarity>();
        public ICollection<LotteryPrize> Prizes { get; set; } = new List<LotteryPrize>();
    }

    /// <summary>
    /// レアリティごとの重み設定（例：SSR 3%, SR 17%, R 80% など）
    /// </summary>
    public class LotteryRarity : IEntity
    {
        public int Id { get; set; }
        public int LotteryId { get; set; }
        public Lottery Lottery { get; set; } = null!;

        public ItemRarity Rarity { get; set; } = ItemRarity.COMMON;
        public int Weight { get; set; } = 1;
    }

    /// <summary>
    /// 具体的な景品とその重み
    /// </summary>
    public class LotteryPrize : IEntity
    {
        public int Id { get; set; }
        public int LotteryId { get; set; }
        public Lottery Lottery { get; set; } = null!;

        // 景品となるアイテム（CatalogItemBaseを継承したものの共通ID）
        public Guid CatalogUuid { get; set; }
        public Catalog Catalog { get; set; } = null!;

        public ItemRarity Rarity { get; set; } = ItemRarity.COMMON;
        public int Weight { get; set; } = 1;
        public bool IsPickup { get; set; } = false;
    }

    // --- Lottery Configuration ---
    public class LotteryConfiguration : CatalogItemBaseConfiguration<Lottery>
    {
        public override void Configure(EntityTypeBuilder<Lottery> builder)
        {
            base.Configure(builder);
            builder.ToTable("lotteries");

            builder.Property(e => e.PityNumber).HasDefaultValue(0);

            // 以前定義したカタログユニーク制約などをここでも適用
            builder.HasIndex(e => new { e.CatalogUuid, e.Revision }).IsUnique();
        }
    }

    // --- LotteryRarity Configuration ---
    public class LotteryRarityConfiguration : IEntityTypeConfiguration<LotteryRarity>
    {
        public void Configure(EntityTypeBuilder<LotteryRarity> builder)
        {
            builder.ToTable("lottery_rarities");

            builder.HasOne(e => e.Lottery)
                   .WithMany(l => l.Rarities)
                   .HasForeignKey(e => e.LotteryId)
                   .OnDelete(DeleteBehavior.Cascade); // ガチャが消えたらレアリティ設定も消す

            builder.Property(e => e.Rarity).HasConversion<string>().HasMaxLength(20);
        }
    }

    // --- LotteryPrize Configuration ---
    public class LotteryPrizeConfiguration : IEntityTypeConfiguration<LotteryPrize>
    {
        public void Configure(EntityTypeBuilder<LotteryPrize> builder)
        {
            builder.ToTable("lottery_prizes");

            // ガチャ本体とのリレーション
            builder.HasOne(e => e.Lottery)
                   .WithMany(l => l.Prizes)
                   .HasForeignKey(e => e.LotteryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // アイテム本体とのリレーション (Django: PROTECT)
            builder.HasOne(e => e.Catalog)
                   .WithMany()
                   .HasForeignKey(e => e.CatalogUuid)
                   .OnDelete(DeleteBehavior.Restrict); // 景品に設定されているアイテムの削除を禁止

            builder.Property(e => e.Rarity).HasConversion<string>().HasMaxLength(20);

            // パフォーマンスのためのインデックス
            builder.HasIndex(e => new { e.LotteryId, e.Rarity });
        }
    }
    public enum PityType
    {
        NONE,
        RARITY,     //レアリティ確定
        PRIZE       //特定の景品確定
    }
}