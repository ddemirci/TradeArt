using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
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
        public void InvertText_Succeeded()
        {
            //Arrange
            var text = "abcd";
            
            //Act
            var result = taskService.InvertText(text);
            
            //Assert
            Assert.AreEqual("dcba", result);
        }

        [Test]
        public async Task FunctionA_Succeeded()
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

        [Test]
        public void CalculateSHA256Hash_Succeeded()
        {
            //Arrange
            var fileName= "Test.txt";
            var filePath = @$"{AppDomain.CurrentDomain.BaseDirectory}{fileName}"; 
            File.WriteAllText(filePath, "This is a new text file");

            //Act
            var hashResult = taskService.CalculateSHA256Hash(filePath);

            //Clean
            File.Delete(filePath);

            //Assert
            Assert.IsTrue(hashResult.IsSuccess);
            Assert.AreEqual("0445e45d70d62074cc6a608ddf95f89e275f495d3f6a2d9d0f1ddae36bb4ab50", hashResult.Data);
        }
    }
}