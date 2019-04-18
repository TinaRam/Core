using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.Migrations
{
    public partial class Notification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "app2000g11",
                table: "Notification",
                type: "int(11)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "NotificationUserFK",
                schema: "app2000g11",
                table: "Notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "NotificationUserFK",
                schema: "app2000g11",
                table: "Notification",
                column: "UserId",
                principalSchema: "app2000g11",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "NotificationUserFK",
                schema: "app2000g11",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "NotificationUserFK",
                schema: "app2000g11",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app2000g11",
                table: "Notification");
        }
    }
}
