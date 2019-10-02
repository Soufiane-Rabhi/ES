using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ES.Controllers;
using ES.Domain.Models;
using ES.Infrastructure.Serialization;
using ES.Test.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.Protected;

namespace ES.Test
{
    [TestClass]
    public class PlayersControllerTest
    {
        private static IConfiguration _configuration;
        private static IJsonSerializer _jsonSerializer;
        private static IHttpClientFactory _httpClientFactory;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var _serviceProvider = TestHelper.GetServiceProvider();

            if (_serviceProvider == null)
            {
                throw new Exception("Missing or invalid serviceProvider.");
            }

            _configuration = _serviceProvider.GetService<IConfiguration>();
            _jsonSerializer = _serviceProvider.GetService<IJsonSerializer>();
        }

        [TestMethod]
        public async Task Should_Return_All_Players_List()
        {
            //Arrange
            _httpClientFactory = SetupHttpClientFactory();
            var controller = new PlayersController(_configuration, _jsonSerializer, _httpClientFactory);

            // Act
            try
            {
                var result = await controller.Players();

                // Assert
                var okResult = result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

                var model = okResult.Value as IOrderedEnumerable<Player>;
                Assert.IsNotNull(model);
                Assert.AreEqual(5, model.Count());
            }
            catch (Exception ex)
            {

                throw;
            }
            

        }

        [TestMethod]
        public async Task Given_Correct_Id_should_return_Novak_Player()
        {
            // Arrange
            _httpClientFactory = SetupHttpClientFactory();
            var controller = new PlayersController(_configuration, _jsonSerializer, _httpClientFactory);

            // Act
            var result = await controller.GetPlayer(52);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            var model = okResult.Value as Player;
            Assert.IsNotNull(model);
            Assert.AreEqual("Novak", model.FirstName);
        }

        [TestMethod]
        public async Task Given_InCorrect_Id_should_return_Http_Not_Found()
        {
            // Arrange
            _httpClientFactory = SetupHttpClientFactory();
            var controller = new PlayersController(_configuration, _jsonSerializer, _httpClientFactory);

            // Act
            var result = await controller.GetPlayer(152);

            // Assert
            var okResult = result as NotFoundObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, okResult.StatusCode);

            var model = okResult.Value as Player;
            Assert.IsNull(model);
        }

        [TestMethod]
        public async Task Given_Correct_Id_Delete_should_return_Deleted_Player()
        {
            // Arrange
            _httpClientFactory = SetupHttpClientFactory();
            var controller = new PlayersController(_configuration, _jsonSerializer, _httpClientFactory);

            // Act
            var result = await controller.DeletePlayer(17);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            var model = okResult.Value as Player;
            Assert.IsNotNull(model);
            Assert.AreEqual("Rafael", model.FirstName);
        }

        [TestMethod]
        public async Task Given_InCorrect_Id_Delete_should_return_Http_Not_Found()
        {
            // Arrange
            _httpClientFactory = SetupHttpClientFactory();
            var controller = new PlayersController(_configuration, _jsonSerializer, _httpClientFactory);

            // Act
            var result = await controller.DeletePlayer(655);

            // Assert
            var okResult = result as NotFoundObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, okResult.StatusCode);

            var model = okResult.Value as Player;
            Assert.IsNull(model);
        }

        #region CreateHttpClientFactory Mock"

        private IHttpClientFactory SetupHttpClientFactory()
        {
            var expectedPlayersList = new PlayersList
            {
                Players = new List<Player>
                {
                    new Player{ Id=52, FirstName = "Novak"  ,LastName = "Djokovic"  , ShortName="N.DJO", Sex="M"},
                    new Player{ Id=95, FirstName = "Venus"  ,LastName = "Williams"  , ShortName="V.WIL", Sex="F"},
                    new Player{ Id=65, FirstName = "Stan"   ,LastName = "Wawrinka"  , ShortName="S.WAW", Sex="M"},
                    new Player{ Id=102,FirstName = "Serena" ,LastName = "Williams"  , ShortName="S.WIL", Sex="F"},
                    new Player{ Id=17, FirstName = "Rafael" ,LastName = "Nadal"     , ShortName="R.NAD", Sex="M"},
                }.ToArray()
            };

            //  1-Mock Http Client
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               // Response
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent(_jsonSerializer.Serialize(expectedPlayersList), Encoding.UTF8, "application/json")
               })
               .Verifiable();

            var MockHttpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://eurosportdigital.github.io/eurosport-csharp-developer-recruitment/"),
            };

            //Mock HttpClientFactory
            var httpClientMockFactory = new Mock<IHttpClientFactory>();
            httpClientMockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(MockHttpClient);
            return httpClientMockFactory.Object;
        }

        #endregion

    }
}
