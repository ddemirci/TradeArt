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
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsNotEmpty(result.Data);
            Assert.AreEqual(count, result.Data.Count);
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
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(request.BaseSymbol, result.Data.BaseSymbol);
            Assert.AreEqual(request.QuoteSymbol, result.Data.QuoteSymbol);
        }
    }
}
