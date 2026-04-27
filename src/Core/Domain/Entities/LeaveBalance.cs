namespace Domain.Entities;

public class LeaveBalance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public int Balance { get; set; }

    // For EFCore Foreign Key relationship
    public Employee Employee { get; set; }
    public LeaveType LeaveType { get; set; }
}
