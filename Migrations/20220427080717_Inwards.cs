using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SalesRegister.Migrations
{
    public partial class Inwards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Measure",
                table: "StockBalanceUpdates");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "StockBalanceUpdates");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "StockBalanceUpdates");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockBalanceUpdates");

            migrationBuilder.CreateTable(
                name: "StockBalanceUpdateDetailsModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductCode = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Product = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Measure = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    AdminId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StockBalanceUpdateModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockBalanceUpdateDetailsModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockBalanceUpdateDetailsModel_StockBalanceUpdates_StockBal~",
                        column: x => x.StockBalanceUpdateModelId,
                        principalTable: "StockBalanceUpdates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockInwards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    AdminId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Approve = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInwards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockInwardDetailsModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductCode = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Product = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Measure = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    AdminId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    StockInwardModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInwardDetailsModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockInwardDetailsModel_StockInwards_StockInwardModelId",
                        column: x => x.StockInwardModelId,
                        principalTable: "StockInwards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockBalanceUpdateDetailsModel_StockBalanceUpdateModelId",
                table: "StockBalanceUpdateDetailsModel",
                column: "StockBalanceUpdateModelId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInwardDetailsModel_StockInwardModelId",
                table: "StockInwardDetailsModel",
                column: "StockInwardModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockBalanceUpdateDetailsModel");

            migrationBuilder.DropTable(
                name: "StockInwardDetailsModel");

            migrationBuilder.DropTable(
                name: "StockInwards");

            migrationBuilder.AddColumn<string>(
                name: "Measure",
                table: "StockBalanceUpdates",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "my_collation");

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "StockBalanceUpdates",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "my_collation");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "StockBalanceUpdates",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "my_collation");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "StockBalanceUpdates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
