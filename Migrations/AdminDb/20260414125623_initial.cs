using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rest.Migrations.AdminDb
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addressable_assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PublicUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addressable_assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "app_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Platform = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BuildNumber = table.Column<int>(type: "integer", nullable: false),
                    ForceUpdate = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    StoreUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Major = table.Column<int>(type: "integer", nullable: false),
                    Minor = table.Column<int>(type: "integer", nullable: false),
                    Patch = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "asset_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Major = table.Column<int>(type: "integer", nullable: false),
                    Minor = table.Column<int>(type: "integer", nullable: false),
                    Patch = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SnapShot = table.Column<string>(type: "jsonb", nullable: true),
                    Major = table.Column<int>(type: "integer", nullable: false),
                    Minor = table.Column<int>(type: "integer", nullable: false),
                    Patch = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DagType = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "drop_tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KeyCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drop_tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eligibility_conditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Data = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eligibility_conditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exp_tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MinLevel = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    MaxLevel = table.Column<int>(type: "integer", nullable: false, defaultValue: 99),
                    Logic = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exp_tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "objective_criteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Criteria = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    TrackingMode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objective_criteria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayFabItemId = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsRecommended = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CustomData = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "virtual_currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CurrencyType = table.Column<int>(type: "integer", nullable: false),
                    RechargeRate = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MaxCapacity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MaxQuantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 2000000000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "asset_version_assets",
                columns: table => new
                {
                    AssetVersionId = table.Column<int>(type: "integer", nullable: false),
                    AddressableAssetId = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppVersionId = table.Column<int>(type: "integer", nullable: false),
                    AssetVersionId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                name: "catalog_series",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Prefix = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_catalog_series_catalog_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "catalog_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reward_definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    reward_type = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Achievement_ObjectiveCriteriaId = table.Column<int>(type: "integer", nullable: true),
                    Achievement_EligibilityConditionId = table.Column<int>(type: "integer", nullable: true),
                    ObjectiveCriteriaId = table.Column<int>(type: "integer", nullable: true),
                    DailyMission_EligibilityConditionId = table.Column<int>(type: "integer", nullable: true),
                    RequiredConsecutiveDays = table.Column<int>(type: "integer", nullable: true),
                    EligibilityConditionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reward_definitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reward_definitions_eligibility_conditions_Achievement_Eligi~",
                        column: x => x.Achievement_EligibilityConditionId,
                        principalTable: "eligibility_conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reward_definitions_eligibility_conditions_DailyMission_Elig~",
                        column: x => x.DailyMission_EligibilityConditionId,
                        principalTable: "eligibility_conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reward_definitions_eligibility_conditions_EligibilityCondit~",
                        column: x => x.EligibilityConditionId,
                        principalTable: "eligibility_conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reward_definitions_objective_criteria_Achievement_Objective~",
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
                name: "DagNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NodeType = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: true),
                    Cost = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
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
                name: "catalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    SeriesId = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogs", x => x.Id);
                    table.UniqueConstraint("AK_catalogs_Uuid", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_catalogs_catalog_series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "catalog_series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "achievement_tiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RequiredProgress = table.Column<int>(type: "integer", nullable: false),
                    AchievementId = table.Column<int>(type: "integer", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    ChildId = table.Column<int>(type: "integer", nullable: false),
                    DagId = table.Column<int>(type: "integer", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DagId = table.Column<int>(type: "integer", nullable: false),
                    NodeId = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
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
                name: "consumable_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsageCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    UsagePeriodSeconds = table.Column<int>(type: "integer", nullable: true),
                    Rarity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MaxStack = table.Column<int>(type: "integer", nullable: false, defaultValue: 99),
                    CustomData = table.Column<string>(type: "jsonb", nullable: true),
                    IsConsumableByTime = table.Column<bool>(type: "boolean", nullable: false),
                    IsConsumableByCount = table.Column<bool>(type: "boolean", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    DisplayName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "jsonb", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsStackable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsTradable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsLimitedEdition = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumable_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_consumable_items_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drop_items",
                columns: table => new
                {
                    DropTableId = table.Column<int>(type: "integer", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    MinQuantity = table.Column<int>(type: "integer", nullable: false),
                    MaxQuantity = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    IsGuaranteed = table.Column<bool>(type: "boolean", nullable: false),
                    Rarity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drop_items", x => new { x.DropTableId, x.CatalogUuid });
                    table.CheckConstraint("CK_DropTableItem_PositiveWeight", "\"Weight\" > 0");
                    table.CheckConstraint("CK_DropTableItem_QuantityRange", "\"MinQuantity\" <= \"MaxQuantity\"");
                    table.ForeignKey(
                        name: "FK_drop_items_catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "catalogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomData = table.Column<string>(type: "text", nullable: true),
                    DropTableId = table.Column<int>(type: "integer", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enemies_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
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
                name: "equipment_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rarity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CustomData = table.Column<string>(type: "jsonb", nullable: true),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    DisplayName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "jsonb", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsStackable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsTradable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsLimitedEdition = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_equipment_items_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lotteries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PityNumber = table.Column<int>(type: "integer", nullable: false),
                    PityType = table.Column<int>(type: "integer", nullable: false),
                    SingleCostCurrencyId = table.Column<int>(type: "integer", nullable: false),
                    SingleCostAmount = table.Column<int>(type: "integer", nullable: false),
                    TenCostCurrencyId = table.Column<int>(type: "integer", nullable: false),
                    TenCostAmount = table.Column<int>(type: "integer", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lotteries_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
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
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaxQuantity = table.Column<int>(type: "integer", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsStackable = table.Column<bool>(type: "boolean", nullable: false),
                    IsTradable = table.Column<bool>(type: "boolean", nullable: false),
                    IsLimitedEdition = table.Column<bool>(type: "boolean", nullable: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    ExpTableId = table.Column<int>(type: "integer", nullable: false),
                    CustomData = table.Column<string>(type: "text", nullable: true),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
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
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: true),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CustomData = table.Column<string>(type: "text", nullable: true),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Titles_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bundled_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    ItemType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AchievementTierId = table.Column<int>(type: "integer", nullable: true),
                    RewardDefinitionId = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "LotteryPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LotteryId = table.Column<int>(type: "integer", nullable: false),
                    PrizeCatalogId = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    IsPickup = table.Column<bool>(type: "boolean", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotteryPrizes_Lotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "Lotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotteryPrizes_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotteryPrizes_catalogs_PrizeCatalogId",
                        column: x => x.PrizeCatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotteryRarities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LotteryId = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryRarities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotteryRarities_Lotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "Lotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotteryRarities_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
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
                name: "IX_bundled_items_CatalogId",
                table: "bundled_items",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_bundled_items_RewardDefinitionId",
                table: "bundled_items",
                column: "RewardDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_categories_Code",
                table: "catalog_categories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_catalog_series_Prefix",
                table: "catalog_series",
                column: "Prefix",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CatalogSeries_Category_Prefix",
                table: "catalog_series",
                columns: new[] { "CategoryId", "Prefix" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CatalogVersion_SemVer",
                table: "catalog_versions",
                columns: new[] { "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_catalogs_SeriesId_Number",
                table: "catalogs",
                columns: new[] { "SeriesId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Catalog_Uuid",
                table: "catalogs",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_CatalogId",
                table: "consumable_items",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_IsCurrentVersion",
                table: "consumable_items",
                column: "IsCurrentVersion");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_Status",
                table: "consumable_items",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_Consumable_Catalog_Revision",
                table: "consumable_items",
                columns: new[] { "CatalogUuid", "Revision" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Consumable_CurrentVersion",
                table: "consumable_items",
                column: "CatalogUuid",
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
                name: "IX_Enemies_CatalogId",
                table: "Enemies",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_DropTableId",
                table: "Enemies",
                column: "DropTableId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_CatalogId",
                table: "equipment_items",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_IsCurrentVersion",
                table: "equipment_items",
                column: "IsCurrentVersion");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_Status",
                table: "equipment_items",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_Equipment_Catalog_Revision",
                table: "equipment_items",
                columns: new[] { "CatalogUuid", "Revision" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Equipment_CurrentVersion",
                table: "equipment_items",
                column: "CatalogUuid",
                unique: true,
                filter: "\"IsCurrentVersion\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_CatalogId",
                table: "Lotteries",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_SingleCostCurrencyId",
                table: "Lotteries",
                column: "SingleCostCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_TenCostCurrencyId",
                table: "Lotteries",
                column: "TenCostCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_CatalogId",
                table: "LotteryPrizes",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_LotteryId",
                table: "LotteryPrizes",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_PrizeCatalogId",
                table: "LotteryPrizes",
                column: "PrizeCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_CatalogId",
                table: "LotteryRarities",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_LotteryId",
                table: "LotteryRarities",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CatalogId",
                table: "Materials",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CatalogId",
                table: "Players",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ExpTableId",
                table: "Players",
                column: "ExpTableId");

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
                name: "IX_Titles_CatalogId",
                table: "Titles",
                column: "CatalogId");

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
                name: "IX_virtual_currencies_Code",
                table: "virtual_currencies",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "ShopItem");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "update_versions");

            migrationBuilder.DropTable(
                name: "addressable_assets");

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
                name: "app_versions");

            migrationBuilder.DropTable(
                name: "asset_versions");

            migrationBuilder.DropTable(
                name: "reward_definitions");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "catalogs");

            migrationBuilder.DropTable(
                name: "virtual_currencies");

            migrationBuilder.DropTable(
                name: "eligibility_conditions");

            migrationBuilder.DropTable(
                name: "objective_criteria");

            migrationBuilder.DropTable(
                name: "catalog_series");

            migrationBuilder.DropTable(
                name: "catalog_categories");
        }
    }
}
