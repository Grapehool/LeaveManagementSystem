using ConsoleUI.Menus;
using ConsoleUI.Services;

namespace ConsoleUI.App
{
    public class AppRunner
    {
        private readonly ApiClient _apiClient;

        public AppRunner()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5017/api/")
            };

            _apiClient = new ApiClient(httpClient);
        }

        public async Task Run()
        {
            while (true)
            {
                var authMenu = new AuthMenu(_apiClient);
                bool isLoggedIn = await authMenu.Authenticate();

                if (isLoggedIn)
                {
                    var role = await authMenu.GetRole();

                    if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        var adminMainMenu = new AdminMainMenu(_apiClient);
                        await adminMainMenu.Show();
                    }
                    else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
                    {
                        var userMainMenu = new MainMenu(_apiClient);
                        await userMainMenu.Show();
                    }
                }
            }
        }
    }
}