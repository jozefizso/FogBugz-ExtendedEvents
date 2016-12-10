using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FBExtendedEvents.Tests
{
    [TestFixture]
    public class UrlExTests
    {
        public static IEnumerable<object[]> PluginIds
        {
            get
            {
                yield return new object[] { "plugin1", "default.asp?pg=pgPluginRaw&sPluginId=plugin1" };
                yield return new object[] { "FBExtendedEvents@goit.io", "default.asp?pg=pgPluginRaw&sPluginId=FBExtendedEvents%40goit.io" };
            }
        }

        [Test]
        [TestCaseSource(nameof(PluginIds))]
        public void PluginRawPageUrl_PluginId_ReturnsUrlEncodedPathToPlugin(string pluginId, string expectedPluginUrl)
        {
            // Arrange & Act
            var actualPluginUrl = UrlEx.PluginRawPageUrl(pluginId);

            // Assert
            Assert.AreEqual(expectedPluginUrl, actualPluginUrl);
        }
    }
}
