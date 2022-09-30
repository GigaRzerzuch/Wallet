using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet.Server.Services;
using Wallet.Shared.Models;

namespace Wallet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthServerService _authServerService;

        public AuthorizationController(IAuthServerService authServerService)
        {
            _authServerService = authServerService;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin user)
        {
            var response = await _authServerService.Login(user.Email, user.Password);

            if (!response.Succes)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister user)
        {
            var response = await _authServerService.Register(
                new User
                {
                    Username = user.UserName,
                    Email = user.Email,
                }, user.Password);

            if (!response.Succes)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
    }
}
