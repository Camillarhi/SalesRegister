using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesRegister.Migrations
{
    public partial class productchild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7b01g63f0-5201-4317-abd8-c21hm81f91b7330",
                column: "NormalizedName",
                value: "STAFF");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fab4fah75c1-c546-41de-aebc-a14da6v45895711",
                column: "NormalizedName",
                value: "ADMIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7b01g63f0-5201-4317-abd8-c21hm81f91b7330",
                column: "NormalizedName",
                value: "Staff");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fab4fah75c1-c546-41de-aebc-a14da6v45895711",
                column: "NormalizedName",
                value: "Admin");
        }
    }
}
