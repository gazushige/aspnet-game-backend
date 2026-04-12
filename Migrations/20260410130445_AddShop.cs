using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations
{
    /// <inheritdoc />
    public partial class AddShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lotteries_VirtualCurrencies_SingleCostCurrencyId",
                table: "Lotteries");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotteries_VirtualCurrencies_TenCostCurrencyId",
                table: "Lotteries");

            migrationBuilder.DropForeignKey(
                name: "FK_VirtualCurrencies_catalogs_CatalogId",
                table: "VirtualCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_catalogs_KeyCode",
                table: "catalogs");

            migrationBuilder.DropIndex(
                name: "UQ_CatalogVersion_SemVer",
                table: "catalog_versions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VirtualCurrencies",
                table: "VirtualCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_VirtualCurrencies_CatalogId",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "RealCurrencyPrices",
                table: "equipment_items");

            migrationBuilder.DropColumn(
                name: "VirtualCurrencyPrices",
                table: "equipment_items");

            migrationBuilder.DropColumn(
                name: "RealCurrencyPrices",
                table: "consumable_items");

            migrationBuilder.DropColumn(
                name: "VirtualCurrencyPrices",
                table: "consumable_items");

            migrationBuilder.DropColumn(
                name: "KeyCode",
                table: "catalogs");

            migrationBuilder.DropColumn(
                name: "PlayfabCatalogVersion",
                table: "catalog_versions");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "CatalogUuid",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "IsCurrentVersion",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "IsStackable",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "IsTradable",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "MinQuantity",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "Revision",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "VirtualCurrencies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VirtualCurrencies");

            migrationBuilder.RenameTable(
                name: "VirtualCurrencies",
                newName: "virtual_currencies");

            migrationBuilder.RenameColumn(
                name: "MinQuantity",
                table: "Materials",
                newName: "IsLimitedEdition");

            migrationBuilder.AddColumn<int>(
                name: "InitialLimitedEditionCount",
                table: "Materials",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "ForceUpdate",
                table: "app_versions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "MaxQuantity",
                table: "virtual_currencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 2000000000,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "virtual_currencies",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxCapacity",
                table: "virtual_currencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "virtual_currencies",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RechargeRate",
                table: "virtual_currencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_virtual_currencies",
                table: "virtual_currencies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "eligibility_conditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Data = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eligibility_conditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "objective_criteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Criteria = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    TrackingMode = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objective_criteria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayFabItemId = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRecommended = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "update_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppVersionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetVersionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_update_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_update_versions_app_versions_AppVersionId",
                        column: x => x.AppVersionId,
                        principalTable: "app_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_update_versions_asset_versions_AssetVersionId",
                        column: x => x.AssetVersionId,
                        principalTable: "asset_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reward_definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    reward_type = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Achievement_ObjectiveCriteriaId = table.Column<int>(type: "INTEGER", nullable: true),
                    Achievement_EligibilityConditionId = table.Column<int>(type: "INTEGER", nullable: true),
                    ObjectiveCriteriaId = table.Column<int>(type: "INTEGER", nullable: true),
                    DailyMission_EligibilityConditionId = table.Column<int>(type: "INTEGER", nullable: true),
                    RequiredConsecutiveDays = table.Column<int>(type: "INTEGER", nullable: true),
                    EligibilityConditionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reward_definitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reward_definitions_eligibility_conditions_Achievement_EligibilityConditionId",
                        column: x => x.Achievement_EligibilityConditionId,
                        principalTable: "eligibility_conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reward_definitions_eligibility_conditions_DailyMission_EligibilityConditionId",
                        column: x => x.DailyMission_EligibilityConditionId,
                        principalTable: "eligibility_conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reward_definitions_eligibility_conditions_EligibilityConditionId",
                        column: x => x.EligibilityConditionId,
                        principalTable: "eligibility_conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reward_definitions_objective_criteria_Achievement_ObjectiveCriteriaId",
                        column: x => x.Achievement_ObjectiveCriteriaId,
                        principalTable: "objective_criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reward_definitions_objective_criteria_ObjectiveCriteriaId",
                        column: x => x.ObjectiveCriteriaId,
                        principalTable: "objective_criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "achievement_tiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    RequiredProgress = table.Column<int>(type: "INTEGER", nullable: false),
                    AchievementId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievement_tiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_achievement_tiers_reward_definitions_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "reward_definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bundled_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    ItemType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    AchievementTierId = table.Column<int>(type: "INTEGER", nullable: true),
                    RewardDefinitionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bundled_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bundled_items_achievement_tiers_AchievementTierId",
                        column: x => x.AchievementTierId,
                        principalTable: "achievement_tiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bundled_items_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bundled_items_reward_definitions_RewardDefinitionId",
                        column: x => x.RewardDefinitionId,
                        principalTable: "reward_definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_CatalogVersion_SemVer",
                table: "catalog_versions",
                columns: new[] { "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_AssetVersion_SemVer",
                table: "asset_versions",
                columns: new[] { "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_AppVersion_Platform_SemVer",
                table: "app_versions",
                columns: new[] { "Platform", "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_virtual_currencies_Code",
                table: "virtual_currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_achievement_tiers_AchievementId",
                table: "achievement_tiers",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_bundled_items_AchievementTierId",
                table: "bundled_items",
                column: "AchievementTierId");

            migrationBuilder.CreateIndex(
                name: "IX_bundled_items_CatalogId",
                table: "bundled_items",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_bundled_items_RewardDefinitionId",
                table: "bundled_items",
                column: "RewardDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_reward_definitions_Achievement_EligibilityConditionId",
                table: "reward_definitions",
                column: "Achievement_EligibilityConditionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reward_definitions_Achievement_ObjectiveCriteriaId",
                table: "reward_definitions",
                column: "Achievement_ObjectiveCriteriaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reward_definitions_DailyMission_EligibilityConditionId",
                table: "reward_definitions",
                column: "DailyMission_EligibilityConditionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reward_definitions_EligibilityConditionId",
                table: "reward_definitions",
                column: "EligibilityConditionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reward_definitions_ObjectiveCriteriaId",
                table: "reward_definitions",
                column: "ObjectiveCriteriaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_update_versions_AssetVersionId",
                table: "update_versions",
                column: "AssetVersionId");

            migrationBuilder.CreateIndex(
                name: "UQ_UpdateVersion_App_Asset",
                table: "update_versions",
                columns: new[] { "AppVersionId", "AssetVersionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotteries_virtual_currencies_SingleCostCurrencyId",
                table: "Lotteries",
                column: "SingleCostCurrencyId",
                principalTable: "virtual_currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotteries_virtual_currencies_TenCostCurrencyId",
                table: "Lotteries",
                column: "TenCostCurrencyId",
                principalTable: "virtual_currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lotteries_virtual_currencies_SingleCostCurrencyId",
                table: "Lotteries");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotteries_virtual_currencies_TenCostCurrencyId",
                table: "Lotteries");

            migrationBuilder.DropTable(
                name: "bundled_items");

            migrationBuilder.DropTable(
                name: "ShopItem");

            migrationBuilder.DropTable(
                name: "update_versions");

            migrationBuilder.DropTable(
                name: "achievement_tiers");

            migrationBuilder.DropTable(
                name: "reward_definitions");

            migrationBuilder.DropTable(
                name: "eligibility_conditions");

            migrationBuilder.DropTable(
                name: "objective_criteria");

            migrationBuilder.DropIndex(
                name: "UQ_CatalogVersion_SemVer",
                table: "catalog_versions");

            migrationBuilder.DropIndex(
                name: "UQ_AssetVersion_SemVer",
                table: "asset_versions");

            migrationBuilder.DropIndex(
                name: "UQ_AppVersion_Platform_SemVer",
                table: "app_versions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_virtual_currencies",
                table: "virtual_currencies");

            migrationBuilder.DropIndex(
                name: "IX_virtual_currencies_Code",
                table: "virtual_currencies");

            migrationBuilder.DropColumn(
                name: "InitialLimitedEditionCount",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "virtual_currencies");

            migrationBuilder.DropColumn(
                name: "MaxCapacity",
                table: "virtual_currencies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "virtual_currencies");

            migrationBuilder.DropColumn(
                name: "RechargeRate",
                table: "virtual_currencies");

            migrationBuilder.RenameTable(
                name: "virtual_currencies",
                newName: "VirtualCurrencies");

            migrationBuilder.RenameColumn(
                name: "IsLimitedEdition",
                table: "Materials",
                newName: "MinQuantity");

            migrationBuilder.AddColumn<string>(
                name: "RealCurrencyPrices",
                table: "equipment_items",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VirtualCurrencyPrices",
                table: "equipment_items",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealCurrencyPrices",
                table: "consumable_items",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VirtualCurrencyPrices",
                table: "consumable_items",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyCode",
                table: "catalogs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlayfabCatalogVersion",
                table: "catalog_versions",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "ForceUpdate",
                table: "app_versions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "MaxQuantity",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 2000000000);

            migrationBuilder.AddColumn<int>(
                name: "CatalogId",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CatalogUuid",
                table: "VirtualCurrencies",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VirtualCurrencies",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VirtualCurrencies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "VirtualCurrencies",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentVersion",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStackable",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTradable",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "VirtualCurrencies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinQuantity",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Revision",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VirtualCurrencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "VirtualCurrencies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VirtualCurrencies",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_VirtualCurrencies",
                table: "VirtualCurrencies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_catalogs_KeyCode",
                table: "catalogs",
                column: "KeyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CatalogVersion_SemVer",
                table: "catalog_versions",
                columns: new[] { "PlayfabCatalogVersion", "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VirtualCurrencies_CatalogId",
                table: "VirtualCurrencies",
                column: "CatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lotteries_VirtualCurrencies_SingleCostCurrencyId",
                table: "Lotteries",
                column: "SingleCostCurrencyId",
                principalTable: "VirtualCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotteries_VirtualCurrencies_TenCostCurrencyId",
                table: "Lotteries",
                column: "TenCostCurrencyId",
                principalTable: "VirtualCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualCurrencies_catalogs_CatalogId",
                table: "VirtualCurrencies",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
