namespace Domain.Entities;

public class Holiday
{
    public int Id { get; set; }
    public string Occasion { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
}

