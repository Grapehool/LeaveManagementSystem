using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
    {
        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Employee employee)
        {
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
        }

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            return await context.Employees.Select(e => new EmployeeDto{
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department ?? "Unassigned",
                Role = e.Role.ToString()}).ToListAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            context.Employees.Update(employee);
            await context.SaveChangesAsync();
        }
    }
}
