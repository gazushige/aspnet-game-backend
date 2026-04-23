namespace MyApi.Models
{
    /// <summary>
    /// DAG(有向非巡回グラフ)を表すクラス
    /// </summary>
    public abstract class Dag : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // DagNodeとの多対多（中間テーブル経由）
        public ICollection<DagMembership> Memberships { get; set; } = [];

        public IEnumerable<DagNode> Nodes => Memberships.Select(m => m.Node);
        public IEnumerable<DagNode> RootNodes => Nodes.Where(n => n.IsRoot);
    }

    public abstract class DagNode : IEntity
    {
        public int Id { get; set; }

        public ICollection<DagMembership> Memberships { get; set; } = [];
        public ICollection<DagEdge> ParentEdges { get; set; } = [];
        public ICollection<DagEdge> ChildEdges { get; set; } = [];

        public IEnumerable<DagNode> Parents => ParentEdges.Select(e => e.Parent);
        public IEnumerable<DagNode> Children => ChildEdges.Select(e => e.Child);
        public bool IsRoot => !ParentEdges.Any();
        public bool IsLeaf => !ChildEdges.Any();
        public int Depth => CalculateDepth(this, []);

        // 所属DAG一覧
        public IEnumerable<Dag> Dags => Memberships.Select(m => m.Dag);

        public void AddChild(DagNode child)
        {
            if (child == this)
                throw new ArgumentException("自己参照は許可されていません。");
            if (IsAncestor(child))
                throw new InvalidOperationException($"循環参照になります。ノードId: {child.Id}");

            var edge = new DagEdge { Parent = this, Child = child };
            ChildEdges.Add(edge);
            child.ParentEdges.Add(edge);
        }

        public IReadOnlyList<T> FindChildren<T>() where T : DagNode
            => ChildEdges.Select(e => e.Child).OfType<T>().ToList().AsReadOnly();

        public IReadOnlyList<T> GetAllDescendants<T>() where T : DagNode
        {
            var result = new List<T>();
            var visited = new HashSet<int>();
            var queue = new Queue<DagNode>(Children);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (!visited.Add(current.Id)) continue;
                if (current is T typed) result.Add(typed);
                foreach (var child in current.Children)
                    queue.Enqueue(child);
            }
            return result.AsReadOnly();
        }

        protected bool IsAncestor(DagNode target)
        {
            var visited = new HashSet<int>();
            var queue = new Queue<DagNode>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (!visited.Add(current.Id)) continue;
                if (current.Id == target.Id) return true;
                foreach (var parent in current.Parents)
                    queue.Enqueue(parent);
            }
            return false;
        }

        protected static int CalculateDepth(DagNode node, HashSet<int> visited)
        {
            if (!visited.Add(node.Id))
                throw new InvalidOperationException($"循環参照を検出しました。ノードId: {node.Id}");
            if (!node.ParentEdges.Any()) return 0;

            return node.Parents
                .Max(p => CalculateDepth(p, [.. visited])) + 1;
        }
    }

    /// <summary>
    /// DAGとノードの所属関係を表す中間テーブル。
    /// ツリー固有のメタ情報（表示順など）もここに持たせられる。
    /// </summary>
    public class DagMembership : IEntity
    {
        public int Id { get; set; }
        public int DagId { get; set; }
        public Dag Dag { get; set; } = null!;
        public int NodeId { get; set; }
        public DagNode Node { get; set; } = null!;

        // ツリーごとに異なる可能性のある情報はここに置く
        // 例: そのツリーでのノードの有効/無効フラグ
        public bool IsEnabled { get; set; } = true;
    }
    /// <summary>
    /// Dag上の繋がりを表す
    /// </summary>
    public class DagEdge : IEntity
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public DagNode Parent { get; set; } = null!;
        public int ChildId { get; set; }
        public DagNode Child { get; set; } = null!;

        // どのDAGにおけるエッジか（nullなら全DAG共通）
        public int? DagId { get; set; }
        public Dag? Dag { get; set; }
    }
}