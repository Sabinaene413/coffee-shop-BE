using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCoffeeShopNomenclatureAndLocationFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "ShopProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "ShopProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "ShopProductOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "ShopProductOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "ShopOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "SaleProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "SaleProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "SaleProductOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "SaleProductOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "SaleOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "SaleOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "Inventories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoffeeShops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShops", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "t2AvMZ56NfxFs5dwcW0NOr3V9m4GcWM7KVYmNpJ2Lo8=", "WD7B85roO6LMwzJh3usedw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ShopProductOrders");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "ShopProductOrders");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "SaleProducts");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "SaleProducts");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "SaleProductOrder");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "SaleProductOrder");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "SaleOrders");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "SaleOrders");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "9ROTWTDklBXHMB1gRgc6mgB3x26PMOb6wvSfWMpxs1o=", "Zfv2jfnYuRmVGn7za5Ewhw==" });
        }
    }
}
