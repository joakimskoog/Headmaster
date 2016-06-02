using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Http.Controllers;
using Headmaster.Core.Tests.Outer.v1;
using Headmaster.Core.Tests.v0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Headmaster.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpControllerDescriptorExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatControllerDescriptorIsNull_WhenTryingToGetSupportedVersion_ThenArgumentNullExceptionIsThrown()
        {
            HttpControllerDescriptor desc = null;
            desc.GetSupportedVersion();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatControllerDescriptorIsNull_WhenCheckingIfItHasSupportForVersion_ThenArgumentNullExceptionIsThrown()
        {
            HttpControllerDescriptor desc = null;
            desc.HasSupportForVersion("v1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatVersionIsNull_WhenCheckingIfControllerDescriptorHasSupportForVersion_ThenArgumentNullExceptionIsThrown()
        {
            var descriptor = CreateControllerDescriptor(typeof(TestApiController));

            descriptor.HasSupportForVersion(null);
        }

        [TestMethod]
        public void GivenControllerWithSupportForVersionOne_WhenTryingToGetSupportedVersion_ThenVersionOneIsReturned()
        {
            var descriptor = CreateControllerDescriptor(typeof(InnerTestApiController));

            var supportedVersion = descriptor.GetSupportedVersion();

            Assert.AreEqual("v1", supportedVersion);
        }

        [TestMethod]
        public void GivenControllerWithSupportForVersionZero_WhenCheckingIfItHasSupportForVersionZero_ThenTrueIsReturned()
        {
            var descriptor = CreateControllerDescriptor(typeof(TestApiController));

            var hasSupport = descriptor.HasSupportForVersion("v0");

            Assert.IsTrue(hasSupport);
        }

        [TestMethod]
        public void GivenControllerWithSupportForVersionZero_WhenCheckingIfItHasSupportForVersionOne_ThenFalseIsReturned()
        {
            var descriptor = CreateControllerDescriptor(typeof(TestApiController));

            var hasSupport = descriptor.HasSupportForVersion("v1");

            Assert.IsFalse(hasSupport);
        }

        private HttpControllerDescriptor CreateControllerDescriptor(Type controllerType)
        {
            if (controllerType == null) throw new ArgumentNullException(nameof(controllerType));

            return new HttpControllerDescriptor(new HttpConfiguration(new HttpRouteCollection()), controllerType.Name, controllerType);
        }
    }
}