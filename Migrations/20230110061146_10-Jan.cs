using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class _10Jan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateClosed",
                table: "CaseDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOpened",
                table: "CaseDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "CaseDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateClosed",
                table: "CaseDetail");

            migrationBuilder.DropColumn(
                name: "DateOpened",
                table: "CaseDetail");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "CaseDetail");
        }
    }
}
