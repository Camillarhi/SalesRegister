using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesRegister.Migrations
{
    public partial class measure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Measure",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Measure",
                table: "DailyRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "DailyRecords",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Measure",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Measure",
                table: "DailyRecords");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "DailyRecords");
        }
    }
}
