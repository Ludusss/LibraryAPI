using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);

        builder.Property(b => b.Author).IsRequired().HasMaxLength(100);

        builder.Property(b => b.PageCount).IsRequired();

        builder.Property(b => b.PublicationDate).IsRequired();

        builder.Property(b => b.TotalCopies).IsRequired();

        builder.Property(b => b.AvailableCopies).IsRequired();

        builder.Property(b => b.Genre).HasMaxLength(50);

        builder.Property(b => b.Description).HasMaxLength(1000);

        // Ignore domain events
        builder.Ignore(b => b.DomainEvents);
    }
}
