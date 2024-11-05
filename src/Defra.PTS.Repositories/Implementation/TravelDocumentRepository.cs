using modelEntity = Defra.PTS.Application.Entities;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Repositories.Implementation
{
    [ExcludeFromCodeCoverageAttribute]
    public class TravelDocumentRepository : Repository<modelEntity.TravelDocument>, ITravelDocumentRepository
    {        
        private AppDbContext AppContext
        {
            get
            {
                return (AppDbContext)_dbContext;
            }
        }
        public TravelDocumentRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
             
        public async Task<bool> DoesTravelDocumentReferenceNumberExists(string documentRefrenceNumber)
        {
            return await AppContext.TravelDocument.AnyAsync(a => a.DocumentReferenceNumber == documentRefrenceNumber);
        }        
    }
}
