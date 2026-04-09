using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations
{
    /// <inheritdoc />
    public partial class RebuildInitial : Migration
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
                    Label = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Path = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    PublicUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addressable_assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "app_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    BuildNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ForceUpdate = table.Column<bool>(type: "INTEGER", nullable: false),
                    StoreUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Major = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Minor = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Patch = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    Major = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Minor = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Patch = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayfabCatalogVersion = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SnapShot = table.Column<string>(type: "jsonb", nullable: true),
                    Major = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Minor = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Patch = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_versions", x => x.Id);
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
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drop_tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exp_tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MinLevel = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    MaxLevel = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 99),
                    Logic = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exp_tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CustomData = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
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
                name: "catalog_series",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Prefix = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastNumber = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "DagNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DagId = table.Column<int>(type: "INTEGER", nullable: true),
                    NodeType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: true),
                    Cost = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DagNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DagNodes_Dags_DagId",
                        column: x => x.DagId,
                        principalTable: "Dags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    SeriesId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    KeyCode = table.Column<string>(type: "TEXT", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false)
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
                name: "DagDagNode",
                columns: table => new
                {
                    DagsId = table.Column<int>(type: "INTEGER", nullable: false),
                    RootNodesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DagDagNode", x => new { x.DagsId, x.RootNodesId });
                    table.ForeignKey(
                        name: "FK_DagDagNode_DagNodes_RootNodesId",
                        column: x => x.RootNodesId,
                        principalTable: "DagNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DagDagNode_Dags_DagsId",
                        column: x => x.DagsId,
                        principalTable: "Dags",
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
                name: "DagNodeDagNode",
                columns: table => new
                {
                    ChildrenId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DagNodeDagNode", x => new { x.ChildrenId, x.ParentsId });
                    table.ForeignKey(
                        name: "FK_DagNodeDagNode_DagNodes_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "DagNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DagNodeDagNode_DagNodes_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "DagNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "consumable_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsageCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    UsagePeriodSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    Rarity = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    MaxStack = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 99),
                    CustomData = table.Column<string>(type: "jsonb", nullable: true),
                    IsConsumableByTime = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsConsumableByCount = table.Column<bool>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "jsonb", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    VirtualCurrencyPrices = table.Column<string>(type: "jsonb", nullable: true),
                    RealCurrencyPrices = table.Column<string>(type: "jsonb", nullable: true),
                    IsStackable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsTradable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsLimitedEdition = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
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
                    DropTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    MinQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    IsGuaranteed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    DropTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rarity = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CustomData = table.Column<string>(type: "jsonb", nullable: true),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "jsonb", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    VirtualCurrencyPrices = table.Column<string>(type: "jsonb", nullable: true),
                    RealCurrencyPrices = table.Column<string>(type: "jsonb", nullable: true),
                    IsStackable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsTradable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsLimitedEdition = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    InitialLimitedEditionCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
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
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: true),
                    StartAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CustomData = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                name: "VirtualCurrencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MinQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsStackable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTradable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CurrencyType = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirtualCurrencies_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lotteries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PityNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    PityType = table.Column<int>(type: "INTEGER", nullable: false),
                    SingleCostCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SingleCostAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    TenCostCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    TenCostAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lotteries_VirtualCurrencies_SingleCostCurrencyId",
                        column: x => x.SingleCostCurrencyId,
                        principalTable: "VirtualCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotteries_VirtualCurrencies_TenCostCurrencyId",
                        column: x => x.TenCostCurrencyId,
                        principalTable: "VirtualCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotteries_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotteryPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LotteryId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrizeCatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPickup = table.Column<bool>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LotteryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCurrentVersion = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                name: "IX_AddressableAsset_Label",
                table: "addressable_assets",
                column: "Label");

            migrationBuilder.CreateIndex(
                name: "UQ_AddressableAsset_Path",
                table: "addressable_assets",
                column: "Path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asset_version_assets_AddressableAssetId",
                table: "asset_version_assets",
                column: "AddressableAssetId");

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
                columns: new[] { "PlayfabCatalogVersion", "Major", "Minor", "Patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_catalogs_KeyCode",
                table: "catalogs",
                column: "KeyCode",
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
                name: "IX_DagDagNode_RootNodesId",
                table: "DagDagNode",
                column: "RootNodesId");

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
                name: "IX_DagNodeDagNode_ParentsId",
                table: "DagNodeDagNode",
                column: "ParentsId");

            migrationBuilder.CreateIndex(
                name: "IX_DagNodes_DagId",
                table: "DagNodes",
                column: "DagId");

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
                name: "IX_Players_CatalogId",
                table: "Players",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ExpTableId",
                table: "Players",
                column: "ExpTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_CatalogId",
                table: "Titles",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualCurrencies_CatalogId",
                table: "VirtualCurrencies",
                column: "CatalogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_versions");

            migrationBuilder.DropTable(
                name: "asset_version_assets");

            migrationBuilder.DropTable(
                name: "catalog_versions");

            migrationBuilder.DropTable(
                name: "consumable_items");

            migrationBuilder.DropTable(
                name: "DagDagNode");

            migrationBuilder.DropTable(
                name: "DagEdges");

            migrationBuilder.DropTable(
                name: "DagMemberships");

            migrationBuilder.DropTable(
                name: "DagNodeDagNode");

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
                name: "Players");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "addressable_assets");

            migrationBuilder.DropTable(
                name: "asset_versions");

            migrationBuilder.DropTable(
                name: "DagNodes");

            migrationBuilder.DropTable(
                name: "drop_tables");

            migrationBuilder.DropTable(
                name: "Lotteries");

            migrationBuilder.DropTable(
                name: "exp_tables");

            migrationBuilder.DropTable(
                name: "Dags");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "VirtualCurrencies");

            migrationBuilder.DropTable(
                name: "catalogs");

            migrationBuilder.DropTable(
                name: "catalog_series");

            migrationBuilder.DropTable(
                name: "catalog_categories");
        }
    }
}
