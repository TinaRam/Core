using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.Migrations
{
    public partial class DeletedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Deleted",
                schema: "app2000g11",
                table: "TaskList",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.AddColumn<byte>(
                name: "Deleted",
                schema: "app2000g11",
                table: "PTask",
                type: "tinyint(1)",
                nullable: false,
                defaultValueSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                schema: "app2000g11",
                table: "TaskList");

            migrationBuilder.DropColumn(
                name: "Deleted",
                schema: "app2000g11",
                table: "PTask");
        }
    }
}
