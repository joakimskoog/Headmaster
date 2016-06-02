using System;
using System.Diagnostics.CodeAnalysis;
using Headmaster.Core.Tests.Outer.v1;
using Headmaster.Core.Tests.v0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Headmaster.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TypeExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatTypeIsNull_WhenGetControllerSupportedVersionIsCalled_ThenArgumentNullExceptionIsThrown()
        {
            Type t = null;

            var supportedVersion = t.GetControllerSupportedVersion();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenThatTypeIsNotApiController_WhenGetControllerSupportedVersionIsCalled_ThenArgumentExceptionIsThrown()
        {
            Type t = typeof(string);

            var supportedVersion = t.GetControllerSupportedVersion();
        }

        [TestMethod]
        public void GivenThatControllerHasNamespacev0_WhenTryingToGetSupportedVersion_ThenReturnedVersionIsv0()
        {
            var type = typeof(TestApiController);

            var suppportedVersion = type.GetControllerSupportedVersion();

            Assert.AreEqual("v0", suppportedVersion);
        }

        [TestMethod]
        public void GivenControllerInsideNestedNamespaces_WhenTryingToGetSupportedVersion_ThenInnermostNamespaceIsReturned()
        {
            var type = typeof(InnerTestApiController);

            var supportedVersion = type.GetControllerSupportedVersion();

            Assert.AreEqual("v1", supportedVersion);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv0_WhenCheckingIfItHasSupportForVersionv1_ThenFalseShouldBeReturned()
        {
            var type = typeof(TestApiController);

            var isSupported = type.HasControllerSupportForVersion("v1");

            Assert.IsFalse(isSupported);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv1_WhenCheckingIfItHasSupportForVersionv0_ThenFalseShouldBeReturned()
        {
            var type = typeof(InnerTestApiController);

            var isSupported = type.HasControllerSupportForVersion("v0");

            Assert.IsFalse(isSupported);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv0_WhenCheckingIfItHasSupportForVersionv0_ThenTrueShouldBeReturend()
        {
            var type = typeof(TestApiController);

            var isSupported = type.HasControllerSupportForVersion("v0");

            Assert.IsTrue(isSupported);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv1_WhenCheckingIfItHasSupportForVersionv1_ThenTrueShouldBeReturned()
        {
            var type = typeof(InnerTestApiController);

            var isSupported = type.HasControllerSupportForVersion("v1");

            Assert.IsTrue(isSupported);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatTypeIsNull_WhenTryingToCheckIfTypeHascontrollerSupportForVersion_ThenArgumentNullExceptionIsThrown()
        {
            Type t = null;

            var isSupported = t.HasControllerSupportForVersion("v1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatVersionIsNull_WhenTryingToCheckIfTypeHasControllerSupportForVersion_ThenArgumentNullExceptionIsThrown()
        {
            Type t = typeof(TestApiController);

            var isSupported = t.HasControllerSupportForVersion(null);
        }
    }
}