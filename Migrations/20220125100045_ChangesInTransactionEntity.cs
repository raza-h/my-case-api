using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class ChangesInTransactionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Debit",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "transactionType",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transactionType",
                table: "Transaction");

            migrationBuilder.AddColumn<string>(
                name: "Credit",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Debit",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
