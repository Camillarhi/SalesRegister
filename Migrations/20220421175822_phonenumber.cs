using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesRegister.Migrations
{
    public partial class phonenumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "DailyRecords",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "my_collation");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "CustomerInvoice",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "my_collation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "DailyRecords");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "CustomerInvoice");
        }
    }
}
