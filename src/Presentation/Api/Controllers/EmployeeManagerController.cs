using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/manage/employees")]
    [ApiController]
    public class EmployeeManagerController(IManagerService managerService) : ControllerBase
    {
        private readonly IManagerService _managerService = managerService;

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _managerService.ListAllEmployeesAsync();
            return Ok(employees);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var result = await _managerService.UpdateEmployeeAsync(id, dto);
            return Ok(result);
        }
    }
}
