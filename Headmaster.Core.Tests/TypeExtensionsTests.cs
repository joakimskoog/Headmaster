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
        public void GivenThatTypeIsNull_WhenGetInnermostNamespaceNameIsCalled_ThenArgumentNullExceptionIsThrown()
        {
            Type t = null;

            var supportedVersion = t.GetInnermostNamespaceName();
        }

        [TestMethod]
        public void GivenThatControllerHasNamespacev0_WhenTryingToGetSupportedVersion_ThenReturnedVersionIsv0()
        {
            var type = typeof(TestApiController);

            var suppportedVersion = type.GetInnermostNamespaceName();

            Assert.AreEqual("v0", suppportedVersion);
        }

        [TestMethod]
        public void GivenControllerInsideNestedNamespaces_WhenTryingToGetSupportedVersion_ThenInnermostNamespaceIsReturned()
        {
            var type = typeof(InnerTestApiController);

            var supportedVersion = type.GetInnermostNamespaceName();

            Assert.AreEqual("v1", supportedVersion);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv0_WhenCheckingIfItHasSupportForVersionv1_ThenFalseShouldBeReturned()
        {
            var type = typeof(TestApiController);

            var isSupported = type.IsInnermostNamespaceName("v1");

            Assert.IsFalse(isSupported);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv1_WhenCheckingIfItHasSupportForVersionv0_ThenFalseShouldBeReturned()
        {
            var type = typeof(InnerTestApiController);

            var isSupported = type.IsInnermostNamespaceName("v0");

            Assert.IsFalse(isSupported);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv0_WhenCheckingIfItHasSupportForVersionv0_ThenTrueShouldBeReturend()
        {
            var type = typeof(TestApiController);

            var isSupported = type.IsInnermostNamespaceName("v0");

            Assert.IsTrue(isSupported);
        }

        [TestMethod]
        public void GivenControllerWithSupportedVersionv1_WhenCheckingIfItHasSupportForVersionv1_ThenTrueShouldBeReturned()
        {
            var type = typeof(InnerTestApiController);

            var isSupported = type.IsInnermostNamespaceName("v1");

            Assert.IsTrue(isSupported);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatTypeIsNull_WhenTryingToCheckIfTypeHascontrollerSupportForVersion_ThenArgumentNullExceptionIsThrown()
        {
            Type t = null;

            var isSupported = t.IsInnermostNamespaceName("v1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatVersionIsNull_WhenTryingToCheckIfTypeHasControllerSupportForVersion_ThenArgumentNullExceptionIsThrown()
        {
            Type t = typeof(TestApiController);

            var isSupported = t.IsInnermostNamespaceName(null);
        }
    }
}