using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;

namespace FBExtendedEvents.Tests
{
    [TestFixture]
    public class DatabaseHelpersTests
    {
        public static IEnumerable<object[]> ValidDateTimeInputValues
        {
            get
            {
                yield return new object[] { "2015-01-01T17:47:27.0640000Z", new DateTime(2015, 1, 1, 17, 47, 27, 64, DateTimeKind.Utc) };
                yield return new object[] { "2015-01-01T17:47:27.064000Z", new DateTime(2015, 1, 1, 17, 47, 27, 64, DateTimeKind.Utc) };
                yield return new object[] { "2015-01-01T17:47:27Z", new DateTime(2015, 1, 1, 17, 47, 27, 0, DateTimeKind.Utc) };
                yield return new object[] { "2015-01-01T17:47:27.0000000+01:00", new DateTime(2015, 1, 1, 16, 47, 27, 0, DateTimeKind.Utc) };
                yield return new object[] { "2016-12-09T14:50:47+01:00", new DateTime(2016, 12, 9, 13, 50, 47, 0, DateTimeKind.Utc) };

                yield return new object[] { "2015-01-01T17:47:27.0645630Z", new DateTime(635557312470645630, DateTimeKind.Utc) };
                yield return new object[] { "2015-01-01T17:47:27.064563Z", new DateTime(635557312470645630, DateTimeKind.Utc) };
            }
        }

        [Test]
        [TestCaseSource(nameof(ValidDateTimeInputValues))]
        public void GetDateTime_ValidDateTimeStringValue_ConvertsToCorrectDateTime(string inputValue, DateTime expecteDateTimeUtc)
        {
            // Arrange
            var defaultValue = DateTime.MinValue;

            // Act
            var actualDateTime = DatabaseHelpers.GetDateTime(inputValue, defaultValue);

            // Assert
            Assert.AreEqual(actualDateTime.Kind, DateTimeKind.Utc);
            Assert.AreEqual(expecteDateTimeUtc, actualDateTime);
        }
    }
}
