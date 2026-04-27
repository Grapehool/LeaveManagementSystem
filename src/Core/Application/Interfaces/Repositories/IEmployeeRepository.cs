using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee?> GetByEmailAsync(string email);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
    }
}
