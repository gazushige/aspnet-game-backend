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
              public string Label { get; set; } = string.Empty;       // Addressablesのラベル。複数のアセットを同じラベルでグループ化できる。例: "character_prefabs", "ui_sprites" など
              public string Path { get; set; } = string.Empty;        // Addressablesのアセットパス (例: "Assets/Prefabs/Characters/Hero.prefab") Unity側での一意識別子として使用
              public Uri? PublicUrl { get; set; }                     // S3などのCDN配信URL       
              public DateTimeOffset CreatedAt { get; set; }
              public DateTimeOffset UpdatedAt { get; set; }

              // AssetVersion との多対多リレーションシップ
              // Django の assets.ManyToManyField 相当
              public ICollection<AssetVersion> Versions { get; set; } = new List<AssetVersion>();
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
                            .HasConversion(
                                v => v == null ? null : v.ToString(),
                                v => string.IsNullOrEmpty(v) ? null : new Uri(v)
                            )
                            .HasMaxLength(2048);

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