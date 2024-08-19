using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Tables")]
public class Stock
{
  [Key]
  public int Id { get; set; }
  public string Symbol { get; set; } = string.Empty;
  public string CompanyName { get; set; } = string.Empty;
  [Column(TypeName = "decimal(18, 2)")]
  public decimal Purchase { get; set; }
  [Column(TypeName = "decimal(18, 2)")]
  public decimal LastDiv { get; set; }
  public string Industry { get; set; } = string.Empty;
  public long MarketCap { get; set; }
  public List<Comment> Comments { get; set; } = [];
  public List<Portfolio> Portfolios { get; set; } = [];

}