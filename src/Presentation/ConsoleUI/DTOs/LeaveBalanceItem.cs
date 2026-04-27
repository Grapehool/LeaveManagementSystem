using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleUI.DTOs
{
    public class LeaveBalanceItem
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public int Balance { get; set; }
    }
}
