using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Headmaster.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpRequestMessageExtensionsTests
    {
        private const string MediaType = "application/vnd.vendor+json";
        private const string MediaTypeParameter = "version";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatRequestMessageIsNull_WhenTryingToGetApiVersion_ThenArgumentNullExceptionIsThrown()
        {
            HttpRequestMessage message = null;

            message.GetApiVersion("mediaType", "mediaTypeParameter");
        }

        [TestMethod]
        public void GivenThatRequestMessageDoesNotHaveCorrectHeader_WhenTryingToGetApiVersion_ThenEmptyStringIsReturned()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/api/stuff");

            var apiVersion = message.GetApiVersion("mediaType", "mediaTypeParameter");

            Assert.AreEqual(string.Empty, apiVersion);
        }

        [TestMethod]
        public void GivenThatRequestMessageHasCorrectHeaderButNoValue_WhenTryingToGetApiVersion_ThenEmptyStringIsReturned()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/api/stuff");
            message.Headers.Add("Accept", string.Empty);

            var apiVersion = message.GetApiVersion(MediaType, MediaTypeParameter);

            Assert.AreEqual(string.Empty, apiVersion);
        }

        [TestMethod]
        public void GivenThatRequestHasCorrectHeaderAndValueOfOne_WhenTryingToGetApiVersion_ThenStringWithContentOfOneIsReturned()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/api/stuff");
            message.Headers.Add("Accept", $"{MediaType}; " +
                                                                $"{MediaTypeParameter}=V1");

            var apiVersion = message.GetApiVersion(MediaType, MediaTypeParameter);

            Assert.AreEqual("V1", apiVersion);
        }
    }
}