using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ILeaveBalanceRepository
    {
        Task<List<LeaveBalance>> GetAllAsync();
        Task<List<LeaveBalance>> GetAllBalancesByEmployeeIdAsync(int employeeId);
        Task<bool> HasSufficientBalanceAsync(int employeeId, int leaveTypeId, int days);
        Task AddBalanceAsync(LeaveBalance leaveBalance);
        Task DeductBalance(int leaveId);
    }
}
