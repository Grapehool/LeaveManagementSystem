using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;

namespace Persistence.Repositories
{
    public class LeaveRequestRepository(AppDbContext context) : ILeaveRequestRepository
    {
        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            await context.LeaveRequests.AddAsync(leaveRequest);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int empId,  DateOnly startDate, DateOnly endDate)
        {
            return await context.LeaveRequests.AnyAsync(l =>
                l.EmployeeId == empId &&
                l.StartDate <= endDate &&
                l.EndDate >= startDate
                );
        }
        public async Task<List<LeaveRequest>> GetAllByIdAsync(int empId)
        {
            return await context.LeaveRequests.Where(x => x.EmployeeId == empId).ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetAllPendingAsync()
        {
            return await context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Where(l => l.Status == Domain.Enums.LeaveRequestStatus.Pending)
                .ToListAsync();
        }

        public async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task UpdateAsync(LeaveRequest leave)
        {
            context.LeaveRequests.Update(leave);
            await context.SaveChangesAsync();
        }
    }
}


