using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public enum LeaveAction
    {
        Approve,
        Reject
    }
    public class ManageLeaveDto
    {
        public int LeaveId { get; set; }
        public LeaveAction Action { get; set; }
    }
}
