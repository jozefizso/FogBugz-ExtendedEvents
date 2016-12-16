using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Vereyon.Web;

namespace FBExtendedEvents.Tests
{
    [TestFixture]
    public class HtmlSanitizerTests
    {
        [Test]
        public void Sanitizer_ImgTag_AllowTagWithSrcAttribute()
        {
            // Arrange
            var html = @"<img src=""https://releasenotes/data/trunk/1481719514715/database.png"">";
            var expectedHtml = @"<img src=""https://releasenotes/data/trunk/1481719514715/database.png"">";

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

            // Act
            var actualHtml = sanitizer.Sanitize(html);

            // Assert
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void Sanitizer_ComplexReleaseNoteWithImg_RemoveOnlyClassAttribute()
        {
            // Arrange
            var html = @"<p>This is a test release note</p><p><img alt src=""https://releasenotes/data/trunk/1481719514715/database.png"" rel=""g1481719514715"" class=""cboxElement""></p>";
            var expectedHtml = @"<p>This is a test release note</p><p><img alt="""" src=""https://releasenotes/data/trunk/1481719514715/database.png"" rel=""g1481719514715""></p>";

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

            // Act
            var actualHtml = sanitizer.Sanitize(html);

            // Assert
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void Sanitizer_UnknownHtmlTag_FlattensTagAndPreservesContent()
        {
            // Arrange
            var html = @"Custom <x>tag</x>";
            var expectedHtml = @"Custom tag";

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

            // Act
            var actualHtml = sanitizer.Sanitize(html);

            // Assert
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void Sanitizer_HtmlScriptTag_RemovesTagAndContent()
        {
            // Arrange
            var html = @"Hello <script>alert('world')</script>";
            var expectedHtml = @"Hello ";

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

            // Act
            var actualHtml = sanitizer.Sanitize(html);

            // Assert
            Assert.AreEqual(expectedHtml, actualHtml);
        }
    }
}
