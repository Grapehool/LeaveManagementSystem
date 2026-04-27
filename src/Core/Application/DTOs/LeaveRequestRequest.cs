using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LeaveRequestRequest
    {
        [Required]
        public int LeaveTypeId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
