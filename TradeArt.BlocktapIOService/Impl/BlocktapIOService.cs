using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeArt.BlocktapIOService.Data.Models;
using TradeArt.BlocktapIOService.Data.Models.Request;
using TradeArt.BlocktapIOService.Data.Models.Response;
using TradeArt.BlocktapIOService.Helpers;
using TradeArt.Core;
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

        public async Task<Result<List<Asset>>> GetAllAssets(int? limit)
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
                return Result<List<Asset>>.AsSuccess(response.Data.Assets);
            }

            catch (Exception ex)
            {
                return Result<List<Asset>>.AsFailure(ex.Message);
            }
        }

        public async Task<Result<Market>> GetMarketForBaseAndQuoteCurrency(FindExchangeRequest request)
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
                    ? Result<Market>.AsFailure($"Price info for {request.BaseSymbol}/{request.QuoteSymbol} could not be found.")
                    : Result<Market>.AsSuccess(response.Data.Markets.Where(x => x.Ticker != null).OrderByDescending(x => x.Ticker.LastPrice).First());
            }
            catch (Exception ex)
            {
                return Result<Market>.AsFailure(ex.Message);
            }
        }
    }
}
