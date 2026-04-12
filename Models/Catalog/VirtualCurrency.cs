using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    public class VirtualCurrency : IEntity
    {
        public int Id { get; set; }

        // PlayFabの仮想通貨コード (例: "GD", "ST")
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public CurrencyType CurrencyType { get; set; } = CurrencyType.FREE;

        // --- スタミナ/時間回復設定 ---

        /// <summary>
        /// 時間回復するかどうか
        /// </summary>
        public bool IsRechargeable => CurrencyType == CurrencyType.Recharge;

        /// <summary>
        /// 24時間あたりの回復量（PlayFabの仕様に準拠）
        /// 例: 5分に1回復なら 1440/5 = 288
        /// </summary>
        public int RechargeRate { get; set; } = 0;

        /// <summary>
        /// 自動回復で到達する最大値（スタミナ上限）
        /// 課金等でこれを超えて所持することは可能（オーバーフロー）
        /// </summary>
        public int MaxCapacity { get; set; } = 0;

        /// <summary>
        /// 絶対的な最大所持制限（DB/システム上の上限）
        /// </summary>
        public int MaxQuantity { get; set; } = 2000000000;
    }

    public class VirtualCurrencyConfiguration : IEntityTypeConfiguration<VirtualCurrency>
    {
        public void Configure(EntityTypeBuilder<VirtualCurrency> builder)
        {
            builder.ToTable("virtual_currencies");
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Code).IsUnique();

            // スタミナ関連のデフォルト
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Code).IsRequired().HasMaxLength(10);
            builder.Property(e => e.RechargeRate).HasDefaultValue(0);
            builder.Property(e => e.MaxCapacity).HasDefaultValue(0);

            builder.Property(e => e.MaxQuantity).HasDefaultValue(2000000000);
        }
    }

    public enum CurrencyType
    {
        PAID = 1,   // 課金
        FREE = 2,   // 無料
        Recharge = 3 // リチャージ
    }
}