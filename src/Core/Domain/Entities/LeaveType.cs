namespace Domain.Entities;

public class LeaveType
{
    public int Id { get; set; }
    public string LeaveName { get; set; } = string.Empty;

    // For Foreign Key relationship
    public ICollection<LeaveRequest> LeaveRequests { get; set; }
    public ICollection<LeaveBalance> LeaveBalances { get; set; }
}
