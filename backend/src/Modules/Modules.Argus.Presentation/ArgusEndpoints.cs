using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Argus.Application.Abstractions;
using Modules.Argus.Application.Models;
using Modules.Argus.Presentation.Contracts;

namespace Modules.Argus.Presentation;

public static class ArgusEndpoints
{
    public static IEndpointRouteBuilder
        MapArgusEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group =
            endpoints
                .MapGroup("/api/chatbot")
                .WithTags("Chatbot")
                .AllowAnonymous();

        group
            .MapPost("/ask", AskAsync)
            .WithName("AskArgus")
            .WithSummary(
                "Kérdés küldése Argusnak.")
            .RequireRateLimiting(
                ArgusPresentationExtensions
                    .RateLimitPolicyName);

        return endpoints;
    }

    private static async Task<IResult> AskAsync(
        AskArgusRequest request,
        IArgusAnswerService answerService,
        CancellationToken cancellationToken)
    {
        string question =
            request.Question?.Trim() ??
            string.Empty;

        if (string.IsNullOrWhiteSpace(question))
        {
            return Results.ValidationProblem(
                new Dictionary<
                    string,
                    string[]>
                {
                    ["question"] =
                    [
                        "A kérdés megadása kötelező."
                    ]
                });
        }

        if (question.Length > 300)
        {
            return Results.ValidationProblem(
                new Dictionary<
                    string,
                    string[]>
                {
                    ["question"] =
                    [
                        "A kérdés legfeljebb " +
                        "300 karakter lehet."
                    ]
                });
        }

        ArgusAnswer answer =
            await answerService.AskAsync(
                question,
                cancellationToken);

        ArgusSourceResponse? source =
            answer.SourcePath is null
                ? null
                : new ArgusSourceResponse(
                    answer.SourceTitle ??
                        string.Empty,
                    answer.SourcePath);

        return Results.Ok(
            new AskArgusResponse(
                answer.Answer,
                answer.Confidence,
                answer.IsFallback,
                source));
    }
}