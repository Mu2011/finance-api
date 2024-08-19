using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;

namespace api.Service;

public class FMPService(HttpClient httpClient, IConfiguration config) : IFMPService
{
  private HttpClient _httpClient = httpClient;
  private IConfiguration _config = config;

  public async Task<Stock> FindStockBySymbolAsync(string symbol)
  {
    try// https://financialmodelingprep.com/api/v3/profile/AAPL"]}
    {
      var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apiKey={_config["FMPKey"]}");
      if (result.IsSuccessStatusCode)
      {
        var content = await result.Content.ReadAsStringAsync();
        var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
        var stock = tasks[0];
        if (stock is not null)
          return stock.ToStockFromFMP();
        return null;
      }
      return null;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return null;
    }
  }
}