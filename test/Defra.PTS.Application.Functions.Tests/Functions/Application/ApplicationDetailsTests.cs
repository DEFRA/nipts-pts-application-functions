using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Functions.Functions.Application;
using Defra.PTS.Application.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;

namespace Defra.PTS.Application.Functions.Tests.Functions.Application
{
    [TestFixture]
    public class ApplicationDetailsTests
    {
        private Mock<HttpRequest> _requestMoq = null!;
        private Mock<IApplicationService> _applicationServiceMoq = null!;
        private Mock<ISignatoryService> _signatoryServiceMoq = null!;
        private Mock<ILogger<ApplicationDetails>> _loggerMock = null!;
        private ApplicationDetails _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _requestMoq = new Mock<HttpRequest>();
            _applicationServiceMoq = new Mock<IApplicationService>();
            _signatoryServiceMoq = new Mock<ISignatoryService>();
            _loggerMock = new Mock<ILogger<ApplicationDetails>>();

            _sut = new ApplicationDetails(_applicationServiceMoq.Object, _signatoryServiceMoq.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationServiceMoq.Reset();
            _signatoryServiceMoq.Reset();
        }

        [Test]
        public async Task GetApplicationDetails_WhenRequestBodyIsNull_Then_ReturnsBadRequest()
        {
            // Arrange
            var expectedResult = $"Failed to retrieve application details";
            _requestMoq.Setup(a => a.Body).Returns(value: null!);

            // Act
            var result = await _sut.GetApplicationDetails(_requestMoq.Object);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsInstanceOf<BadRequestObjectResult>(badRequestResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, badRequestResult!.Value);
        }

        [Test]
        public async Task GetApplicationDetails_WhenApplicationNotFound_Then_ReturnsNotFound()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var json = $"{{\"ApplicationId\":\"{applicationId}\"}}";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            _requestMoq.Setup(a => a.Body).Returns(memoryStream);

            _applicationServiceMoq.Setup(service => service.GetApplicationDetails(applicationId))
                                  .ReturnsAsync((VwApplication)null!);

            // Act
            var result = await _sut.GetApplicationDetails(_requestMoq.Object);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsInstanceOf<NotFoundObjectResult>(notFoundResult);
            Assert.IsNotNull(result);
            Assert.AreEqual($"No application found for id {applicationId}", notFoundResult!.Value);
        }

        [Test]
        public async Task GetApplicationDetails_WhenSignatoryNotFound_Then_ReturnsNotFound()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var json = $"{{\"ApplicationId\":\"{applicationId}\"}}";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            _requestMoq.Setup(a => a.Body).Returns(memoryStream);

            var expectedApplicationDetails = new VwApplication
            {
                ApplicationId = applicationId,
                Status = "Pending"
            };

            _applicationServiceMoq.Setup(service => service.GetApplicationDetails(applicationId))
                                  .ReturnsAsync(expectedApplicationDetails);

            _signatoryServiceMoq.Setup(service => service.GetLatestSignatory())
                                .ReturnsAsync((SignatoryDto)null!);

            // Act
            var result = await _sut.GetApplicationDetails(_requestMoq.Object);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsInstanceOf<NotFoundObjectResult>(notFoundResult);
            Assert.IsNotNull(result);
            Assert.AreEqual("No signatory information available", notFoundResult!.Value);
        }

        [Test]
        public async Task GetApplicationDetails_WhenApplicationAndSignatoryExist_Then_ReturnsOk()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var json = $"{{\"ApplicationId\":\"{applicationId}\"}}";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            _requestMoq.Setup(a => a.Body).Returns(memoryStream);

            var expectedApplicationDetails = new VwApplication
            {
                ApplicationId = applicationId,
                Status = "Pending"
            };
            var expectedSignatoryDetails = new SignatoryDto
            {
                Name = "John Doe",
                Title = "Veterinary Director",
                SignatureImage = Convert.FromBase64String("SGVsbG8gd29ybGQ=") // Base64 representation of "Hello world"
            };

            _applicationServiceMoq.Setup(service => service.GetApplicationDetails(applicationId))
                                  .ReturnsAsync(expectedApplicationDetails);

            _signatoryServiceMoq.Setup(service => service.GetLatestSignatory())
                                .ReturnsAsync(expectedSignatoryDetails);

            // Act
            var result = await _sut.GetApplicationDetails(_requestMoq.Object);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);

            // Deserialize the response to check the values
            var responseBodyJson = JsonConvert.SerializeObject(okResult?.Value);
            var responseBody = JsonConvert.DeserializeObject<CombinedResponse>(responseBodyJson);

            Assert.IsNotNull(responseBody);
            Assert.AreEqual(expectedApplicationDetails.ApplicationId, responseBody!.ApplicationId);
            Assert.AreEqual(expectedApplicationDetails.Status, responseBody.Status);
            Assert.AreEqual(expectedSignatoryDetails.Name, responseBody.DocumentSignedBy);
            Assert.AreEqual(expectedSignatoryDetails.Title, responseBody.DocumentSignedByTitle);
            Assert.AreEqual(expectedSignatoryDetails.SignatureImage, responseBody.DocumentSignedBySignature);
        }
    }

    public class CombinedResponse
    {
        public Guid ApplicationId { get; set; }
        public string? Status { get; set; }
        public string? DocumentSignedBy { get; set; }
        public string? DocumentSignedByTitle { get; set; }
        public byte[]? DocumentSignedBySignature { get; set; }
    }
}
