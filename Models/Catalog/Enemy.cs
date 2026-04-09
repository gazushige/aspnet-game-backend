using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyApi.Models
{
    /// <summary>
    /// 敵キャラクターを表す（マスターデータ）
    /// </summary>
    public class Enemy : CatalogItemBase, IHasEffect
    {
        // 敵固有のパラメータ（HP, ATKなど）を JSONB で保持
        // 頻繁に検索対象になる項目（Levelなど）があれば、プロパティとして切り出すのが吉
        public string? CustomData { get; set; }

        // ドロップテーブルへの外部キー
        public int DropTableId { get; set; }
        public DropTable DropTable { get; set; } = null!;
    }

    public class EnemyConfiguration : CatalogItemBaseConfiguration<Enemy>
    {
        public override void Configure(EntityTypeBuilder<Enemy> builder)
        {
            // 1. 基底クラス(DisplayName, CreatedAtなど)の設定を適用
            base.Configure(builder);

            // 2. テーブル名の明示
            builder.ToTable("enemies");

            // 3. プロパティ設定
            builder.Property(e => e.CustomData)
                   .HasColumnType("jsonb");

            // 4. リレーションシップ (Django の PROTECT 相当)
            // ドロップテーブルが削除される際、それを使用している敵がいればエラーにする
            builder.HasOne(e => e.DropTable)
                   .WithMany() // DropTable 側から敵一覧を引く必要がなければ空でOK
                   .HasForeignKey(e => e.DropTableId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 5. インデックスとユニーク制約 (Django Meta 相当)

            // UQ: カタログUUIDとリビジョンの一意性
            builder.HasIndex(e => new { e.CatalogUuid, e.Revision })
                   .IsUnique()
                   .HasDatabaseName("UQ_Enemy_Catalog_Revision");

            // Partial Index: 現在有効なバージョンはカタログごとに1つだけ
            // Django の condition=models.Q(is_current_version=True) 相当
            builder.HasIndex(e => e.CatalogUuid)
                   .IsUnique()
                   .HasFilter("\"IsCurrentVersion\" = TRUE")
                   .HasDatabaseName("UQ_Enemy_CurrentVersion");
        }
    }
}