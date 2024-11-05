using Defra.PTS.Application.Entities;
using modelEntity = Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Repositories.Interfaces
{
    public interface IApplicationRepository : IRepository<modelEntity.Application>
    {
        Task<VwApplication?> GetApplicationDetails(Guid applicationId);
        Task<IEnumerable<VwApplication>> GetApplicationsForUser(Guid userId);

        modelEntity.Application GetApplication(Guid id);
        Task<bool> DoesReferenceNumberExists(string referenceNumber);
        int DeleteApplication(Guid id);
        Task<bool> PerformHealthCheckLogic();
    }
}
