using entity = Defra.PTS.Application.Entities;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Repositories.Implementation
{
    [ExcludeFromCodeCoverageAttribute]
    public class TravelDocumentRepository : Repository<entity.TravelDocument>, ITravelDocumentRepository
    {        
        private AppDbContext appContext
        {
            get
            {
                return _dbContext as AppDbContext;
            }
        }
        public TravelDocumentRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
             
        public async Task<bool> DoesTravelDocumentReferenceNumberExists(string documentRefrenceNumber)
        {
            return await appContext.TravelDocument.AnyAsync(a => a.DocumentReferenceNumber == documentRefrenceNumber);
        }        
    }
}
