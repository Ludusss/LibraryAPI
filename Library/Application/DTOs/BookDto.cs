namespace Application.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public DateTime PublicationDate { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public int BorrowedCopies { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public double AverageReadingRate { get; set; }
}

public class BookSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int AvailableCopies { get; set; }
    public int TotalCopies { get; set; }
    public bool IsAvailable { get; set; }
}

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public DateTime PublicationDate { get; set; }
    public int TotalCopies { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
} 