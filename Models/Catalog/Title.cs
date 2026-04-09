using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    /// <summary>
    /// タイトル（称号）を表すアイテム。プレイヤーに付与されることを想定。例: "勇者", "魔王", "伝説の剣士" など
    /// タイトルはプレイヤーに装備させることができ、特定の効果を持つことがあるため、IHasEffectも実装する。
    /// さらに、期間限定のタイトルもあるかもしれないので、IHasExpiryも実装する。       
    /// </summary> 
    public class Title : CatalogItemBase, IHasExpiry, IHasEffect
    {
        public string Name { get; set; } = string.Empty;
        public string? IconUrl { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string? CustomData { get; set; }
    }
    public class TitleConfiguration : CatalogItemBaseConfiguration<Title>
    {
        public override void Configure(EntityTypeBuilder<Title> builder)
        {
            base.Configure(builder);
            builder.ToTable("titles");
            builder.Property(t => t.CustomData).HasColumnType("jsonb");
        }
    }
}
