using Defra.PTS.Application.Entities;
using entity = Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Repositories.Interfaces
{
    public interface IApplicationRepository : IRepository<entity.Application>
    {
        Task<VwApplication?> GetApplicationDetails(Guid applicationId);
        Task<IEnumerable<VwApplication>> GetApplicationsForUser(Guid userId);

        entity.Application GetApplication(Guid id);
        Task<bool> DoesReferenceNumberExists(string referenceNumber);
        int DeleteApplication(Guid id);
        Task<bool> PerformHealthCheckLogic();
    }
}
