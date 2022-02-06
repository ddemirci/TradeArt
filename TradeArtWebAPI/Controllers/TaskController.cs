using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeArt.BlocktapIOService.Data.Models;
using TradeArt.BlocktapIOService.Data.Models.Request;
using TradeArt.Core;
using TradeArt.Core.Models;
using TradeArt.Interfaces;
using TradeArt.Core.Extensions;

namespace TradeArtWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IBlocktapIOService _blocktapIOService;

        public TaskController(ITaskService taskService, IBlocktapIOService blocktapIOService)
        {
            _taskService = taskService;
            _blocktapIOService = blocktapIOService;
        }

        /// <summary>
        /// Inverts given text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Inverted text</returns>
        [HttpPost]
        [Route("task1")]
        public IActionResult Task1_InvertText([FromBody] string text)
        {
            var invertedText = _taskService.InvertText(text);
            return Ok(invertedText);
        }

        /// <summary>
        /// Works as fast as possible
        /// </summary>
        /// <returns>isAllCompleted</returns>
        [HttpGet]
        [Route("task2")]
        public async Task<IActionResult> Task2_WithoutBlocking()
        {
            var isAllCompleted = await _taskService.FunctionA();
            return Ok(isAllCompleted);
        }

        /// <summary>
        /// Calculates SHA256 Hash of requested file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>SHA256 hash, if the file exists.</returns>
        /// <returns>NotFound, if the file does not exist.</returns>
        [HttpPost]
        [Route("task3")]
        public IActionResult Task3_CalculateSHA256Hash([FromBody] string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return NotFound($"Specified file does not exists in path {filePath}");
            var res = _taskService.CalculateSHA256Hash(filePath);
            return res.IsSuccess ? Ok(res.Data) : StatusCode(500, res.ErrorMessage);
        }

        /// <summary>
        /// Receives top 100 assets and fetches the last prices of them.
        /// </summary>
        /// <param name="quoteCurrency"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("task4")]
        public async Task<IActionResult> Task4_GetAssetPrices(string quoteCurrency)
        {
            var topAssetsResult = await _blocktapIOService.GetAllAssets(100);
            if(!topAssetsResult.IsSuccess)
                return StatusCode(500, topAssetsResult.ErrorMessage);

            quoteCurrency = quoteCurrency.ToUpperInvariant();

            var assetChunks = topAssetsResult.Data.Select(x=> x.AssetSymbol).Chunk(20);
            var successList = new List<Market>(); 
            var failureList = new List<string>();
            foreach (var assetChunk in assetChunks)
            {
                var result = await Task4_RetrieveMarketInformation(assetChunk, quoteCurrency);
                successList.AddRange(result.Where(r => r.IsSuccess).Select(r=> r.Data));
                failureList.AddRange(result.Where(r => !r.IsSuccess).Select(r => r.ErrorMessage));
            }

            return !successList.Any() 
                ? StatusCode(500, failureList)
                : Ok(new AssetsPriceInformation{ 
                    SuccessCount = successList.Count, 
                    FailureCount = failureList.Count,
                    SuccessList = successList,
                    FailureList = failureList 
                });
        }

        private async Task<Result<Market>[]> Task4_RetrieveMarketInformation(IEnumerable<string> assets, string quoteSymbol)
        {
            var taskList = new List<Task<Result<Market>>>();
            foreach (var asset in assets)
            {
                taskList.Add(Task.Run(() =>_blocktapIOService.GetMarketForBaseAndQuoteCurrency(new FindExchangeRequest
                {
                    BaseSymbol = asset,
                    QuoteSymbol = quoteSymbol
                })));
            }
            return await Task.WhenAll(taskList);
        }
    }
}
