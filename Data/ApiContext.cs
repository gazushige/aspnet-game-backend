using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    // データベース接続クラス（Djangoのsettings.py + Managerに相当）
    public partial class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

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
        public DbSet<DropTableItem> DropTableItems => Set<DropTableItem>();
        public DbSet<VirtualCurrency> VirtualCurrencies => Set<VirtualCurrency>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // この一行で、プロジェクト内のすべての IEntityTypeConfiguration を自動適用
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
        }
    }
}