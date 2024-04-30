using Defra.PTS.Application.Api.Services.Implementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Api.Services.Tests.Implementation
{
    [TestFixture]
    public class TravelDocumentServiceHelperTests
    {
        [Test]
        public void GenerateUniqueAlphaNumericCode_ReturnsValidCode()
        {
            // Arrange
            var serviceHelper = new TravelDocumentServiceHelper();
            var length = 10; // assuming length of the generated code
            var expectedResultLength = length + 5; // assuming the prefix length is always 5 characters

            // Act
            var result = serviceHelper.GenerateUniqueAlphaNumericCode(length);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResultLength, result.Length);
        }

        [Test]
        public void GenerateUniqueAlphaNumericCode_DoesNotStartWithAD()
        {
            // Arrange
            var serviceHelper = new TravelDocumentServiceHelper();
            var length = 10; // assuming length of the generated code

            // Act
            var result = serviceHelper.GenerateUniqueAlphaNumericCode(length);

            // Assert
            Assert.IsFalse(result.StartsWith("AD"));
        }
    }
}
