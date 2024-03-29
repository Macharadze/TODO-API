using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TODO.Persistence.Migrations
{
    public partial class InitialAuditTableAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnName",
                table: "ActionLogs");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "ActionLogs",
                newName: "TableName");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "ActionLogs",
                newName: "KeyValues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TableName",
                table: "ActionLogs",
                newName: "ItemType");

            migrationBuilder.RenameColumn(
                name: "KeyValues",
                table: "ActionLogs",
                newName: "ItemId");

            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                table: "ActionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
