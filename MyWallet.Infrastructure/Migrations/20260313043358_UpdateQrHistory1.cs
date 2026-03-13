using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQrHistory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "QRHistories");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "QRHistories",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "VND");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "QRHistories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "CREATED");

            migrationBuilder.AddColumn<string>(
                name: "TransactionRef",
                table: "QRHistories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QRHistory_TransactionRef",
                table: "QRHistories",
                column: "TransactionRef",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QRHistory_TransactionRef",
                table: "QRHistories");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "QRHistories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "QRHistories");

            migrationBuilder.DropColumn(
                name: "TransactionRef",
                table: "QRHistories");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "QRHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
