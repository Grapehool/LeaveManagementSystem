using Domain.Enums;

namespace Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Department { get; set; }
    public DateOnly JoiningDate { get; set; }
    public DateOnly? ResignationDate { get; set; }
    public Role Role { get; set; }

    public string Password { get; set; } = string.Empty;


    // For Foreign Key relationship
    public ICollection<LeaveRequest> LeaveRequests { get; set; }
    public ICollection<LeaveBalance> LeaveBalances { get; set; }
}
