using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Models.Dto;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Application.Api.Services.Implementation
{
    public class SignatoryService : ISignatoryService
    {
        private readonly ISignatoryRepository _signatoryRepository;
        private readonly ILogger<SignatoryService> _logger;

        public SignatoryService(ISignatoryRepository signatoryRepository, ILogger<SignatoryService> logger)
        {
            _signatoryRepository = signatoryRepository;
            _logger = logger;
        }

        public async Task<SignatoryDto?> GetLatestSignatory()
        {
            _logger.LogInformation("Retrieving the latest signatory.");
            var signatory = await _signatoryRepository.GetLatestSignatory();
            return signatory != null ? MapToDto(signatory) : null;
        }

        public async Task<SignatoryDto?> GetSignatoryById(Guid signatoryId)
        {
            _logger.LogInformation("Retrieving signatory with ID: {SignatoryId}", signatoryId);
            var signatory = await _signatoryRepository.GetSignatoryById(signatoryId);
            return signatory != null ? MapToDto(signatory) : null;
        }

        public async Task<SignatoryDto?> GetSignatoryByName(string name)
        {
            _logger.LogInformation("Retrieving signatory with Name: {Name}", name);
            var signatory = await _signatoryRepository.GetSignatoryByName(name);
            return signatory != null ? MapToDto(signatory) : null;
        }

        private static SignatoryDto MapToDto(Signatory signatory)
        {
            return new SignatoryDto
            {
                ID = signatory.ID,
                Name = signatory.Name,
                Title = signatory.Title,
                ValidFrom = signatory.ValidFrom,
                ValidTo = signatory.ValidTo,
                SignatureImage = signatory.SignatureImage
            };
        }
    }
}
