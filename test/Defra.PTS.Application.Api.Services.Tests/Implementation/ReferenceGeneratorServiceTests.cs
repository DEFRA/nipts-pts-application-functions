using Defra.PTS.Application.Api.Services.Implementation;
using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Api.Services.Tests.Implementation
{

    [TestFixture]
    public class ReferenceGeneratorServiceTests
    {        
        private readonly Mock<IApplicationServiceHelper> _applicationServiceHelperMock = new();
        private readonly Mock<ITravelDocumentServiceHelper> _travelDocumentServiceHelperMock = new();
        private readonly Mock<IApplicationRepository> _applicationRepositoryMock = new();
        private readonly Mock<ITravelDocumentRepository> _travelDocumentRepositoryMock = new();

        ReferenceGeneratorService? sut;

        [SetUp]
        public void SetUp()
        {                        
            sut = new ReferenceGeneratorService(                  
                _applicationServiceHelperMock.Object
                , _travelDocumentServiceHelperMock.Object
                , _applicationRepositoryMock.Object
                , _travelDocumentRepositoryMock.Object
                );
        }

        [Test]
        public async Task GetUniqueApplicationReference_ReturnsUniqueReference()
        {
            // Arrange
            var uniqueReference = "uniqueReference";
            _applicationServiceHelperMock.Setup(helper => helper.GenerateUniqueAlphaNumericCode(It.IsAny<int>()))
                                        .Returns(uniqueReference);

            _applicationRepositoryMock.Setup(repo => repo.DoesReferenceNumberExists(uniqueReference))
                                      .ReturnsAsync(false);

            // Act
            var result = await sut!.GetUniqueApplicationReference();

            // Assert
            Assert.AreEqual(uniqueReference, result);
        }

        [Test]
        public async Task GetUniqueApplicationReference_GeneratesNewReferenceIfExisting()
        {
            // Arrange
            var existingReference = "existingReference";
            var uniqueReference = "uniqueReference";

            _applicationServiceHelperMock.SetupSequence(helper => helper.GenerateUniqueAlphaNumericCode(It.IsAny<int>()))
                                        .Returns(existingReference)
                                        .Returns(uniqueReference);

            _applicationRepositoryMock.SetupSequence(repo => repo.DoesReferenceNumberExists(It.IsAny<string>()))
                                      .ReturnsAsync(true) // First call returns true, simulating existing reference
                                      .ReturnsAsync(false); // Second call returns false, simulating non-existing reference

            // Act
            var result = await sut!.GetUniqueApplicationReference();

            // Assert
            Assert.AreEqual(uniqueReference, result);
        }

        [Test]
        public async Task GetTravelDocumentReference_ReturnsUniqueReference()
        {
            // Arrange
            var uniqueReference = "uniqueReference";
            _travelDocumentServiceHelperMock.Setup(helper => helper.GenerateUniqueAlphaNumericCode(It.IsAny<int>()))
                                            .Returns(uniqueReference);

            _travelDocumentRepositoryMock.Setup(repo => repo.DoesTravelDocumentReferenceNumberExists(uniqueReference))
                                        .ReturnsAsync(false);


            // Act
            var result = await sut!.GetTravelDocumentReference();

            // Assert
            Assert.AreEqual(uniqueReference, result);
        }

        [Test]
        public async Task GetTravelDocumentReference_GeneratesNewReferenceIfExisting()
        {
            // Arrange
            var existingReference = "existingReference";
            var uniqueReference = "uniqueReference";
            _travelDocumentServiceHelperMock.SetupSequence(helper => helper.GenerateUniqueAlphaNumericCode(It.IsAny<int>()))
                                            .Returns(existingReference)
                                            .Returns(uniqueReference);

            _travelDocumentRepositoryMock.SetupSequence(repo => repo.DoesTravelDocumentReferenceNumberExists(It.IsAny<string>()))
                                        .ReturnsAsync(true) // First call returns true, simulating existing reference
                                        .ReturnsAsync(false); // Second call returns false, simulating non-existing reference

            // Act
            var result = await sut!.GetTravelDocumentReference();

            // Assert
            Assert.AreEqual(uniqueReference, result);
        }
    }
}
