using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [Authorize]
    [ApiController]
    public class LeaveController(ILeaveService leaveService) : ControllerBase
    {
        private readonly ILeaveService _leaveService = leaveService;

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequestRequest request)
        {
            if (request.LeaveTypeId <= 0)
            {
                return BadRequest("Invalid LeaveTypeId");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(employeeIdClaim, out var employeeId))
                return Unauthorized();

            var msg = await _leaveService.ApplyLeaveAsync(employeeId, request.LeaveTypeId, request.StartDate, request.EndDate, request.Reason);

            if (msg == "Overlapping leave request exists.")
                return BadRequest(msg);

            if (msg == "Insufficient leave balance.")
                return BadRequest(msg);

            if (msg == "Leave request submitted successfully.")
                return Created();

            return BadRequest("An unexpected error occurred.");
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetLeaveHistory()
        {
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(employeeIdClaim, out var employeeId))
                return Unauthorized();

            var history = await _leaveService.GetLeaveHistoryByIdAsync(employeeId);
            return Ok(history);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetLeaveBalance()
        {
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(employeeIdClaim, out var employeeId))
                return Unauthorized();

            var balance = await _leaveService.GetLeaveBalanceByIdAsync(employeeId);
            return Ok(balance);
        }
    }
}
