using Defra.PTS.Application.Models.Helper;
using NUnit.Framework;
using System;
using System.ComponentModel;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Defra.PTS.Application.Functions.Tests.Models
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        enum TestEnum
        {
            [Description("Description for Value1")]
            Value1,
            Value2,
            [Description("Description for Value3")]
            Value3
        }

        [Test]
        public void GetDescription_EnumWithValue1_ReturnsDescriptionForValue1()
        {
            // Arrange
            var enumValue = TestEnum.Value1;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.AreEqual("Description for Value1", description);
        }

        [Test]
        public void GetDescription_EnumWithValue2_ReturnsValue2AsString()
        {
            // Arrange
            var enumValue = TestEnum.Value2;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.AreEqual("Value2", description);
        }

        [Test]
        public void GetDescription_EnumWithValue3_ReturnsDescriptionForValue3()
        {
            // Arrange
            var enumValue = TestEnum.Value3;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.AreEqual("Description for Value3", description);
        }
    }
}
