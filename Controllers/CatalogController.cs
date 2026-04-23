using MyApi.Models;

namespace MyApi.Controllers
{
    public class ConsumableItemController(StaffDbContext db) : CrudController<ConsumableItem>(db) { }
    public class EquipmentItemController(StaffDbContext db) : CrudController<EquipmentItem>(db) { }
    public class PlayerController(StaffDbContext db) : CrudController<Player>(db) { }
    public class EnemyController(StaffDbContext db) : CrudController<Enemy>(db) { }
    public class MaterialController(StaffDbContext db) : CrudController<Material>(db) { }
    public class TitleController(StaffDbContext db) : CrudController<Title>(db) { }
}