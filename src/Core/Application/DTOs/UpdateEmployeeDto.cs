using Domain.Enums;

namespace Application.DTOs
{
    public class UpdateEmployeeDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Role { get; set; }
    }
}
