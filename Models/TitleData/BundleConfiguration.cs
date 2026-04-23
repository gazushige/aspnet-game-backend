using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApi.Models
{
    // =========================================================
    // BundledItem
    // =========================================================

    public class BundledItemConfiguration : IEntityTypeConfiguration<BundledItem>
    {
        public void Configure(EntityTypeBuilder<BundledItem> builder)
        {
            builder.ToTable("bundled_items");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>()    // DB上はstringで保存
                .HasMaxLength(32);

            builder.Property(x => x.CatalogId)
                .IsRequired();
        }
    }

    // =========================================================
    // EligibilityCondition
    // =========================================================

    public class EligibilityConditionConfiguration : IEntityTypeConfiguration<EligibilityCondition>
    {
        public void Configure(EntityTypeBuilder<EligibilityCondition> builder)
        {
            builder.ToTable("eligibility_conditions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.Data)
                .IsRequired()
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'{}'::jsonb");
        }
    }

    // =========================================================
    // ObjectiveCriteria
    // =========================================================

    public class ObjectiveCriteriaConfiguration : IEntityTypeConfiguration<ObjectiveCriteria>
    {
        public void Configure(EntityTypeBuilder<ObjectiveCriteria> builder)
        {
            builder.ToTable("objective_criteria");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Criteria)
                .IsRequired()
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'{}'::jsonb");

            builder.Property(x => x.TrackingMode)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(32);
        }
    }

    // =========================================================
    // RewardDefinition (TPH: Table Per Hierarchy)
    // =========================================================

    /// <summary>
    /// LoginBonus / DailyMission / Achievement を1テーブルに収める。
    /// マスターデータかつ構造が近いのでTPHが適切。
    /// TPTにする場合は各サブクラスConfigでToTable()を個別指定する。
    /// </summary>
    public class RewardDefinitionConfiguration : IEntityTypeConfiguration<RewardDefinition>
    {
        public void Configure(EntityTypeBuilder<RewardDefinition> builder)
        {
            builder.ToTable("reward_definitions");
            builder.HasKey(x => x.Id);

            // Discriminator列でサブクラスを区別
            builder.HasDiscriminator<string>("reward_type")
                .HasValue<LoginBonus>("LoginBonus")
                .HasValue<DailyMission>("DailyMission")
                .HasValue<Achievement>("Achievement");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            builder.Property(x => x.StartAt)
                .HasColumnType("timestamptz");

            builder.Property(x => x.ExpiredAt)
                .HasColumnType("timestamptz");

            // RewardDefinition → BundledItem[] (1対多)
            // 親が消えたらBundledItemも消す
            builder.HasMany(x => x.BundledItems)
                .WithOne()
                .HasForeignKey("RewardDefinitionId")    // Shadow FK
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // =========================================================
    // LoginBonus
    // =========================================================

    public class LoginBonusConfiguration : IEntityTypeConfiguration<LoginBonus>
    {
        public void Configure(EntityTypeBuilder<LoginBonus> builder)
        {
            builder.Property(x => x.RequiredConsecutiveDays);  // null許容のままでOK

            // LoginBonus → EligibilityCondition (0..1 対 1)
            // 条件は任意。ただしEligibilityConditionは独立して管理したいのでRestrict。
            builder.HasOne(x => x.EligibilityCondition)
                .WithOne()
                .HasForeignKey<LoginBonus>("EligibilityConditionId")    // Shadow FK
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    // =========================================================
    // DailyMission
    // =========================================================

    public class DailyMissionConfiguration : IEntityTypeConfiguration<DailyMission>
    {
        public void Configure(EntityTypeBuilder<DailyMission> builder)
        {
            // DailyMission → ObjectiveCriteria (1対1, 必須)
            builder.HasOne(x => x.Objective)
                .WithOne()
                .HasForeignKey<DailyMission>("ObjectiveCriteriaId")     // Shadow FK
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            // DailyMission → EligibilityCondition (0..1 対 1)
            builder.HasOne(x => x.EligibilityCondition)
                .WithOne()
                .HasForeignKey<DailyMission>("EligibilityConditionId")  // Shadow FK
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    // =========================================================
    // Achievement
    // =========================================================

    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            // Achievement → ObjectiveCriteria (1対1, 必須)
            builder.HasOne(x => x.Objective)
                .WithOne()
                .HasForeignKey<Achievement>("ObjectiveCriteriaId")      // Shadow FK
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            // Achievement → EligibilityCondition (0..1 対 1)
            builder.HasOne(x => x.EligibilityCondition)
                .WithOne()
                .HasForeignKey<Achievement>("EligibilityConditionId")   // Shadow FK
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Achievement → AchievementTier[] (1対多)
            builder.HasMany(x => x.Tiers)
                .WithOne()
                .HasForeignKey("AchievementId")                         // Shadow FK
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // =========================================================
    // AchievementTier
    // =========================================================

    public class AchievementTierConfiguration : IEntityTypeConfiguration<AchievementTier>
    {
        public void Configure(EntityTypeBuilder<AchievementTier> builder)
        {
            builder.ToTable("achievement_tiers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Label)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.RequiredProgress)
                .IsRequired();

            // AchievementTier → BundledItem[] (1対多)
            builder.HasMany(x => x.BundledItems)
                .WithOne()
                .HasForeignKey("AchievementTierId")                     // Shadow FK
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}