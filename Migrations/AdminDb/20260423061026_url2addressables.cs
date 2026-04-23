using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations.AdminDb
{
    /// <inheritdoc />
    public partial class url2addressables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "LotteryRarities");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "LotteryPrizes");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "Lotteries");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "equipment_items");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "consumable_items");

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "Players",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "Materials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "LotteryRarities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "LotteryPrizes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "Lotteries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "equipment_items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "Enemies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconAssetId",
                table: "consumable_items",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CatalogId",
                table: "bundled_items",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "addressable_assets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_IconAssetId",
                table: "Players",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_IconAssetId",
                table: "Materials",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRarities_IconAssetId",
                table: "LotteryRarities",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryPrizes_IconAssetId",
                table: "LotteryPrizes",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotteries_IconAssetId",
                table: "Lotteries",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_items_IconAssetId",
                table: "equipment_items",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_IconAssetId",
                table: "Enemies",
                column: "IconAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_items_IconAssetId",
                table: "consumable_items",
                column: "IconAssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_consumable_items_addressable_assets_IconAssetId",
                table: "consumable_items",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Enemies_addressable_assets_IconAssetId",
                table: "Enemies",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_items_addressable_assets_IconAssetId",
                table: "equipment_items",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotteries_addressable_assets_IconAssetId",
                table: "Lotteries",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryPrizes_addressable_assets_IconAssetId",
                table: "LotteryPrizes",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LotteryRarities_addressable_assets_IconAssetId",
                table: "LotteryRarities",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_addressable_assets_IconAssetId",
                table: "Materials",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_addressable_assets_IconAssetId",
                table: "Players",
                column: "IconAssetId",
                principalTable: "addressable_assets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_consumable_items_addressable_assets_IconAssetId",
                table: "consumable_items");

            migrationBuilder.DropForeignKey(
                name: "FK_Enemies_addressable_assets_IconAssetId",
                table: "Enemies");

            migrationBuilder.DropForeignKey(
                name: "FK_equipment_items_addressable_assets_IconAssetId",
                table: "equipment_items");

            migrationBuilder.DropForeignKey(
                name: "FK_Lotteries_addressable_assets_IconAssetId",
                table: "Lotteries");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryPrizes_addressable_assets_IconAssetId",
                table: "LotteryPrizes");

            migrationBuilder.DropForeignKey(
                name: "FK_LotteryRarities_addressable_assets_IconAssetId",
                table: "LotteryRarities");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_addressable_assets_IconAssetId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_addressable_assets_IconAssetId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_IconAssetId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Materials_IconAssetId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_LotteryRarities_IconAssetId",
                table: "LotteryRarities");

            migrationBuilder.DropIndex(
                name: "IX_LotteryPrizes_IconAssetId",
                table: "LotteryPrizes");

            migrationBuilder.DropIndex(
                name: "IX_Lotteries_IconAssetId",
                table: "Lotteries");

            migrationBuilder.DropIndex(
                name: "IX_equipment_items_IconAssetId",
                table: "equipment_items");

            migrationBuilder.DropIndex(
                name: "IX_Enemies_IconAssetId",
                table: "Enemies");

            migrationBuilder.DropIndex(
                name: "IX_consumable_items_IconAssetId",
                table: "consumable_items");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "LotteryRarities");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "LotteryPrizes");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "Lotteries");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "equipment_items");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "IconAssetId",
                table: "consumable_items");

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "Titles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "Materials",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "LotteryRarities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "LotteryPrizes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "Lotteries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "equipment_items",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "Enemies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "consumable_items",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CatalogId",
                table: "bundled_items",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "addressable_assets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
