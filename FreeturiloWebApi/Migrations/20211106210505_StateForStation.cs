using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeturiloWebApi.Migrations
{
    public partial class StateForStation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Stations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RouteJSON",
                table: "Routes",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "RouteJSON",
                table: "Routes");
        }
    }
}
