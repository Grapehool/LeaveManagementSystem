using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ILeaveRequestRepository
    {
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<bool> ExistsAsync(int empId, DateOnly startDate, DateOnly endDate);
        Task<List<LeaveRequest>> GetAllByIdAsync(int empId);
        Task<List<LeaveRequest>> GetAllPendingAsync();
        Task<LeaveRequest?> GetByIdAsync(int id);
        Task UpdateAsync(LeaveRequest leave);
    }
}
