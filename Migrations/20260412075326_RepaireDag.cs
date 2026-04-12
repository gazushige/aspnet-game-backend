using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rest.Migrations
{
    /// <inheritdoc />
    public partial class RepaireDag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DagNodes_Dags_DagId",
                table: "DagNodes");

            migrationBuilder.DropTable(
                name: "DagDagNode");

            migrationBuilder.DropTable(
                name: "DagNodeDagNode");

            migrationBuilder.DropIndex(
                name: "IX_DagNodes_DagId",
                table: "DagNodes");

            migrationBuilder.DropColumn(
                name: "DagId",
                table: "DagNodes");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "eligibility_conditions",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "'{}'",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "eligibility_conditions",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValueSql: "'{}'");

            migrationBuilder.AddColumn<int>(
                name: "DagId",
                table: "DagNodes",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_DagNodes_DagId",
                table: "DagNodes",
                column: "DagId");

            migrationBuilder.CreateIndex(
                name: "IX_DagDagNode_RootNodesId",
                table: "DagDagNode",
                column: "RootNodesId");

            migrationBuilder.CreateIndex(
                name: "IX_DagNodeDagNode_ParentsId",
                table: "DagNodeDagNode",
                column: "ParentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DagNodes_Dags_DagId",
                table: "DagNodes",
                column: "DagId",
                principalTable: "Dags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
