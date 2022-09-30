using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Server.Data;
using Wallet.Server.Services;
using Wallet.Shared.Models;

namespace Wallet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TradeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utilityService;

        public TradeController(DataContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTrades()
        {
            var user = await _utilityService.GetUser();
            var trades = await _context.Trades.Where(t => t.UserId == user.Id).ToListAsync();

            //var trades = await _context.Trades.ToListAsync();
            return Ok(trades);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddTrade(Trade trade)
        {
            var user = await _utilityService.GetUser();
            trade.UserId = user.Id;

            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();

            return Ok(await _context.Trades.Where(t => t.UserId == user.Id).ToListAsync());
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTrade(int id)
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
        

    }
}
