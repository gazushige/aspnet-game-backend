namespace MyApi.Models
{
    public class SkillTree : Dag
    {
        public bool IsActive { get; set; } = true;
        // 特になし。シンプルで良い。
    }

    public class SkillNode : DagNode
    {
        // Skill は必須にすべき（SkillのないNodeは意味をなさない）
        public int SkillId { get; set; }           // FK を明示（EF Core推奨）
        public Skill Skill { get; set; } = null!;  // null許容をやめる

        public int Cost { get; set; }              // 命名規則をPascalCaseに統一
    }

    public class Skill : IEntity, IHasEffect
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CustomData { get; set; }

        // 逆参照ナビゲーション（どのノードで使われているか）
        public ICollection<SkillNode> Nodes { get; set; } = [];
    }
}