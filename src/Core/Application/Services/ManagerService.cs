using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Domain.Enums;

namespace Application.Services
{
    public class ManagerService(IEmployeeRepository employeeRepo, ILeaveRequestRepository leaveRequestRepository, ILeaveBalanceRepository leaveBalanceRepository) : IManagerService
    {
        private readonly IEmployeeRepository _employeeRepo = employeeRepo;
        private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository = leaveBalanceRepository;
        public Task<List<EmployeeDto>> ListAllEmployeesAsync()
        {
            return _employeeRepo.GetAllAsync();
        }
        public async Task<string> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepo.GetByIdAsync(id);
            if (employee == null) return "Employee not found";

            if (dto.Name != null) employee.Name = dto.Name;
            if (dto.Email != null) employee.Email = dto.Email;
            if (dto.Department != null) employee.Department = dto.Department;
            if (dto.Role != null)
            {
                if (Enum.TryParse<Domain.Enums.Role>(dto.Role, true, out var role))
                {
                    employee.Role = role;
                }
                else
                {
                    return "Invalid role";
                }
            }
            await _employeeRepo.UpdateAsync(employee);

            return "Employee updated successfully";
        }
        public async Task<List<LeaveDto>> GetLeavesAsync()
        {
            var leaves = await _leaveRequestRepository.GetAllPendingAsync();

            return leaves.Select(l => new LeaveDto
            {
                Id = l.Id,
                EmployeeName = l.Employee.Name,
                LeaveType = l.LeaveType.LeaveName,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason
            }).ToList();
        }

        public async Task<string> ManageLeaveAsync(ManageLeaveDto request)
        {
            var leave = await _leaveRequestRepository.GetByIdAsync(request.LeaveId);

            if (leave == null)
                return "An Unknown Error Occured";

            if (leave.Status != LeaveRequestStatus.Pending)
                return "No Pending Leaves";

            switch (request.Action)
            {
                case LeaveAction.Approve:
                    try
                    {
                        await _leaveBalanceRepository.DeductBalance(request.LeaveId);
                    }
                    catch
                    {
                        return "Not enough leave balance!";
                    }
                    leave.Status = LeaveRequestStatus.Approved;
                    break;

                case LeaveAction.Reject:
                    leave.Status = LeaveRequestStatus.Rejected;
                    break;
            }

            await _leaveRequestRepository.UpdateAsync(leave);
            return "Succesfully completed the operation!";
        }
    }
}
