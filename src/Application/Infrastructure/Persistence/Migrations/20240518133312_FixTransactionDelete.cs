using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTransactionDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetail_Transactions_TransactionId",
                table: "TransactionDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionDetail",
                table: "TransactionDetail");

            migrationBuilder.RenameTable(
                name: "TransactionDetail",
                newName: "TransactionDetails");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetail_TransactionId",
                table: "TransactionDetails",
                newName: "IX_TransactionDetails_TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionDetails",
                table: "TransactionDetails",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "5N/Gx2JmkQ3bvEtoxX6rhd7LTjwB666DkKIVwhJMp1Q=", "Xdj4CyyU1RsxKUfNKrOLsA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Transactions_TransactionId",
                table: "TransactionDetails",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Transactions_TransactionId",
                table: "TransactionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionDetails",
                table: "TransactionDetails");

            migrationBuilder.RenameTable(
                name: "TransactionDetails",
                newName: "TransactionDetail");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetails_TransactionId",
                table: "TransactionDetail",
                newName: "IX_TransactionDetail_TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionDetail",
                table: "TransactionDetail",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "UserCredentials",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "WqNvlKN9RhV051Y4L3FNVMUeC3spJb2pl40z36gapuw=", "RxDmc20FgTlXqqcYuWb+tg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetail_Transactions_TransactionId",
                table: "TransactionDetail",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }
    }
}
