using Defra.PTS.Application.Entities;
using modelEntity = Defra.PTS.Application.Entities;
namespace Defra.PTS.Application.Repositories.Interfaces
{
    public interface ISignatoryRepository : IRepository<modelEntity.Signatory>
    {
        Task<Signatory?> GetLatestSignatory();
        Task<Signatory?> GetCurrentSignatory();
        Task<Signatory?> GetSignatoryById(Guid signatoryId);
        Task<Signatory?> GetSignatoryByName(string name);
    }
}
