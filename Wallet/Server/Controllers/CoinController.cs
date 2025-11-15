using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Server.Services;
using Wallet.Shared.Models;

namespace Wallet.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CoinController : ControllerBase
{
    private readonly ICoinsService _coinsService;
    private readonly ILogger<CoinController> _logger;

    public CoinController(ICoinsService coinsService, ILogger<CoinController> logger)
    {
        _coinsService = coinsService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> GetCoins(List<Coin> coinList)
    {
        try
        {
            var updatedList = await _coinsService.GetCurrentPrice(coinList);
            return Ok(updatedList);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to fetch coins.", e.Message);
            throw;
        }
    }

    [HttpGet("gettrades")]
    public async Task<IActionResult> GenerateTradesDocument()
    {
        try
        {
            await _coinsService.SendTradesMessage();
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to generate trades message.", e.Message);
            throw;
        }
    }
}
