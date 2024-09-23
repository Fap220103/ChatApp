﻿using ChatApp_Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var user = await _userManager.Users
                                       .Include(r => r.UserRoles)
                                       .ThenInclude(r => r.Role)
                                       .OrderBy(u => u.UserName)
                                       .Select(u => new
                                       {
                                           u.Id,
                                           Username = u.UserName,
                                           Roles = u.UserRoles.Select(r => r.Role.Name).ToList(),
                                       })
                                       .ToListAsync();
            return Ok(user);
        }
        [HttpPost("edit-roles/{username}")]
        public async Task<IActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(',').ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
         
            if (!result.Succeeded) return BadRequest("Failed to add to roles");
           
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
          
            if (!result.Succeeded) return BadRequest("Failed to remove from roles");
          
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }
    }
}
