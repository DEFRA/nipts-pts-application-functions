﻿using Defra.PTS.Application.Api.Services.Configuration;
using Defra.PTS.Application.Functions;
using Defra.PTS.Application.Functions.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.IO;


[assembly: FunctionsStartup(typeof(Startup))]
namespace Defra.PTS.Application.Functions
{
    /// <summary>
    /// Startup for function app
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        private static IConfiguration Configuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            Configuration = builder.GetContext().Configuration;
            var connection = string.Empty;
#if DEBUG
            connection = Configuration["sql_db"];
#else
                        connection = Configuration.GetConnectionString("sql_db");
#endif
            // retrieve the logger


            builder.Services.AddDefraRepositoriesServices(connection);
            builder.Services.AddDefraApiServices();
        }
        /// <summary>
        /// Configures app env settings
        /// </summary>
        /// <param name="builder"></param>
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
