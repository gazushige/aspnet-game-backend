using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
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

              public DateTimeOffset CreatedAt { get; set; }
              public DateTimeOffset UpdatedAt { get; set; }

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