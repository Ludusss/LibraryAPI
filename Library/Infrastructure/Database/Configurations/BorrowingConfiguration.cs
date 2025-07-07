using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
{
    public void Configure(EntityTypeBuilder<Borrowing> builder)
    {
        builder.ToTable("Borrowings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BorrowerId).IsRequired();

        builder.Property(b => b.BookId).IsRequired();

        builder.Property(b => b.BorrowDate).IsRequired();

        builder.Property(b => b.ReturnDate).IsRequired(false);

        builder.Property(b => b.DueDate).IsRequired();

        builder.Property(b => b.IsReturned).IsRequired();

        // Ignore computed properties
        builder.Ignore(b => b.IsOverdue);
        builder.Ignore(b => b.DomainEvents);
    }
}
