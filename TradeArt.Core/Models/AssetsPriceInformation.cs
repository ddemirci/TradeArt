using System.Collections.Generic;
using TradeArt.BlocktapIOService.Data.Models;

namespace TradeArt.Core.Models
{
    public class AssetsPriceInformation
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<Market> SuccessList { get; set; }
        public List<string> FailureList { get; set; }
    }
}
