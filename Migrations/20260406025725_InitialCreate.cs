using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
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
                    table.PrimaryKey("PK_Categories", x => x.Id);
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
                name: "Series",
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
                    table.PrimaryKey("PK_Series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Series_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    SeriesId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    KeyCode = table.Column<string>(type: "TEXT", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Catalogs_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_consumable_items_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
                        principalColumn: "Uuid",
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
                        name: "FK_drop_items_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
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
                        name: "FK_Enemies_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
                        principalColumn: "Uuid",
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
                        name: "FK_equipment_items_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
                        principalColumn: "Uuid",
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
                        name: "FK_Players_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_exp_tables_ExpTableId",
                        column: x => x.ExpTableId,
                        principalTable: "exp_tables",
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
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
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
                        name: "FK_VirtualCurrencies_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lotteries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PityNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    PityType = table.Column<int>(type: "INTEGER", nullable: false),
                    SingleCostId = table.Column<int>(type: "INTEGER", nullable: false),
                    TenCostId = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
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
                        name: "FK_Lotteries_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotteries_VirtualCurrencies_SingleCostId",
                        column: x => x.SingleCostId,
                        principalTable: "VirtualCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotteries_VirtualCurrencies_TenCostId",
                        column: x => x.TenCostId,
                        principalTable: "VirtualCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lottery_prizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LotteryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CatalogUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rarity = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPickup = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lottery_prizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lottery_prizes_Catalogs_CatalogUuid",
                        column: x => x.CatalogUuid,
                        principalTable: "Catalogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lottery_prizes_Lotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "Lotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lottery_rarities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LotteryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lottery_rarities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lottery_rarities_Lotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "Lotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_KeyCode",
                table: "Catalogs",
                column: "KeyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_SeriesId_Number",
                table: "Catalogs",
                columns: new[] { "SeriesId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                table: "Categories",
                column: "Code",
                unique: true);

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
                name: "IX_drop_items_CatalogUuid",
                table: "drop_items",
                column: "CatalogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_drop_tables_KeyCode",
                table: "drop_tables",
                column: "KeyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_CatalogUuid",
                table: "Enemies",
                column: "CatalogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_DropTableId",
                table: "Enemies",
                column: "DropTableId");

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
                name: "IX_Lotteries_CatalogUuid",
                table: "Lotteries",
                column: "CatalogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_SingleCostId",
                table: "Lotteries",
                column: "SingleCostId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_TenCostId",
                table: "Lotteries",
                column: "TenCostId");

            migrationBuilder.CreateIndex(
                name: "IX_lottery_prizes_CatalogUuid",
                table: "lottery_prizes",
                column: "CatalogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_lottery_prizes_LotteryId_Rarity",
                table: "lottery_prizes",
                columns: new[] { "LotteryId", "Rarity" });

            migrationBuilder.CreateIndex(
                name: "IX_lottery_rarities_LotteryId",
                table: "lottery_rarities",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CatalogUuid",
                table: "Players",
                column: "CatalogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ExpTableId",
                table: "Players",
                column: "ExpTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Prefix",
                table: "Series",
                column: "Prefix",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CatalogSeries_Category_Prefix",
                table: "Series",
                columns: new[] { "CategoryId", "Prefix" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VirtualCurrencies_CatalogUuid",
                table: "VirtualCurrencies",
                column: "CatalogUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "consumable_items");

            migrationBuilder.DropTable(
                name: "drop_items");

            migrationBuilder.DropTable(
                name: "Enemies");

            migrationBuilder.DropTable(
                name: "equipment_items");

            migrationBuilder.DropTable(
                name: "lottery_prizes");

            migrationBuilder.DropTable(
                name: "lottery_rarities");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "drop_tables");

            migrationBuilder.DropTable(
                name: "Lotteries");

            migrationBuilder.DropTable(
                name: "exp_tables");

            migrationBuilder.DropTable(
                name: "VirtualCurrencies");

            migrationBuilder.DropTable(
                name: "Catalogs");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
