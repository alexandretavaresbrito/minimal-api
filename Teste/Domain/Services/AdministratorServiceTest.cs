using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.Services;
using MINIMALAPI.Infrastructure.Db;

namespace Teste.Domain.Services
{
    [TestClass]
    public class AdministratorServiceTest
    {
        private ContextDb CreateTestContext()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
                .AddEnvironmentVariables();
            
            var configuration = builder.Build();

            return new ContextDb(configuration);
        }

        [TestMethod]
        public void SaveAdminTest()
        {
            //Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");

            var adm = new Administrator();
            adm.Email = "teste@taste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var serviceAdm = new AdministratorService(context);
            //Act
            serviceAdm.Incluir(adm);

            //Assert
            Assert.AreEqual(1, serviceAdm.Todos(1).Count());
        }

        [TestMethod]
        public void SearchAdminTest()
        {
            //Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");

            var adm = new Administrator();
            adm.Email = "teste@taste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var serviceAdm = new AdministratorService(context);
            //Act
            serviceAdm.Incluir(adm);
            var searchedAdm = serviceAdm.BuscaPorId(adm.Id);

            //Assert
            Assert.AreEqual(1, searchedAdm?.Id);
        }

        [TestMethod]
        public void CountAdminTest()
        {
            //Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");

            var adm = new Administrator();
            adm.Email = "teste@taste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var serviceAdm = new AdministratorService(context);
            //Act
            serviceAdm.Incluir(adm);
            var countedAdm = serviceAdm.Todos(1).Count();

            //Assert
            Assert.AreEqual(1, countedAdm);
        }
    }

}