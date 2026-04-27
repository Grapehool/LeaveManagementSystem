using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/manage/leaves")]
    [ApiController]
    public class LeaveManagerController(IManagerService managerService) : ControllerBase
    {
        private readonly IManagerService _managerService = managerService;

        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            var leaves = await _managerService.GetLeavesAsync();
            return Ok(leaves);
        }
        [HttpPost]
        public async Task<IActionResult> Manage([FromBody] ManageLeaveDto request)
        {
            var msg = await _managerService.ManageLeaveAsync(request);
            if (msg == "Succesfully completed the operation!")
                return Ok(msg);
            if (msg == "Not enough leave balance!")
                return BadRequest(msg);
            if (msg == "No Pending Leaves")
                return BadRequest(msg);
            else
                return BadRequest(msg);
        }
    }
}
