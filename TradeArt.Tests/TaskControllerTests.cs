using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using TradeArt.Interfaces;
using TradeArtWebAPI.Controllers;

namespace TradeArt.Tests
{
    public class TaskControllerTests
    {
        private Mock<ITaskService> mockTaskService;
        private Mock<IBlocktapIOService> mockBlocktapIOService;
        private TaskController taskController;


        [SetUp]
        public void Setup()
        {
            mockTaskService = new Mock<ITaskService>();
            mockBlocktapIOService = new Mock<IBlocktapIOService>();
            taskController = new TaskController(mockTaskService.Object, mockBlocktapIOService.Object);
        }

        [Test]
        public void Task1_Succeeded()
        {
            //Arrange
            var text = "Lorem ipsum, dolor.";
            var invertedText = ".rolod ,muspi meroL";
            mockTaskService.Setup(x => x.InvertText(text)).Returns(invertedText);

            //Act
            IActionResult actionResult = taskController.Task1_InvertText(text);

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var inverted = okObjectResult.Value as string ?? string.Empty;
            Assert.AreEqual(inverted, invertedText);
        }

        [Test]
        public void Task2_Succeeded()
        {
            //Arrange
            mockTaskService.Setup(x => x.FunctionA()).Returns(Task.FromResult(true));

            //Act
            IActionResult actionResult = taskController.Task2_WithoutBlocking().Result;

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            Assert.IsTrue(bool.TryParse(okObjectResult?.Value?.ToString(), out var response));
            Assert.AreEqual(response, true);
        }

        [Test]
        public void Task3_ExistedFile_Succeeded()
        {
            //Arrange
            var fileName = "Test.txt";
            var filePath = @$"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";
            File.WriteAllText(filePath, "This is a new text file");

            var sha256OfFile = "0445e45d70d62074cc6a608ddf95f89e275f495d3f6a2d9d0f1ddae36bb4ab50";
            mockTaskService.Setup(x => x.CalculateSHA256Hash(filePath)).Returns(sha256OfFile);
            IActionResult actionResult = taskController.Task3_CalculateSHA256Hash(filePath);

            //Clean
            File.Delete(filePath);
            
            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var sha256 = okObjectResult.Value as string ?? string.Empty;
            Assert.AreEqual(sha256, sha256OfFile);
        }


        [Test]
        public void Task3_NotExistedFile_Failed()
        {
            //Arrange
            var fileName = "Test.txt";
            var filePath = @$"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";

            IActionResult actionResult = taskController.Task3_CalculateSHA256Hash(filePath);

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.IsNull(okObjectResult);

            var notFoundObjectResult = actionResult as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var message = notFoundObjectResult.Value as string ?? string.Empty;
            Assert.AreEqual(message, $"Specified file does not exists in path {filePath}");
        }
    }
}
