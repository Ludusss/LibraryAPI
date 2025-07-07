using Domain.ValueObjects;

namespace Unit.Domain.ValueObjects;

public class IsbnTests
{
    [Theory]
    [InlineData("978-0-7475-3269-9")] // Harry Potter and the Philosopher's Stone
    [InlineData("9780747532699")] // Same ISBN without dashes
    [InlineData("0-7475-3269-9")] // ISBN-10 version
    [InlineData("0747532699")] // ISBN-10 without dashes
    public void Create_WithValidIsbn_ShouldCreateIsbn(string validIsbn)
    {
        // Act
        var isbn = Isbn.Create(validIsbn);

        // Assert
        isbn.Should().NotBeNull();
        isbn.Value.Should().Be(validIsbn);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("978-0-123456-78")]
    [InlineData("invalid-isbn")]
    public void Create_WithInvalidIsbn_ShouldThrowArgumentException(string? invalidIsbn)
    {
        // Act & Assert
        var action = () => Isbn.Create(invalidIsbn);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid ISBN format.*");
    }

    [Fact]
    public void Equals_WithSameIsbn_ShouldReturnTrue()
    {
        // Arrange
        var isbn1 = Isbn.Create("978-0-7475-3269-9");
        var isbn2 = Isbn.Create("978-0-7475-3269-9");

        // Act & Assert
        isbn1.Should().Be(isbn2);
        isbn1.Equals(isbn2).Should().BeTrue();
        (isbn1 == isbn2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentIsbn_ShouldReturnFalse()
    {
        // Arrange
        var isbn1 = Isbn.Create("978-0-7475-3269-9");
        var isbn2 = Isbn.Create("978-0-7475-3270-5"); // Different valid ISBN

        // Act & Assert
        isbn1.Should().NotBe(isbn2);
        isbn1.Equals(isbn2).Should().BeFalse();
        (isbn1 != isbn2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WithSameIsbn_ShouldReturnSameHashCode()
    {
        // Arrange
        var isbn1 = Isbn.Create("978-0-7475-3269-9");
        var isbn2 = Isbn.Create("978-0-7475-3269-9");

        // Act & Assert
        isbn1.GetHashCode().Should().Be(isbn2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnIsbnValue()
    {
        // Arrange
        var isbnValue = "978-0-7475-3269-9";
        var isbn = Isbn.Create(isbnValue);

        // Act & Assert
        isbn.ToString().Should().Be(isbnValue);
    }
}
