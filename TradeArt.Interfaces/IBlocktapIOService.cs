using TradeArt.BlocktapIOService.Data.Models;
using TradeArt.BlocktapIOService.Data.Models.Request;

namespace TradeArt.Interfaces
{
    public interface IBlocktapIOService
    {
        Task<List<Asset>> GetAllAssets(int? limit);
        Task<Market> GetMarketForBaseAndQuoteCurrency(FindExchangeRequest request);
    }
}
