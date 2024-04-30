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
    public class ApplicationServiceHelperTests
    {
        ApplicationServiceHelper? sut;

        [SetUp]
        public void SetUp()
        {
            sut = new ApplicationServiceHelper();
        }

        [Test]
        public void GenerateUniqueAlphaNumericCode_ReturnsValidCode()
        {
            const int length = 8;
            var result = sut!.GenerateUniqueAlphaNumericCode(length);

            Assert.AreEqual(length, result.Length);
        }
    }
}
