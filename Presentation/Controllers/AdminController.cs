using Application.DTOs.RequestDtos;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("Get User{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("PagedUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationRequest request)
        {
            var result = await _userService.GetUsersAsync(request);
            return Ok(result);
        }
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        [HttpPost("Create User")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {

            var user = await _userService.CreateUserAsync(request);
            return Ok(user);

        }
        [HttpDelete("users/SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(Guid id)
        {
            await _userService.SoftDeleteUserAsync(id);
            return NoContent();
        }
    }
}