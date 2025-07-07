using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrower> Borrowers { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new BorrowerConfiguration());
        modelBuilder.ApplyConfiguration(new BorrowingConfiguration());

        // Configure value objects
        modelBuilder
            .Entity<Book>()
            .Property(b => b.Isbn)
            .HasConversion(isbn => isbn.Value, value => Isbn.Create(value));

        modelBuilder
            .Entity<Borrower>()
            .Property(b => b.Email)
            .HasConversion(email => email.Value, value => Email.Create(value));

        // Configure relationships
        modelBuilder
            .Entity<Borrowing>()
            .HasOne(b => b.Book)
            .WithMany(b => b.Borrowings)
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Borrowing>()
            .HasOne(b => b.Borrower)
            .WithMany(b => b.Borrowings)
            .HasForeignKey(b => b.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure indexes
        modelBuilder.Entity<Book>().HasIndex(b => b.Isbn).IsUnique();

        modelBuilder.Entity<Borrower>().HasIndex(b => b.Email).IsUnique();

        modelBuilder
            .Entity<Borrowing>()
            .HasIndex(b => new
            {
                b.BorrowerId,
                b.BookId,
                b.BorrowDate,
            });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Books
        modelBuilder
            .Entity<Book>()
            .HasData(
                new
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Isbn = Isbn.Create("9780743273565"),
                    PageCount = 180,
                    PublicationDate = new DateTime(1925, 4, 10),
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Genre = "Classic Fiction",
                    Description = "A classic American novel set in the Jazz Age.",
                },
                new
                {
                    Id = 2,
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Isbn = Isbn.Create("9780061120084"),
                    PageCount = 376,
                    PublicationDate = new DateTime(1960, 7, 11),
                    TotalCopies = 2,
                    AvailableCopies = 2,
                    Genre = "Classic Fiction",
                    Description = "A gripping tale of racial injustice and childhood innocence.",
                },
                new
                {
                    Id = 3,
                    Title = "1984",
                    Author = "George Orwell",
                    Isbn = Isbn.Create("9780452284234"),
                    PageCount = 328,
                    PublicationDate = new DateTime(1949, 6, 8),
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Genre = "Dystopian Fiction",
                    Description = "A dystopian social science fiction novel.",
                },
                new
                {
                    Id = 4,
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Isbn = Isbn.Create("9780141439518"),
                    PageCount = 432,
                    PublicationDate = new DateTime(1813, 1, 28),
                    TotalCopies = 2,
                    AvailableCopies = 2,
                    Genre = "Romance",
                    Description = "A romantic novel of manners.",
                },
                new
                {
                    Id = 5,
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    Isbn = Isbn.Create("9780316769174"),
                    PageCount = 234,
                    PublicationDate = new DateTime(1951, 7, 16),
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Genre = "Coming-of-Age Fiction",
                    Description = "A controversial novel about teenage rebellion.",
                }
            );

        // Seed Borrowers
        modelBuilder
            .Entity<Borrower>()
            .HasData(
                new
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = Email.Create("john.doe@example.com"),
                    PhoneNumber = "+1234567890",
                    MembershipDate = new DateTime(2023, 1, 15),
                    IsActive = true,
                    MaxBorrowLimit = 5,
                },
                new
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = Email.Create("jane.smith@example.com"),
                    PhoneNumber = "+1234567891",
                    MembershipDate = new DateTime(2023, 2, 20),
                    IsActive = true,
                    MaxBorrowLimit = 5,
                },
                new
                {
                    Id = 3,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = Email.Create("bob.johnson@example.com"),
                    PhoneNumber = "+1234567892",
                    MembershipDate = new DateTime(2023, 3, 10),
                    IsActive = true,
                    MaxBorrowLimit = 5,
                },
                new
                {
                    Id = 4,
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = Email.Create("alice.brown@example.com"),
                    PhoneNumber = "+1234567893",
                    MembershipDate = new DateTime(2023, 4, 5),
                    IsActive = true,
                    MaxBorrowLimit = 5,
                },
                new
                {
                    Id = 5,
                    FirstName = "Charlie",
                    LastName = "Davis",
                    Email = Email.Create("charlie.davis@example.com"),
                    PhoneNumber = "+1234567894",
                    MembershipDate = new DateTime(2023, 5, 12),
                    IsActive = true,
                    MaxBorrowLimit = 5,
                }
            );
    }
}
