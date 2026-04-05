using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MyApi.Models.MyApi.Models;

namespace MyApi.Models
{
    public class CatalogVersion : VersionBase
    {
        public string PlayfabCatalogVersion { get; set; } = string.Empty;

        // JSONField 相当（System.Text.Json.JsonDocument を使用）
        public JsonDocument? SnapShot { get; set; }
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

    public abstract class VersionBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : VersionBase
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

    public class CatalogVersionConfiguration : VersionBaseConfiguration<CatalogVersion>
    {
        public override void Configure(EntityTypeBuilder<CatalogVersion> builder)
        {
            base.Configure(builder);
            builder.ToTable("catalog_versions");

            builder.HasOne(e => e.PlayfabCatalogVersion)
                   .WithMany()
                   .HasForeignKey(e => e.PlayfabCatalogVersion)
                   .OnDelete(DeleteBehavior.Restrict); // PROTECT

            // unique_together 相当
            builder.HasIndex(e => new { e.PlayfabCatalogVersion, e.Major, e.Minor, e.Patch })
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

            builder.Property(e => e.Platform).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.StoreUrl).HasMaxLength(500);
        }
    }
    namespace MyApi.Models
    {
        /// <summary>
        /// Unity Addressables のアセット情報を表す
        /// </summary>
        public class AddressableAsset
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
    }
}
