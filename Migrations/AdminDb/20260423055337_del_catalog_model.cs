using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rest.Migrations.AdminDb
{
    /// <inheritdoc />
    public partial class del_catalog_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bundled_items_catalogs_CatalogId",
                table: "bundled_items");

            migrationBuilder.DropForeignKey(
                name: "FK_consumable_items_catalogs_CatalogId",
                table: "consumable_items");

            migrationBuilder.DropForeignKey(
                name: "FK_drop_items_catalogs_CatalogUuid",
                table: "drop_items");

            migrationBuilder.DropForeignKey(
                name: "FK_Enemies_catalogs_CatalogId",
                table: "Enemies");

            migrationBuilder.DropForeignKey(
                name: "FK_equipment_items_catalogs_CatalogId",
                table: "equipment_items");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotteries_catalogs_CatalogId",
                table: "Lotteries");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryPrizes_Lotteries_LotteryId",
                table: "LotteryPrizes");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryPrizes_catalogs_CatalogId",
                table: "LotteryPrizes");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryPrizes_catalogs_PrizeCatalogId",
                table: "LotteryPrizes");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryRarities_Lotteries_LotteryId",
                table: "LotteryRarities");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryRarities_catalogs_CatalogId",
                table: "LotteryRarities");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_catalogs_CatalogId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_catalogs_CatalogId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Titles_catalogs_CatalogId",
                table: "Titles");

            migrationBuilder.DropTable(
                name: "catalogs");

            migrationBuilder.DropTable(
                name: "catalog_series");

            migrationBuilder.DropTable(
                name: "catalog_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Titles",
                table: "Titles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materials",
                table: "Materials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotteryRarities",
                table: "LotteryRarities");

            migrationBuilder.DropIndex(
                name: "IX_LotteryRarities_LotteryId",
                table: "LotteryRarities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotteryPrizes",
                table: "LotteryPrizes");

            migrationBuilder.DropIndex(
                name: "IX_LotteryPrizes_CatalogId",
                table: "LotteryPrizes");

            migrationBuilder.DropIndex(
                name: "IX_LotteryPrizes_LotteryId",
                table: "LotteryPrizes");

            migrationBuilder.DropIndex(
                name: "IX_LotteryPrizes_PrizeCatalogId",
                table: "LotteryPrizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lotteries",
                table: "Lotteries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipment_items",
                table: "equipment_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enemies",
                table: "Enemies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_consumable_items",
                table: "consumable_items");

            migrationBuilder.DropIndex(
                name: "IX_bundled_items_CatalogId",
                table: "bundled_items");

            migrationBuilder.DropColumn(
                name: "IsStackable",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "IsGuaranteed",
                table: "drop_items");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "drop_items");

            migrationBuilder.DropColumn(
                name: "IsStackable",
                table: "consumable_items");

            migrationBuilder.RenameColumn(
                name: "IconUrl",
                table: "Titles",
                newName: "Requirement");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "Titles",
                newName: "Uuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "Titles",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_Titles_CatalogId",
                table: "Titles",
                newName: "IX_Titles_PrefixId");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "Players",
                newName: "Uuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "Players",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_CatalogId",
                table: "Players",
                newName: "IX_Players_PrefixId");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "Materials",
                newName: "Uuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "Materials",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_CatalogId",
                table: "Materials",
                newName: "IX_Materials_PrefixId");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "LotteryRarities",
                newName: "LotteryUuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "LotteryRarities",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_LotteryRarities_CatalogId",
                table: "LotteryRarities",
                newName: "IX_LotteryRarities_PrefixId");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "LotteryPrizes",
                newName: "LotteryUuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "LotteryPrizes",
                newName: "PrizeCategory");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "Lotteries",
                newName: "Uuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "Lotteries",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_Lotteries_CatalogId",
                table: "Lotteries",
                newName: "IX_Lotteries_PrefixId");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "equipment_items",
                newName: "Uuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "equipment_items",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_equipment_items_CatalogId",
                table: "equipment_items",
                newName: "IX_equipment_items_PrefixId");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "Enemies",
                newName: "Uuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "Enemies",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_Enemies_CatalogId",
                table: "Enemies",
                newName: "IX_Enemies_PrefixId");

            migrationBuilder.RenameColumn(
                name: "CatalogUuid",
                table: "consumable_items",
                newName: "Uuid");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "consumable_items",
                newName: "PrefixId");

            migrationBuilder.RenameIndex(
                name: "IX_consumable_items_CatalogId",
                table: "consumable_items",
                newName: "IX_consumable_items_PrefixId");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "bundled_items",
                newName: "Category");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Titles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "Titles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "Titles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Players",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Materials",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "Materials",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LotteryRarities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "LotteryRarities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "LotteryRarities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "PrizeCatalogId",
                table: "LotteryPrizes",
                type: "uuid",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LotteryPrizes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "LotteryPrizes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "LotteryPrizes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrefixId",
                table: "LotteryPrizes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Lotteries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "Lotteries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "equipment_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "equipment_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Enemies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "Enemies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomData",
                table: "drop_tables",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CatalogCategory",
                table: "drop_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "consumable_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "NumberInPrefix",
                table: "consumable_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CatalogId",
                table: "bundled_items",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Titles",
                table: "Titles",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materials",
                table: "Materials",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotteryRarities",
                table: "LotteryRarities",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotteryPrizes",
                table: "LotteryPrizes",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lotteries",
                table: "Lotteries",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipment_items",
                table: "equipment_items",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enemies",
                table: "Enemies",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_consumable_items",
                table: "consumable_items",
                column: "Uuid");

            migrationBuilder.CreateTable(
                name: "catalog_prefixes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Prefix = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_prefixes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Titles_IconAssetId",
                table: "Titles",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_LotteryUuid",
                table: "LotteryRarities",
                column: "LotteryUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_LotteryUuid",
                table: "LotteryPrizes",
                column: "LotteryUuid");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_PrefixId",
                table: "LotteryPrizes",
                column: "PrefixId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_consumable_items_catalog_prefixes_PrefixId",
                table: "consumable_items",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enemies_catalog_prefixes_PrefixId",
                table: "Enemies",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_items_catalog_prefixes_PrefixId",
                table: "equipment_items",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotteries_catalog_prefixes_PrefixId",
                table: "Lotteries",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryPrizes_Lotteries_LotteryUuid",
                table: "LotteryPrizes",
                column: "LotteryUuid",
                principalTable: "Lotteries",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryPrizes_catalog_prefixes_PrefixId",
                table: "LotteryPrizes",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryRarities_Lotteries_LotteryUuid",
                table: "LotteryRarities",
                column: "LotteryUuid",
                principalTable: "Lotteries",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryRarities_catalog_prefixes_PrefixId",
                table: "LotteryRarities",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_catalog_prefixes_PrefixId",
                table: "Materials",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_catalog_prefixes_PrefixId",
                table: "Players",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_addressable_assets_IconAssetId",
                table: "Titles",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_catalog_prefixes_PrefixId",
                table: "Titles",
                column: "PrefixId",
                principalTable: "catalog_prefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_consumable_items_catalog_prefixes_PrefixId",
                table: "consumable_items");

            migrationBuilder.DropForeignKey(
                name: "FK_Enemies_catalog_prefixes_PrefixId",
                table: "Enemies");

            migrationBuilder.DropForeignKey(
                name: "FK_equipment_items_catalog_prefixes_PrefixId",
                table: "equipment_items");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotteries_catalog_prefixes_PrefixId",
                table: "Lotteries");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryPrizes_Lotteries_LotteryUuid",
                table: "LotteryPrizes");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryPrizes_catalog_prefixes_PrefixId",
                table: "LotteryPrizes");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryRarities_Lotteries_LotteryUuid",
                table: "LotteryRarities");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryRarities_catalog_prefixes_PrefixId",
                table: "LotteryRarities");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_catalog_prefixes_PrefixId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_catalog_prefixes_PrefixId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Titles_addressable_assets_IconAssetId",
                table: "Titles");

            migrationBuilder.DropForeignKey(
                name: "FK_Titles_catalog_prefixes_PrefixId",
                table: "Titles");

            migrationBuilder.DropTable(
                name: "catalog_prefixes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Titles",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_IconAssetId",
                table: "Titles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materials",
                table: "Materials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotteryRarities",
                table: "LotteryRarities");

            migrationBuilder.DropIndex(
                name: "IX_LotteryRarities_LotteryUuid",
                table: "LotteryRarities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotteryPrizes",
                table: "LotteryPrizes");

            migrationBuilder.DropIndex(
                name: "IX_LotteryPrizes_LotteryUuid",
                table: "LotteryPrizes");

            migrationBuilder.DropIndex(
                name: "IX_LotteryPrizes_PrefixId",
                table: "LotteryPrizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lotteries",
                table: "Lotteries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipment_items",
                table: "equipment_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enemies",
                table: "Enemies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_consumable_items",
                table: "consumable_items");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "LotteryRarities");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "LotteryRarities");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "LotteryPrizes");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "LotteryPrizes");

            migrationBuilder.DropColumn(
                name: "PrefixId",
                table: "LotteryPrizes");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "Lotteries");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "equipment_items");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "CustomData",
                table: "drop_tables");

            migrationBuilder.DropColumn(
                name: "CatalogCategory",
                table: "drop_items");

            migrationBuilder.DropColumn(
                name: "NumberInPrefix",
                table: "consumable_items");

            migrationBuilder.RenameColumn(
                name: "Requirement",
                table: "Titles",
                newName: "IconUrl");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "Titles",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "Titles",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Titles_PrefixId",
                table: "Titles",
                newName: "IX_Titles_CatalogId");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "Players",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "Players",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Players_PrefixId",
                table: "Players",
                newName: "IX_Players_CatalogId");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "Materials",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "Materials",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_PrefixId",
                table: "Materials",
                newName: "IX_Materials_CatalogId");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "LotteryRarities",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "LotteryUuid",
                table: "LotteryRarities",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_LotteryRarities_PrefixId",
                table: "LotteryRarities",
                newName: "IX_LotteryRarities_CatalogId");

            migrationBuilder.RenameColumn(
                name: "PrizeCategory",
                table: "LotteryPrizes",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "LotteryUuid",
                table: "LotteryPrizes",
                newName: "CatalogUuid");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "Lotteries",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "Lotteries",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Lotteries_PrefixId",
                table: "Lotteries",
                newName: "IX_Lotteries_CatalogId");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "equipment_items",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "equipment_items",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_equipment_items_PrefixId",
                table: "equipment_items",
                newName: "IX_equipment_items_CatalogId");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "Enemies",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "Enemies",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Enemies_PrefixId",
                table: "Enemies",
                newName: "IX_Enemies_CatalogId");

            migrationBuilder.RenameColumn(
                name: "PrefixId",
                table: "consumable_items",
                newName: "CatalogId");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "consumable_items",
                newName: "CatalogUuid");

            migrationBuilder.RenameIndex(
                name: "IX_consumable_items_PrefixId",
                table: "consumable_items",
                newName: "IX_consumable_items_CatalogId");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "bundled_items",
                newName: "ItemType");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Titles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Players",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Materials",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsStackable",
                table: "Materials",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LotteryRarities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "PrizeCatalogId",
                table: "LotteryPrizes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LotteryPrizes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Lotteries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "equipment_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Enemies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuaranteed",
                table: "drop_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Rarity",
                table: "drop_items",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "consumable_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsStackable",
                table: "consumable_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CatalogId",
                table: "bundled_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Titles",
                table: "Titles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materials",
                table: "Materials",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotteryRarities",
                table: "LotteryRarities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotteryPrizes",
                table: "LotteryPrizes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lotteries",
                table: "Lotteries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipment_items",
                table: "equipment_items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enemies",
                table: "Enemies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_consumable_items",
                table: "consumable_items",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "catalog_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_series",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    LastNumber = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Prefix = table.Column<string>(type: "text", nullable: false)
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
                name: "catalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeriesId = table.Column<int>(type: "integer", nullable: false),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_LotteryId",
                table: "LotteryRarities",
                column: "LotteryId");

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
                name: "IX_bundled_items_CatalogId",
                table: "bundled_items",
                column: "CatalogId");

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
                name: "IX_catalogs_SeriesId_Number",
                table: "catalogs",
                columns: new[] { "SeriesId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Catalog_Uuid",
                table: "catalogs",
                column: "Uuid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bundled_items_catalogs_CatalogId",
                table: "bundled_items",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_consumable_items_catalogs_CatalogId",
                table: "consumable_items",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_drop_items_catalogs_CatalogUuid",
                table: "drop_items",
                column: "CatalogUuid",
                principalTable: "catalogs",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enemies_catalogs_CatalogId",
                table: "Enemies",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_items_catalogs_CatalogId",
                table: "equipment_items",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotteries_catalogs_CatalogId",
                table: "Lotteries",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryPrizes_Lotteries_LotteryId",
                table: "LotteryPrizes",
                column: "LotteryId",
                principalTable: "Lotteries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryPrizes_catalogs_CatalogId",
                table: "LotteryPrizes",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryPrizes_catalogs_PrizeCatalogId",
                table: "LotteryPrizes",
                column: "PrizeCatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryRarities_Lotteries_LotteryId",
                table: "LotteryRarities",
                column: "LotteryId",
                principalTable: "Lotteries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryRarities_catalogs_CatalogId",
                table: "LotteryRarities",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_catalogs_CatalogId",
                table: "Materials",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_catalogs_CatalogId",
                table: "Players",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_catalogs_CatalogId",
                table: "Titles",
                column: "CatalogId",
                principalTable: "catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
