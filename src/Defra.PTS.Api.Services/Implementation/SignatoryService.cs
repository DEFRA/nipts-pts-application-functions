using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Models.Dto;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Application.Api.Services.Implementation
{
    public partial class SignatoryService : ISignatoryService
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
            LogRetrievingLatestSignatory();
            var signatory = await _signatoryRepository.GetLatestSignatory();
            return signatory != null ? MapToDto(signatory) : null;
        }

        public async Task<SignatoryDto?> GetCurrentSignatory()
        {
            LogRetrievingLatestSignatory();
            var signatory = await _signatoryRepository.GetCurrentSignatory();
            return signatory != null ? MapToDto(signatory) : null;
        }

        public async Task<SignatoryDto?> GetSignatoryById(Guid signatoryId)
        {
            LogRetrievingSignatoryById(signatoryId);
            var signatory = await _signatoryRepository.GetSignatoryById(signatoryId);
            return signatory != null ? MapToDto(signatory) : null;
        }

        public async Task<SignatoryDto?> GetSignatoryByName(string name)
        {
            LogRetrievingSignatoryByName(name);
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

        [LoggerMessage(Level = LogLevel.Information, Message = "Retrieving the latest signatory.")]
        private partial void LogRetrievingLatestSignatory();

        [LoggerMessage(Level = LogLevel.Information, Message = "Retrieving signatory with ID: {SignatoryId}")]
        private partial void LogRetrievingSignatoryById(Guid signatoryId);

        [LoggerMessage(Level = LogLevel.Information, Message = "Retrieving signatory with Name: {Name}")]
        private partial void LogRetrievingSignatoryByName(string name);
    }
}
