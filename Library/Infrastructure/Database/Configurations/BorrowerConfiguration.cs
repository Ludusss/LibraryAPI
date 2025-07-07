using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class BorrowerConfiguration : IEntityTypeConfiguration<Borrower>
{
    public void Configure(EntityTypeBuilder<Borrower> builder)
    {
        builder.ToTable("Borrowers");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.FirstName).IsRequired().HasMaxLength(50);

        builder.Property(b => b.LastName).IsRequired().HasMaxLength(50);

        builder.Property(b => b.PhoneNumber).HasMaxLength(20);

        builder.Property(b => b.MembershipDate).IsRequired();

        builder.Property(b => b.IsActive).IsRequired();

        builder.Property(b => b.MaxBorrowLimit).IsRequired();

        // Ignore computed properties
        builder.Ignore(b => b.FullName);
        builder.Ignore(b => b.CurrentBorrowedCount);
        builder.Ignore(b => b.CanBorrow);
        builder.Ignore(b => b.DomainEvents);
    }
}
