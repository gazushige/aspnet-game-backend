using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations.AdminDb
{
    /// <inheritdoc />
    public partial class vip_world : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vip_masters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    required_point = table.Column<long>(type: "bigint", nullable: false),
                    point_multiplier = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    icon_asset_id = table.Column<int>(type: "integer", nullable: true),
                    requirement = table.Column<string>(type: "jsonb", nullable: true),
                    custom_data = table.Column<string>(type: "jsonb", nullable: true)
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
                    id = table.Column<int>(type: "integer", nullable: false),
                    phase = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    requirement = table.Column<string>(type: "jsonb", nullable: true),
                    unlock_after = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    force_progress_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    custom_data = table.Column<string>(type: "jsonb", nullable: true),
                    banner_asset_id = table.Column<int>(type: "integer", nullable: true)
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
                name: "world_progress_state",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    current_phase = table.Column<int>(type: "integer", nullable: false),
                    phase_started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    avg_player_level = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    total_boss_kill_count = table.Column<long>(type: "bigint", nullable: false),
                    active_player_count = table.Column<int>(type: "integer", nullable: false),
                    last_aggregated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_world_progress_state", x => x.id);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_version_major_positive",
                table: "catalog_versions",
                sql: "\"Major\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_version_minor_positive",
                table: "catalog_versions",
                sql: "\"Minor\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_version_patch_positive",
                table: "catalog_versions",
                sql: "\"Patch\" >= 0");

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
                name: "vip_masters");

            migrationBuilder.DropTable(
                name: "world_phase_masters");

            migrationBuilder.DropTable(
                name: "world_progress_state");

            migrationBuilder.DropCheckConstraint(
                name: "CK_version_major_positive",
                table: "catalog_versions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_version_minor_positive",
                table: "catalog_versions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_version_patch_positive",
                table: "catalog_versions");
        }
    }
}
