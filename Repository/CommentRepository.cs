using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository
{
  private readonly ApplicationDbContext _context = context;
  public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
  {
    var comments = _context.Comments.Include(a => a.AppUser).AsQueryable();
    if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
      comments = comments.Where(s => s.Stock.Symbol == queryObject.Symbol);
    if (queryObject.IsDescending)
      comments = comments.OrderByDescending(c => c.CreatedOn);
    return await comments.ToListAsync();
  }

  public async Task<Comment?> GetByIdAsync(int id) =>
    await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);

  public async Task<Comment> CreateAsync(Comment commentModel)
  {
    await _context.Comments.AddAsync(commentModel);
    await _context.SaveChangesAsync();
    return commentModel;
  }

  public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
  {
    var existingComment = await _context.Comments.FindAsync(id);
    if (existingComment is null)
      return null;
    existingComment.Title = commentModel.Title;
    existingComment.Content = commentModel.Content;

    await _context.SaveChangesAsync();
    return existingComment;
  }

  public async Task<Comment?> DeleteAsync(int id)
  {
    var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
    if (commentModel is null) return null;
    _context.Comments.Remove(commentModel);
    await _context.SaveChangesAsync();
    return commentModel;
  }
}