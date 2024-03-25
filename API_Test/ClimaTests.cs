using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using Moq.Protected;
using API_Clima.Service.ClimaService;

namespace API_Test
{
    [TestClass]
    public class ClimaTests
    {
        [TestMethod]
        public async Task GetClimaLocal_ReturnsSuccess_WhenApiCallIsSuccessful()
        {
            // Mock da dependência HttpClient
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{""Cidade"":""TestCity"",""Estado"":""TestState"",""AtualizadoEm"":""2023-04-03T14:00:00"",""Clima"":[{""Data"":""2023-04-04"",""Condicao"":""Sunny"",""CondicaoDesc"":""Clear sky"",""Min"":18,""Max"":28,""IndiceUv"":5.0}]}")
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var client = new HttpClient(mockHttpMessageHandler.Object);

            // Substitua 'SeuService' pelo nome real do seu serviço que contém GetClimaLocal
            var seuService = new ClimaService(client);

            // Act
            var result = await seuService.GetClimaLocal(123);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.CodigoHttp == HttpStatusCode.OK);
            Assert.IsNotNull(result.Dados);
            Assert.AreEqual("TestCity", result.Dados.Cidade);
        }
    }
}