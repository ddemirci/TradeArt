namespace TradeArt.BlocktapIOService.Data.Models
{
    public class Asset
    {
        public string AssetName { get; set; }
        public string AssetSymbol { get; set; }
        public long? MarketCap { get; set; }
        public int? MarketCapRank { get; set; }
    }
}
