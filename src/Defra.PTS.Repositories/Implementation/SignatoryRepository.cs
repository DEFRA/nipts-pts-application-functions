using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Repositories.Implementation
{
    [ExcludeFromCodeCoverageAttribute]
    public class SignatoryRepository : Repository<Signatory>, ISignatoryRepository
    {
        private readonly ILogger<SignatoryRepository> _log;
        private AppDbContext AppContext
        {
            get
            {
                return (AppDbContext)_dbContext;
            }
        }

        public SignatoryRepository(DbContext dbContext, ILogger<SignatoryRepository> log) : base(dbContext)
        {
            _log = log;
        }

        public async Task<Signatory?> GetLatestSignatory()
        {
            _log.LogInformation("Getting the latest signatory from the database.");
            return await AppContext.Signatories
                .OrderByDescending(s => s.ValidFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<Signatory?> GetSignatoryById(Guid signatoryId)
        {
            _log.LogInformation($"Getting signatory with ID: {signatoryId} from the database.");
            return await AppContext.Signatories
                .FirstOrDefaultAsync(s => s.ID == signatoryId);
        }

        public async Task<Signatory?> GetSignatoryByName(string name)
        {
            _log.LogInformation($"Getting signatory with Name: {name} from the database.");
            return await AppContext.Signatories
                .FirstOrDefaultAsync(s => s.Name == name);
        }
    }

}
