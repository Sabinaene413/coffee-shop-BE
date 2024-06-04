using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetOnCoffeShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Budget",
                table: "CoffeeShops",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "X2cmCrQsXqcvJlnzTxq2F86nHVWq3XdiPgUtfvC2bJo=", "IIGciIVkIzX4mQVoGjKspw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "CoffeeShops");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "v5eQAHdb7GSMTm6zFhaTu80dFCsEjEVZMt9LFYCvlq4=", "ozJcKZ4CNA0kUSbyaxjWaA==" });
        }
    }
}
