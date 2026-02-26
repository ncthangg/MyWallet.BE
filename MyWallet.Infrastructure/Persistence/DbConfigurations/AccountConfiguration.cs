using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWallet.Domain.Entities;

namespace MyWallet.Infrastructure.Persistence.DbConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Table name
            builder.ToTable("Accounts");

            // Primary key
            builder.HasKey(a => a.Id)
                .IsClustered();

            // Properties configuration
            builder.Property(a => a.UserId)
                .IsRequired();

            builder.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.AccountHolder)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.BankCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.BankName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.AccountType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Balance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            builder.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // BaseEntity properties
            builder.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.Status)
                .HasDefaultValue(true);

            // Indexes
            // Uniqueness: 1 user không được trùng AccountNumber
            builder.HasIndex(a => new { a.UserId, a.AccountNumber })
                .IsUnique()
                .HasDatabaseName("IX_Accounts_UserId_AccountNumber");

            // Truy vấn phổ biến: lấy danh sách account theo user (covering index)
            builder.HasIndex(a => a.UserId)
                .HasDatabaseName("IX_Accounts_UserId")
                .IncludeProperties(a => new
                {
                    a.AccountNumber,
                    a.AccountHolder,
                    a.BankCode,
                    a.BankName,
                    a.AccountType,
                    a.Balance,
                    a.IsActive
                });

            // Truy vấn lọc account đang active theo user
            builder.HasIndex(a => new { a.UserId, a.IsActive })
                .HasDatabaseName("IX_Accounts_UserId_IsActive")
                .IncludeProperties(a => new
                {
                    a.AccountNumber,
                    a.AccountHolder,
                    a.BankCode,
                    a.BankName,
                    a.AccountType,
                    a.Balance
                });

            builder.HasIndex(a => a.BankCode)
                .HasDatabaseName("IX_Accounts_BankCode")
                .IncludeProperties(a => new { a.BankName });

            // Relationships
            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.QRHistories)
                .WithOne(qr => qr.Account)
                .HasForeignKey(qr => qr.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
