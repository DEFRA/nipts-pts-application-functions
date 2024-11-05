using Defra.PTS.Application.Api.Services.Implementation;
using Defra.PTS.Application.Api.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Functions.Configuration
{
    /// <summary>
    /// Configures service dependancies
    /// </summary>
    [ExcludeFromCodeCoverageAttribute]    
    public static class ConfigureApi
    {
        /// <summary>
        /// Adds service dependancies
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefraApiServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<ITravelDocumentService, TravelDocumentService>();
            services.AddScoped<IReferenceGeneratorService, ReferenceGeneratorService>();
            services.AddTransient<IApplicationServiceHelper, ApplicationServiceHelper>();
            services.AddScoped<ITravelDocumentServiceHelper, TravelDocumentServiceHelper>();
            return services;
        }
    }
}
