using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;
using TradeArt.Interfaces;

namespace TradeArt.Tests
{
    public class TaskServiceTests
    {
        private ServiceProvider serviceProvider;
        private ITaskService taskService;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddTransient<ITaskService,TaskService.Impl.TaskService>();
            serviceProvider = services.BuildServiceProvider();

            taskService = serviceProvider.GetRequiredService<ITaskService>();
        }

        [Test]
        public void Task1_Succeeded()
        {
            //Arrange
            var text = "abcd";
            
            //Act
            var result = taskService.InvertText(text);
            
            //Assert
            Assert.AreEqual("dcba", result);
        }

        [Test]
        public async Task Task2_Succeeded()
        {
            //Arrange
            var sw = System.Diagnostics.Stopwatch.StartNew();
            
            //Act
            var completed = await taskService.FunctionA();
            sw.Stop();

            //Assert
            Assert.AreEqual(true, completed);
            Assert.Less(sw.ElapsedMilliseconds, 1000 * 100); //1000 tasks, 100 ms
        }
    }
}