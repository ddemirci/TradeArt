using Microsoft.AspNetCore.Mvc;
using TradeArt.Interfaces;

namespace TradeArtWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Route("task1")]
        public IActionResult Task1_InvertText([FromBody] string text)
        {
            var invertedText = _taskService.InvertText(text);
            return Ok(invertedText);
        }

        [HttpGet]
        [Route("task2")]
        public async Task<IActionResult> Task2_WithoutBlocking()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            await _taskService.FunctionA();
            sw.Stop();
            return Ok($"{sw.ElapsedMilliseconds} ms elapsed.");
        }

        [HttpPost]
        [Route("task3")]
        public IActionResult Task3_CalculateSHA256Hash([FromBody] string filePath)
        {
            //TODOO: Handle if the file does not exist.
            var res = _taskService.CalculateSHA256Hash(filePath);
            return Ok(res);
        }
    }
}
