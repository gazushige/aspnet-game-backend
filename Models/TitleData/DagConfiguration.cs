using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;


namespace MyApi.Configurations
{
    // =========================================================
    // DAG層
    // =========================================================

    public class DagConfiguration : IEntityTypeConfiguration<Dag>
    {
        public void Configure(EntityTypeBuilder<Dag> builder)
        {
            builder.ToTable("Dags");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Description)
                .HasMaxLength(500);

            // ✅ 計算プロパティをEF Coreから無視させる
            builder.Ignore(d => d.Nodes);
            builder.Ignore(d => d.RootNodes);

            // ✅ MembershipsへのリレーションはDagMembershipConfigurationに任せる
            //    ここでの HasMany(d => d.Nodes) は削除

            builder.HasDiscriminator<string>("DagType")
                   .HasValue<SkillTree>("SkillTree");
        }
    }

    public class DagNodeConfiguration : IEntityTypeConfiguration<DagNode>
    {
        public void Configure(EntityTypeBuilder<DagNode> builder)
        {
            builder.ToTable("DagNodes");
            builder.HasKey(n => n.Id);

            // ✅ 計算プロパティを無視
            builder.Ignore(n => n.Parents);
            builder.Ignore(n => n.Children);
            builder.Ignore(n => n.IsRoot);
            builder.Ignore(n => n.IsLeaf);
            builder.Ignore(n => n.Depth);
            builder.Ignore(n => n.Dags);

            builder.HasDiscriminator<string>("NodeType")
                .HasValue<SkillNode>("SkillNode");
        }
    }

    public class DagMembershipConfiguration : IEntityTypeConfiguration<DagMembership>
    {
        public void Configure(EntityTypeBuilder<DagMembership> builder)
        {
            builder.ToTable("DagMemberships");
            builder.HasKey(m => m.Id);

            // ユニーク制約（同じDAGに同じNodeが重複登録されるのを防ぐ）
            builder.HasIndex(m => new { m.DagId, m.NodeId }).IsUnique();

            builder.Property(m => m.IsEnabled)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(m => m.Dag)
                .WithMany(d => d.Memberships)
                .HasForeignKey(m => m.DagId)
                .OnDelete(DeleteBehavior.Cascade);  // DAG削除 → Membership削除

            builder.HasOne(m => m.Node)
                .WithMany(n => n.Memberships)
                .HasForeignKey(m => m.NodeId)
                .OnDelete(DeleteBehavior.Cascade);  // Node削除 → Membership削除
        }
    }

    public class DagEdgeConfiguration : IEntityTypeConfiguration<DagEdge>
    {
        public void Configure(EntityTypeBuilder<DagEdge> builder)
        {
            builder.ToTable("DagEdges");
            builder.HasKey(e => e.Id);

            // 同じ親子ペアかつ同じDAGスコープの重複を防ぐ
            builder.HasIndex(e => new { e.ParentId, e.ChildId, e.DagId }).IsUnique();

            builder.HasOne(e => e.Parent)
                .WithMany(n => n.ChildEdges)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // 誤削除防止

            builder.HasOne(e => e.Child)
                .WithMany(n => n.ParentEdges)
                .HasForeignKey(e => e.ChildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Dag)
                .WithMany()
                .HasForeignKey(e => e.DagId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); // DAG削除 → エッジのスコープをnullに
        }
    }

    // =========================================================
    // ドメイン層
    // =========================================================

    public class SkillTreeConfiguration : IEntityTypeConfiguration<SkillTree>
    {
        public void Configure(EntityTypeBuilder<SkillTree> builder)
        {
            // TPH継承のためToTable不要（DagConfigurationに従う）
            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }

    public class SkillNodeConfiguration : IEntityTypeConfiguration<SkillNode>
    {
        public void Configure(EntityTypeBuilder<SkillNode> builder)
        {
            builder.Property(n => n.Cost)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(n => n.Skill)
                .WithMany(s => s.Nodes)
                .HasForeignKey(n => n.SkillId)
                .OnDelete(DeleteBehavior.Restrict); // Skill削除時はノードを残す
        }
    }

    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.CustomData);
            // .HasColumnType("jsonb"); // PostgreSQL jsonb型
        }
    }
}