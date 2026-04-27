using Application.DTOs;

namespace Application.Interfaces
{
    public interface ILeaveService
    {
        Task<string> ApplyLeaveAsync(int empId, int typeId, DateOnly startDate, DateOnly endDate, string? reason);
        Task<List<LeaveHistoryDto>> GetLeaveHistoryByIdAsync(int empId);
        Task<List<LeaveBalanceDto>> GetLeaveBalanceByIdAsync(int empId);
    }
}
