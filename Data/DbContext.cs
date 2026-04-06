using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    public class ApiDbContext(DbContextOptions<ApiDbContext> options) : BaseDbContext(options)
    {
    }
    public class StaffDbContext(DbContextOptions<StaffDbContext> options) : BaseDbContext(options)
    {
    }
    // データベース接続クラス（Djangoのsettings.py + Managerに相当）
    public abstract partial class BaseDbContext(DbContextOptions options) : DbContext(options)
    {
        // 各エンティティの登録
        public DbSet<CatalogCategory> Categories => Set<CatalogCategory>();
        public DbSet<CatalogSeries> Series => Set<CatalogSeries>();
        public DbSet<Catalog> Catalogs => Set<Catalog>();
        public DbSet<ConsumableItem> ConsumableItems => Set<ConsumableItem>();
        public DbSet<EquipmentItem> EquipmentItems => Set<EquipmentItem>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<ExpTable> ExpTables => Set<ExpTable>();
        public DbSet<Enemy> Enemies => Set<Enemy>();
        public DbSet<DropTable> DropTables => Set<DropTable>();
        public DbSet<DropItem> DropTableItems => Set<DropItem>();
        public DbSet<VirtualCurrency> VirtualCurrencies => Set<VirtualCurrency>();
        public DbSet<Lottery> Lotteries => Set<Lottery>();
        public DbSet<LotteryRarity> LotteryRarities => Set<LotteryRarity>();
        public DbSet<LotteryPrize> LotteryPrizes => Set<LotteryPrize>();
        public DbSet<AssetVersion> AssetVersions => Set<AssetVersion>();
        public DbSet<AppVersion> AppVersions => Set<AppVersion>();
        public DbSet<CatalogVersion> CatalogVersions => Set<CatalogVersion>();
        public DbSet<AddressableAsset> AddressableAssets => Set<AddressableAsset>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = typeof(BaseDbContext).Assembly;

            // 抽象・ジェネリッククラスを除外して適用
            var configTypes = assembly.GetTypes()
                .Where(t =>
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.IsGenericTypeDefinition && // ← これが重要
                    t.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                    )
                );

            foreach (var type in configTypes)
            {
                dynamic config = Activator.CreateInstance(type)!;
                modelBuilder.ApplyConfiguration(config);
            }
        }
    }
}