using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TODO.Persistence.Migrations
{
    public partial class InitialAuditTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLog_TODOs_ToDoId",
                table: "ActionLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionLog",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ActionLog");

            migrationBuilder.RenameTable(
                name: "ActionLog",
                newName: "ActionLogs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActionLogs",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ActionLogs",
                newName: "OperationType");

            migrationBuilder.RenameColumn(
                name: "OldResult",
                table: "ActionLogs",
                newName: "OldValues");

            migrationBuilder.RenameColumn(
                name: "NewResult",
                table: "ActionLogs",
                newName: "NewValues");

            migrationBuilder.RenameIndex(
                name: "IX_ActionLog_ToDoId",
                table: "ActionLogs",
                newName: "IX_ActionLogs_ToDoId");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "ActionLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionLogs",
                table: "ActionLogs",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLogs_TODOs_ToDoId",
                table: "ActionLogs",
                column: "ToDoId",
                principalTable: "TODOs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLogs_TODOs_ToDoId",
                table: "ActionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionLogs",
                table: "ActionLogs");

            migrationBuilder.RenameTable(
                name: "ActionLogs",
                newName: "ActionLog");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ActionLog",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OperationType",
                table: "ActionLog",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "OldValues",
                table: "ActionLog",
                newName: "OldResult");

            migrationBuilder.RenameColumn(
                name: "NewValues",
                table: "ActionLog",
                newName: "NewResult");

            migrationBuilder.RenameIndex(
                name: "IX_ActionLogs_ToDoId",
                table: "ActionLog",
                newName: "IX_ActionLog_ToDoId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ActionLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ActionLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ActionLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionLog",
                table: "ActionLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLog_TODOs_ToDoId",
                table: "ActionLog",
                column: "ToDoId",
                principalTable: "TODOs",
                principalColumn: "Id");
        }
    }
}
