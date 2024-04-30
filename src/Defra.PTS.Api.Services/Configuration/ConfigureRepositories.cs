using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Repositories;
using Defra.PTS.Application.Repositories.Implementation;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Api.Services.Configuration
{
    [ExcludeFromCodeCoverageAttribute]
    public static class ConfigureRepositories
    {
        public static IServiceCollection AddDefraRepositoriesServices(this IServiceCollection services,string conn)
        {
            services.AddDbContext<AppDbContext>((context) =>
            {
                context.UseSqlServer(conn);
            });
            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<ITravelDocumentRepository, TravelDocumentRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
