using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rest.Migrations.AdminDb
{
    /// <inheritdoc />
    public partial class announcement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistributedItems_EligibilityConditionId",
                table: "reward_definitions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ExecutedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Requirement = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reward_definitions_DistributedItems_EligibilityConditionId",
                table: "reward_definitions",
                column: "DistributedItems_EligibilityConditionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_reward_definitions_eligibility_conditions_DistributedItems_~",
                table: "reward_definitions",
                column: "DistributedItems_EligibilityConditionId",
                principalTable: "eligibility_conditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reward_definitions_eligibility_conditions_DistributedItems_~",
                table: "reward_definitions");

            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropIndex(
                name: "IX_reward_definitions_DistributedItems_EligibilityConditionId",
                table: "reward_definitions");

            migrationBuilder.DropColumn(
                name: "DistributedItems_EligibilityConditionId",
                table: "reward_definitions");
        }
    }
}
