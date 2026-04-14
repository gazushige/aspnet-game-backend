
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApi.Models
{
    // -------------------------------------------------------
    // インターフェース
    // -------------------------------------------------------

    /// <summary>
    /// バージョン管理の共通インターフェース
    /// </summary>
    public interface IVersion
    {
        int Id { get; }
        ushort Major { get; }
        ushort Minor { get; }
        ushort Patch { get; }
        string SemVer { get; }
        VersionStatus Status { get; }
        DateTimeOffset? ReleasedAt { get; }
    }

    // -------------------------------------------------------
    // 基底クラス
    // -------------------------------------------------------

    /// <summary>
    /// バージョン管理の基底クラス。
    /// DBエンティティへの依存を避けるため、ロジックメソッドは
    /// 同型バージョンのコレクションを引数として受け取る設計にしている。
    /// 「必ずしも最新が有効とは限らない（巻き戻しあり）」を前提とする。
    /// </summary>
    public abstract class VersionBase : IEntity, IHasTimestamps, IVersion
    {
        public int Id { get; set; }
        public ushort Major { get; set; }
        public ushort Minor { get; set; }
        public ushort Patch { get; set; }
        public string? Description { get; set; }
        public VersionStatus Status { get; set; } = VersionStatus.Draft;
        public DateTimeOffset? ReleasedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>SemVer文字列（例: "1.2.3"）</summary>
        public string SemVer => $"{Major}.{Minor}.{Patch}";

        /// <summary>
        /// バージョンの大小比較用タプル（Major, Minor, Patch の辞書順）
        /// </summary>
        private (ushort, ushort, ushort) VersionTuple => (Major, Minor, Patch);

        // -------------------------------------------------------
        // クエリ系：DbContextへの依存を避け、呼び出し元がコレクションを渡す
        // -------------------------------------------------------

        /// <summary>
        /// 現在有効（Released）なバージョンを返す。
        /// Releasedは常に0または1つのみ存在する想定。
        /// </summary>
        public static T? CurrentVersion<T>(IEnumerable<T> allVersions)
            where T : VersionBase
            => allVersions.FirstOrDefault(v => v.Status == VersionStatus.Released);

        /// <summary>
        /// Released バージョンの一つ前のバージョン（SemVer降順で Released の直前）を返す。
        /// 「一つ前」= Released より SemVer が小さいもののうち最大のもの。
        /// </summary>
        public static T? PreviousVersion<T>(IEnumerable<T> allVersions)
            where T : VersionBase
        {
            var released = CurrentVersion(allVersions);
            if (released == null) return null;

            return allVersions
                .Where(v => v.Status != VersionStatus.Blocked
                         && Compare(v, released) < 0)
                .OrderByDescending(v => v.Major)
                .ThenByDescending(v => v.Minor)
                .ThenByDescending(v => v.Patch)
                .FirstOrDefault();
        }

        /// <summary>
        /// SemVer が最も大きいバージョンを返す（Statusは問わない）。
        /// </summary>
        public static T? LatestVersion<T>(IEnumerable<T> allVersions)
            where T : VersionBase
            => allVersions
                .OrderByDescending(v => v.Major)
                .ThenByDescending(v => v.Minor)
                .ThenByDescending(v => v.Patch)
                .FirstOrDefault();

        // -------------------------------------------------------
        // ミューテーション系：失敗時は例外をスロー（呼び出し元でtry-catchまたはResult型でラップ可）
        // -------------------------------------------------------

        /// <summary>
        /// このバージョンを Released にする。
        /// - このバージョンが Draft であること
        /// - コレクション内に Released が存在しないこと
        /// どちらかを満たさない場合は何もしない（冪等）。
        /// </summary>
        public void ReleaseThisVersion<T>(IEnumerable<T> allVersions)
            where T : VersionBase
        {
            if (Status != VersionStatus.Draft)
                return;

            if (allVersions.Any(v => v.Status == VersionStatus.Released))
                return;

            Status = VersionStatus.Released;
            ReleasedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// このバージョンを Draft に戻し、一つ前の（Releasedより小さい最大の）バージョンを Released にする。
        /// - このバージョンが Released であること
        /// - 前のバージョンが存在すること
        /// </summary>
        public void ReleasePreviousVersion<T>(IEnumerable<T> allVersions)
            where T : VersionBase
        {
            if (Status != VersionStatus.Released)
                return;

            var previous = PreviousVersion(allVersions);
            if (previous == null)
                return;

            Status = VersionStatus.Draft;
            UpdatedAt = DateTime.UtcNow;

            previous.Status = VersionStatus.Released;
            previous.ReleasedAt = DateTime.UtcNow;
            previous.UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// このバージョンを Deprecated にし、指定した次のバージョンを Released にする。
        /// - このバージョンが Released であること
        /// - nextVersion が Draft であること
        /// - nextVersion の SemVer がこのバージョンを上回ること
        /// </summary>
        public void ReleaseNextVersion<T>(T nextVersion)
            where T : VersionBase
        {
            if (Status != VersionStatus.Released)
                throw new InvalidOperationException(
                    $"現在のバージョン ({SemVer}) は Released ではありません。");

            if (nextVersion.Status != VersionStatus.Draft)
                throw new InvalidOperationException(
                    $"次のバージョン ({nextVersion.SemVer}) は Draft ではありません。");

            if (Compare(nextVersion, this) <= 0)
                throw new InvalidOperationException(
                    $"次のバージョン ({nextVersion.SemVer}) は現在のバージョン ({SemVer}) を上回っている必要があります。");

            Status = VersionStatus.Deprecated;
            UpdatedAt = DateTime.UtcNow;

            nextVersion.Status = VersionStatus.Released;
            nextVersion.ReleasedAt = DateTime.UtcNow;
            nextVersion.UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>SemVer の大小比較（戻り値は IComparable 規約に従う）</summary>
        private static int Compare<T>(T a, T b) where T : VersionBase
        {
            var result = a.Major.CompareTo(b.Major);
            if (result != 0) return result;
            result = a.Minor.CompareTo(b.Minor);
            if (result != 0) return result;
            return a.Patch.CompareTo(b.Patch);
        }
    }

    // -------------------------------------------------------
    // 具象バージョンクラス
    // -------------------------------------------------------

    /// <summary>
    /// Playfab カタログのバージョン
    /// </summary>
    public class CatalogVersion : VersionBase
    {
        /// <summary>カタログに実際に送信される raw JSON データ</summary>
        public string? SnapShot { get; set; }
    }

    /// <summary>
    /// Unity Addressable のアセットバンドルバージョン
    /// </summary>
    public class AssetVersion : VersionBase
    {
        public ICollection<AddressableAsset> Assets { get; set; } = new List<AddressableAsset>();
    }

    /// <summary>
    /// クライアントアプリのバージョン
    /// </summary>
    public class AppVersion : VersionBase
    {
        public AppPlatform Platform { get; set; }
        public int BuildNumber { get; set; }
        public bool ForceUpdate { get; set; } = false;
        public string StoreUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// クライアントが使用するアプリ＋アセットの複合バージョン。
    /// AppVersion と AssetVersion の組み合わせを管理する。
    /// VersionBase は継承せず、独立したエンティティとして扱う。
    /// </summary>
    public class UpdateVersion : IEntity, IHasTimestamps, IVersion
    {
        public int Id { get; set; }

        // IVersion の実装：AppVersion の値を委譲
        public ushort Major => App.Major;
        public ushort Minor => App.Minor;
        public ushort Patch => App.Patch;
        public string SemVer => App.SemVer;
        public VersionStatus Status => App.Status;
        public DateTimeOffset? ReleasedAt => App.ReleasedAt;

        public AppPlatform Platform => App.Platform;

        public int AppVersionId { get; set; }
        public AppVersion App { get; set; } = null!;

        public int AssetVersionId { get; set; }
        public AssetVersion Asset { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    // -------------------------------------------------------
    // Enum
    // -------------------------------------------------------

    public enum VersionStatus
    {
        Draft,
        Staging,
        Released,
        Deprecated,
        Blocked
    }

    public enum AppPlatform
    {
        Android,
        Ios,
        Windows
    }

    // -------------------------------------------------------
    // EF Core Configuration
    // -------------------------------------------------------

    public abstract class VersionBaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : VersionBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Major).IsRequired();
            builder.Property(e => e.Minor).IsRequired();
            builder.Property(e => e.Patch).IsRequired();

            builder.Property(e => e.Description).HasMaxLength(1000);

            builder.Property(e => e.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(e => e.ReleasedAt);

            builder.Property(e => e.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .ValueGeneratedOnAddOrUpdate();

            // 計算プロパティはDBカラムにマップしない
            builder.Ignore(e => e.SemVer);

            // リリース済みバージョンは常に1つのみ（DBレベルの部分ユニーク制約はマイグレーションで追加）
            // 例: CREATE UNIQUE INDEX UQ_{Table}_Released ON ... (Status) WHERE Status = 'Released'
        }
    }

    public class CatalogVersionConfiguration : VersionBaseConfiguration<CatalogVersion>
    {
        public override void Configure(EntityTypeBuilder<CatalogVersion> builder)
        {
            base.Configure(builder);
            builder.ToTable("catalog_versions");

            builder.HasIndex(e => new { e.Major, e.Minor, e.Patch })
                   .IsUnique()
                   .HasDatabaseName("UQ_CatalogVersion_SemVer");

            builder.Property(e => e.SnapShot).HasColumnType("jsonb");
        }
    }

    public class AppVersionConfiguration : VersionBaseConfiguration<AppVersion>
    {
        public override void Configure(EntityTypeBuilder<AppVersion> builder)
        {
            base.Configure(builder);
            builder.ToTable("app_versions");

            builder.HasIndex(e => new { e.Platform, e.Major, e.Minor, e.Patch })
                   .IsUnique()
                   .HasDatabaseName("UQ_AppVersion_Platform_SemVer");

            builder.Property(e => e.Platform)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(e => e.BuildNumber).IsRequired();
            builder.Property(e => e.ForceUpdate).HasDefaultValue(false);
            builder.Property(e => e.StoreUrl).HasMaxLength(500).IsRequired();
        }
    }

    public class AssetVersionConfiguration : VersionBaseConfiguration<AssetVersion>
    {
        public override void Configure(EntityTypeBuilder<AssetVersion> builder)
        {
            base.Configure(builder);
            builder.ToTable("asset_versions");

            builder.HasIndex(e => new { e.Major, e.Minor, e.Patch })
                   .IsUnique()
                   .HasDatabaseName("UQ_AssetVersion_SemVer");

            builder.HasMany(av => av.Assets)
                   .WithMany(a => a.Versions)
                   .UsingEntity<Dictionary<string, object>>(
                       "AssetVersionAssets",
                       j => j.HasOne<AddressableAsset>()
                             .WithMany()
                             .HasForeignKey("AddressableAssetId")
                             .OnDelete(DeleteBehavior.Cascade),
                       j => j.HasOne<AssetVersion>()
                             .WithMany()
                             .HasForeignKey("AssetVersionId")
                             .OnDelete(DeleteBehavior.Cascade),
                       j =>
                       {
                           j.HasKey("AssetVersionId", "AddressableAssetId");
                           j.ToTable("asset_version_assets");
                       });
        }
    }

    public class UpdateVersionConfiguration : IEntityTypeConfiguration<UpdateVersion>
    {
        public void Configure(EntityTypeBuilder<UpdateVersion> builder)
        {
            builder.ToTable("update_versions");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .ValueGeneratedOnAddOrUpdate();

            // AppVersion との関係（必須）
            builder.HasOne(e => e.App)
                   .WithMany()
                   .HasForeignKey(e => e.AppVersionId)
                   .OnDelete(DeleteBehavior.Restrict);

            // AssetVersion との関係（必須）
            builder.HasOne(e => e.Asset)
                   .WithMany()
                   .HasForeignKey(e => e.AssetVersionId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 同じ App+Asset の組み合わせは1つのみ
            builder.HasIndex(e => new { e.AppVersionId, e.AssetVersionId })
                   .IsUnique()
                   .HasDatabaseName("UQ_UpdateVersion_App_Asset");

            // 計算プロパティはDBにマップしない
            builder.Ignore(e => e.Major);
            builder.Ignore(e => e.Minor);
            builder.Ignore(e => e.Patch);
            builder.Ignore(e => e.SemVer);
            builder.Ignore(e => e.Status);
            builder.Ignore(e => e.ReleasedAt);
            builder.Ignore(e => e.Platform);
        }
    }
}
