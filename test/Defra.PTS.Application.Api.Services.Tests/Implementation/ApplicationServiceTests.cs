using Defra.PTS.Application.Api.Services.Implementation;
using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using entity = Defra.PTS.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.PTS.Application.Repositories.Implementation;
using Defra.PTS.Application.Models.CustomException;
using Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Api.Services.Tests.Implementation
{
    [TestFixture]
    public class ApplicationServiceTests
    {
        private Mock<ILogger<ApplicationService>> _loggerMock;
        private Mock<IApplicationRepository> _applicationRepository;
        private Mock<IReferenceGeneratorService> _referenceGeneratorService;
        ApplicationService sut;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ApplicationService>>();
            _applicationRepository = new Mock<IApplicationRepository>();
            _referenceGeneratorService = new Mock<IReferenceGeneratorService>();            
        }

        [Test]
        public async Task CreateApplication_WhenValidData_ReturnsGuid() 
        {
            var uniqueReferenceNumber = "ABC123XY";
            Guid guid = Guid.Empty;
          
            var newApplicationItem = new entity.Application
            {
                // Populate applicationItem with expected values
                PetId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Status = "COMPLETED",
                IsDeclarationSigned = true,
                IsConsentAgreed = true,
                IsPrivacyPolicyAgreed = true,
                DateOfApplication = DateTime.Now
            };

            _referenceGeneratorService.Setup(svc => svc.GetUniqueApplicationReference()).ReturnsAsync(uniqueReferenceNumber);

            var applicationDB = new entity.Application
            {
                // Populate applicationDB with expected values
                PetId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Status = "COMPLETED",
                ReferenceNumber = uniqueReferenceNumber,
                IsDeclarationSigned = true,
                IsConsentAgreed = true,
                IsPrivacyPolicyAgreed = true,
                DateOfApplication = DateTime.Now
            };

            _applicationRepository.Setup(a => a.Add(applicationDB)).Returns(Task.FromResult(applicationDB.Id = guid));

            await _applicationRepository.Object.Add(applicationDB);
            _applicationRepository.Setup(a => a.SaveChanges()).ReturnsAsync(1);
            

            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);

            var result = sut.CreateApplication(newApplicationItem);
            Assert.AreEqual(guid, result.Result.Id);
        }

        [Test]
        public async Task CreateApplication_WhenInValidValidData_ThrowsCustomException()
        {
            string uniqueReferenceNumber = null;
            var expectedResult = $"Cannot create Unique Application Reference";
            Guid guid = Guid.Empty;

            var newApplicationItem = new entity.Application
            {
                // Populate applicationDB with expected values
                PetId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Status = "COMPLETED",
                IsDeclarationSigned = true,
                IsConsentAgreed = true,
                IsPrivacyPolicyAgreed = true,
                DateOfApplication = DateTime.Now
            };

            _referenceGeneratorService.Setup(svc => svc.GetUniqueApplicationReference()).ReturnsAsync(uniqueReferenceNumber);

            var applicationDB = new entity.Application
            {
                // Populate applicationDB with expected values
                PetId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Status = "COMPLETED",
                ReferenceNumber = uniqueReferenceNumber,
                IsDeclarationSigned = true,
                IsConsentAgreed = true,
                IsPrivacyPolicyAgreed = true,
                DateOfApplication = DateTime.Now
            };

            _applicationRepository.Setup(a => a.Add(applicationDB)).Returns(Task.FromResult(applicationDB.Id = guid));

            await _applicationRepository.Object.Add(applicationDB);
            _applicationRepository.Setup(a => a.SaveChanges()).ReturnsAsync(1);


            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);

            ApplicationFunctionException result = Assert.ThrowsAsync<ApplicationFunctionException>(() => sut.CreateApplication(newApplicationItem));
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Message);
        }

        [Test]
        public void GetApplication_ReturnsCorrectApplication()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedApplication = new entity.Application(); // Assuming entity.Application is your application model
            _applicationRepository.Setup(repo => repo.GetApplication(id))
                                      .Returns(expectedApplication);

            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);

            // Act
            var result = sut.GetApplication(id);

            // Assert
            Assert.AreEqual(expectedApplication, result);
        }


        [Test]
        public async Task GetApplicationsForUser_ReturnsApplicationSummaryDtoList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var applicationData = new List<VwApplication>(); // Assuming Application is your entity model

            _applicationRepository.Setup(repo => repo.GetApplicationsForUser(userId))
                                      .ReturnsAsync(applicationData);

            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);

            // Act
            var result = await sut.GetApplicationsForUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(applicationData.Count, result.Count());
        }

        [Test]
        public async Task GetApplicationDetails_ReturnsApplication()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var expectedApplication = new VwApplication(); // Assuming VwApplication is the return type

            _applicationRepository.Setup(repo => repo.GetApplicationDetails(applicationId))
                                      .ReturnsAsync(expectedApplication);

            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);


            // Act
            var result = await sut.GetApplicationDetails(applicationId);

            // Assert
            Assert.AreEqual(expectedApplication, result);
        }

        [Test]
        public async Task GetApplicationDetails_InvalidApplicationId_ReturnsNull()
        {
            // Arrange
            var invalidApplicationId = Guid.Empty; // or any invalid GUID

            _applicationRepository.Setup(repo => repo.GetApplicationDetails(invalidApplicationId))
                                      .ReturnsAsync((VwApplication)null);

            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);


            // Act
            var result = await sut.GetApplicationDetails(invalidApplicationId);


            // Assert
            Assert.IsNull(result);
        }


        [Test]
        public async Task PerformHealthCheckLogic_RepositoryReturnsTrue_ReturnsTrue()
        {
            // Arrange
            _applicationRepository.Setup(repo => repo.PerformHealthCheckLogic())
                                      .ReturnsAsync(true);

            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);

            // Act
            var result = await sut.PerformHealthCheckLogic();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void PerformHealthCheckLogic_RepositoryThrowsException_LogsAndRethrowsException()
        {
            // Arrange
            _applicationRepository.Setup(repo => repo.PerformHealthCheckLogic())
                                      .ThrowsAsync(new Exception("Simulated exception"));

            sut = new ApplicationService(_loggerMock.Object, _applicationRepository.Object, _referenceGeneratorService.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await sut.PerformHealthCheckLogic());
        }
    }
}
