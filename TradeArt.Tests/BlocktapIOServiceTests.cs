using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TradeArt.Interfaces;

namespace TradeArt.Tests
{
    public class BlocktapIOServiceTests
    {
        private ServiceProvider serviceProvider;
        private IBlocktapIOService blocktapIOService;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddTransient<IBlocktapIOService, BlocktapIOService.Impl.BlocktapIOService>();
            serviceProvider = services.BuildServiceProvider();

            blocktapIOService = serviceProvider.GetRequiredService<IBlocktapIOService>();
        }

        [Test]
        public void GetAllAssets_Succeeded()
        {
            //Arrange
            int count = 5;

            //Act
            var result = blocktapIOService.GetAllAssets(count).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(count, result.Count);
        }

        [Test]
        public void GetMarketForBaseAndQuoteCurrency_Succeeded()
        {
            //Arrange
            var request = new BlocktapIOService.Data.Models.Request.FindExchangeRequest
            {
                BaseSymbol = "BTC",
                QuoteSymbol = "USDT"
            };

            //Act
            var result = blocktapIOService.GetMarketForBaseAndQuoteCurrency(request).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(request.BaseSymbol, result.BaseSymbol);
            Assert.AreEqual(request.QuoteSymbol, result.QuoteSymbol);
        }
    }
}
