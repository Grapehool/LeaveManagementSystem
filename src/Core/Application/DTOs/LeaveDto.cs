using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class LeaveDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
