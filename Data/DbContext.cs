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
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<Lottery> Lotteries => Set<Lottery>();
        public DbSet<LotteryRarity> LotteryRarities => Set<LotteryRarity>();
        public DbSet<LotteryPrize> LotteryPrizes => Set<LotteryPrize>();
        public DbSet<AssetVersion> AssetVersions => Set<AssetVersion>();
        public DbSet<AppVersion> AppVersions => Set<AppVersion>();
        public DbSet<CatalogVersion> CatalogVersions => Set<CatalogVersion>();
        public DbSet<AddressableAsset> AddressableAssets => Set<AddressableAsset>();
        public DbSet<UpdateVersion> UpdateVersions => Set<UpdateVersion>();
        public DbSet<SkillTree> SkillTrees => Set<SkillTree>();
        public DbSet<SkillNode> SkillNodes => Set<SkillNode>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<Title> Titles => Set<Title>();
        public DbSet<DailyMission> DailyMission => Set<DailyMission>();
        public DbSet<LoginBonus> LoginBonus => Set<LoginBonus>();
        public DbSet<Achievement> Achievements => Set<Achievement>();
        public DbSet<ShopItem> ShopItems => Set<ShopItem>();
        public DbSet<ShopItem> ShopCategories => Set<ShopItem>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);

            var dataProperty = modelBuilder.Entity<EligibilityCondition>().Property(e => e.Data);

            if (Database.IsNpgsql()) // PostgreSQLの場合
            {
                dataProperty
                    .HasColumnType("jsonb")
                    .HasDefaultValueSql("'{}'::jsonb");
            }
            else // SQLiteなどの場合
            {
                // SQLiteはJSONを単なる文字列(TEXT)として扱う
                dataProperty
                    .HasColumnType("TEXT")
                    .HasDefaultValueSql("'{}'"); // キャスト(::jsonb)を消す
            }
        }
    }
}