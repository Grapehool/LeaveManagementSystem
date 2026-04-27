using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class HolidayRepository(AppDbContext context) : IHolidayRepository
    {
        public async Task<List<DateOnly>> GetHolidaysAsync(DateOnly start, DateOnly end)
        {
            return await context.Holidays
                .Where(h => h.Date >= start && h.Date <= end)
                .Select(h => h.Date)
                .ToListAsync();
        }
    }
}
