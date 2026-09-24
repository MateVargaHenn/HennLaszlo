using Microsoft.AspNetCore.Antiforgery;

namespace WebApi.Authentication;

internal static class AntiforgeryEndpointExtensions
{
    internal static TBuilder ValidateAntiforgery<TBuilder>(
        this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilter(
            async (context, next) =>
            {
				HttpRequest request =
					context.HttpContext.Request;

				bool isSafeMethod =
					HttpMethods.IsGet(request.Method) ||
					HttpMethods.IsHead(request.Method) ||
					HttpMethods.IsOptions(request.Method) ||
					HttpMethods.IsTrace(request.Method);

				if (isSafeMethod)
				{
					return await next(context);
				}
				
                IAntiforgery antiforgery =
                    context.HttpContext
                        .RequestServices
                        .GetRequiredService<IAntiforgery>();

                try
                {
                    await antiforgery
                        .ValidateRequestAsync(
                            context.HttpContext);
                }
                catch (
                    AntiforgeryValidationException)
                {
                    return Results.Problem(
                        statusCode:
                            StatusCodes
                                .Status400BadRequest,
                        title:
                            "Invalid antiforgery token.");
                }

                return await next(context);
            });
    }
}