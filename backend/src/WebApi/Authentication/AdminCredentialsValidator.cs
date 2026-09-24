using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace WebApi.Authentication;

internal sealed class AdminCredentialsValidator(
    IOptions<AdminAuthenticationOptions> options,
    IPasswordHasher<string> passwordHasher)
{
    internal bool IsValid(
        string username,
        string password)
    {
        AdminAuthenticationOptions configured =
            options.Value;

        bool usernameIsValid =
            string.Equals(
                username,
                configured.Username,
                StringComparison.Ordinal);

        PasswordVerificationResult passwordResult =
            passwordHasher.VerifyHashedPassword(
                configured.Username,
                configured.PasswordHash,
                password);

        return usernameIsValid &&
               passwordResult is
                   PasswordVerificationResult.Success or
                   PasswordVerificationResult
                       .SuccessRehashNeeded;
    }
}