using Defra.PTS.Application.Api.Services.Implementation;
using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using entity = Defra.PTS.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.PTS.Application.Repositories.Implementation;
using Defra.PTS.Application.Models.CustomException;

namespace Defra.PTS.Application.Api.Services.Tests.Implementation
{
    [TestFixture]
    public class TravelDocumentServiceTests
    {        
        private readonly Mock<ITravelDocumentRepository> _travelDocumentRepositoryMock = new();
        private readonly Mock<IReferenceGeneratorService> _referenceGeneratorServiceMock = new();

        TravelDocumentService? sut;

        [SetUp]
        public void SetUp()
        {           
            sut = new TravelDocumentService(                  
                _travelDocumentRepositoryMock.Object
                , _referenceGeneratorServiceMock.Object
                );
        }

        [Test]
        public async Task CreateTravelDocument_ValidApplication_CreatesAndReturnsTravelDocument()
        {
            // Arrange
            Guid guid = Guid.Empty;
            var application = new entity.Application(); // assuming you have a valid application instance
            var travelDocumentDB = new TravelDocument(); // assuming you have a valid travel document instance
            var travelDocumentReference = "ABC123"; // assuming a valid travel document reference

            _referenceGeneratorServiceMock.Setup(service => service.GetTravelDocumentReference())
                                         .ReturnsAsync(travelDocumentReference);

            _travelDocumentRepositoryMock.Setup(repo => repo.Add(It.IsAny<TravelDocument>())).Returns(Task.FromResult(travelDocumentDB.Id = guid));
            await _travelDocumentRepositoryMock.Object.Add(travelDocumentDB);
            _travelDocumentRepositoryMock.Setup(a => a.SaveChanges()).ReturnsAsync(1);


            // Act
            var result = await sut!.CreateTravelDocument(application);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(travelDocumentReference, result.DocumentReferenceNumber);
        }

        [Test]
        public void CreateTravelDocument_InvalidReference_ThrowsException()
        {
            // Arrange
            var application = new entity.Application(); // assuming you have a valid application instance

            _referenceGeneratorServiceMock.Setup(service => service.GetTravelDocumentReference())
                                         .ReturnsAsync(string.Empty); // simulate invalid reference


            // Act & Assert
            Assert.ThrowsAsync<ApplicationFunctionException>(() => sut!.CreateTravelDocument(application));
        }
    }
}
