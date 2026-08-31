using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.ExceptionHandling;

internal sealed class NotFoundExceptionHandler
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not KeyNotFoundException)
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