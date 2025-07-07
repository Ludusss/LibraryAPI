using System.Net;
using Domain.Common;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var response = exception switch
        {
            KeyNotFoundException ex => CreateErrorResponse(
                HttpStatusCode.NotFound,
                "Resource not found",
                ex.Message
            ),

            ArgumentException ex => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Invalid argument",
                ex.Message
            ),

            InvalidOperationException ex => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Invalid operation",
                ex.Message
            ),

            ValidationException ex => CreateValidationErrorResponse(ex),

            BookNotAvailableException ex => CreateErrorResponse(
                HttpStatusCode.Conflict,
                "Book not available",
                ex.Message
            ),

            BorrowLimitExceededException ex => CreateErrorResponse(
                HttpStatusCode.Conflict,
                "Borrow limit exceeded",
                ex.Message
            ),

            DomainException ex => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Business rule violation",
                ex.Message
            ),

            UnauthorizedAccessException ex => CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                ex.Message
            ),

            _ => CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                "Internal server error",
                "An unexpected error occurred. Please try again later."
            ),
        };

        context.Result = response;
        context.ExceptionHandled = true;
    }

    private ObjectResult CreateErrorResponse(HttpStatusCode statusCode, string title, string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = (int)statusCode,
            Instance = null,
        };

        return new ObjectResult(problemDetails) { StatusCode = (int)statusCode };
    }

    private ObjectResult CreateValidationErrorResponse(ValidationException validationException)
    {
        var validationProblemDetails = new ValidationProblemDetails
        {
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Status = (int)HttpStatusCode.BadRequest,
            Instance = null,
        };

        foreach (var error in validationException.Errors)
        {
            if (validationProblemDetails.Errors.ContainsKey(error.PropertyName))
            {
                validationProblemDetails.Errors[error.PropertyName] = validationProblemDetails
                    .Errors[error.PropertyName]
                    .Concat(new[] { error.ErrorMessage })
                    .ToArray();
            }
            else
            {
                validationProblemDetails.Errors.Add(
                    error.PropertyName,
                    new[] { error.ErrorMessage }
                );
            }
        }

        return new ObjectResult(validationProblemDetails)
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
        };
    }
}
