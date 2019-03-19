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
                    UserId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Username = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    Password = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    FirstName = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    LastName = table.Column<string>(unicode: false, maxLength: 55, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 11, nullable: true),
                    Role = table.Column<int>(type: "int(11)", nullable: true),
                    About = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeave",
                schema: "Workflow",
                columns: table => new
                {
                    LeaveId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserId = table.Column<int>(type: "int(11)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "date", nullable: true),
                    ToDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLeave", x => x.LeaveId);
                    table.ForeignKey(
                        name: "EmployeeLeaveUserFK",
                        column: x => x.UserId,
                        principalSchema: "Workflow",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "Workflow",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectName = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    ProjectDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    ProjectStart = table.Column<DateTime>(type: "date", nullable: true),
                    ProjectDeadline = table.Column<DateTime>(type: "date", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "date", nullable: true),
                    ProjectManager = table.Column<int>(type: "int(11)", nullable: true),
                    MarkedAsFinished = table.Column<byte>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "ProjectUserFK",
                        column: x => x.ProjectManager,
                        principalSchema: "Workflow",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectParticipant",
                schema: "Workflow",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int(11)", nullable: false),
                    UserId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectParticipant", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "ProjectParticipantProjectFK",
                        column: x => x.ProjectId,
                        principalSchema: "Workflow",
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ProjectParticipantUserFK",
                        column: x => x.UserId,
                        principalSchema: "Workflow",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                schema: "Workflow",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectId = table.Column<int>(type: "int(11)", nullable: true),
                    CompletionDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Comment = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "ReportProjectFK",
                        column: x => x.ProjectId,
                        principalSchema: "Workflow",
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskList",
                schema: "Workflow",
                columns: table => new
                {
                    TaskListId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectId = table.Column<int>(type: "int(11)", nullable: false),
                    ListName = table.Column<string>(unicode: false, maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskList", x => x.TaskListId);
                    table.ForeignKey(
                        name: "TaskListFK",
                        column: x => x.ProjectId,
                        principalSchema: "Workflow",
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PTask",
                schema: "Workflow",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    TaskName = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Priority = table.Column<string>(type: "enum('low','normal','high')", nullable: true),
                    TaskCreationDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TaskDeadline = table.Column<DateTime>(type: "date", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "date", nullable: true),
                    TaskProjectId = table.Column<int>(type: "int(11)", nullable: true),
                    TaskListId = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PTask", x => x.TaskId);
                    table.ForeignKey(
                        name: "PTaskTaskListFK",
                        column: x => x.TaskListId,
                        principalSchema: "Workflow",
                        principalTable: "TaskList",
                        principalColumn: "TaskListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "PTaskProjectFK",
                        column: x => x.TaskProjectId,
                        principalSchema: "Workflow",
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignedTask",
                schema: "Workflow",
                columns: table => new
                {
                    AssignedTaskId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectId = table.Column<int>(type: "int(11)", nullable: false),
                    UserId = table.Column<int>(type: "int(11)", nullable: false),
                    TaskId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedTask", x => x.AssignedTaskId);
                    table.ForeignKey(
                        name: "AssignedTaskPTaskFK",
                        column: x => x.TaskId,
                        principalSchema: "Workflow",
                        principalTable: "PTask",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "AssignedTaskPTaskFK",
                schema: "Workflow",
                table: "AssignedTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "EmployeeLeaveUserFK",
                schema: "Workflow",
                table: "EmployeeLeave",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "ProjectUserFK",
                schema: "Workflow",
                table: "Project",
                column: "ProjectManager");

            migrationBuilder.CreateIndex(
                name: "ProjectParticipantUserFK",
                schema: "Workflow",
                table: "ProjectParticipant",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "PTaskTaskListFK",
                schema: "Workflow",
                table: "PTask",
                column: "TaskListId");

            migrationBuilder.CreateIndex(
                name: "PTaskProjectFK",
                schema: "Workflow",
                table: "PTask",
                column: "TaskProjectId");

            migrationBuilder.CreateIndex(
                name: "ReportProjectFK",
                schema: "Workflow",
                table: "Report",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "TaskListFK",
                schema: "Workflow",
                table: "TaskList",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedTask",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "EmployeeLeave",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "ProjectParticipant",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "Report",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "PTask",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "TaskList",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "Workflow");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Workflow");
        }
    }
}
