using System;
using api.Data;
using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

// [Route("api/[controller]")]
[Route("api/comment")]
[ApiController]
public class CommentController(ICommentRepository commentRepo, IStockRepository stockRepo,
  UserManager<AppUser> userManager, IFMPService fMPService) : ControllerBase
{
  private readonly ICommentRepository _commentRepo = commentRepo;
  private readonly IFMPService _fMPService = fMPService;
  private readonly IStockRepository _stockRepo = stockRepo;
  private readonly UserManager<AppUser> _userManager = userManager;

  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    // var stocks = await _context.Stocks.ToListAsync();
    var comments = await _commentRepo.GetAllAsync(queryObject);
    return Ok(comments.Select(s => s.ToCommentDto()));
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById([FromRoute] int id)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    // var stock = await _context.Stocks.FindAsync(id);
    var command = await _commentRepo.GetByIdAsync(id);
    if (command is null)
      return NotFound();
    return Ok(command.ToCommentDto());
  }

  [HttpPost("{symbol:alpha}")]
  public async Task<IActionResult> Create([FromRoute] string symbol, [FromBody] CreateCommentDto commentDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var stock = await _stockRepo.GetBySymbolAsync(symbol);
    if (stock is null)
    {
      stock = await _fMPService.FindStockBySymbolAsync(symbol);
      if (stock is null)
        return BadRequest("Stock dose not exist");
      else
        await _stockRepo.CreateAsync(stock);
    }

    var username = User.GetUsername();
    var appUser = await _userManager.FindByNameAsync(username);

    var commentModel = commentDto.ToCommentFromCreate(stock.Id);
    commentModel.AppUserId = appUser.Id;
    await _commentRepo.CreateAsync(commentModel);

    return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
  }

  [HttpPut]
  [Route("{id:int}")]
  public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());
    return comment is null ? NotFound("Comment not found") : Ok(comment.ToCommentDto());
  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<IActionResult> Delete([FromRoute] int id)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var commentModel = await _commentRepo.DeleteAsync(id);
    return commentModel is null ? NotFound("Comment does not exist") : Ok(commentModel);
  }
}