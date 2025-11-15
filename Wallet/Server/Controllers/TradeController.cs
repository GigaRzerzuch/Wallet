using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Server.Data;
using Wallet.Server.Services;
using Wallet.Shared.Models;

namespace Wallet.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TradeController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IUtilityService _utilityService;
    private readonly ILogger<TradeController> _logger;

    public TradeController(DataContext context, IUtilityService utilityService, ILogger<TradeController> logger)
    {
        _context = context;
        _utilityService = utilityService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserTrades()
    {
        try
        {
            var user = await _utilityService.GetUser();
            var trades = await _context.Trades.Where(t => t.UserId == user.Id).ToListAsync();
            return Ok(trades);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to get user trades.", e.Message);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddTrade(Trade trade)
    {
        try
        {
            var user = await _utilityService.GetUser();
            trade.UserId = user.Id;
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
            return Ok(await _context.Trades.Where(t => t.UserId == user.Id).ToListAsync());
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add trade.", e.Message);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveTrade(int id)
    {
        try
        {
            var user = await _utilityService.GetUser();
            var dbTrade = await _context.Trades.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            if (dbTrade == null)
            {
                return NotFound("No trade with this id");
            }

            _context.Trades.Remove(dbTrade);
            await _context.SaveChangesAsync();
            return Ok(await _context.Trades.ToListAsync());
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to remove trade.", e.Message);
            throw;
        }
    }
}
