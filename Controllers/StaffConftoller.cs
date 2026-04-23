using MyApi.Models;

namespace MyApi.Controllers
{
    public class CatalogSeriesController(StaffDbContext db) : CrudController<CatalogPrefix>(db) { }
    public class ShopController(StaffDbContext db) : CrudController<Shop>(db) { }
    public class ShopItemController(StaffDbContext db) : CrudController<ShopItem>(db) { }
    public class AddressableAssetController(StaffDbContext db) : CrudController<AddressableAsset>(db) { }
    public class BundledItemController(StaffDbContext db) : CrudController<BundledItem>(db) { }
    public class EligibilityConditionController(StaffDbContext db) : CrudController<EligibilityCondition>(db) { }
    public class ObjectiveCriteriaController(StaffDbContext db) : CrudController<ObjectiveCriteria>(db) { }
    public class LoginBonusController(StaffDbContext db) : CrudController<LoginBonus>(db) { }
    public class DailyMissionController(StaffDbContext db) : CrudController<DailyMission>(db) { }
    public class AchievementController(StaffDbContext db) : CrudController<Achievement>(db) { }
    public class AchievementTierController(StaffDbContext db) : CrudController<AchievementTier>(db) { }
    public class DropTableController(StaffDbContext db) : CrudController<DropTable>(db) { }
    public class DropItemController(StaffDbContext db) : CrudController<DropItem>(db) { }
    public class ExpTableController(StaffDbContext db) : CrudController<ExpTable>(db) { }
    public class LotteryController(StaffDbContext db) : CrudController<Lottery>(db) { }
    public class LotteryPrizeController(StaffDbContext db) : CrudController<LotteryPrize>(db) { }
    public class LotteryRarityController(StaffDbContext db) : CrudController<LotteryRarity>(db) { }
    public class CatalogVersionController(StaffDbContext db) : CrudController<CatalogVersion>(db) { }
    public class AppVersionController(StaffDbContext db) : CrudController<AppVersion>(db) { }
    public class AssetVersionController(StaffDbContext db) : CrudController<AssetVersion>(db) { }
    public class UpdateVersionController(StaffDbContext db) : CrudController<UpdateVersion>(db) { }
    public class VipMasterController(StaffDbContext db) : CrudController<VipMaster>(db) { }
    public class VirtualCurrencyController(StaffDbContext db) : CrudController<VirtualCurrency>(db) { }
    public class WorldPhaseMasterController(StaffDbContext db) : CrudController<WorldPhaseMaster>(db) { }
    public class WorldProgressStateController(StaffDbContext db) : CrudController<WorldProgressState>(db) { }


}

