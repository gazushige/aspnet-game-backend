using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApi.Models
{
    /// <summary>
    /// 運営からの告知を表すモデル。例: メンテナンスのお知らせ、イベント開始の告知、緊急メンテナンスの通知など
    /// SignalRなどのリアルタイム通信を通じてプレイヤーに配信されることを想定。告知の表示条件をJSON形式で保存するためのRequirementフィールドも持つ。
    /// </summary> 
    public class Announcement : IEntity, IHasRequirement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTimeOffset ExecutedAt { get; set; }
        public string? Requirement { get; set; } = string.Empty;// 告知の表示条件をJSON形式で保存するためのフィールド。例: {"minLevel": 10, "vipOnly": true} など
    }
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("announcements");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title).HasMaxLength(200);
            builder.Property(a => a.Message).HasMaxLength(2000);
            builder.Property(a => a.Requirement).HasColumnType("jsonb");
        }
    }
}