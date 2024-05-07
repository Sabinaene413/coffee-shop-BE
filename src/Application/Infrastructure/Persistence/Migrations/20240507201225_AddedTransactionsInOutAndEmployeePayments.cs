using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedTransactionsInOutAndEmployeePayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Transactions",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "ShopProductId",
                table: "Transactions",
                newName: "ShopOrderId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Transactions",
                newName: "SaleOrderId");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "Transactions",
                newName: "LocationId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EmployeePaymentId",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeePayments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmployeePaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePayments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    ShopProductId = table.Column<long>(type: "bigint", nullable: true),
                    SaleProductId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionDetail_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "D3/v9BGjPjSluefqYfh3DHl8wZ2CYHGuQkbNuB5f9cI=", "SKloB1qLi+ecYyZLhaoAkA==" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayments_EmployeeId",
                table: "EmployeePayments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetail_TransactionId",
                table: "TransactionDetail",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePayments");

            migrationBuilder.DropTable(
                name: "TransactionDetail");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EmployeePaymentId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Transactions",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ShopOrderId",
                table: "Transactions",
                newName: "ShopProductId");

            migrationBuilder.RenameColumn(
                name: "SaleOrderId",
                table: "Transactions",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Transactions",
                newName: "InvoiceId");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "t2AvMZ56NfxFs5dwcW0NOr3V9m4GcWM7KVYmNpJ2Lo8=", "WD7B85roO6LMwzJh3usedw==" });
        }
    }
}
