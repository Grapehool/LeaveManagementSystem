using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<Holiday> Holidays { get; set; }

}