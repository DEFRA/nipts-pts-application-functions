using entity = Defra.PTS.Application.Entities;
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

        public DbSet<entity.Application> Application { get; set; }
        public DbSet<entity.VwApplication> VwApplications { get; set; }
        public DbSet<entity.TravelDocument> TravelDocument { get; set; }
    }
}