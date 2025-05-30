using System.Security.Claims;
using ECommerce.AuthService.Application.ViewsModel;
using ECommerce.AuthService.Domain.Interfaces;
using ECommerce.AuthService.Api.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.AuthService.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(IUserUseCases userUseCases) : ControllerBase
    {
        private readonly IUserUseCases _userUseCases = userUseCases;

        protected int GetCurrentUserId()
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new InvalidOperationException("Failed to find the user.");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                throw new InvalidOperationException("Failed to find the user.");
            }

            return userId;
        }


        [HttpGet("me")]
        public async Task<IActionResult> MyProfile()
        {
            try
            {
                int userId = GetCurrentUserId();

                ResponseViewModel<UserViewModel> response = await _userUseCases.GetUserById(userId);

                if (!response.IsSuccess)
                {
                    return Unauthorized();
                }

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                Dictionary<string, object> errors = new() {
                    {
                        "General",
                        new List<string> {
                            ex.Message
                        }
                    }
                };
                return Unauthorized(ResponseViewModel<string>.Failure(errors));
            }
        }
    }
}