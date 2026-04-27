using Application.DTOs;

namespace Application.Interfaces
{
    public interface IManagerService
    {
        Task<List<EmployeeDto>> ListAllEmployeesAsync();
        Task<string> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
        Task<List<LeaveDto>> GetLeavesAsync();
        Task<string> ManageLeaveAsync(ManageLeaveDto request);
    }
}
