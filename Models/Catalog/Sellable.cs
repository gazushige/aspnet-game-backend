using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    /// <summary>
    /// 売買可能なアイテム。ガチャアイテムやショップアイテムなど、ユーザーが入手可能なアイテムを表す。CatalogItemBaseを継承して、価格や在庫などの売買に関する属性を追加する。
    /// </summary>
    public abstract class SellableCatalogItem : CatalogItemBase
    {
        public bool IsStackable { get; set; }
        public bool IsTradable { get; set; }

        public bool IsLimitedEdition { get; set; } = false;
        public int InitialLimitedEditionCount { get; set; }
    }
    public abstract class SellableCatalogItemConfiguration<TEntity> : CatalogItemBaseConfiguration<TEntity>
    where TEntity : SellableCatalogItem
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // 基底クラス(DisplayName, CreatedAtなど)の設定を適用
            base.Configure(builder);

            // 数値・フラグ系のデフォルト値設定
            builder.Property(e => e.IsStackable).HasDefaultValue(false);
            builder.Property(e => e.IsTradable).HasDefaultValue(false);
            builder.Property(e => e.IsLimitedEdition).HasDefaultValue(false);
            builder.Property(e => e.InitialLimitedEditionCount).HasDefaultValue(0);
        }
    }
}