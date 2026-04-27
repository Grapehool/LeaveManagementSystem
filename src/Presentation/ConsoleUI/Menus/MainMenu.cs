using ConsoleUI.Helpers;
using ConsoleUI.Services;
using static ConsoleUI.Helpers.UIHelpers;

namespace ConsoleUI.Menus
{
    internal class MainMenu(ApiClient apiClient)
    {
        private readonly ApiClient _apiClient = apiClient;

        public async Task Show()
        {
            while (true)
            {
                Console.Clear();
                PrintTitle("Leave Management System");
                Console.WriteLine("1. Apply Leave");
                Console.WriteLine("2. View Leave Request History");
                Console.WriteLine("3. View Leave Balance");
                Console.WriteLine("0. Logout");
                PrintLine();

                var input = Console.ReadKey(true).KeyChar.ToString();

                switch (input)
                {
                    case "1":
                        await ApplyLeave(_apiClient);
                        break;
                    case "2":
                        await ViewLeaveRequests(_apiClient);
                        break;
                    case "3":
                        await ViewBalance(_apiClient);
                        break;
                    case "0":
                        await Logout(_apiClient);
                        return;
                }
            }
        }
        protected static async Task ApplyLeave(ApiClient _apiClient)
        {
            Console.Clear();
            PrintTitle("Apply for Leave");

            Console.WriteLine("Select Leave Type:");
            Console.WriteLine("1: Casual");
            Console.WriteLine("2: Sick");
            Console.WriteLine("3: Earned");
            Console.WriteLine("0: Cancel");

            var key = Console.ReadKey(true).KeyChar;

            if (key == '0')
            {
                return;
            }

            if (!int.TryParse(key.ToString(), out int typeId) ||
                typeId < 1 || typeId > 3)
            {
                WriteWarning("Invalid selection! Press any key to continue...");
                Console.ReadKey(true);
                return;
            }


            Console.Write("[INCLUSIVE] Start Date (yyyy-mm-dd): ");
            DateOnly startDate = DateHelper.PromptForDate();

            Console.Write("[INCLUSIVE] End Date (yyyy-mm-dd): ");
            DateOnly endDate = DateHelper.PromptForDate();

            if (endDate < startDate)
            {
                WriteWarning("Warning! End date cannot be before start date.");
                Console.ReadKey(true);
                return;
            }

            Console.Write("Reason: ");
            string? reason = Console.ReadLine();

            Console.WriteLine("\nLoading...");
            bool ableToApply = await _apiClient.ApplyLeaveAsync(typeId, startDate, endDate, reason);
            if (!ableToApply)
            {
                WriteWarning("Failed to apply for leave. Press any key...");
                Console.ReadKey(true);
            }
            else
            {
                WriteSuccess("Applied Succesfully! Await confirmation. Press any key to continue");
                Console.ReadKey(true);
            }
        }
        protected static async Task ViewLeaveRequests(ApiClient _apiClient)
        {
            Console.Clear();
            PrintTitle("Requested Leaves");
            bool ableToGet = await _apiClient.GetLeaveHistoryAsync();
            if (!ableToGet)
            {
                WriteWarning("Failed to retrieve leave requests. Press any key");
                Console.ReadKey(true);
            }
            PrintLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
            return;
        }
        protected static async Task ViewBalance(ApiClient _apiClient)
        {
            Console.Clear();
            PrintTitle("Leave Balance");
            bool ableToGet = await _apiClient.GetLeaveBalanceAsync();
            if (!ableToGet)
            {
                Console.WriteLine("Failed to retrieve balance. Press any key");
                Console.ReadKey(true);
            }
            PrintLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
            return;
        }
        protected static async Task Logout(ApiClient _apiClient)
        {
            Console.Clear();
            Console.WriteLine("Logging out...");
            await _apiClient.LogoutAsync();
        }
    }
}
