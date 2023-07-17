using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class _31Jan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "RefferalSource",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RefferalSource",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "PracticeArea",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PracticeArea",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "ContactGroup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ContactGroup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOpened",
                table: "CaseDetail",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateClosed",
                table: "CaseDetail",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "BillingMethod",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BillingMethod",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "RefferalSource");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefferalSource");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "PracticeArea");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PracticeArea");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "ContactGroup");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContactGroup");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "BillingMethod");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BillingMethod");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOpened",
                table: "CaseDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateClosed",
                table: "CaseDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
