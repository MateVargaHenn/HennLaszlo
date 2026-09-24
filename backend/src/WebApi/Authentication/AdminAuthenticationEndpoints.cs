using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Antiforgery;

namespace WebApi.Authentication;

internal static class AdminAuthenticationEndpoints
{
    internal static IEndpointRouteBuilder
        MapAdminAuthenticationEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group =
            endpoints.MapGroup("/api/admin/auth")
                .WithTags("Admin authentication");

		group.MapPost("/login", LoginAsync)
			.AllowAnonymous()
			.RequireRateLimiting(
				AuthenticationExtensions
					.LoginRateLimitPolicy)
					.ValidateAntiforgery();

		group.MapPost(
				"/logout",
				(Delegate)LogoutAsync)
			.RequireAuthorization(
				AuthenticationExtensions
					.AdminAuthorizationPolicy)
			.ValidateAntiforgery();

        group.MapGet("/session", GetSession)
            .RequireAuthorization(
                AuthenticationExtensions
                    .AdminAuthorizationPolicy);

		group.MapGet(
				"/csrf",
				IssueAntiforgeryToken)
			.AllowAnonymous();

        return endpoints;
    }

	private static IResult IssueAntiforgeryToken(
		HttpContext context,
		IAntiforgery antiforgery,
		IWebHostEnvironment environment)
	{
		AntiforgeryTokenSet tokens =
			antiforgery.GetAndStoreTokens(context);

		context.Response.Cookies.Append(
			"XSRF-TOKEN",
			tokens.RequestToken!,
			new CookieOptions
			{
				HttpOnly = false,
				Secure =
					!environment.IsDevelopment(),
				SameSite = SameSiteMode.Strict,
				Path = "/",
				IsEssential = true
			});

		return Results.NoContent();
	}

    private static async Task<IResult> LoginAsync(
        AdminLoginRequest request,
        AdminCredentialsValidator validator,
        HttpContext context)
    {
        if (!validator.IsValid(
                request.Username,
                request.Password))
        {
            return Results.Unauthorized();
        }

        Claim[] claims =
        [
            new(
                ClaimTypes.NameIdentifier,
                request.Username),
            new(
                ClaimTypes.Name,
                request.Username),
            new(
                ClaimTypes.Role,
                "Admin")
        ];

        ClaimsIdentity identity = new(
            claims,
            CookieAuthenticationDefaults
                .AuthenticationScheme);

        ClaimsPrincipal principal = new(identity);

        AuthenticationProperties properties = new()
        {
            IsPersistent = false,
            AllowRefresh = false
        };

        await context.SignInAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme,
            principal,
            properties);

        return Results.NoContent();
    }

    private static async Task<IResult> LogoutAsync(
        HttpContext context)
    {
        await context.SignOutAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme);

        return Results.NoContent();
    }

    private static IResult GetSession(
        HttpContext context)
    {
        return Results.Ok(
            new AdminSessionResponse(
                context.User.Identity?.Name ??
                string.Empty));
    }
}

internal sealed record AdminLoginRequest(
    string Username,
    string Password);

internal sealed record AdminSessionResponse(
    string Username);