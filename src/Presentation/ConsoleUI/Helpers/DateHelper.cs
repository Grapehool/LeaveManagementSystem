namespace ConsoleUI.Helpers
{
    public static class DateHelper
    {
        public static DateOnly PromptForDate()
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (DateOnly.TryParse(input, out DateOnly date))
                {
                    DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                    if (date >= today)
                    {
                        return date;
                    }

                    UIHelpers.WriteWarning("Date cannot be in the past. Please enter today or a future date.");
                    continue;
                }

                UIHelpers.WriteWarning("Invalid date format. Please try again.");
            }
        }
    }
}