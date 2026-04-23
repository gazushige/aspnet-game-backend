namespace MyApi.Models
{
    // =========================================================
    // 共通基底クラス
    // =========================================================

    /// <summary>
    /// 報酬定義の基底クラス。ユーザー状態は持たない。
    /// </summary>
    public abstract class RewardDefinition : IEntity, IHasExpiry
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? ExpiredAt { get; set; }
        public ICollection<BundledItem> BundledItems { get; set; } = new List<BundledItem>();
    }

    // =========================================================
    // BundledItem
    // =========================================================

    /// <summary>
    /// 報酬として配布するアイテム1種の定義。
    /// CatalogはPlayFabのCatalog Item IDへの参照を想定。
    /// </summary>
    public class BundledItem : IEntity
    {
        public int Id { get; set; }
        public Guid CatalogId { get; set; }
        public CatalogCategory Category { get; set; } = CatalogCategory.OTHER;
        public int Quantity { get; set; }
    }


    // =========================================================
    // 条件定義（JSONB）
    // =========================================================

    /// <summary>
    /// 取得資格条件の定義。
    /// 例: {"min_level": 10, "vip_only": true}
    /// PlayFab側のPlayer Dataと照合する前提。
    /// </summary>
    public class EligibilityCondition : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Data { get; set; } = "{}"; // JSONB
    }

    /// <summary>
    /// ミッション・アチーブメントの達成目標定義。
    /// 例: {"battle_win": 5, "earn_coin": 100}
    /// 進捗値の追跡はPlayFab側が担う。
    /// </summary>
    public class ObjectiveCriteria : IEntity
    {
        public int Id { get; set; }
        public string Criteria { get; set; } = "{}"; // JSONB
        public ProgressTrackingMode TrackingMode { get; set; } = ProgressTrackingMode.Cumulative;
    }

    public enum ProgressTrackingMode
    {
        Cumulative,     // 累積カウント（例：総バトル勝利数）
        SingleSession,  // リセット前提（例：本日の勝利数）
    }

    // =========================================================
    // インターフェース
    // =========================================================

    public interface IHasEligibility
    {
        EligibilityCondition? EligibilityCondition { get; }
    }

    public interface IHasBundleItems
    {
        IReadOnlyCollection<BundledItem> BundledItems { get; }
    }

    // =========================================================
    // 各報酬定義エンティティ
    // =========================================================

    /// <summary>
    /// ログインボーナス定義。
    /// 連続ログイン日数ごとに異なる報酬を出す場合は
    /// LoginBonusStep（別エンティティ）で管理する想定。
    /// </summary>
    public class LoginBonus : RewardDefinition, IHasEligibility
    {
        /// <summary>
        /// 連続ログイン何日目の報酬か。
        /// nullは日数条件なし（毎日同じ報酬）。
        /// </summary>
        public int? RequiredConsecutiveDays { get; set; }
        public EligibilityCondition? EligibilityCondition { get; set; }
    }

    /// <summary>
    /// デイリーミッション定義。
    /// 達成判定・受取状態はPlayFab側が管理。
    /// </summary>
    public class DailyMission : RewardDefinition, IHasEligibility
    {
        /// <summary>達成すべき目標の定義（PlayFabの統計値・イベントと紐づく）</summary>
        public ObjectiveCriteria Objective { get; set; } = null!;
        public EligibilityCondition? EligibilityCondition { get; set; }
    }

    /// <summary>
    /// アチーブメント定義。
    /// デイリーミッションと異なり永続的・累積的な目標を想定。
    /// 段階報酬がある場合はTiersで定義。
    /// </summary>
    public class Achievement : RewardDefinition, IHasEligibility
    {
        public ObjectiveCriteria Objective { get; set; } = null!;
        public ICollection<AchievementTier>? Tiers { get; set; }
        public EligibilityCondition? EligibilityCondition { get; set; }
    }

    /// <summary>
    /// アチーブメントの段階報酬定義。
    /// 例: 10勝→ブロンズ, 50勝→シルバー, 100勝→ゴールド
    /// RequiredProgressの値をPlayFabの統計値と比較して達成判定する。
    /// </summary>
    public class AchievementTier : IEntity
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public int RequiredProgress { get; set; }
        public ICollection<BundledItem> BundledItems { get; set; } = new List<BundledItem>();
    }

    /// <summary>
    /// 配布アイテム定義。
    /// 例: イベント報酬やギフトなど、ユーザーにアイテ
    /// </summary> 
    public class DistributedItems : RewardDefinition, IHasEligibility
    {
        public EligibilityCondition? EligibilityCondition { get; set; }
    }
}