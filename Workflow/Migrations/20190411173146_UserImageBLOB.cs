using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.Migrations
{
    public partial class UserImageBLOB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                schema: "app2000g11",
                table: "User",
                type: "mediumblob",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "EventTaskListFK",
                schema: "app2000g11",
                table: "Event",
                column: "TaskListId");

            migrationBuilder.AddForeignKey(
                name: "EventTaskListFK",
                schema: "app2000g11",
                table: "Event",
                column: "TaskListId",
                principalSchema: "app2000g11",
                principalTable: "TaskList",
                principalColumn: "TaskListId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "EventTaskListFK",
                schema: "app2000g11",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "EventTaskListFK",
                schema: "app2000g11",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "app2000g11",
                table: "User");
        }
    }
}
