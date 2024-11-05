using modelEntity = Defra.PTS.Application.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Repositories
{
    [ExcludeFromCodeCoverageAttribute]
    public class AppDbContext : DbContext
    {
        //Configuration from settings
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<modelEntity.Application> Application { get; set; }
        public DbSet<modelEntity.VwApplication> VwApplications { get; set; }
        public DbSet<modelEntity.TravelDocument> TravelDocument { get; set; }
    }
}