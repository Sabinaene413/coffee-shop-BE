using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSalesOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_ShopProducts_ShopProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProductOrders_ShopOrders_ShopOrderId",
                table: "ShopProductOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProductOrders_ShopProducts_ShopProductId",
                table: "ShopProductOrders");

            migrationBuilder.CreateTable(
                name: "SaleOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleProductOrder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleProductId = table.Column<long>(type: "bigint", nullable: false),
                    SaleOrderId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProductOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleProductOrder_SaleOrders_SaleOrderId",
                        column: x => x.SaleOrderId,
                        principalTable: "SaleOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleProductOrder_SaleProducts_SaleProductId",
                        column: x => x.SaleProductId,
                        principalTable: "SaleProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "9ROTWTDklBXHMB1gRgc6mgB3x26PMOb6wvSfWMpxs1o=", "Zfv2jfnYuRmVGn7za5Ewhw==" });

            migrationBuilder.CreateIndex(
                name: "IX_SaleProductOrder_SaleOrderId",
                table: "SaleProductOrder",
                column: "SaleOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProductOrder_SaleProductId",
                table: "SaleProductOrder",
                column: "SaleProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_ShopProducts_ShopProductId",
                table: "Inventories",
                column: "ShopProductId",
                principalTable: "ShopProducts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProductOrders_ShopOrders_ShopOrderId",
                table: "ShopProductOrders",
                column: "ShopOrderId",
                principalTable: "ShopOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProductOrders_ShopProducts_ShopProductId",
                table: "ShopProductOrders",
                column: "ShopProductId",
                principalTable: "ShopProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_ShopProducts_ShopProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProductOrders_ShopOrders_ShopOrderId",
                table: "ShopProductOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProductOrders_ShopProducts_ShopProductId",
                table: "ShopProductOrders");

            migrationBuilder.DropTable(
                name: "SaleProductOrder");

            migrationBuilder.DropTable(
                name: "SaleOrders");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "R7ekti4xBTg9gR6d0zh1c5+cgqdnATjmCbNFUUq5HuI=", "ku+YyvEtGXC0y3q8UI7GIA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_ShopProducts_ShopProductId",
                table: "Inventories",
                column: "ShopProductId",
                principalTable: "ShopProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProductOrders_ShopOrders_ShopOrderId",
                table: "ShopProductOrders",
                column: "ShopOrderId",
                principalTable: "ShopOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProductOrders_ShopProducts_ShopProductId",
                table: "ShopProductOrders",
                column: "ShopProductId",
                principalTable: "ShopProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
