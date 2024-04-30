using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Functions.Functions.Application;
using Defra.PTS.Application.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Functions.Tests.Functions.Application
{
    [TestFixture]
    public class GetUserApplicationsTests
    {
        private Mock<IApplicationService> _applicationServiceMock;
        private Mock<ILogger<GetUserApplications>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _applicationServiceMock = new Mock<IApplicationService>();
            _loggerMock = new Mock<ILogger<GetUserApplications>>();
        }

        [Test]
        public async Task Run_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedApplications = new List<ApplicationSummaryDto>(); // Fill in with your expected result
            _applicationServiceMock.Setup(service => service.GetApplicationsForUser(userId))
                                   .ReturnsAsync(expectedApplications);
            var httpRequestMock = new Mock<HttpRequest>();
            var sut = new GetUserApplications(_applicationServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await sut.Run(httpRequestMock.Object, userId.ToString());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedApplications, okResult.Value);
        }

        [Test]
        public async Task Run_InvalidUserId_ReturnsBadRequestResult()
        {
            // Arrange
            var invalidUserId = "invalid-id";
            var httpRequestMock = new Mock<HttpRequest>();
            var sut = new GetUserApplications(_applicationServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await sut.Run(httpRequestMock.Object, invalidUserId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("You must provide a valid value for userId", badRequestResult.Value);
        }
    }
}
