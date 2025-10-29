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

    public CoinController(ICoinsService coinsService)
    {
        _coinsService = coinsService;
    }

    [HttpPost]
    public async Task<IActionResult> GetCoins(List<Coin> coinList)
    {
        var updatedList = await _coinsService.GetCurrentPrice(coinList);

        return Ok(updatedList);
    }
}
