// MasterDataCache.cs
//Singletonパターンでマスターデータをキャッシュするクラス
using System.Collections.Frozen;

public class MasterDataCache
{
    private readonly Dictionary<Type, object> _store = new();

    // Seed時に登録
    internal void Register<T>(FrozenDictionary<int, T> dict) where T : class
        => _store[typeof(T)] = dict;

    // ID指定で1件取得
    public T? Get<T>(int id) where T : class
    {
        var dict = GetDict<T>();
        return dict.TryGetValue(id, out var item) ? item : null;
    }

    // 全件取得
    public IReadOnlyCollection<T> GetAll<T>() where T : class
        => GetDict<T>().Values;

    private FrozenDictionary<int, T> GetDict<T>() where T : class
    {
        if (_store.TryGetValue(typeof(T), out var obj) && obj is FrozenDictionary<int, T> dict)
            return dict;
        throw new InvalidOperationException($"{typeof(T).Name} はキャッシュに登録されていません");
    }
}
