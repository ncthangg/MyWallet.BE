using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountIndex3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QRHistories_Account_Paging",
                table: "QRHistories");

            migrationBuilder.DropIndex(
                name: "IX_QRHistories_User_Paging",
                table: "QRHistories");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BankCode",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId_IsActive",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId_IsPinned_CreatedAt",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QRHistories");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_UserId_AccountNumber_BankCode_Provider",
                table: "Accounts",
                newName: "IX_Accounts_UserId_AccountNumber_BankCode_Provider_Unique");

            migrationBuilder.CreateIndex(
                name: "IX_QRHistories_Account_Paging",
                table: "QRHistories",
                columns: new[] { "AccountId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_QRHistories_User_Paging",
                table: "QRHistories",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BankCode",
                table: "Accounts",
                column: "BankCode");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Status_DeletedAt_CreatedAt",
                table: "Accounts",
                columns: new[] { "Status", "DeletedAt", "CreatedAt" })
                .Annotation("SqlServer:Include", new[] { "AccountNumber", "AccountHolder", "BankCode", "Provider", "Balance", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId_Filter_Sort",
                table: "Accounts",
                columns: new[] { "UserId", "Status", "DeletedAt", "IsPinned", "CreatedAt" })
                .Annotation("SqlServer:Include", new[] { "AccountNumber", "AccountHolder", "BankCode", "Provider", "Balance", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QRHistories_Account_Paging",
                table: "QRHistories");

            migrationBuilder.DropIndex(
                name: "IX_QRHistories_User_Paging",
                table: "QRHistories");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BankCode",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Status_DeletedAt_CreatedAt",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId_Filter_Sort",
                table: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_UserId_AccountNumber_BankCode_Provider_Unique",
                table: "Accounts",
                newName: "IX_Accounts_UserId_AccountNumber_BankCode_Provider");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QRHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Accounts",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_QRHistories_Account_Paging",
                table: "QRHistories",
                columns: new[] { "AccountId", "CreatedAt" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_QRHistories_User_Paging",
                table: "QRHistories",
                columns: new[] { "UserId", "CreatedAt" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BankCode",
                table: "Accounts",
                column: "BankCode")
                .Annotation("SqlServer:Include", new[] { "BankName" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId_IsActive",
                table: "Accounts",
                columns: new[] { "UserId", "IsActive" })
                .Annotation("SqlServer:Include", new[] { "AccountNumber", "AccountHolder", "BankCode", "BankName", "Provider", "Balance", "IsPinned", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId_IsPinned_CreatedAt",
                table: "Accounts",
                columns: new[] { "UserId", "IsPinned", "CreatedAt" })
                .Annotation("SqlServer:Include", new[] { "AccountNumber", "AccountHolder", "BankCode", "BankName", "Provider", "Balance", "IsActive" });
        }
    }
}
