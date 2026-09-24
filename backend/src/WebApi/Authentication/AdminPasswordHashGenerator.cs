using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Authentication;

internal static class AdminPasswordHashGenerator
{
    internal static void Run()
    {
        Console.Write("Admin felhasználónév: ");

        string username =
            Console.ReadLine()?.Trim() ??
            string.Empty;

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException(
                "A felhasználónév nem lehet üres.");
        }

        string password =
            ReadPassword("Admin jelszó: ");

        if (password.Length < 12)
        {
            throw new InvalidOperationException(
                "A jelszó legalább 12 karakter legyen.");
        }

        string confirmation =
            ReadPassword("Admin jelszó újra: ");

        if (!string.Equals(
                password,
                confirmation,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "A két jelszó nem egyezik.");
        }

        var passwordHasher =
            new PasswordHasher<string>();

        string passwordHash =
            passwordHasher.HashPassword(
                username,
                password);

        Console.WriteLine();
        Console.WriteLine("Felhasználónév:");
        Console.WriteLine(username);

        Console.WriteLine();
        Console.WriteLine("Jelszóhash:");
        Console.WriteLine(passwordHash);
    }

    private static string ReadPassword(
        string prompt)
    {
        Console.Write(prompt);

        var password = new StringBuilder();

        while (true)
        {
            ConsoleKeyInfo key =
                Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password.Length--;
                    Console.Write("\b \b");
                }

                continue;
            }

            if (char.IsControl(key.KeyChar))
            {
                continue;
            }

            password.Append(key.KeyChar);
            Console.Write('*');
        }

        return password.ToString();
    }
}