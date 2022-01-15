using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesRegister.Migrations
{
    public partial class removeproductcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "DailyRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "DailyRecords",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
