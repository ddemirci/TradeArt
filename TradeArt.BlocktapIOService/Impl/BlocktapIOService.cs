using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using TradeArt.BlocktapIOService.Data.Models;
using TradeArt.BlocktapIOService.Data.Models.Request;
using TradeArt.BlocktapIOService.Data.Models.Response;
using TradeArt.BlocktapIOService.Helpers;
using TradeArt.Interfaces;

namespace TradeArt.BlocktapIOService.Impl
{
    public class BlocktapIOService : IBlocktapIOService
    {
        private readonly GraphQLHttpClient _client;
        public BlocktapIOService()
        {
            _client = new GraphQLHttpClient(Constants.GraphQLApi, new NewtonsoftJsonSerializer());
        }

        public async Task<List<Asset>> GetAllAssets(int? limit)
        {
            if (!limit.HasValue) limit = 20;
            try
            {
                var query = new GraphQLRequest
                {
                    Query = $@"
                    query PageAssets{{
                        assets(page: {{limit: {limit} }} sort: [{{marketCapRank: ASC}}]) {{
                            assetName 
                            assetSymbol 
                            marketCap 
                            marketCapRank
                         }}
                    }}"
                };
                var response = await _client.SendQueryAsync<AssetListResponse>(query);
                return response.Data.Assets;
            }
            //TODO: Handle error
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Market> GetMarketForBaseAndQuoteCurrency(FindExchangeRequest request)
        {
            try
            {
                var query = new GraphQLRequest
                {
                    Query = $@"
                            query MarketsByBaseAndQuoteSymbol {{
                              markets(filter: {{
                                baseSymbol: {{
                                  _eq:  ""{request.BaseSymbol}""
                                }}
                                quoteSymbol: {{
                                  _eq: ""{request.QuoteSymbol}""
                                }}
                              }}) {{
                                baseSymbol
                                quoteSymbol
                                marketSymbol
                                exchangeSymbol
                                ticker {{
                                   lastPrice
                                }}
                              }}
                            }}"
                };

                var response = await _client.SendQueryAsync<MarketListResponse>(query);

                return !response.Data.Markets.Any() || !response.Data.Markets.Any(x => x.Ticker != null)
                    ? new Market { BaseSymbol = request.BaseSymbol, QuoteSymbol = request.QuoteSymbol }
                    : response.Data.Markets.Where(x => x.Ticker != null).OrderByDescending(x => x.Ticker.LastPrice).First();
            }
            //TODO: Handle error
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
