namespace TradeArt.BlocktapIOService.Data.Models
{
    public class Market
    {
        public string ExchangeSymbol { get; set; }
        public string MarketSymbol { get; set; }
        public string BaseSymbol { get; set; }
        public string QuoteSymbol { get; set; }
        public Ticker Ticker { get; set; }
    }
}
