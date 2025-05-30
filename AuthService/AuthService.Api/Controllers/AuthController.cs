using ECommerce.AuthService.Api.ViewsModel;
using ECommerce.AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController (IUserUseCases userUseCases) : ControllerBase
    {
        private readonly IUserUseCases _userUseCases = userUseCases;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel request)
        {
            var response = await _userUseCases.Register(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return StatusCode(201, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestViewModel request)
        {
            var response = await _userUseCases.Login(request);

            if (!response.IsSuccess)
            {
                return Unauthorized(response);
            }

            return StatusCode(201, response);
        }

    }
}