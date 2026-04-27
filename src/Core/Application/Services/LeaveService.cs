using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;


namespace Application.Services
{
    public class LeaveService(ILeaveRequestRepository leaveRequestRepo, ILeaveBalanceRepository leaveBalanceRepo, IHolidayRepository holidayRepo) : ILeaveService
    {
        public async Task<string> ApplyLeaveAsync(int empId, int typeId, DateOnly startDate, DateOnly endDate, string? reason)
        {
            // Get holidays in range
            var holidays = await holidayRepo.GetHolidaysAsync(startDate, endDate);
            var holidaySet = holidays.ToHashSet();

            int days = 0;

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Skip weekends
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                // Skip holidays
                if (holidaySet.Contains(date))
                    continue;

                days++;
            }

            if (days == 0)
                return "No working days selected.";
            var hasSufficientBalance = await leaveBalanceRepo.HasSufficientBalanceAsync(empId, typeId, days);
            if (!hasSufficientBalance)
            {
                return "Insufficient leave balance.";
            }
            var exists = await leaveRequestRepo.ExistsAsync(empId, startDate, endDate);
            if (exists)
            {
                return "Overlapping leave request exists.";
            }

            var leaveRequest = new LeaveRequest
            {
                EmployeeId = empId,
                LeaveTypeId = typeId,
                StartDate = startDate,
                EndDate = endDate,
                Status = LeaveRequestStatus.Pending,
                Reason = reason
            };
            await leaveRequestRepo.AddLeaveRequestAsync(leaveRequest);
            return "Leave request submitted successfully.";
        }

        public async Task<List<LeaveHistoryDto>> GetLeaveHistoryByIdAsync(int empId)
        {
            var leaveRequests = await leaveRequestRepo.GetAllByIdAsync(empId);
            return [.. leaveRequests.Select(x => new LeaveHistoryDto
            {
                Id = x.LeaveTypeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status.ToString(),
                Reason = x.Reason
            })];
        }
        public async Task<List<LeaveBalanceDto>> GetLeaveBalanceByIdAsync(int empId)
        {
            var leaveBalances = await leaveBalanceRepo.GetAllBalancesByEmployeeIdAsync(empId);
            return [.. leaveBalances.Select(x => new LeaveBalanceDto
            {
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId,
                Balance = x.Balance
            })];
        }
    }
}
