using api.Dtos.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
  public static StockDto ToStockDTO(this Stock stockModel) => new()
  {
    Id = stockModel.Id,
    Symbol = stockModel.Symbol,
    CompanyName = stockModel.CompanyName,
    Purchase = stockModel.Purchase,
    LastDiv = stockModel.LastDiv,
    Industry = stockModel.Industry,
    MarketCap = stockModel.MarketCap,
    Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList(),
  };

  public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockDto) => new()
  {
    Symbol = stockDto.Symbol,
    CompanyName = stockDto.CompanyName,
    Purchase = stockDto.Purchase,
    LastDiv = stockDto.LastDiv,
    Industry = stockDto.Industry,
    MarketCap = stockDto.MarketCap,
  };

  public static Stock ToStockFromFMP(this FMPStock fMPStock) => new()
  {
    Symbol = fMPStock.symbol,
    CompanyName = fMPStock.companyName,
    Purchase = (decimal)fMPStock.price,
    LastDiv = (decimal)fMPStock.lastDiv,
    Industry = fMPStock.industry,
    MarketCap = fMPStock.mktCap
  };
}