using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workflow.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Workflow");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Workflow",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Username = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    Password = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    IsAdmin = table.Column<byte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "Workflow",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    FirstName = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    LastName = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 11, nullable: true),
                    UserID = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "EmployeeUserFK",
                        column: x => x.UserID,
                        principalSchema: "Workflow",
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "Workflow",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectName = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    ProjectDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    ProjectStart = table.Column<DateTime>(type: "date", nullable: true),
                    ProjectDeadline = table.Column<DateTime>(type: "date", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "date", nullable: true),
                    ProjectManager = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectID);
                    table.ForeignKey(
                        name: "ProjectEmployeeFK",
                        column: x => x.ProjectManager,
                        principalSchema: "Workflow",
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectParticipant",
                schema: "Workflow",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int(11)", nullable: false),
                    EmployeeID = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectParticipant", x => new { x.ProjectID, x.EmployeeID });
                    table.ForeignKey(
                        name: "ProjectParticipantEmployeeFK",
                        column: x => x.EmployeeID,
                        principalSchema: "Workflow",
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ProjectParticipantProjectFK",
                        column: x => x.ProjectID,
                        principalSchema: "Workflow",
                        principalTable: "Project",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PTask",
                schema: "Workflow",
                columns: table => new
                {
                    TaskID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    TaskName = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Priority = table.Column<string>(type: "enum('low','normal','high')", nullable: true),
                    TaskCreationDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TaskDeadline = table.Column<DateTime>(type: "date", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "date", nullable: true),
                    TaskProjectID = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PTask", x => x.TaskID);
                    table.ForeignKey(
                        name: "PTaskProjectFK",
                        column: x => x.TaskProjectID,
                        principalSchema: "Workflow",
                        principalTable: "Project",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignedTask",
                schema: "Workflow",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int(11)", nullable: false),
                    TaskID = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedTask", x => new { x.EmployeeID, x.TaskID });
                    table.ForeignKey(
                        name: "AssignedTaskEmployeeFK",
                        column: x => x.EmployeeID,
                        principalSchema: "Workflow",
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "AssignedTaskPTaskFK",
                        column: x => x.TaskID,
                        principalSchema: "Workflow",
                        principalTable: "PTask",
                        principalColumn: "TaskID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Workflow",
                table: "User",
                columns: new[] { "UserID", "IsAdmin", "Password", "Username" },
                values: new object[] { 1, (byte)1, "123", "pernille" });

            migrationBuilder.InsertData(
                schema: "Workflow",
                table: "User",
                columns: new[] { "UserID", "IsAdmin", "Password", "Username" },
                values: new object[] { 2, (byte)1, "paracet", "tinahodepina" });

            migrationBuilder.CreateIndex(
                name: "AssignedTaskPTaskFK",
                schema: "Workflow",
                table: "AssignedTask",
                column: "TaskID");

            migrationBuilder.CreateIndex(
                name: "EmployeeUserFK",
                schema: "Workflow",
                table: "Employee",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "ProjectEmployeeFK",
                schema: "Workflow",
                table: "Project",
                column: "ProjectManager");

            migrationBuilder.CreateIndex(
                name: "ProjectParticipantEmployeeFK",
                schema: "Workflow",
                table: "ProjectParticipant",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "PTaskProjectFK",
                schema: "Workflow",
                table: "PTask",
                column: "TaskProjectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedTask",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "ProjectParticipant",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "PTask",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Workflow");
        }
    }
}
