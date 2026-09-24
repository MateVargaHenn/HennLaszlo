using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Threading.RateLimiting;

namespace WebApi.Authentication;

internal static class AuthenticationExtensions
{
	internal const string LoginRateLimitPolicy =
    "admin-login";

	internal const string AdminAuthorizationPolicy =
    "admin-only";

    internal static IServiceCollection
        AddAdminAuthentication(
            this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        services
            .AddAuthentication(
                CookieAuthenticationDefaults
                    .AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name =
					environment.IsDevelopment()
						? "HennLaszlo.Admin"
						: "__Host-HennLaszlo.Admin";

                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy =
					environment.IsDevelopment()
						? CookieSecurePolicy.SameAsRequest
						: CookieSecurePolicy.Always;
						
                options.Cookie.SameSite =
                    SameSiteMode.Strict;
                options.Cookie.Path = "/";

                options.ExpireTimeSpan =
                    TimeSpan.FromHours(8);
                options.SlidingExpiration = false;

                options.Events.OnRedirectToLogin =
                    context =>
                    {
                        context.Response.StatusCode =
                            StatusCodes
                                .Status401Unauthorized;

                        return Task.CompletedTask;
                    };

                options.Events.OnRedirectToAccessDenied =
                    context =>
                    {
                        context.Response.StatusCode =
                            StatusCodes
                                .Status403Forbidden;

                        return Task.CompletedTask;
                    };
            });

		services.AddSingleton<
			IPasswordHasher<string>,
			PasswordHasher<string>>();

		services.AddSingleton<
			AdminCredentialsValidator>();

        services.AddAuthorization(options =>
		{
			options.AddPolicy(
				AdminAuthorizationPolicy,
				policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireRole("Admin");
				});
		});

		services.AddRateLimiter(options =>
		{
			options.RejectionStatusCode =
				StatusCodes.Status429TooManyRequests;

			options.AddPolicy(
				LoginRateLimitPolicy,
				context =>
					RateLimitPartition
						.GetFixedWindowLimiter(
							partitionKey:
								context.Connection
									.RemoteIpAddress?
									.ToString() ??
								"unknown",
							factory: _ =>
								new FixedWindowRateLimiterOptions
								{
									PermitLimit = 5,
									Window =
										TimeSpan.FromMinutes(1),
									QueueLimit = 0,
									QueueProcessingOrder =
										QueueProcessingOrder
											.OldestFirst,
									AutoReplenishment = true
								}));
		});

		services.AddAntiforgery(options =>
		{
			options.HeaderName = "X-XSRF-TOKEN";

			options.Cookie.Name =
				environment.IsDevelopment()
					? "HennLaszlo.Antiforgery"
					: "__Host-HennLaszlo.Antiforgery";

			options.Cookie.HttpOnly = true;

			options.Cookie.SecurePolicy =
				environment.IsDevelopment()
					? CookieSecurePolicy.SameAsRequest
					: CookieSecurePolicy.Always;

			options.Cookie.SameSite =
				SameSiteMode.Strict;

			options.Cookie.Path = "/";
		});

        return services;
    }
}