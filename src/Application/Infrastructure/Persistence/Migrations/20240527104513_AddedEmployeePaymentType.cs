using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmployeePaymentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PLATA", null, null });

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "v5eQAHdb7GSMTm6zFhaTu80dFCsEjEVZMt9LFYCvlq4=", "ozJcKZ4CNA0kUSbyaxjWaA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "xoeNy2jtzFS3EMyl54INFjQchsp5fHRcDajAvgedZV8=", "S1N4faTQUQAVQxjCI7W08g==" });
        }
    }
}
