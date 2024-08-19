using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository(ApplicationDbContext context) : IPortfolioRepository
{
  private readonly ApplicationDbContext _context = context;

  public async Task<Portfolio> CreateAsync(Portfolio portfolio)
  {
    await _context.Portfolios.AddAsync(portfolio);
    await _context.SaveChangesAsync();
    return portfolio;
  }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
  public async Task<Portfolio?> DeletePortfolio(AppUser appUser, string symbol)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
  {
    var portfolioModel =
      await _context.Portfolios.FirstOrDefaultAsync(
        x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

    if (portfolioModel is null) return null;

    _context.Portfolios.Remove(portfolioModel);
    await _context.SaveChangesAsync();

    return portfolioModel;
  }

  public async Task<List<Stock>> GetUserPortfolio(AppUser user) =>
await _context.Portfolios.Where(u => u.AppUserId == user.Id).Select(stock => new Stock
{
  Id = stock.StockId,
  Symbol = stock.Stock.Symbol,
  CompanyName = stock.Stock.CompanyName,
  Purchase = stock.Stock.Purchase,
  LastDiv = stock.Stock.LastDiv,
  Industry = stock.Stock.Industry,
  MarketCap = stock.Stock.MarketCap
}).ToListAsync();
}
