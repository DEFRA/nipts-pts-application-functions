using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Models.Enums;
using Defra.PTS.Application.Repositories.Interfaces;

namespace Defra.PTS.Application.Api.Services.Implementation
{
    public class ReferenceGeneratorService(
        IApplicationServiceHelper applicationServiceHelper
            , ITravelDocumentServiceHelper travelDocumentServiceHelper
            , IApplicationRepository applicationRepository
            , ITravelDocumentRepository travelDocumentRepository) : IReferenceGeneratorService
    {        
        private readonly IApplicationServiceHelper _applicationServiceHelper = applicationServiceHelper;
        private readonly ITravelDocumentServiceHelper _travelDocumentServiceHelper = travelDocumentServiceHelper;
        private readonly IApplicationRepository _applicationRepository = applicationRepository;
        private readonly ITravelDocumentRepository _travelDocumentRepository = travelDocumentRepository;

        public async Task<string> GetUniqueApplicationReference()
        {
            string uniqueReferenceNumber;
            do
            {
                uniqueReferenceNumber = _applicationServiceHelper.GenerateUniqueAlphaNumericCode((int)ApplicationReferenceEnum.Length);
            } while (await _applicationRepository.DoesReferenceNumberExists(uniqueReferenceNumber));
            return uniqueReferenceNumber;
        }

        public async Task<string> GetTravelDocumentReference()
        {
            string travelDocumentReference;
            do
            {
             travelDocumentReference = _travelDocumentServiceHelper.GenerateUniqueAlphaNumericCode((int)TravelDocumentReferenceEnum.Length);
            } while (await _travelDocumentRepository.DoesTravelDocumentReferenceNumberExists(travelDocumentReference));
            return travelDocumentReference;
        }
    }
}
