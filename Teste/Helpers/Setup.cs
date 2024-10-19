using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.Interfaces;
using MINIMALAPI.Infrastructure.Db;
using Teste.Mocks;

namespace Teste.Helpers
{
    public class Setup
    {
        public const string PORT = "5001";
        public static TestContext testContext = default!;
        public static WebApplicationFactory<StartUp> http = default!;
        public static HttpClient client = default!;

        public static void ClassInit(TestContext testContext)
        {
            Setup.testContext = testContext;
            Setup.http = new WebApplicationFactory<StartUp>();
            Setup.http = Setup.http.WithWebHostBuilder(builder => {
                builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");
                builder.ConfigureServices(services => {
                    services.AddScoped<IAdministratorService, AdministratorServiceMock>();
                });
            });

            Setup.client = Setup.http.CreateClient();
        }
        public static void ClassCleanup()
        {
            Setup.http.Dispose();
        }
    }
}