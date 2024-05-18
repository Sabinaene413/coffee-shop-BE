using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToSaleProductOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleProductOrder_SaleOrders_SaleOrderId",
                table: "SaleProductOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleProductOrder_SaleProducts_SaleProductId",
                table: "SaleProductOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleProductOrder",
                table: "SaleProductOrder");

            migrationBuilder.RenameTable(
                name: "SaleProductOrder",
                newName: "SaleProductOrders");

            migrationBuilder.RenameIndex(
                name: "IX_SaleProductOrder_SaleProductId",
                table: "SaleProductOrders",
                newName: "IX_SaleProductOrders_SaleProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SaleProductOrder_SaleOrderId",
                table: "SaleProductOrders",
                newName: "IX_SaleProductOrders_SaleOrderId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "SaleProductOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleProductOrders",
                table: "SaleProductOrders",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "WqNvlKN9RhV051Y4L3FNVMUeC3spJb2pl40z36gapuw=", "RxDmc20FgTlXqqcYuWb+tg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProductOrders_SaleOrders_SaleOrderId",
                table: "SaleProductOrders",
                column: "SaleOrderId",
                principalTable: "SaleOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProductOrders_SaleProducts_SaleProductId",
                table: "SaleProductOrders",
                column: "SaleProductId",
                principalTable: "SaleProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleProductOrders_SaleOrders_SaleOrderId",
                table: "SaleProductOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleProductOrders_SaleProducts_SaleProductId",
                table: "SaleProductOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleProductOrders",
                table: "SaleProductOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SaleProductOrders");

            migrationBuilder.RenameTable(
                name: "SaleProductOrders",
                newName: "SaleProductOrder");

            migrationBuilder.RenameIndex(
                name: "IX_SaleProductOrders_SaleProductId",
                table: "SaleProductOrder",
                newName: "IX_SaleProductOrder_SaleProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SaleProductOrders_SaleOrderId",
                table: "SaleProductOrder",
                newName: "IX_SaleProductOrder_SaleOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleProductOrder",
                table: "SaleProductOrder",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "MPVDEjHuhXpVk+QB14zY+nRZu/8F9Dpsl8kf1RUqLsw=", "vLrTEvC2lGuJNlDCzEeTnw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProductOrder_SaleOrders_SaleOrderId",
                table: "SaleProductOrder",
                column: "SaleOrderId",
                principalTable: "SaleOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProductOrder_SaleProducts_SaleProductId",
                table: "SaleProductOrder",
                column: "SaleProductId",
                principalTable: "SaleProducts",
                principalColumn: "Id");
        }
    }
}
