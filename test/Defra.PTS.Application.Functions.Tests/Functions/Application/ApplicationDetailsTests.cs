using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Functions.Functions.Application;
using Defra.PTS.Application.Models;
using Defra.PTS.Application.Models.CustomException;
using Defra.PTS.Application.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Functions.Tests.Functions.Application
{
    [TestFixture]
    public class ApplicationDetailsTests
    {
        private readonly Mock<HttpRequest> _requestMoq = new();
        private readonly Mock<IApplicationService> _applicationServiceMoq = new();
        private readonly Mock<ILogger<ApplicationDetails>> _loggerMock = new();
        ApplicationDetails? _sut;

        [SetUp]
        public void SetUp()
        {            
            var applicationId = Guid.NewGuid();
            var expectedApplicationDetails = new VwApplication(); // Fill in with your expected result
            _applicationServiceMoq.Setup(service => service.GetApplicationDetails(applicationId))
                                   .ReturnsAsync(expectedApplicationDetails);
            _sut = new ApplicationDetails(_applicationServiceMoq.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown() 
        {
            _applicationServiceMoq.Reset();
        }

        [Test]
        public async Task GetApplicationDetails_WhenRequestDoesntExist_Then_ReturnsNotFound()
        {
            var applicationId = Guid.NewGuid();

            var json = "{\"ApplicationId\":\"00000000-0000-0000-0000-000000000000\"}";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            _requestMoq!.Setup(a => a.Body).Returns(memoryStream);

            _applicationServiceMoq.Setup(service => service.GetApplicationDetails(applicationId))
                                   .ThrowsAsync(new Exception());
            
            var result = await _sut!.GetApplicationDetails(_requestMoq.Object);

            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsInstanceOf<NotFoundObjectResult>(notFoundResult);

            Assert.IsNotNull(result);
            Assert.AreEqual($"No application found for id {Guid.Empty}", notFoundResult!.Value);
        }

        [Test]
        public async Task GetApplicationDetails_WhenRequestBodyDoesntExist_Then_ReturnsUserException()
        {
            var expectedResult = $"Failed to retrieve application details";
            
            _requestMoq!.Setup(a => a.Body).Returns(value: null!);

            var result = await _sut!.GetApplicationDetails(_requestMoq.Object);            

            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsInstanceOf<BadRequestObjectResult>(badRequestResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, badRequestResult!.Value);

        }

        [Test]
        public void GetApplicationDetails_WhenRequestBodyExists_Then_ReturnsSuccessMessageWithNotFoundObjectResult()
        {
            var applicationId = Guid.NewGuid();
                        
            var json = "{\"ApplicationId\":\"00000000-0000-0000-0000-000000000000\"}";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            _requestMoq!.Setup(a => a.Body).Returns(memoryStream);


            _applicationServiceMoq.Setup(a => a.GetApplicationDetails(applicationId)).ReturnsAsync(new VwApplication());

            var result = _sut!.GetApplicationDetails(_requestMoq.Object);
            var okResult = result.Result as NotFoundObjectResult;

            Assert.IsInstanceOf<NotFoundObjectResult>(okResult);

        }

        [Test]
        public void GetApplicationDetails_WhenRequestBodyExists_Then_ReturnsSuccessMessageWithValidGuid()
        {
            var applicationId = Guid.Empty;            
            
            var json = "{\"ApplicationId\":\"00000000-0000-0000-0000-000000000000\"}";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            _requestMoq.Setup(a => a.Body).Returns(memoryStream);


            _applicationServiceMoq.Setup(a => a.GetApplicationDetails(applicationId)).ReturnsAsync(new VwApplication());

            var result = _sut!.GetApplicationDetails(_requestMoq.Object);

            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);
        }
    }
}
