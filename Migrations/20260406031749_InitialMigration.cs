using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                name: "UQ_CatalogVersion_SemVer",
                table: "catalog_versions",
                columns: new[] { "PlayfabCatalogVersion", "Major", "Minor", "Patch" },
                unique: true);
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
                name: "addressable_assets");

            migrationBuilder.DropTable(
                name: "asset_versions");
        }
    }
}
