using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class FirmIdToNotesDocTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "NotesTag",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "DocumentTag",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "NotesTag");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "DocumentTag");
        }
    }
}
