using BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace WebApi.ExceptionHandling;

internal sealed class ConflictExceptionHandler
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        string message;

        if (exception is ConflictException conflict)
        {
            message = conflict.Message;
        }
        else if (
            exception is DbUpdateConcurrencyException)
        {
            message =
                "A tartalom időközben megváltozott. " +
                "A művelet nem sikerült. " +
                "Másold ki a módosításaidat, " +
                "mielőtt újratöltöd a tartalmat.";
        }
        else
        {
            return false;
        }

        await Results.Problem(
                statusCode:
                    StatusCodes.Status409Conflict,
                title: message)
            .ExecuteAsync(httpContext);

        return true;
    }
}