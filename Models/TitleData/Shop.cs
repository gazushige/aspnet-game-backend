namespace MyApi.Models
{
    /// <summary>
    /// ショップのマスター定義
    /// </summary>
    public class Shop : IEntity, IHasExpiry
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // 武器屋、期間限定交換所など

        // PlayFabのStoreIdと紐付ける（例："WeaponShop", "EventSwap_01"）
        // これをキーにPlayFabから「現在の価格リスト」を取得する
        public string PlayFabStoreId { get; set; } = string.Empty;


        // 使用する主な通貨コード（PlayFabのVirtual Currency ID: "GD", "CN"など）
        // ショップ全体で統一されている場合に便利
        public string DefaultCurrencyCode { get; set; } = "GD";

        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? ExpiredAt { get; set; }

        public ICollection<ShopItem> Items { get; set; } = new List<ShopItem>();
    }

    public class ShopItem : IEntity
    {
        public int Id { get; set; }

        // アイテム（中身）の識別子。これ自体は「価値」を知らない
        public string PlayFabItemId { get; set; } = null!;

        // 「いくらで売るか」はPlayFabのStore側に持たせるのが前提ですが、
        // もしDB側で管理したくなった場合でも、ここに Price を持たせれば
        // アイテム定義（薬草）を汚さずに済みます。

        // 陳列時のメタデータ（ショップが変わればこれらも変わる）
        public int Quantity { get; set; } = 1; // 1個売りか、10個セットか
        public int DisplayOrder { get; set; }
        public bool IsRecommended { get; set; } // この店でのイチオシか
    }
}