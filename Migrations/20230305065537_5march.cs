using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class _5march : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HireReasons",
                table: "HireReasons");

            migrationBuilder.RenameTable(
                name: "HireReasons",
                newName: "HireReason");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HireReason",
                table: "HireReason",
                column: "ReasonId");

            migrationBuilder.CreateTable(
                name: "CFieldValues",
                columns: table => new
                {
                    FieldValueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldID = table.Column<int>(type: "int", nullable: false),
                    ModuleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcernID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFieldValues", x => x.FieldValueID);
                });

            migrationBuilder.CreateTable(
                name: "CustomFields",
                columns: table => new
                {
                    FieldID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomFieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomFieldType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullPractice = table.Column<bool>(type: "bit", nullable: false),
                    PartialPractice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFields", x => x.FieldID);
                });

            migrationBuilder.CreateTable(
                name: "CustomPractices",
                columns: table => new
                {
                    PracticeFieldID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracticeAreaID = table.Column<int>(type: "int", nullable: false),
                    FieldID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPractices", x => x.PracticeFieldID);
                });

            migrationBuilder.CreateTable(
                name: "LeadStatus",
                columns: table => new
                {
                    LStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadStatus", x => x.LStatusId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CFieldValues");

            migrationBuilder.DropTable(
                name: "CustomFields");

            migrationBuilder.DropTable(
                name: "CustomPractices");

            migrationBuilder.DropTable(
                name: "LeadStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HireReason",
                table: "HireReason");

            migrationBuilder.RenameTable(
                name: "HireReason",
                newName: "HireReasons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HireReasons",
                table: "HireReasons",
                column: "ReasonId");
        }
    }
}
