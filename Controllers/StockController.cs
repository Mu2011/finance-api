using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

// [Route("api/[controller]")]
[Route("api/stock")]
[ApiController]
public class StockController(/*ApplicationDbContext context,*/ IStockRepository stockRepo) : ControllerBase
{
  // private readonly ApplicationDbContext _context = context;
  private readonly IStockRepository _stockRepo = stockRepo;

  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
  {
    // var stocks = await _context.Stocks.ToListAsync();
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var stocks = await _stockRepo.GetAllAsync(query);
    return Ok(stocks.Select(s => s.ToStockDTO()).ToList());
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById([FromRoute] int id)
  {
    // var stock = await _context.Stocks.FindAsync(id);
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var stock = await _stockRepo.GetByIdAsync(id);
    if (stock is null)
      return NotFound();
    return Ok(stock.ToStockDTO());
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var stockModel = stockDto.ToStockFromCreateDTO();
    // await _context.Stocks.AddAsync(stockModel);
    // await _context.SaveChangesAsync();
    await _stockRepo.CreateAsync(stockModel);
    return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
  }

  [HttpPut]
  [Route("{id:int}")]
  public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
  {
    // var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
    // stockModel.Symbol = updateDto.Symbol;
    // stockModel.CompanyName = updateDto.CompanyName;
    // stockModel.Purchase = updateDto.Purchase;
    // stockModel.LastDiv = updateDto.LastDiv;
    // stockModel.Industry = updateDto.Industry;
    // stockModel.MarketCap = updateDto.MarketCap;
    // await _context.SaveChangesAsync();
    // if (stockModel is null)
    //   return NotFound();
    // return Ok(stockModel.ToStockDTO());

    if (!ModelState.IsValid) return BadRequest(ModelState);
    var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
    return stockModel is null ? NotFound() : Ok(stockModel.ToStockDTO());
  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<IActionResult> Delete([FromRoute] int id)
  {
    // var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
    // if (stockModel is null)
    //   return NotFound();
    // _context.Stocks.Remove(stockModel);
    // await _context.SaveChangesAsync();

    if (!ModelState.IsValid) return BadRequest(ModelState);
    var stockModel = await _stockRepo.DeleteAsync(id);
    return stockModel is null ? NotFound() : NoContent();
  }
}
