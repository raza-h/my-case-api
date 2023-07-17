using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class FirmIdAdded2nd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "Lead",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "Company",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "CaseDetail",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "CaseDetail");
        }
    }
}
