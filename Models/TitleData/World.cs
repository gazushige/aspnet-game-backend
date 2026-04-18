using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApi.Models
{
    /// <summary>
    /// ワールドフェーズのマスターデータ。
    /// 「どうなったら進む」「進んだら何が変わる」を定義する。
    /// </summary>
    public class WorldPhaseMaster : IEntity, IHasEffect, IHasRequirement
    {
        public int Id { get; set; }

        /// <summary>フェーズ番号 (0=初期状態, 昇順で進行)</summary>
        public int Phase { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // ── 進行条件 ─────────────────────────────────────────

        /// <summary>
        /// 集計ベースの進行条件 (JSON / AND評価)
        /// e.g. {"avg_player_level":50, "total_boss_kill":10000}
        /// </summary>
        public string? Requirement { get; set; }

        /// <summary>
        /// このフェーズへの進行が解禁される最短日時。
        /// 条件を満たしていてもこの日時以前には進行しない。
        /// null = 日時制限なし (条件のみで進行)
        /// </summary>
        public DateTimeOffset? UnlockAfter { get; set; }

        /// <summary>
        /// 運営による強制進行日時。
        /// この日時を過ぎたら条件未達でも強制的に進行する。
        /// null = 強制進行なし
        /// </summary>
        public DateTimeOffset? ForceProgressAt { get; set; }

        // ── 効果定義 ─────────────────────────────────────────

        /// <summary>
        /// このフェーズで解放されるコンテンツ (JSON)
        /// e.g. {"level_cap":60, "unlocked_quests":[101,102], "unlocked_areas":["area_03"]}
        /// </summary>
        public string? CustomData { get; set; }

        // ── アセット ──────────────────────────────────────────
        public int? BannerAssetId { get; set; }
        public AddressableAsset? BannerAsset { get; set; }
    }

    /// <summary>
    /// サーバー全体の現在フェーズ進行状態。
    /// レコードは常に1件のみ (Id=1固定)。
    /// </summary>
    public class WorldProgressState : IHasTimestamps
    {
        public int Id { get; set; } = 1;

        /// <summary>現在のフェーズ番号</summary>
        public int CurrentPhase { get; set; }

        /// <summary>現在フェーズへの進行日時</summary>
        public DateTimeOffset PhaseStartedAt { get; set; }

        // ── 集計キャッシュ ────────────────────────────────────
        // PlayFabから定期集計してキャッシュする値群

        /// <summary>全プレイヤーの平均レベル (定期集計)</summary>
        public decimal AvgPlayerLevel { get; set; }

        /// <summary>全プレイヤーの累計ボスキル数</summary>
        public long TotalBossKillCount { get; set; }

        /// <summary>アクティブプレイヤー数 (直近7日)</summary>
        public int ActivePlayerCount { get; set; }

        /// <summary>最終集計日時</summary>
        public DateTimeOffset LastAggregatedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
    public class WorldPhaseMasterConfiguration : IEntityTypeConfiguration<WorldPhaseMaster>
    {
        public void Configure(EntityTypeBuilder<WorldPhaseMaster> builder)
        {
            builder.ToTable("world_phase_masters");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Phase)
                .HasColumnName("phase")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(x => x.Requirement)
                .HasColumnName("requirement")
                .HasColumnType("jsonb");

            builder.Property(x => x.UnlockAfter)
                .HasColumnName("unlock_after");

            builder.Property(x => x.ForceProgressAt)
                .HasColumnName("force_progress_at");

            builder.Property(x => x.CustomData)
                .HasColumnName("custom_data")
                .HasColumnType("jsonb");

            builder.Property(x => x.BannerAssetId)
                .HasColumnName("banner_asset_id");

            builder.HasOne(x => x.BannerAsset)
                .WithMany()
                .HasForeignKey(x => x.BannerAssetId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Phase)
                .HasDatabaseName("ix_world_phase_masters_phase")
                .IsUnique();

            builder.HasIndex(x => x.UnlockAfter)
                .HasDatabaseName("ix_world_phase_masters_unlock_after");

            builder.HasIndex(x => x.ForceProgressAt)
                .HasDatabaseName("ix_world_phase_masters_force_progress_at");
        }
    }

    public class WorldProgressStateConfiguration : IEntityTypeConfiguration<WorldProgressState>
    {
        public void Configure(EntityTypeBuilder<WorldProgressState> builder)
        {
            builder.ToTable("world_progress_state");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever(); // 常にId=1の1レコード運用

            builder.Property(x => x.CurrentPhase)
                .HasColumnName("current_phase")
                .IsRequired();

            builder.Property(x => x.PhaseStartedAt)
                .HasColumnName("phase_started_at")
                .IsRequired();

            builder.Property(x => x.AvgPlayerLevel)
                .HasColumnName("avg_player_level")
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.TotalBossKillCount)
                .HasColumnName("total_boss_kill_count")
                .IsRequired();

            builder.Property(x => x.ActivePlayerCount)
                .HasColumnName("active_player_count")
                .IsRequired();

            builder.Property(x => x.LastAggregatedAt)
                .HasColumnName("last_aggregated_at")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        }
    }
}