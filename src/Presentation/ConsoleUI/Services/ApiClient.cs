using ConsoleUI.DTOs;
using static ConsoleUI.Helpers.UIHelpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ConsoleUI.Services
{
    public class ApiClient(HttpClient client)
    {
        private readonly HttpClient _client = client;
        public async Task<bool> LoginAsync(string email, string password)
        {
            var response = await _client.PostAsJsonAsync("auth/login", new { email, password });

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result?.Token != null)
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.Token);
                return true;
            }

            return false;
        }
        public async Task<bool> RegisterAsync(string name, string email, string password)
        {
            var response = await _client.PostAsJsonAsync("auth/register", new { name, email, password });
            var message = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                WriteWarning(message);
            }
            else
            {
                WriteSuccess(message);
            }
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> LogoutAsync()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            return true;
        }
        public async Task<string> GetRoleAsync()
        {
            var response = await _client.GetAsync("auth/role");

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
        public async Task<bool> ApplyLeaveAsync(int leaveTypeId, DateOnly startDate, DateOnly endDate, string? reason)
        {
            var response = await _client.PostAsJsonAsync("apply", new { leaveTypeId, startDate, endDate, reason });
            var message = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
               WriteWarning(message);
            }
            else
            {
                WriteSuccess(message);
            }
            return response.IsSuccessStatusCode;
        }
        public async Task<List<EmployeeItem>?> GetEmployeesAsync()
        {
            var response = await _client.GetAsync("manage/employees");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var employees = await response.Content.ReadFromJsonAsync<List<EmployeeItem>>();

            if (employees != null)
            {
                Console.WriteLine($"{"Id",-5} {"Name",-20} {"Email",-40} {"Department",-25} {"Role",-10}");
                PrintLine();

                foreach (var emp in employees)
                {
                    Console.WriteLine($"{emp.Id,-5} {emp.Name,-20} {emp.Email,-40} {emp.Department,-25} {emp.Role,-10}");
                }
            }

            return employees;
        }
        public async Task<List<LeaveRequestItem>> GetLeavesAsync()
        {
            var response = await _client.GetAsync("manage/leaves");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<LeaveRequestItem>>();
        }
        public async Task<bool> GetLeaveHistoryAsync()
        {
            var response = await _client.GetAsync("history");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var history = await response.Content.ReadFromJsonAsync<List<LeaveHistoryItem>>();
            if (history != null)
            {
                Console.WriteLine($"{"Type",-10}{"Start",-15}{"End",-15}{"Status",-10}{"Reason",-40}");
                PrintLine(100);

                foreach (var item in history)
                {
                    Console.Write($"{GetLeaveTypeName(item.Id),-10}");
                    Console.Write($"{item.StartDate,-15:yyyy-MM-dd}");
                    Console.Write($"{item.EndDate,-15:yyyy-MM-dd}");

                    Console.ForegroundColor = GetStatusColor(item.Status);
                    Console.Write($"{item.Status,-10}");
                    Console.ResetColor();

                    Console.Write($"{item.Reason,-40}");

                    Console.WriteLine();
                }

                return true;
            }
            return false;
        }
        public async Task<bool> GetLeaveBalanceAsync()
        {
            var response = await _client.GetAsync("balance");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var balance = await response.Content.ReadFromJsonAsync<List<LeaveBalanceItem>>();
            if (balance != null)
            {
                Console.WriteLine($"{"Type",-30} {"Balance",-30}");
                PrintLine(100);

                foreach (var item in balance)
                {
                    Console.WriteLine($"{GetLeaveTypeName(item.LeaveTypeId),-30} {item.Balance,-30}");
                }

                return true;
            }
            return false;
        }
        public async Task<bool> ManageLeavesAsync(int leaveId, LeaveAction action)
        {
            var request = new
            {
                LeaveId = leaveId,
                Action = action
            };

            var response = await _client.PostAsJsonAsync("manage/leaves", request);
            var message = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                WriteWarning(message);
            }
            else
            {
                WriteSuccess(message);
            }
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> ManageEmployeeAsync(int empId)
        {
            Console.Write("Enter new name (leave blank to skip): ");
            var name = Console.ReadLine();

            Console.Write("Enter new email (leave blank to skip): ");
            var email = Console.ReadLine();

            Console.Write("Enter new department (leave blank to skip): ");
            var department = Console.ReadLine();

            Console.Write("Enter new role (leave blank to skip): ");
            var role = Console.ReadLine();

            var updateDto = new UpdateEmployee
            {
                Name = string.IsNullOrWhiteSpace(name) ? null : name,
                Email = string.IsNullOrWhiteSpace(email) ? null : email,
                Department = string.IsNullOrWhiteSpace(department) ? null : department,
                Role = string.IsNullOrWhiteSpace(role) ? null : role
            };

            var response = await _client.PutAsJsonAsync($"manage/employees/{empId}", updateDto);

            var message = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || message == "Invalid role")
            {
                WriteWarning(message);
                return false;
            }

            WriteSuccess(message);
            return true;
        }
        private static string GetLeaveTypeName(int leaveTypeId) => leaveTypeId switch
        {
            1 => "Casual",
            2 => "Sick",
            3 => "Earned",
            _ => "Unknown"
        };
        private static ConsoleColor GetStatusColor(string status) => status switch
        {
            "Approved" => ConsoleColor.Green,
            "Rejected" => ConsoleColor.Red,
            "Pending" => ConsoleColor.DarkBlue,
            _ => ConsoleColor.White
        };
    }
}
