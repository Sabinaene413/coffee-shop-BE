using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationIdToEmployeeAddRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "zHGcQmOP0yqEhGiECMF0FozP6LzYMcIhA4HmqIhyiuw=", "Vj/oq8NQX7GDJzdojs3NpA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LocationId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "D3/v9BGjPjSluefqYfh3DHl8wZ2CYHGuQkbNuB5f9cI=", "SKloB1qLi+ecYyZLhaoAkA==" });
        }
    }
}
