using Microsoft.AspNetCore.Diagnostics;
using BuildingBlocks.Application.Exceptions;

namespace WebApi.ExceptionHandling;

internal sealed class NotFoundExceptionHandler
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException)
        {
            return false;
        }

        await Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: exception.Message)
            .ExecuteAsync(httpContext);

        return true;
    }
}