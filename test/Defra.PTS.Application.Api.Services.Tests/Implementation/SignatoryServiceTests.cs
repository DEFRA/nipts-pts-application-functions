using Defra.PTS.Application.Api.Services.Implementation;
using Defra.PTS.Application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Api.Services.Tests.Implementation
{
    [TestFixture]
    public class SignatoryServiceTests
    {
        private Mock<ISignatoryRepository> _signatoryRepositoryMock = new();
        private Mock<ILogger<SignatoryService>> _loggerMock = new();
        private SignatoryService? sut;

        [SetUp]
        public void SetUp()
        {
            _signatoryRepositoryMock = new Mock<ISignatoryRepository>();
            _loggerMock = new Mock<ILogger<SignatoryService>>();
            sut = new SignatoryService(_signatoryRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetLatestSignatory_WhenSignatoryExists_ReturnsSignatoryDto()
        {
            // Arrange
            var signatory = new Signatory
            {
                ID = Guid.NewGuid(),
                Name = "John Doe",
                Title = "Manager",
                ValidFrom = DateTime.Now.AddYears(-1),
                ValidTo = DateTime.Now.AddYears(1),
                SignatureImage = Convert.FromBase64String("aW1hZ2VfZGF0YQ==") // Example byte array data
            };
            _signatoryRepositoryMock.Setup(repo => repo.GetLatestSignatory()).ReturnsAsync(signatory);

            // Act
            var result = await sut!.GetLatestSignatory();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(signatory.ID, result?.ID);
            Assert.AreEqual(signatory.Name, result?.Name);
        }

        [Test]
        public async Task GetLatestSignatory_WhenSignatoryDoesNotExist_ReturnsNull()
        {
            // Arrange
            _signatoryRepositoryMock.Setup(repo => repo.GetLatestSignatory()).ReturnsAsync((Signatory?)null);

            // Act
            var result = await sut!.GetLatestSignatory();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetCurrentSignatory_WhenSignatoryDoesNotExist_ReturnsNull()
        {
            // Arrange
            _signatoryRepositoryMock.Setup(repo => repo.GetCurrentSignatory()).ReturnsAsync((Signatory?)null);

            // Act
            var result = await sut!.GetCurrentSignatory();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetSignatoryById_WhenSignatoryExists_ReturnsSignatoryDto()
        {
            // Arrange
            var signatoryId = Guid.NewGuid();
            var signatory = new Signatory
            {
                ID = signatoryId,
                Name = "Jane Doe",
                Title = "Director",
                ValidFrom = DateTime.Now.AddYears(-2),
                ValidTo = DateTime.Now.AddYears(2),
                SignatureImage = Convert.FromBase64String("c2lnbmF0dXJlX2RhdGE=") // Example byte array data
            };
            _signatoryRepositoryMock.Setup(repo => repo.GetSignatoryById(signatoryId)).ReturnsAsync(signatory);

            // Act
            var result = await sut!.GetSignatoryById(signatoryId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(signatoryId, result?.ID);
            Assert.AreEqual(signatory.Name, result?.Name);
        }

        [Test]
        public async Task GetSignatoryById_WhenSignatoryDoesNotExist_ReturnsNull()
        {
            // Arrange
            var signatoryId = Guid.NewGuid();
            _signatoryRepositoryMock.Setup(repo => repo.GetSignatoryById(signatoryId)).ReturnsAsync((Signatory?)null);

            // Act
            var result = await sut!.GetSignatoryById(signatoryId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetSignatoryByName_WhenSignatoryExists_ReturnsSignatoryDto()
        {
            // Arrange
            var name = "Jane Doe";
            var signatory = new Signatory
            {
                ID = Guid.NewGuid(),
                Name = name,
                Title = "Director",
                ValidFrom = DateTime.Now.AddYears(-2),
                ValidTo = DateTime.Now.AddYears(2),
                SignatureImage = Convert.FromBase64String("c2lnbmF0dXJlX2RhdGE=") // Example byte array data
            };
            _signatoryRepositoryMock.Setup(repo => repo.GetSignatoryByName(name)).ReturnsAsync(signatory);

            // Act
            var result = await sut!.GetSignatoryByName(name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result?.Name);
        }

        [Test]
        public async Task GetSignatoryByName_WhenSignatoryDoesNotExist_ReturnsNull()
        {
            // Arrange
            var name = "Nonexistent Signatory";
            _signatoryRepositoryMock.Setup(repo => repo.GetSignatoryByName(name)).ReturnsAsync((Signatory?)null);

            // Act
            var result = await sut!.GetSignatoryByName(name);

            // Assert
            Assert.IsNull(result);
        }
    }
}
