using Application.Commands.Borrowings.BorrowBook;
using Application.DTOs;
using Application.UseCases.Commands.Borrowings.ReturnBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingsController(IMediator mediator, ILogger<BorrowingsController> logger)
    : ControllerBase
{
    [HttpPost("borrow")]
    public async Task<ActionResult<BorrowingDto>> BorrowBook([FromBody] BorrowBookCommand command)
    {
        logger.LogInformation(
            "Borrowing book ID {BookId} for borrower ID {BorrowerId}",
            command.BookId,
            command.BorrowerId
        );

        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("return")]
    public async Task<ActionResult<BorrowingDto>> ReturnBook([FromBody] ReturnBookCommand command)
    {
        logger.LogInformation("Returning borrowing ID {BorrowingId}", command.BorrowingId);

        var result = await mediator.Send(command);
        return Ok(result);
    }
}
