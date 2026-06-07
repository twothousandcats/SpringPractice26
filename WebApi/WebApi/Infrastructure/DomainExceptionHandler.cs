using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Infrastructure;

public class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        (int Status, string Title)? mapped = Map( exception );
        if ( mapped == null )
        {
            return false;
        }

        ProblemDetails problemDetails = new ProblemDetails
        {
            Status = mapped.Value.Status,
            Title = mapped.Value.Title,
            Detail = exception.Message,
        };

        httpContext.Response.StatusCode = mapped.Value.Status;
        await httpContext.Response.WriteAsJsonAsync( problemDetails, cancellationToken );

        return true;
    }

    private (int Status, string Title)? Map( Exception exception )
    {
        return exception switch
        {
            EntityNotFoundException => ( StatusCodes.Status404NotFound, "Entity not found" ),
            BookingValidationException => ( StatusCodes.Status422UnprocessableEntity, "Business rule violated" ),
            InvalidOperationException => ( StatusCodes.Status409Conflict, "Operation conflict" ),
            ArgumentException => ( StatusCodes.Status400BadRequest, "Invalid argument" ),
            _ => null,
        };
    }
}