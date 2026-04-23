using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    /// <summary>
    /// タイトル（称号）を表すアイテム。プレイヤーに付与されることを想定。例: "勇者", "魔王", "伝説の剣士" など
    /// タイトルはプレイヤーに装備させることができ、特定の効果を持つことがあるため、IHasEffectも実装する。
    /// さらに、期間限定のタイトルもあるかもしれないので、IHasExpiryも実装する。       
    /// </summary> 
    public class Title : CatalogItemBase, IHasExpiry, IHasCustomData, IHasRequirement
    {
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? ExpiredAt { get; set; }
        public string? CustomData { get; set; } // タイトルの効果やパラメータをJSON形式で保存するためのフィールド。例: {"attackBoost": 10, "defenseBoost": 5} など
        public string? Requirement { get; set; }// タイトルの獲得条件をJSON形式で保存するためのフィールド。例: {"minLevel": 10, "requiredQuests": ["quest001", "quest002"]} など
    }
    public class TitleConfiguration : CatalogItemBaseConfiguration<Title>
    {
        public override void Configure(EntityTypeBuilder<Title> builder)
        {
            base.Configure(builder);
            builder.ToTable("titles");
            builder.Property(t => t.CustomData).HasColumnType("jsonb");
            builder.Property(t => t.Requirement).HasColumnType("jsonb");
        }
    }
}
