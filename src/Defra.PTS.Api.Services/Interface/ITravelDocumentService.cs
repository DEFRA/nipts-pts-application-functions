
using Defra.PTS.Application.Entities;
using entity = Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Api.Services.Interface
{
    public interface ITravelDocumentService
    {
        Task<TravelDocument> CreateTravelDocument(entity.Application application);
    }
}
