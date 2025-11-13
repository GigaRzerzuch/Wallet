using Microsoft.AspNetCore.Mvc;
using Wallet.Server.Services;
using Wallet.Shared.Models;

namespace Wallet.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthServerService _authServerService;
    private readonly ILogger<AuthorizationController> _logger;


    public AuthorizationController(IAuthServerService authServerService, ILogger<AuthorizationController> logger)
    {
        _authServerService = authServerService;
        _logger = logger;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLogin user)
    {
        try
        {
            var response = await _authServerService.Login(user.Email, user.Password);

            if (!response.Succes)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to login.", e.Message);
            throw;
        }
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegister user)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError("Failed to register.", e.Message);
            throw;
        }
    }
}
