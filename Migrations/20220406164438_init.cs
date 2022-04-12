using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SalesRegister.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:my_collation", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    FirstName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    LastName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Gender = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    StaffId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    CreatedById = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyName",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    CompanyName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    AdminId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoice",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    CustomerName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    InvoiceId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AdminId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Total = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Product = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Measure = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    AdminId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Department = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ProductCode = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ProductName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    AdminId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockBalances",
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
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockBalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockBalanceUpdates",
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
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockBalanceUpdates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Totals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ClaimType = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ClaimType = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ProviderKey = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    UserId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    LoginProvider = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Name = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Value = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoiceDetailModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    InvoiceId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    AdminId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Product = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ProductId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Measure = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    MeasureId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CustomerInvoiceModelId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoiceDetailModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerInvoiceDetailModel_CustomerInvoice_CustomerInvoiceM~",
                        column: x => x.CustomerInvoiceModelId,
                        principalTable: "CustomerInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductMeasureModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    ProductId = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Quantity = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    Measure = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    QtyPerMeasure = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation"),
                    CostPrice = table.Column<float>(type: "real", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    ProductsModelId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "my_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMeasureModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMeasureModel_Products_ProductsModelId",
                        column: x => x.ProductsModelId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoiceDetailModel_CustomerInvoiceModelId",
                table: "CustomerInvoiceDetailModel",
                column: "CustomerInvoiceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMeasureModel_ProductsModelId",
                table: "ProductMeasureModel",
                column: "ProductsModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CompanyName");

            migrationBuilder.DropTable(
                name: "CustomerInvoiceDetailModel");

            migrationBuilder.DropTable(
                name: "DailyRecords");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "ProductMeasureModel");

            migrationBuilder.DropTable(
                name: "StockBalances");

            migrationBuilder.DropTable(
                name: "StockBalanceUpdates");

            migrationBuilder.DropTable(
                name: "Totals");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CustomerInvoice");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
