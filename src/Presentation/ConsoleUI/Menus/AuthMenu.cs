using ConsoleUI.DTOs;
using ConsoleUI.Services;
using static ConsoleUI.Helpers.UIHelpers;
using System.Text.Json;

namespace ConsoleUI.Menus
{
    internal class AuthMenu(ApiClient apiClient)
    {
        private readonly ApiClient _apiClient = apiClient;
        public async Task<bool> Authenticate()
        {
            while (true)
            {
                Console.Clear();
                PrintTitle("Leave Management System");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                PrintLine();

                var input = Console.ReadKey(true).KeyChar.ToString();

                switch (input)
                {
                    case "1":
                        return await Login();
                    case "2":
                        await Register();
                        break;
                }
            }
        }
        public async Task<string> GetRole()
        {
            var response = await _apiClient.GetRoleAsync();
            var roleObj = JsonSerializer.Deserialize<RoleResponse>(response);
            return roleObj?.Role;
        }
        private async Task<bool> Login()
        {
            Console.Clear();
            PrintTitle("Login");

            Console.Write("Email Address: ");
            var email = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            WriteColor("Loading...",ConsoleColor.Cyan);
            var isSuccess = await _apiClient.LoginAsync(email, password);
            if (isSuccess)
                return true;
            else
            {
                WriteWarning("Login failed. Press any key to try again...");
                Console.ReadKey(true);
                return false;
            }
        }
        private async Task Register()
        {
            Console.Clear();
            PrintTitle("Register");

            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Email Address: ");
            var email = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            WriteColor("Loading...", ConsoleColor.Cyan);
            var isRegistered = await _apiClient.RegisterAsync(name, email, password);
            if (isRegistered)
            {
                Console.WriteLine("Registration successful. Please login to continue.");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("Registration failed. Press any key to try again...");
                Console.ReadKey(true);
            }
        }
    }
}
