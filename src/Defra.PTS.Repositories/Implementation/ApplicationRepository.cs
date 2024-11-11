using modelEntity = Defra.PTS.Application.Entities;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Defra.PTS.Application.Repositories;
using System.Data;
using Microsoft.Extensions.Logging;
using Defra.PTS.Application.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Repositories.Implementation
{
    [ExcludeFromCodeCoverageAttribute]
    public class ApplicationRepository : Repository<modelEntity.Application>, IApplicationRepository
    {
        private readonly ILogger<ApplicationRepository> _log;
        private AppDbContext AppContext
        {
            get
            {
                return (AppDbContext)_dbContext;
            }
        }
        public ApplicationRepository(DbContext dbContext, ILogger<ApplicationRepository> log) : base(dbContext)
        {
            _log = log;
        }

        public async Task<VwApplication?> GetApplicationDetails(Guid applicationId)
        {
            return await AppContext.VwApplications.FirstOrDefaultAsync(c => c.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<VwApplication>> GetApplicationsForUser(Guid userId)
        {
            var query = AppContext.VwApplications.Where(c => c.UserId == userId);
            return await query.ToListAsync();
        }

        public modelEntity.Application GetApplication(Guid id)
        {
            return AppContext.Application.Include("Address").Include("PetApplicationStatus").Where(c => c.Id == id).FirstOrDefault()!;
        }

        public int DeleteApplication(Guid id)
        {
            var applicationId = AppContext.Application.Where(c => c.Id == id).FirstOrDefault();
            if (applicationId != null)
            {
                AppContext.Application.Remove(applicationId);
                return AppContext.SaveChanges();
            }
            return 0;
        }

        public async Task<bool> DoesReferenceNumberExists(string referenceNumber)
        {
            return await AppContext.Application.AnyAsync(a => a.ReferenceNumber == referenceNumber);
        }

        public async Task<bool> PerformHealthCheckLogic()
        {
            // Attempt to open a connection to the database
            await AppContext.Database.OpenConnectionAsync();

            // Check if the connection is open
            if (AppContext.Database.GetDbConnection().State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
