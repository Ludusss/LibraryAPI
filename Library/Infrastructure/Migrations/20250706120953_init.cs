using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Isbn = table.Column<string>(type: "TEXT", nullable: false),
                    PageCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalCopies = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailableCopies = table.Column<int>(type: "INTEGER", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Borrowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    MembershipDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxBorrowLimit = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Borrowings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BorrowerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsReturned = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Borrowings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Borrowings_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AvailableCopies", "Description", "Genre", "Isbn", "PageCount", "PublicationDate", "Title", "TotalCopies" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", 3, "A classic American novel set in the Jazz Age.", "Classic Fiction", "9780743273565", 180, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Great Gatsby", 3 },
                    { 2, "Harper Lee", 2, "A gripping tale of racial injustice and childhood innocence.", "Classic Fiction", "9780061120084", 376, new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "To Kill a Mockingbird", 2 },
                    { 3, "George Orwell", 4, "A dystopian social science fiction novel.", "Dystopian Fiction", "9780452284234", 328, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "1984", 4 },
                    { 4, "Jane Austen", 2, "A romantic novel of manners.", "Romance", "9780141439518", 432, new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pride and Prejudice", 2 },
                    { 5, "J.D. Salinger", 3, "A controversial novel about teenage rebellion.", "Coming-of-Age Fiction", "9780316769174", 234, new DateTime(1951, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Catcher in the Rye", 3 }
                });

            migrationBuilder.InsertData(
                table: "Borrowers",
                columns: new[] { "Id", "Email", "FirstName", "IsActive", "LastName", "MaxBorrowLimit", "MembershipDate", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "john.doe@example.com", "John", true, "Doe", 5, new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1234567890" },
                    { 2, "jane.smith@example.com", "Jane", true, "Smith", 5, new DateTime(2023, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1234567891" },
                    { 3, "bob.johnson@example.com", "Bob", true, "Johnson", 5, new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1234567892" },
                    { 4, "alice.brown@example.com", "Alice", true, "Brown", 5, new DateTime(2023, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1234567893" },
                    { 5, "charlie.davis@example.com", "Charlie", true, "Davis", 5, new DateTime(2023, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1234567894" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn",
                table: "Books",
                column: "Isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrowers_Email",
                table: "Borrowers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_BookId",
                table: "Borrowings",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_BorrowerId_BookId_BorrowDate",
                table: "Borrowings",
                columns: new[] { "BorrowerId", "BookId", "BorrowDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borrowings");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Borrowers");
        }
    }
}
