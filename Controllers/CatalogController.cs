using MyApi.Models;

namespace MyApi.Controllers
{
    public class CatalogController(StaffDbContext db) : CrudController<Catalog>(db) { }

    public class CatalogCategoryController(StaffDbContext db) : CrudController<CatalogCategory>(db) { }

    public class CatalogSeriesController(StaffDbContext db) : CrudController<CatalogSeries>(db) { }

    public class ConsumableItemController(StaffDbContext db) : CrudController<ConsumableItem>(db) { }

    public class EquipmentItemController(StaffDbContext db) : CrudController<EquipmentItem>(db) { }

    public class ExpTableController(StaffDbContext db) : CrudController<ExpTable>(db) { }

    public class PlayerController(StaffDbContext db) : CrudController<Player>(db) { }

    public class EnemyController(StaffDbContext db) : CrudController<Enemy>(db) { }

    public class DropTableController(StaffDbContext db) : CrudController<DropTable>(db) { }

    public class DropItemController(StaffDbContext db) : CrudController<DropItem>(db) { }

    public class VirtualCurrencyController(StaffDbContext db) : CrudController<VirtualCurrency>(db) { }

}