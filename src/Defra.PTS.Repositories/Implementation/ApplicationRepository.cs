using entity = Defra.PTS.Application.Entities;
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
    public class ApplicationRepository : Repository<entity.Application>, IApplicationRepository
    {
        private readonly ILogger<ApplicationRepository> _log;
        private AppDbContext appContext
        {
            get
            {
                return _dbContext as AppDbContext;
            }
        }
        public ApplicationRepository(DbContext dbContext, ILogger<ApplicationRepository> log) : base(dbContext)
        {
            _log = log;
        }

        public async Task<VwApplication?> GetApplicationDetails(Guid applicationId)
        {
            return await appContext.VwApplications.FirstOrDefaultAsync(c => c.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<VwApplication>> GetApplicationsForUser(Guid userId)
        {
            var query = appContext.VwApplications.Where(c => c.UserId == userId);
            return await query.ToListAsync();
        }

        public entity.Application GetApplication(Guid id)
        {
            return appContext.Application.Include("Address").Include("PetApplicationStatus").Where(c => c.Id == id).FirstOrDefault();
        }

        public int DeleteApplication(Guid id)
        {
            var applicationId = appContext.Application.Where(c => c.Id == id).FirstOrDefault();
            if (applicationId != null)
            {
                appContext.Application.Remove(applicationId);
                return appContext.SaveChanges();
            }
            return 0;
        }

        public async Task<bool> DoesReferenceNumberExists(string referenceNumber)
        {
            return await appContext.Application.AnyAsync(a => a.ReferenceNumber == referenceNumber);
        }

        public async Task<bool> PerformHealthCheckLogic()
        {
            try
            {
                // Attempt to open a connection to the database
                await appContext.Database.OpenConnectionAsync();

                // Check if the connection is open
                if (appContext.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error Stack: " + ex.StackTrace);
                _log.LogError("Exception Message: " + ex.Message);
                throw;
            }
        }
    }
}
