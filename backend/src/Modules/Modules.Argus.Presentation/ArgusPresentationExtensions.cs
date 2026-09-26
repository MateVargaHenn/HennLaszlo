using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Modules.Argus.Application.Abstractions;
using Modules.Argus.Application.Answering;

namespace Modules.Argus.Presentation;

public static class ArgusPresentationExtensions
{
    internal const string
        RateLimitPolicyName =
            "argus-chatbot";

    public static IServiceCollection
        AddArgusPresentation(
            this IServiceCollection services)
    {
        services.AddSingleton<
            IArgusAnswerService,
            ArgusAnswerService>();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode =
                StatusCodes
                    .Status429TooManyRequests;

            options.AddPolicy(
                RateLimitPolicyName,
                httpContext =>
                {
                    string partitionKey =
                        httpContext
                            .Connection
                            .RemoteIpAddress?
                            .ToString()
                        ?? "unknown";

                    return RateLimitPartition
                        .GetFixedWindowLimiter(
                            partitionKey,
                            _ =>
                                new FixedWindowRateLimiterOptions
                                {
                                    PermitLimit = 20,
                                    Window =
                                        TimeSpan
                                            .FromMinutes(1),
                                    QueueLimit = 0,
                                    QueueProcessingOrder =
                                        QueueProcessingOrder
                                            .OldestFirst,
                                    AutoReplenishment = true
                                });
                });
        });

        return services;
    }
}