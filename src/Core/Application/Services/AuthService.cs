using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;

namespace Application.Services;

public class AuthService(IEmployeeRepository employeeRepo, ILeaveBalanceRepository leaveBalanceRepo, IConfiguration config) : IAuthService
{
    private readonly IEmployeeRepository _employeeRepo = employeeRepo;
    private readonly ILeaveBalanceRepository _leaveBalanceRepo = leaveBalanceRepo;
    private readonly IConfiguration _config = config;

    public async Task<string?> LoginAsync(string email, string password)
    {
        var employee = await _employeeRepo.GetByEmailAsync(email);

        if (employee == null)
            return "User does not exist";

        if (!BCrypt.Net.BCrypt.Verify(password, employee.Password))
            return "Invalid password";

        var key = _config["authKey"];

        if (string.IsNullOrEmpty(key))
            throw new Exception("Missing Auth key");

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, employee.Email),
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Role, employee.Role.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: "http://localhost:5017/api/auth",
            audience: "http://localhost:5017/api",
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<string> RegisterAsync(string name, string email, string password)
    {
        var existing = await _employeeRepo.GetByEmailAsync(email);

        if (existing != null)
            return "A user is already registered in that Email Address!";

        var employee = new Employee
        {
            Name = name,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            JoiningDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Role = Role.User
        };

        await _employeeRepo.AddAsync(employee);

        var casualLeaveBalance = new LeaveBalance
        {
            EmployeeId = employee.Id,
            LeaveTypeId = 1, 
            Balance = 12
        };

        var sickLeaveBalance = new LeaveBalance
        {
            EmployeeId = employee.Id,
            LeaveTypeId = 2,
            Balance = 12
        };

        var earnedLeaveBalance = new LeaveBalance
        {
            EmployeeId = employee.Id,
            LeaveTypeId = 3,
            Balance = 0
        };

        await _leaveBalanceRepo.AddBalanceAsync(casualLeaveBalance);
        await _leaveBalanceRepo.AddBalanceAsync(sickLeaveBalance);
        await _leaveBalanceRepo.AddBalanceAsync(earnedLeaveBalance);
        return "Registered Successfully!";
    }
}