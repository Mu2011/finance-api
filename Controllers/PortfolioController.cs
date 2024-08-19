using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo,
  IPortfolioRepository portfolioRepo, IFMPService fMPService) : ControllerBase
{
  private readonly UserManager<AppUser> _userManager = userManager;
  private readonly IStockRepository _stockRepo = stockRepo;
  private readonly IFMPService _fMPService = fMPService;
  private readonly IPortfolioRepository _portfolioRepo = portfolioRepo;

  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetUserPortfolio()
  {
    var username = User.GetUsername();
    var appUser = await _userManager.FindByNameAsync(username);
    var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
    return Ok(userPortfolio);
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> AddPortfolio(string symbol)
  {
    var username = User.GetUsername();
    var appUser = await _userManager.FindByNameAsync(username);
    var stock = await _stockRepo.GetBySymbolAsync(symbol);

    if (stock is null)
    {
      stock = await _fMPService.FindStockBySymbolAsync(symbol);
      if (stock is null)
        return BadRequest("Stock dose not exist");
      else
        await _stockRepo.CreateAsync(stock);
    }

    if (stock is null)
      return BadRequest("Stock not found");
    var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

    if (userPortfolio.Any(e => e.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase)))
      return BadRequest("Cannot add same stock to portfolio");

    var portfolioModel = new Portfolio
    {

      StockId = stock.Id,
      AppUserId = appUser.Id
    };
    await _portfolioRepo.CreateAsync(portfolioModel);

    return portfolioModel is null ? StatusCode(500, "Could not create") : Created();
  }

  [HttpDelete]
  [Authorize]
  public async Task<IActionResult> DeletePortfolio(string symbol)
  {
    var username = User.GetUsername();
    var appUser = await _userManager.FindByNameAsync(username);
    var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

    var filteredStock = userPortfolio.Where(s => s.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase));

    if (filteredStock.Count() is 1)
      await _portfolioRepo.DeletePortfolio(appUser, symbol);
    else
      return BadRequest("Stock not in your portfolio");

    return Ok();
  }

}
