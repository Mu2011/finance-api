using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository(ApplicationDbContext context) : IStockRepository
{
  private readonly ApplicationDbContext _context = context;

  public async Task<List<Stock>> GetAllAsync(QueryObject query)
  {
    /* Without async And QueryObject */
    // return _context.Stocks.ToListAsync();

    /* With async Without QueryObject*/
    // return await _context.Stocks.Include(c => c.Comments).ToListAsync();

    /* With async With QueryObject*/
    //var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();
    //if (!string.IsNullOrWhiteSpace(query.CompanyName))
    //  stocks = stocks.Where(c => c.CompanyName.Contains(query.CompanyName));

    //if (!string.IsNullOrWhiteSpace(query.Symbol))
    //  stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

    //if (!string.IsNullOrWhiteSpace(query.SortBy))
    //{
    //  if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
    //  {
    //    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
    //  }
    //}

    //var SkipNumber = (query.PageNumber - 1) * query.PageSize;

    //return await stocks.Skip(SkipNumber).Take(query.PageSize).ToListAsync();
    try
    {
      var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();

      if (!string.IsNullOrWhiteSpace(query.CompanyName))
        stocks = stocks.Where(c => c.CompanyName.Contains(query.CompanyName));

      if (!string.IsNullOrWhiteSpace(query.Symbol))
        stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

      if (!string.IsNullOrWhiteSpace(query.SortBy))
      {
        if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
        {
          stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
        }
      }

      var skipNumber = (query.PageNumber - 1) * query.PageSize;

      return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }
    catch (SqlException ex)
    {
      // Log or handle the specific SQL exception
      throw new Exception($"An error occurred while retrieving stocks: {ex.Message}", ex);
    }
    catch (Exception ex)
    {
      // General exception handling
      throw new Exception("An unexpected error occurred.", ex);
    }
  }
  public async Task<Stock?> GetByIdAsync(int id)
  {
    /* Without async */
    // return _context.Stocks.FindAsync(id).AsTask();

    /* With async */
    return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
  }
  public async Task<Stock> CreateAsync(Stock stockModel)
  {
    /* Without async */
    // _context.Stocks.AddAsync(stockModel);
    // _context.SaveChanges();
    // return Task.FromResult(stockModel);

    /* With async */
    await _context.Stocks.AddAsync(stockModel);
    await _context.SaveChangesAsync();
    return stockModel;
  }
  public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
  {
    /* Without async */
    // var existingStock = _context.Stocks.FirstOrDefaultAsync(x => x.Id == id).Result;
    // if (existingStock is null)
    //   return Task.FromResult<Stock?>(null);

    // existingStock.Symbol = stockDto.Symbol;
    // existingStock.CompanyName = stockDto.CompanyName;
    // existingStock.Purchase = stockDto.Purchase;
    // existingStock.LastDiv = stockDto.LastDiv;
    // existingStock.Industry = stockDto.Industry;
    // existingStock.MarketCap = stockDto.MarketCap;

    // _context.SaveChangesAsync();
    // return Task.FromResult<Stock?>(existingStock);

    /* With async */
    var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
    if (existingStock is null)
      return null;

    existingStock.Symbol = stockDto.Symbol;
    existingStock.CompanyName = stockDto.CompanyName;
    existingStock.Purchase = stockDto.Purchase;
    existingStock.LastDiv = stockDto.LastDiv;
    existingStock.Industry = stockDto.Industry;
    existingStock.MarketCap = stockDto.MarketCap;

    await _context.SaveChangesAsync();
    return existingStock;
  }
  public async Task<Stock?> DeleteAsync(int id)
  {
    /* Without async */
    // var stockModel = _context.Stocks.FirstOrDefaultAsync(x => x.Id == id).Result;
    // if (stockModel is null) return Task.FromResult<Stock?>(null);
    // _context.Stocks.Remove(stockModel);
    // _context.SaveChangesAsync();
    // return Task.FromResult<Stock?>(stockModel);

    /* With async */
    var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
    if (stockModel is null) return null;
    _context.Stocks.Remove(stockModel);
    await _context.SaveChangesAsync();
    return stockModel;
  }

  public Task<bool> StockExists(int id)
  {
    var c = _context.Stocks.AnyAsync(s => s.Id == id);
    return c;
  }

  public async Task<Stock?> GetBySymbolAsync(string symbol) =>
    await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
}