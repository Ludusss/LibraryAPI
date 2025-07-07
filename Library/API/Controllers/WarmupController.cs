using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarmupController(ILogger<WarmupController> logger) : ControllerBase
{
    [HttpGet("is-power-of-two/{bookId:int}")]
    public ActionResult<object> CheckIfBookIdIsPowerOfTwo(int bookId)
    {
        logger.LogInformation("Checking if book ID {BookId} is a power of two", bookId);

        var isPowerOfTwo = WarmupTasks.IsBookIdPowerOfTwo(bookId);

        return Ok(
            new
            {
                BookId = bookId,
                IsPowerOfTwo = isPowerOfTwo,
                Message = isPowerOfTwo
                    ? $"Book ID {bookId} is a power of two"
                    : $"Book ID {bookId} is not a power of two",
            }
        );
    }

    [HttpGet("reverse-title")]
    public ActionResult<object> ReverseBookTitle([FromQuery] string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest("Title parameter is required");
        }

        logger.LogInformation("Reversing book title: {Title}", title);

        var reversedTitle = WarmupTasks.ReverseBookTitle(title);

        return Ok(new { OriginalTitle = title, ReversedTitle = reversedTitle });
    }

    [HttpGet("generate-replicas")]
    public ActionResult<object> GenerateBookTitleReplicas(
        [FromQuery] string title,
        [FromQuery] int count = 1
    )
    {
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest("Title parameter is required");
        }

        if (count <= 0)
        {
            return BadRequest("Count must be greater than 0");
        }

        logger.LogInformation("Generating {Count} replicas of book title: {Title}", count, title);

        var replicas = WarmupTasks.GenerateBookTitleReplicas(title, count);

        return Ok(
            new
            {
                OriginalTitle = title,
                ReplicaCount = count,
                Result = replicas,
            }
        );
    }

    [HttpGet("odd-book-ids")]
    public ActionResult<object> GetOddNumberedBookIds()
    {
        logger.LogInformation("Getting odd-numbered book IDs between 1 and 100");

        var oddIds = WarmupTasks.GetOddNumberedBookIds();

        return Ok(
            new
            {
                Description = "Odd-numbered Book IDs between 1 and 100 (simulating limited edition collections)",
                oddIds.Count,
                OddBookIds = oddIds,
            }
        );
    }

    [HttpGet("demo")]
    public ActionResult<object> DemonstrateWarmupTasks()
    {
        logger.LogInformation("Demonstrating all warmup tasks");

        var powerOfTwoDemo = WarmupTasks.IsBookIdPowerOfTwo(8);
        var reverseDemo = WarmupTasks.ReverseBookTitle("Moby Dick");
        var replicaDemo = WarmupTasks.GenerateBookTitleReplicas("Read", 3);
        var oddIdsDemo = WarmupTasks.GetOddNumberedBookIds().Take(20).ToList();

        return Ok(
            new
            {
                Description = "Demonstration of all warmup tasks",
                Tasks = new
                {
                    PowerOfTwo = new
                    {
                        Task = "Check if Book ID 8 is a power of two",
                        Result = powerOfTwoDemo,
                        Expected = true,
                    },
                    ReverseTitle = new
                    {
                        Task = "Reverse 'Moby Dick'",
                        Result = reverseDemo,
                        Expected = "kciD yboM",
                    },
                    GenerateReplicas = new
                    {
                        Task = "Generate 3 replicas of 'Read'",
                        Result = replicaDemo,
                        Expected = "ReadReadRead",
                    },
                    OddBookIds = new
                    {
                        Task = "Get first 20 odd-numbered book IDs",
                        Result = oddIdsDemo,
                        TotalCount = 50,
                    },
                },
            }
        );
    }
}
