using Defra.PTS.Application.Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Defra.PTS.Application.Models.Dto;
using Defra.PTS.Application.Functions.Application;

namespace Defra.PTS.Application.Functions.Tests.Controllers
{
    [TestFixture]
    public class SignatoryControllerTests
    {
        private Mock<ISignatoryService> _signatoryServiceMock = new();
        private Mock<ILogger<SignatoryController>> _loggerMock = new();
        private SignatoryController? sut;

        [SetUp]
        public void SetUp()
        {
            _signatoryServiceMock = new Mock<ISignatoryService>();
            _loggerMock = new Mock<ILogger<SignatoryController>>();
            sut = new SignatoryController(_signatoryServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetLatestSignatory_WhenSignatoryExists_ReturnsOkResult()
        {
            // Arrange
            var signatoryDto = new SignatoryDto
            {
                ID = Guid.NewGuid(),
                Name = "John Doe",
                Title = "Manager",
                ValidFrom = DateTime.Now.AddYears(-1),
                ValidTo = DateTime.Now.AddYears(1),
                SignatureImage = Convert.FromBase64String("aW1hZ2VfZGF0YQ==")
            };
            _signatoryServiceMock.Setup(service => service.GetLatestSignatory()).ReturnsAsync(signatoryDto);
            var httpRequestMock = new Mock<HttpRequest>();

            // Act
            var result = await sut!.GetLatestSignatory(httpRequestMock.Object);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(signatoryDto, okResult.Value);
        }

        [Test]
        public async Task GetLatestSignatory_WhenSignatoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            _signatoryServiceMock.Setup(service => service.GetLatestSignatory()).ReturnsAsync((SignatoryDto?)null);
            var httpRequestMock = new Mock<HttpRequest>();

            // Act
            var result = await sut!.GetLatestSignatory(httpRequestMock.Object);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetSignatoryById_WhenSignatoryExists_ReturnsOkResult()
        {
            // Arrange
            var signatoryId = Guid.NewGuid();
            var signatoryDto = new SignatoryDto
            {
                ID = signatoryId,
                Name = "Jane Doe",
                Title = "Director",
                ValidFrom = DateTime.Now.AddYears(-2),
                ValidTo = DateTime.Now.AddYears(2),
                SignatureImage = Convert.FromBase64String("c2lnbmF0dXJlX2RhdGE=")
            };
            _signatoryServiceMock.Setup(service => service.GetSignatoryById(signatoryId)).ReturnsAsync(signatoryDto);
            var httpRequestMock = new Mock<HttpRequest>();

            // Act
            var result = await sut!.GetSignatoryById(httpRequestMock.Object, signatoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(signatoryDto, okResult.Value);
        }

        [Test]
        public async Task GetSignatoryById_WhenSignatoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var signatoryId = Guid.NewGuid();
            _signatoryServiceMock.Setup(service => service.GetSignatoryById(signatoryId)).ReturnsAsync((SignatoryDto?)null);
            var httpRequestMock = new Mock<HttpRequest>();

            // Act
            var result = await sut!.GetSignatoryById(httpRequestMock.Object, signatoryId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetSignatoryByName_WhenSignatoryExists_ReturnsOkResult()
        {
            // Arrange
            var name = "Jane Doe";
            var signatoryDto = new SignatoryDto
            {
                ID = Guid.NewGuid(),
                Name = name,
                Title = "Director",
                ValidFrom = DateTime.Now.AddYears(-2),
                ValidTo = DateTime.Now.AddYears(2),
                SignatureImage = Convert.FromBase64String("c2lnbmF0dXJlX2RhdGE=")
            };
            _signatoryServiceMock.Setup(service => service.GetSignatoryByName(name)).ReturnsAsync(signatoryDto);
            var httpRequestMock = new Mock<HttpRequest>();

            // Act
            var result = await sut!.GetSignatoryByName(httpRequestMock.Object, name);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(signatoryDto, okResult.Value);
        }

        [Test]
        public async Task GetSignatoryByName_WhenSignatoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var name = "Nonexistent Signatory";
            _signatoryServiceMock.Setup(service => service.GetSignatoryByName(name)).ReturnsAsync((SignatoryDto?)null);
            var httpRequestMock = new Mock<HttpRequest>();

            // Act
            var result = await sut!.GetSignatoryByName(httpRequestMock.Object, name);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
