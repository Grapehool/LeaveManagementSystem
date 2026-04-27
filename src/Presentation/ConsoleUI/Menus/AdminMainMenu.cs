using static ConsoleUI.Helpers.UIHelpers;
using ConsoleUI.Services;

namespace ConsoleUI.Menus
{
    internal class AdminMainMenu(ApiClient apiClient) : MainMenu(apiClient)
    {
        private readonly ApiClient _apiClient = apiClient;
        public new async Task Show()
        {
            while (true)
            {
                Console.Clear();
                PrintTitle("[ADMIN PANEL]", ConsoleColor.Yellow);
                PrintTitle("Leave Management System");
                Console.WriteLine("1. Apply Leave");
                Console.WriteLine("2. Manage Employees");
                Console.WriteLine("3. Manage Leaves");
                Console.WriteLine("4. View Leave Request History");
                Console.WriteLine("5. View Leave Balance");
                Console.WriteLine("0. Logout");
                PrintLine();

                var input = Console.ReadKey(true).KeyChar.ToString();

                switch (input)
                {
                    case "1":
                        await ApplyLeave(_apiClient);
                        break;
                    case "2":
                        await ManageEmployees();
                        break;
                    case "3":
                        await ManageLeaves();
                        break;
                    case "4":
                        await ViewLeaveRequests(_apiClient);
                        break;
                    case "5":
                        await ViewBalance(_apiClient);
                        break;
                    case "0":
                        await Logout(_apiClient);
                        return;
                }
            }
        }
        private async Task ManageEmployees()
        {
            Console.Clear();
            PrintTitle("Manage Employees");

            var employees = await _apiClient.GetEmployeesAsync();

            if (employees == null || employees.Count == 0)
            {
                WriteWarning("\nFailed to retrieve employees...");
                Console.ReadKey(true);
                return;
            }

            PrintLine();

            int employeeId;

            while (true)
            {
                Console.Write("Enter employee ID (0 to exit): ");
                var input = Console.ReadLine();

                // Step 1: validate number
                if (!int.TryParse(input, out employeeId))
                {
                    WriteWarning("Invalid number. Try again.");
                    continue;
                }

                // Step 2: allow exit
                if (employeeId == 0)
                {
                    return;
                }
                if (employeeId == 16)
                {
                    WriteWarning("You cannot manage the main admin account. Press any key...");
                    Console.ReadKey(true);
                    return;
                }

                // Step 3: validate against list  
                if (!employees.Any(e => e.Id == employeeId))
                {
                    WriteWarning("Employee ID not found. Try again.");
                    continue;
                }

                break;
            }

            bool ableToManage = await _apiClient.ManageEmployeeAsync(employeeId);

            if (!ableToManage)
            {
                WriteWarning("Failed to make changes. Press any key...");
            }

            Console.ReadKey(true);
        }
        private async Task ManageLeaves()
        {
            Console.Clear();
            PrintTitle("Manage Leaves", 120);

            var leaves = await _apiClient.GetLeavesAsync();

            if (leaves == null)
            {
                WriteWarning("\nFailed to retrieve leaves...");
                Console.ReadKey(true);
                return;
            }
            if (leaves.Count == 0)
            {
                WriteColor("\nNo Pending Leave Requests", ConsoleColor.Cyan);
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine($"{"Id",-5} {"Employee",-20} {"Leave Type", -20} {"Start",-15} {"End",-15} {"Reason",-40}");
            PrintLine(120);

            foreach (var leave in leaves)
            {
                Console.WriteLine(
                    $"{leave.Id,-5} {leave.EmployeeName,-20} {leave.LeaveType,-20} {leave.StartDate,-15:d} {leave.EndDate,-15:d} {leave.Reason,-40}"
                );
            }
            PrintLine(120);

            int leaveId;

            while (true)
            {
                Console.Write("Enter leave ID (0 to exit): ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out leaveId))
                {
                    WriteWarning("Invalid number. Try again.");
                    continue;
                }

                if (leaveId == 0)
                {
                    return;
                }

                if (!leaves.Any(e => e.Id == leaveId))
                {
                    WriteWarning("Leave ID not found. Try again.");
                    continue;
                }

                break;
            }

            LeaveAction action;

            while (true)
            {
                Console.Write("Approve or Reject (A/R): ");
                var input2 = Console.ReadLine()?.Trim().ToUpper();

                if (input2 == "A")
                {
                    action = LeaveAction.Approve;
                    break;
                }
                else if (input2 == "R")
                {
                    action = LeaveAction.Reject;
                    break;
                }

                WriteWarning("Invalid choice. Enter A or R.");
            }

            WriteColor("Processing Leave", ConsoleColor.DarkCyan);
            bool ableToManage = await _apiClient.ManageLeavesAsync(leaveId, action);

            if (!ableToManage)
            {
                WriteWarning("Failed to make changes. Press any key...");
            }
            else
            {
                WriteSuccess("Leave Processed succesfully!");
            }
            Console.ReadKey(true);
        }
    }
}

namespace ConsoleUI
{
    public enum LeaveAction
    {
        Approve,
        Reject
    }
}