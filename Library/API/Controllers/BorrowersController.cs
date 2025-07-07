using Application.DTOs;
using Application.UseCases.Commands.Borrowers.CreateBorrower;
using Application.UseCases.Commands.Borrowers.DeactivateBorrower;
using Application.UseCases.Commands.Borrowers.DeleteBorrower;
using Application.UseCases.Commands.Borrowers.UpdateBorrower;
using Application.UseCases.Queries.Borrowers.CanBorrowerBorrow;
using Application.UseCases.Queries.Borrowers.GetActiveBorrowers;
using Application.UseCases.Queries.Borrowers.GetAllBorrowers;
using Application.UseCases.Queries.Borrowers.GetBorrowerById;
using Application.UseCases.Queries.Borrowers.GetBorrowerHistory;
using Application.UseCases.Queries.Borrowers.GetBorrowerReadingRate;
using Application.UseCases.Queries.Borrowers.GetCurrentlyBorrowedBooks;
using Application.UseCases.Queries.Borrowers.GetTopBorrowers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowersController(IMediator mediator, ILogger<BorrowersController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<BorrowerDto>>> GetAllBorrowers()
    {
        logger.LogInformation("Getting all borrowers");
        var query = new GetAllBorrowersQuery();
        var borrowers = await mediator.Send(query);
        return Ok(borrowers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BorrowerDto>> GetBorrowerById(int id)
    {
        logger.LogInformation("Getting borrower with ID: {BorrowerId}", id);
        var query = new GetBorrowerByIdQuery(id);
        var borrower = await mediator.Send(query);
        return Ok(borrower);
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<BorrowerSummaryDto>>> GetActiveBorrowers()
    {
        logger.LogInformation("Getting active borrowers");
        var query = new GetActiveBorrowersQuery();
        var borrowers = await mediator.Send(query);
        return Ok(borrowers);
    }

    [HttpGet("top-borrowers")]
    public async Task<ActionResult<List<TopBorrowerDto>>> GetTopBorrowers(
        [FromQuery] int count = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
    )
    {
        logger.LogInformation("Getting top {Count} borrowers", count);
        var query = new GetTopBorrowersQuery(count, startDate, endDate);
        var borrowers = await mediator.Send(query);
        return Ok(borrowers);
    }

    [HttpGet("{id:int}/history")]
    public async Task<ActionResult<List<BorrowingHistoryDto>>> GetBorrowerHistory(
        int id,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
    )
    {
        logger.LogInformation("Getting borrowing history for borrower ID: {BorrowerId}", id);
        var query = new GetBorrowerHistoryQuery(id, startDate, endDate);
        var history = await mediator.Send(query);
        return Ok(history);
    }

    [HttpGet("{id:int}/current-books")]
    public async Task<ActionResult<List<BookSummaryDto>>> GetCurrentlyBorrowedBooks(int id)
    {
        logger.LogInformation("Getting currently borrowed books for borrower ID: {BorrowerId}", id);
        var query = new GetCurrentlyBorrowedBooksQuery(id);
        var books = await mediator.Send(query);
        return Ok(books);
    }

    [HttpGet("{id:int}/reading-rate")]
    public async Task<ActionResult<object>> GetBorrowerReadingRate(int id)
    {
        logger.LogInformation("Getting reading rate for borrower ID: {BorrowerId}", id);
        var query = new GetBorrowerReadingRateQuery(id);
        var readingRate = await mediator.Send(query);

        return Ok(
            new
            {
                BorrowerId = id,
                AverageReadingRate = readingRate,
                Unit = "pages per day",
            }
        );
    }

    [HttpGet("{id:int}/can-borrow")]
    public async Task<ActionResult<object>> CanBorrowerBorrow(int id)
    {
        logger.LogInformation("Checking if borrower ID {BorrowerId} can borrow", id);
        var query = new CanBorrowerBorrowQuery(id);
        var canBorrow = await mediator.Send(query);

        return Ok(new { BorrowerId = id, CanBorrow = canBorrow });
    }

    [HttpPost]
    public async Task<ActionResult<BorrowerDto>> CreateBorrower(
        [FromBody] CreateBorrowerDto createBorrowerDto
    )
    {
        logger.LogInformation(
            "Creating new borrower: {FirstName} {LastName}",
            createBorrowerDto.FirstName,
            createBorrowerDto.LastName
        );

        var command = new CreateBorrowerCommand(
            createBorrowerDto.FirstName,
            createBorrowerDto.LastName,
            createBorrowerDto.Email,
            createBorrowerDto.PhoneNumber,
            createBorrowerDto.MaxBorrowLimit
        );

        var borrower = await mediator.Send(command);
        return CreatedAtAction(nameof(GetBorrowerById), new { id = borrower.Id }, borrower);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BorrowerDto>> UpdateBorrower(
        int id,
        [FromBody] UpdateBorrowerDto updateBorrowerDto
    )
    {
        logger.LogInformation("Updating borrower with ID: {BorrowerId}", id);

        var command = new UpdateBorrowerCommand(
            id,
            updateBorrowerDto.FirstName,
            updateBorrowerDto.LastName,
            updateBorrowerDto.PhoneNumber,
            updateBorrowerDto.MaxBorrowLimit
        );

        var borrower = await mediator.Send(command);
        return Ok(borrower);
    }

    [HttpPut("{id:int}/deactivate")]
    public async Task<ActionResult<BorrowerDto>> DeactivateBorrower(int id)
    {
        logger.LogInformation("Deactivating borrower with ID: {BorrowerId}", id);
        var command = new DeactivateBorrowerCommand(id);
        var borrower = await mediator.Send(command);
        return Ok(borrower);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBorrower(int id)
    {
        logger.LogInformation("Deleting borrower with ID: {BorrowerId}", id);
        var command = new DeleteBorrowerCommand(id);
        var result = await mediator.Send(command);

        if (!result)
        {
            logger.LogWarning("Borrower with ID {BorrowerId} not found for deletion", id);
            return NotFound($"Borrower with ID {id} not found");
        }

        return NoContent();
    }
}
