using Defra.PTS.Application.Models.Dto;

namespace Defra.PTS.Application.Api.Services.Interface
{
    public interface ISignatoryService
    {
        Task<SignatoryDto?> GetLatestSignatory();
        Task<SignatoryDto?> GetSignatoryById(Guid signatoryId);
        Task<SignatoryDto?> GetSignatoryByName(string name);
    }
}
