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

        [Test]
        public void Sanitizer_ImageWithBase64DataUri_PreservesTag()
        {
            // Arrange
            var html = @"<img src=""data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg=="" alt=""Red dot"">";
            var expectedHtml = @"<img src=""data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg=="" alt=""Red dot"">";

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

            // Act
            var actualHtml = sanitizer.Sanitize(html);

            // Assert
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void Sanitizer_ImageWithBase64DataHtmlXssUri_RemovesTag()
        {
            // Arrange
            var html = @"<img src=""data:text/html,<script>alert('hi');</script>"" alt=""Red dot"" />";
            var expectedHtml = String.Empty;

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

            // Act
            var actualHtml = sanitizer.Sanitize(html);

            // Assert
            Assert.AreEqual(expectedHtml, actualHtml);
        }
    }
}
