using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    /// <summary>
    /// ガチャ本体
    /// </summary>
    public class Lottery : CatalogItemBase, IHasExpiry
    {
        public DateTime? StartAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public int PityNumber { get; set; } = 0; // 天井回数
        public PityType PityType { get; set; } = PityType.NONE;
        // 単発ガチャのコスト
        public int SingleCostCurrencyId { get; set; } // どの通貨か（ID）
        public VirtualCurrency SingleCostCurrency { get; set; } = null!;
        public int SingleCostAmount { get; set; }     // いくら消費するか

        // 10連ガチャのコスト
        public int TenCostCurrencyId { get; set; }
        public VirtualCurrency TenCostCurrency { get; set; } = null!;
        public int TenCostAmount { get; set; }

        // ナビゲーションプロパティ
        public ICollection<LotteryRarity> Rarities { get; set; } = new List<LotteryRarity>();
        public ICollection<LotteryPrize> Prizes { get; set; } = new List<LotteryPrize>();
    }

    /// <summary>
    /// レアリティごとの重み設定（例：SSR 3%, SR 17%, R 80% など）
    /// </summary>
    public class LotteryRarity : CatalogItemBase
    {
        public int LotteryId { get; set; }
        public Lottery Lottery { get; set; } = null!;

        public ItemRarity Rarity { get; set; } = ItemRarity.COMMON;
        public int Weight { get; set; } = 1;
    }

    /// <summary>
    /// 具体的な景品とその重み
    /// </summary>
    public class LotteryPrize : CatalogItemBase
    {

        public int LotteryId { get; set; }
        public Lottery Lottery { get; set; } = null!;
        public int PrizeCatalogId { get; set; }
        public Catalog PrizeCatalog { get; set; } = null!;
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

            // 単発コストのリレーション (1つの通貨を多くのガチャが参照)
            builder.HasOne(e => e.SingleCostCurrency)
                   .WithMany()
                   .HasForeignKey(e => e.SingleCostCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 10連コストのリレーション
            builder.HasOne(e => e.TenCostCurrency)
                   .WithMany()
                   .HasForeignKey(e => e.TenCostCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 消費量の設定
            builder.Property(e => e.SingleCostAmount).HasDefaultValue(0);
            builder.Property(e => e.TenCostAmount).HasDefaultValue(0);
        }
    }

    // --- LotteryRarity (継承モデル) ---
    public class LotteryRarityConfiguration : CatalogItemBaseConfiguration<LotteryRarity>
    {
        public override void Configure(EntityTypeBuilder<LotteryRarity> builder)
        {
            base.Configure(builder);
            builder.ToTable("lottery_rarities");

            builder.HasOne(e => e.Lottery)
                   .WithMany(l => l.Rarities)
                   .HasForeignKey(e => e.LotteryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Rarity)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(e => e.Weight).HasDefaultValue(1);
        }
    }

    // --- LotteryPrize (継承モデル) ---
    public class LotteryPrizeConfiguration : CatalogItemBaseConfiguration<LotteryPrize>
    {
        public override void Configure(EntityTypeBuilder<LotteryPrize> builder)
        {
            base.Configure(builder);
            builder.ToTable("lottery_prizes");

            // ガチャ本体とのリレーション
            builder.HasOne(e => e.Lottery)
                   .WithMany(l => l.Prizes)
                   .HasForeignKey(e => e.LotteryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 景品（Catalog本体）とのリレーション
            builder.HasOne(e => e.PrizeCatalog)
                   .WithMany()
                   .HasForeignKey(e => e.PrizeCatalogId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Rarity)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(e => e.Weight).HasDefaultValue(1);

            // 検索効率化のための複合インデックス
            builder.HasIndex(e => new { e.LotteryId, e.Rarity })
                   .HasDatabaseName("IX_LotteryPrize_Lottery_Rarity");
        }
    }

    public enum PityType
    {
        NONE,
        RARITY,     //レアリティ確定
        PRIZE       //特定の景品確定
    }
}