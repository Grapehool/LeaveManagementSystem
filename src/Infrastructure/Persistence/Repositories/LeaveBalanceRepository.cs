using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class LeaveBalanceRepository(AppDbContext context) : ILeaveBalanceRepository
    {
        public async Task AddBalanceAsync(LeaveBalance leaveBalance)
        {
            await context.LeaveBalances.AddAsync(leaveBalance);
            await context.SaveChangesAsync();
        }

        public async Task<List<LeaveBalance>> GetAllAsync()
        {
            return await context.LeaveBalances.ToListAsync();
        }
        public async Task<List<LeaveBalance>> GetAllBalancesByEmployeeIdAsync(int employeeId)
        {
            return await context.LeaveBalances.Where(x => x.EmployeeId == employeeId).ToListAsync();
        }
        public async Task<bool> HasSufficientBalanceAsync(int employeeId, int leaveTypeId, int days)
        {
            var balance = await context.LeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.LeaveTypeId == leaveTypeId);
            if (balance == null || balance.Balance < days)
            {
                return false;
            }
            return true;
        }

        public async Task DeductBalance(int leaveId)
        {
            var leave = await context.Set<LeaveRequest>()
        .FirstOrDefaultAsync(x => x.Id == leaveId);

            if (leave == null)
                throw new Exception("Leave request not found");

            // Calculate number of days (inclusive)
            int days = leave.EndDate.DayNumber - leave.StartDate.DayNumber + 1;

            var balance = await context.LeaveBalances
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == leave.EmployeeId &&
                    x.LeaveTypeId == leave.LeaveTypeId);

            if (balance == null)
                throw new Exception("Leave balance not found");

            if (balance.Balance < days)
                throw new Exception("Insufficient leave balance");

            balance.Balance -= days;

            await context.SaveChangesAsync();
        }
    }
}
