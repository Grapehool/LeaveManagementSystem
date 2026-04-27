namespace Application.DTOs
{
    public class LeaveHistoryDto
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; }

        public string? Reason { get; set; }
    }
}
