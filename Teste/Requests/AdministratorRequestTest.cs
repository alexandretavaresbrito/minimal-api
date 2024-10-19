using System.Text;
using System.Text.Json;
using MINIMALAPI.Domain.DTOS;
using Teste.Helpers;

namespace Teste.Requests
{
    [TestClass]
    public class AdministratorRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();    
        }

        [TestMethod]
        public async Task TesteGetSetPropriedades()
        {
            //Arrange
            var loginDTO = new LoginDTO("admin@admin.com","admin");

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");
            //Act
            var response = await Setup.client.PostAsync("/api/v1/administrator/login", content);

            //Assert
            Assert.AreEqual(200, (int)response.StatusCode);

            var Result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdminLogadoModelView>(Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(admLogado?.Email ?? "");
            Assert.IsNotNull(admLogado?.Perfil ?? "");
            Assert.IsNotNull(admLogado?.Token ?? "");
        }
    }
}