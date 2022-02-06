using System.Collections.Generic;
using System.Threading.Tasks;
using TradeArt.BlocktapIOService.Data.Models;
using TradeArt.BlocktapIOService.Data.Models.Request;
using TradeArt.Core;

namespace TradeArt.Interfaces
{
    public interface IBlocktapIOService
    {
        Task<Result<List<Asset>>> GetAllAssets(int? limit);
        Task<Result<Market>> GetMarketForBaseAndQuoteCurrency(FindExchangeRequest request);
    }
}
