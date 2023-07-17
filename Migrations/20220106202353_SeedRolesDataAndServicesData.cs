using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class SeedRolesDataAndServicesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a7251255-75fc-4b74-a1a2-dcff91eb1bb2", "7fa1a159-2c38-4205-9125-34db975e9c3b", "Admin", "ADMIN" },
                    { "f4e833a7-e496-4448-b762-90e0630a6f91", "f454d783-28c1-4ded-b54d-7403070c7b1f", "Customer", "CUSTOMER" },
                    { "c0fd0c4f-1fdf-4ad0-9594-320e9dbd2b15", "035844f6-e408-418e-b51b-99f986f9a8e2", "Attorney", "ATTORNEY" },
                    { "f19c181c-c43d-42a2-aa90-e06bf7ac33ba", "81474c2e-36b2-4cb4-8367-3633b5b10848", "Client", "CLIENT" },
                    { "33ea1ca0-5973-4eac-a242-94ac321bb916", "04bc9749-050e-4a3a-8b64-684689412755", "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Unlimited Messages" },
                    { 2, "Timeline" },
                    { 3, "Calendar Events" },
                    { 4, "Reporting" },
                    { 5, "Documents Handling" },
                    { 6, "Notes Handling" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33ea1ca0-5973-4eac-a242-94ac321bb916");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7251255-75fc-4b74-a1a2-dcff91eb1bb2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0fd0c4f-1fdf-4ad0-9594-320e9dbd2b15");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f19c181c-c43d-42a2-aa90-e06bf7ac33ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4e833a7-e496-4448-b762-90e0630a6f91");

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
