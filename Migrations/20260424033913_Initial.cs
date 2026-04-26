using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addressable_assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Label = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Path = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    PublicUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addressable_assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    ExecutedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Requirement = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "app_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    BuildNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ForceUpdate = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    StoreUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Major = table.Column<int>(type: "INTEGER", nullable: false),
                    Minor = table.Column<int>(type: "INTEGER", nullable: false),
                    Patch = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "asset_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Major = table.Column<int>(type: "INTEGER", nullable: false),
                    Minor = table.Column<int>(type: "INTEGER", nullable: false),
                    Patch = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_prefixes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    Prefix = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_prefixes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SnapShot = table.Column<string>(type: "TEXT", nullable: true),
                    Major = table.Column<int>(type: "INTEGER", nullable: false),
                    Minor = table.Column<int>(type: "INTEGER", nullable: false),
                    Patch = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_versions", x => x.Id);
                    table.CheckConstraint("CK_version_major_positive", "\"Major\" >= 0");
                    table.CheckConstraint("CK_version_minor_positive", "\"Minor\" >= 0");
                    table.CheckConstraint("CK_version_patch_positive", "\"Patch\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Dags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DagType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "drop_tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KeyCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drop_tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eligibility_conditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eligibility_conditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exp_tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MinLevel = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    MaxLevel = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 99),
                    Logic = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exp_tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "objective_criteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Criteria = table.Column<string>(type: "TEXT", nullable: false),
                    TrackingMode = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objective_criteria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PlayFabStoreId = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultCurrencyCode = table.Column<string>(type: "TEXT", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "virtual_currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CurrencyType = table.Column<int>(type: "INTEGER", nullable: false),
                    RechargeRate = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    MaxCapacity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    MaxQuantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 2000000000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "world_progress_state",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    current_phase = table.Column<int>(type: "INTEGER", nullable: false),
                    phase_started_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    avg_player_level = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                    total_boss_kill_count = table.Column<long>(type: "INTEGER", nullable: false),
                    active_player_count = table.Column<int>(type: "INTEGER", nullable: false),
                    last_aggregated_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_world_progress_state", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vip_masters",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    level = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    required_point = table.Column<long>(type: "INTEGER", nullable: false),
                    point_multiplier = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                    sort_order = table.Column<int>(type: "INTEGER", nullable: false),
                    icon_asset_id = table.Column<int>(type: "INTEGER", nullable: true),
                    requirement = table.Column<string>(type: "TEXT", nullable: true),
                    custom_data = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vip_masters", x => x.id);
                    table.ForeignKey(
                        name: "FK_vip_masters_addressable_assets_icon_asset_id",
                        column: x => x.icon_asset_id,
                        principalTable: "addressable_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "world_phase_masters",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    phase = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    requirement = table.Column<string>(type: "TEXT", nullable: true),
                    unlock_after = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    force_progress_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    custom_data = table.Column<string>(type: "TEXT", nullable: true),
                    banner_asset_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_world_phase_masters", x => x.id);
                    table.ForeignKey(
                        name: "FK_world_phase_masters_addressable_assets_banner_asset_id",
                        column: x => x.banner_asset_id,
                        principalTable: "addressable_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "asset_version_assets",
                columns: table => new
                {
                    AssetVersionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AddressableAssetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_version_assets", x => new { x.AssetVersionId, x.AddressableAssetId });
                    table.ForeignKey(
                        name: "FK_asset_version_assets_addressable_assets_AddressableAssetId",
                        column: x => x.AddressableAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_asset_version_assets_asset_versions_AssetVersionId",
                        column: x => x.AssetVersionId,
                        principalTable: "asset_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "update_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppVersionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetVersionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                name: "consumable_items",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsageCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    UsagePeriodSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    Rarity = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    MaxStack = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 99),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    IsConsumableByTime = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsConsumableByCount = table.Column<bool>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsTradable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsLimitedEdition = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumable_items", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_consumable_items_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_consumable_items_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipment_items",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rarity = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsStackable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsTradable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsLimitedEdition = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_items", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_equipment_items_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_equipment_items_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    MaxQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    IsTradable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLimitedEdition = table.Column<bool>(type: "INTEGER", nullable: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Materials_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    Requirement = table.Column<string>(type: "TEXT", nullable: true),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Titles_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Titles_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drop_items",
                columns: table => new
                {
                    DropTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogCategory = table.Column<int>(type: "INTEGER", nullable: false),
                    MinQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drop_items", x => new { x.DropTableId, x.CatalogUuid });
                    table.CheckConstraint("CK_DropTableItem_PositiveWeight", "\"Weight\" > 0");
                    table.CheckConstraint("CK_DropTableItem_QuantityRange", "\"MinQuantity\" <= \"MaxQuantity\"");
                    table.ForeignKey(
                        name: "FK_drop_items_drop_tables_DropTableId",
                        column: x => x.DropTableId,
                        principalTable: "drop_tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enemies",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    DropTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemies", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Enemies_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Enemies_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enemies_drop_tables_DropTableId",
                        column: x => x.DropTableId,
                        principalTable: "drop_tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Players_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_exp_tables_ExpTableId",
                        column: x => x.ExpTableId,
                        principalTable: "exp_tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    DistributedItems_EligibilityConditionId = table.Column<int>(type: "INTEGER", nullable: true),
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
                        name: "FK_reward_definitions_eligibility_conditions_DistributedItems_EligibilityConditionId",
                        column: x => x.DistributedItems_EligibilityConditionId,
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
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayFabItemId = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRecommended = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShopId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItems_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DagNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NodeType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: true),
                    Cost = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DagNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DagNodes_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lotteries",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    PityNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    PityType = table.Column<int>(type: "INTEGER", nullable: false),
                    SingleCostCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SingleCostAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    TenCostCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    TenCostAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotteries", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Lotteries_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lotteries_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotteries_virtual_currencies_SingleCostCurrencyId",
                        column: x => x.SingleCostCurrencyId,
                        principalTable: "virtual_currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotteries_virtual_currencies_TenCostCurrencyId",
                        column: x => x.TenCostCurrencyId,
                        principalTable: "virtual_currencies",
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
                name: "DagEdges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false),
                    DagId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DagEdges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DagEdges_DagNodes_ChildId",
                        column: x => x.ChildId,
                        principalTable: "DagNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DagEdges_DagNodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DagNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DagEdges_Dags_DagId",
                        column: x => x.DagId,
                        principalTable: "Dags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DagMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DagId = table.Column<int>(type: "INTEGER", nullable: false),
                    NodeId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DagMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DagMemberships_DagNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "DagNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DagMemberships_Dags_DagId",
                        column: x => x.DagId,
                        principalTable: "Dags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotteryPrizes",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    LotteryId = table.Column<int>(type: "INTEGER", nullable: false),
                    LotteryUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PrizeCatalogId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PrizeCategory = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPickup = table.Column<bool>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryPrizes", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_LotteryPrizes_Lotteries_LotteryUuid",
                        column: x => x.LotteryUuid,
                        principalTable: "Lotteries",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotteryPrizes_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LotteryPrizes_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotteryRarities",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    LotteryId = table.Column<int>(type: "INTEGER", nullable: false),
                    LotteryUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PrefixId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInPrefix = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IconAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryRarities", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_LotteryRarities_Lotteries_LotteryUuid",
                        column: x => x.LotteryUuid,
                        principalTable: "Lotteries",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotteryRarities_addressable_assets_IconAssetId",
                        column: x => x.IconAssetId,
                        principalTable: "addressable_assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LotteryRarities_catalog_prefixes_PrefixId",
                        column: x => x.PrefixId,
                        principalTable: "catalog_prefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bundled_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CatalogId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
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
                        name: "FK_bundled_items_reward_definitions_RewardDefinitionId",
                        column: x => x.RewardDefinitionId,
                        principalTable: "reward_definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_achievement_tiers_AchievementId",
                table: "achievement_tiers",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressableAsset_Label",
                table: "addressable_assets",
                column: "Label");

            migrationBuilder.CreateIndex(
                name: "UQ_AddressableAsset_Path",
                table: "addressable_assets",
                column: "Path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_AppVersion_Platform_SemVer",
                table: "app_versions",
                columns: new[] { "Platform", "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asset_version_assets_AddressableAssetId",
                table: "asset_version_assets",
                column: "AddressableAssetId");

            migrationBuilder.CreateIndex(
                name: "UQ_AssetVersion_SemVer",
                table: "asset_versions",
                columns: new[] { "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bundled_items_AchievementTierId",
                table: "bundled_items",
                column: "AchievementTierId");

            migrationBuilder.CreateIndex(
                name: "IX_bundled_items_RewardDefinitionId",
                table: "bundled_items",
                column: "RewardDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_prefixes_Category",
                table: "catalog_prefixes",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_prefixes_Prefix",
                table: "catalog_prefixes",
                column: "Prefix",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CatalogPrefix_Category_Prefix",
                table: "catalog_prefixes",
                columns: new[] { "CategoryId", "Prefix" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CatalogVersion_SemVer",
                table: "catalog_versions",
                columns: new[] { "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_IconAssetId",
                table: "consumable_items",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_IsCurrentVersion",
                table: "consumable_items",
                column: "IsCurrentVersion");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_PrefixId",
                table: "consumable_items",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_Status",
                table: "consumable_items",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_Consumable_Catalog_Revision",
                table: "consumable_items",
                columns: new[] { "Uuid", "Revision" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Consumable_CurrentVersion",
                table: "consumable_items",
                column: "Uuid",
                unique: true,
                filter: "\"IsCurrentVersion\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_DagEdges_ChildId",
                table: "DagEdges",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_DagEdges_DagId",
                table: "DagEdges",
                column: "DagId");

            migrationBuilder.CreateIndex(
                name: "IX_DagEdges_ParentId_ChildId_DagId",
                table: "DagEdges",
                columns: new[] { "ParentId", "ChildId", "DagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DagMemberships_DagId_NodeId",
                table: "DagMemberships",
                columns: new[] { "DagId", "NodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DagMemberships_NodeId",
                table: "DagMemberships",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_DagNodes_SkillId",
                table: "DagNodes",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_drop_items_CatalogUuid",
                table: "drop_items",
                column: "CatalogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_drop_tables_KeyCode",
                table: "drop_tables",
                column: "KeyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_DropTableId",
                table: "Enemies",
                column: "DropTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_IconAssetId",
                table: "Enemies",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_PrefixId",
                table: "Enemies",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_IconAssetId",
                table: "equipment_items",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_IsCurrentVersion",
                table: "equipment_items",
                column: "IsCurrentVersion");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_PrefixId",
                table: "equipment_items",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_Status",
                table: "equipment_items",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_Equipment_Catalog_Revision",
                table: "equipment_items",
                columns: new[] { "Uuid", "Revision" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Equipment_CurrentVersion",
                table: "equipment_items",
                column: "Uuid",
                unique: true,
                filter: "\"IsCurrentVersion\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_IconAssetId",
                table: "Lotteries",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_PrefixId",
                table: "Lotteries",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_SingleCostCurrencyId",
                table: "Lotteries",
                column: "SingleCostCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_TenCostCurrencyId",
                table: "Lotteries",
                column: "TenCostCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_IconAssetId",
                table: "LotteryPrizes",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_LotteryUuid",
                table: "LotteryPrizes",
                column: "LotteryUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_PrefixId",
                table: "LotteryPrizes",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_IconAssetId",
                table: "LotteryRarities",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_LotteryUuid",
                table: "LotteryRarities",
                column: "LotteryUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_PrefixId",
                table: "LotteryRarities",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_IconAssetId",
                table: "Materials",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_PrefixId",
                table: "Materials",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ExpTableId",
                table: "Players",
                column: "ExpTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_IconAssetId",
                table: "Players",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PrefixId",
                table: "Players",
                column: "PrefixId");

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
                name: "IX_reward_definitions_DistributedItems_EligibilityConditionId",
                table: "reward_definitions",
                column: "DistributedItems_EligibilityConditionId",
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
                name: "IX_ShopItems_ShopId",
                table: "ShopItems",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_IconAssetId",
                table: "Titles",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_PrefixId",
                table: "Titles",
                column: "PrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_update_versions_AssetVersionId",
                table: "update_versions",
                column: "AssetVersionId");

            migrationBuilder.CreateIndex(
                name: "UQ_UpdateVersion_App_Asset",
                table: "update_versions",
                columns: new[] { "AppVersionId", "AssetVersionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vip_masters_icon_asset_id",
                table: "vip_masters",
                column: "icon_asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_vip_masters_level",
                table: "vip_masters",
                column: "level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vip_masters_sort_order",
                table: "vip_masters",
                column: "sort_order");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_currencies_Code",
                table: "virtual_currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_world_phase_masters_banner_asset_id",
                table: "world_phase_masters",
                column: "banner_asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_world_phase_masters_force_progress_at",
                table: "world_phase_masters",
                column: "force_progress_at");

            migrationBuilder.CreateIndex(
                name: "ix_world_phase_masters_phase",
                table: "world_phase_masters",
                column: "phase",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_world_phase_masters_unlock_after",
                table: "world_phase_masters",
                column: "unlock_after");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "asset_version_assets");

            migrationBuilder.DropTable(
                name: "bundled_items");

            migrationBuilder.DropTable(
                name: "catalog_versions");

            migrationBuilder.DropTable(
                name: "consumable_items");

            migrationBuilder.DropTable(
                name: "DagEdges");

            migrationBuilder.DropTable(
                name: "DagMemberships");

            migrationBuilder.DropTable(
                name: "drop_items");

            migrationBuilder.DropTable(
                name: "Enemies");

            migrationBuilder.DropTable(
                name: "equipment_items");

            migrationBuilder.DropTable(
                name: "LotteryPrizes");

            migrationBuilder.DropTable(
                name: "LotteryRarities");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "ShopItems");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "update_versions");

            migrationBuilder.DropTable(
                name: "vip_masters");

            migrationBuilder.DropTable(
                name: "world_phase_masters");

            migrationBuilder.DropTable(
                name: "world_progress_state");

            migrationBuilder.DropTable(
                name: "achievement_tiers");

            migrationBuilder.DropTable(
                name: "DagNodes");

            migrationBuilder.DropTable(
                name: "Dags");

            migrationBuilder.DropTable(
                name: "drop_tables");

            migrationBuilder.DropTable(
                name: "Lotteries");

            migrationBuilder.DropTable(
                name: "exp_tables");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "app_versions");

            migrationBuilder.DropTable(
                name: "asset_versions");

            migrationBuilder.DropTable(
                name: "reward_definitions");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "addressable_assets");

            migrationBuilder.DropTable(
                name: "catalog_prefixes");

            migrationBuilder.DropTable(
                name: "virtual_currencies");

            migrationBuilder.DropTable(
                name: "eligibility_conditions");

            migrationBuilder.DropTable(
                name: "objective_criteria");
        }
    }
}
