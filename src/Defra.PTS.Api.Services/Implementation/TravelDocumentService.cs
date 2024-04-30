using Defra.PTS.Application.Repositories.Interfaces;
using Defra.PTS.Application.Api.Services.Interface;
using Microsoft.Extensions.Logging;
using entity = Defra.PTS.Application.Entities;
using Defra.PTS.Application.Models.CustomException;
using Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Api.Services.Implementation
{
    public class TravelDocumentService : ITravelDocumentService
    {        
        private readonly ITravelDocumentRepository _travelDocumentRepository;
        private readonly IReferenceGeneratorService _referenceGeneratorService;
        public TravelDocumentService(              
            ITravelDocumentRepository travelDocumentRepository
            , IReferenceGeneratorService referenceGeneratorService)
        {            
            _travelDocumentRepository = travelDocumentRepository;
            _referenceGeneratorService = referenceGeneratorService;
        }

        public async Task<TravelDocument> CreateTravelDocument(entity.Application application)
        {

            try
            {
                var travelDocumentReference = await _referenceGeneratorService.GetTravelDocumentReference();

                if (string.IsNullOrEmpty(travelDocumentReference))
                {
                    throw new ApplicationFunctionException("Cannot create travel document");
                }

                var travelDocument = await GetTravelDocumentAsync(application);
                travelDocument.DocumentReferenceNumber = travelDocumentReference;

                await _travelDocumentRepository.Add(travelDocument);
                await _travelDocumentRepository.SaveChanges();

                return travelDocument;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw;
            }
        }

        public static Task<TravelDocument> GetTravelDocumentAsync(entity.Application application)
        {            
            var travelDocument = new TravelDocument();
            travelDocument.ApplicationId = application.Id;
            travelDocument.PetId = application.PetId;
            travelDocument.OwnerId = application.OwnerId;         
            travelDocument.CreatedOn = application.CreatedOn;
            travelDocument.CreatedBy = application.CreatedBy;
            travelDocument.IsLifeTIme = true;            
            travelDocument.DocumentSignedBy = "John Smith (APHA)";
            return Task.FromResult(travelDocument);
        }
    }
}
