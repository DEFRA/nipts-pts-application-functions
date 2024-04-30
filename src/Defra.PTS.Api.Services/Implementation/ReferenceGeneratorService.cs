using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Models.Enums;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Api.Services.Implementation
{
    public class ReferenceGeneratorService : IReferenceGeneratorService
    {        
        private readonly IApplicationServiceHelper _applicationServiceHelper;
        private readonly ITravelDocumentServiceHelper _travelDocumentServiceHelper;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ITravelDocumentRepository _travelDocumentRepository;
        public ReferenceGeneratorService(              
            IApplicationServiceHelper applicationServiceHelper
            , ITravelDocumentServiceHelper travelDocumentServiceHelper
            , IApplicationRepository applicationRepository
            , ITravelDocumentRepository travelDocumentRepository)
        {            
            _applicationServiceHelper = applicationServiceHelper;
            _travelDocumentServiceHelper = travelDocumentServiceHelper;
            _applicationRepository = applicationRepository;
            _travelDocumentRepository = travelDocumentRepository;
        }

        public async Task<string> GetUniqueApplicationReference()
        {
            string uniqueReferenceNumber = string.Empty;
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
