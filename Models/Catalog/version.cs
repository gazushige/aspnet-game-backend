using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    public class CatalogVersion : VersionBase
    {
        public string PlayfabCatalogVersion { get; set; } = string.Empty;

        // JSONField 相当（System.Text.Json.JsonDocument を使用）
        public string? SnapShot { get; set; }
    }

    public class AssetVersion : VersionBase
    {
        // Many-to-Many は EF Core ではコレクションで表現
        public ICollection<AddressableAsset> Assets { get; set; } = new List<AddressableAsset>();
    }

    public class AppVersion : VersionBase
    {
        public AppPlatform Platform { get; set; }
        public int BuildNumber { get; set; }
        public bool ForceUpdate { get; set; } = false;
        public string StoreUrl { get; set; } = string.Empty;
    }

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

    public abstract class VersionBase : IEntity
    {
        public int Id { get; set; }
        public ushort Major { get; set; }
        public ushort Minor { get; set; }
        public ushort Patch { get; set; }

        public string? Description { get; set; }
        public VersionStatus Status { get; set; } = VersionStatus.Draft;
        public DateTime? ReleasedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // @property 相当の計算プロパティ
        public string SemVer => $"{Major}.{Minor}.{Patch}";
    }

    public abstract class VersionBaseConfiguration<T> where T : VersionBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            // Enum を文字列として DB に保存する場合（Djangoの挙動に近い）
            builder.Property(e => e.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // タイムスタンプの自動生成設定
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }

    public class CatalogVersionConfiguration : VersionBaseConfiguration<CatalogVersion>, IEntityTypeConfiguration<CatalogVersion>
    {
        public override void Configure(EntityTypeBuilder<CatalogVersion> builder)
        {
            base.Configure(builder);
            builder.ToTable("catalog_versions");

            // ✅ stringプロパティはPropertyで制約を定義する
            builder.Property(e => e.PlayfabCatalogVersion)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(e => new { e.PlayfabCatalogVersion, e.Major, e.Minor, e.Patch })
                   .IsUnique()
                   .HasDatabaseName("UQ_CatalogVersion_SemVer");

            builder.Property(e => e.SnapShot).HasColumnType("jsonb");
        }
    }

    public class AppVersionConfiguration : VersionBaseConfiguration<AppVersion>, IEntityTypeConfiguration<AppVersion>
    {
        public override void Configure(EntityTypeBuilder<AppVersion> builder)
        {
            base.Configure(builder);
            builder.ToTable("app_versions");

            builder.Property(e => e.Platform).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.StoreUrl).HasMaxLength(500);
        }
    }
    public class AssetVersionConfiguration : VersionBaseConfiguration<AssetVersion>, IEntityTypeConfiguration<AssetVersion>
    {
        public override void Configure(EntityTypeBuilder<AssetVersion> builder)
        {
            base.Configure(builder);
            builder.ToTable("asset_versions");

            // Many-to-Many の中間テーブルは EF Core が自動生成するが、必要に応じて Fluent API で細かく制御可能
            builder.HasMany(av => av.Assets)
                   .WithMany(a => a.Versions)
                   .UsingEntity<Dictionary<string, object>>(
                        "AssetVersionAssets", // 中間テーブル名
                        j => j.HasOne<AddressableAsset>().WithMany().HasForeignKey("AddressableAssetId").OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<AssetVersion>().WithMany().HasForeignKey("AssetVersionId").OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey("AssetVersionId", "AddressableAssetId");
                            j.ToTable("asset_version_assets");
                        }
                   );
        }
    }
    /// <summary>
    /// Unity Addressables のアセット情報を表す
    /// </summary>
    public class AddressableAsset : IHasTimestamps, IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Label { get; set; }
        public string Path { get; set; } = string.Empty;
        public string? PublicUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // AssetVersion との多対多リレーションシップ
        // Django の assets.ManyToManyField 相当
        public ICollection<AssetVersion> Versions { get; set; } = new List<AssetVersion>();

        // Django の __str__ 相当（ただしここでは Version への参照が必要）
        public override string ToString() => $"{Name} ({Path})";
    }
    public class AddressableAssetConfiguration : IEntityTypeConfiguration<AddressableAsset>
    {
        public void Configure(EntityTypeBuilder<AddressableAsset> builder)
        {
            // 1. テーブル名
            builder.ToTable("addressable_assets");

            // 2. 主キー
            builder.HasKey(e => e.Id);

            // 3. プロパティ制約
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Label)
                   .HasMaxLength(100);

            // Addressablesのアセットパス (例: "Assets/Prefabs/Characters/Hero.prefab")
            builder.Property(e => e.Path)
                   .IsRequired()
                   .HasMaxLength(500);

            // S3などのCDN配信URL
            builder.Property(e => e.PublicUrl)
                   .HasMaxLength(2048); // URLの標準的な最大長

            // 4. タイムスタンプ
            builder.Property(e => e.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .ValueGeneratedOnAddOrUpdate();

            // 5. インデックス
            // Pathはアセットの一意識別子として使われるため一意制約
            builder.HasIndex(e => e.Path)
                   .IsUnique()
                   .HasDatabaseName("UQ_AddressableAsset_Path");

            // Labelは検索頻度が高いためインデックスを追加
            builder.HasIndex(e => e.Label)
                   .HasDatabaseName("IX_AddressableAsset_Label");

            // 6. Many-to-Many (AssetVersionとの関係はAssetVersionConfiguration側で定義済み)
        }
    }
}

