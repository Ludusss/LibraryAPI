using Application.Commands.Books.CreateBook;
using Application.DTOs;
using Application.UseCases.Queries.Books.GetAllBooks;
using Application.UseCases.Queries.Books.GetBookById;
using Application.UseCases.Queries.Books.GetMostBorrowedBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IMediator mediator, ILogger<BooksController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAllBooks()
    {
        logger.LogInformation("Getting all books");
        var query = new GetAllBooksQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        logger.LogInformation("Getting book with ID: {BookId}", id);
        var query = new GetBookByIdQuery(id);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("most-borrowed")]
    public async Task<ActionResult<List<MostBorrowedBookDto>>> GetMostBorrowedBooks(
        [FromQuery] int count = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
    )
    {
        logger.LogInformation("Getting most borrowed books with count: {Count}", count);
        var query = new GetMostBorrowedBooksQuery(count, startDate, endDate);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}/availability")]
    public async Task<ActionResult<BookDto>> GetBookAvailability(int id)
    {
        logger.LogInformation("Getting availability for book ID: {BookId}", id);
        var query = new GetBookByIdQuery(id);
        var book = await mediator.Send(query);

        return Ok(book);
    }

    [HttpGet("{id:int}/reading-rate")]
    public async Task<ActionResult<BookDto>> GetBookReadingRate(int id)
    {
        logger.LogInformation("Getting reading rate for book ID: {BookId}", id);
        var query = new GetBookByIdQuery(id);
        var book = await mediator.Send(query);

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookCommand command)
    {
        logger.LogInformation("Creating new book: {Title}", command.Title);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetBook), new { id = result.Id }, result);
    }
}
