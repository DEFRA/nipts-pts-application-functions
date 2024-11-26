
using Defra.PTS.Application.Entities;
using modelEntity = Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Api.Services.Interface
{
    public interface ITravelDocumentService
    {
        Task<TravelDocument> CreateTravelDocument(modelEntity.Application application);
    }
}
