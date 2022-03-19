using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesRegister.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "CustomerInvoiceDetailModel",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "CustomerInvoiceDetailModel",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "CustomerInvoiceDetailModel");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "CustomerInvoiceDetailModel",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
