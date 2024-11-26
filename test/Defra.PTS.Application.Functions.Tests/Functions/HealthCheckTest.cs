using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Functions.Functions;
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

namespace Defra.PTS.Application.Functions.Tests.Functions
{
    public class HealthCheckTest
    {
        private Mock<HttpRequest> requestMoq = new();
        private Mock<ILogger> loggerMock = new();
        private Mock<IApplicationService> applicationServiceMoq = new();
        HealthCheck? sut;

        [SetUp]
        public void SetUp()
        {
            requestMoq = new Mock<HttpRequest>();
            loggerMock = new Mock<ILogger>();
            applicationServiceMoq = new Mock<IApplicationService>();

            sut = new HealthCheck(applicationServiceMoq.Object);
        }

        [TearDown]
        public void TearDown()
        {
            requestMoq.Reset();
            loggerMock.Reset();
            applicationServiceMoq.Reset();

            sut = null;
        }

        [Test]
        public void HealthCheck_WhenTrue_Then_ReturnsServiceAvailable()
        {
            applicationServiceMoq.Setup(a => a.PerformHealthCheckLogic()).Returns(Task.FromResult(true));
            var result = sut!.Run(requestMoq.Object, loggerMock.Object);
            var okResult = result.Result as OkResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, okResult?.StatusCode);
            applicationServiceMoq.Verify(a => a.PerformHealthCheckLogic(), Times.Once);
        }

        [Test]
        public void HealthCheck_WhenFalse_Then_ReturnsServiceUnavailable()
        {
            applicationServiceMoq.Setup(a => a.PerformHealthCheckLogic()).Returns(Task.FromResult(false));
            var result = sut!.Run(requestMoq.Object, loggerMock.Object);
            var okResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(503, okResult?.StatusCode);
            applicationServiceMoq.Verify(a => a.PerformHealthCheckLogic(), Times.Once);
        }


     

    }
}
