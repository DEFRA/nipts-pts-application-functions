

using Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Repositories.Interfaces
{
    public interface ITravelDocumentRepository : IRepository<TravelDocument>
    {
        Task<bool> DoesTravelDocumentReferenceNumberExists(string documentRefrenceNumber);
    }
}
