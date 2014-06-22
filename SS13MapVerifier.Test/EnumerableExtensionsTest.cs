using System;
using System.Collections.Generic;

using Common;
using Common.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SS13MapVerifier.Test
{
    [TestClass]
    public class EnumerableExtensionsTest
    {
        [TestMethod]
        public void SameCollectionShallReturnZero()
        {
            var collection = new List<string> { "a", "b", "c" };
            Assert.AreEqual(0, EnumerableExtensions.CompareTo(collection, collection), 0);
        }

        [TestMethod]
        public void DifferentButEqualsCollectionShallReturnZero()
        {
            var collectionOne = new List<string> { "a", "b", "c" };
            var collectionTwo = new List<string> { "a", "b", "c" };
            Assert.AreEqual(0, EnumerableExtensions.CompareTo(collectionOne, collectionTwo));
        }

        [TestMethod]
        public void ShallReturnMinusOneIfSameLengthButLesserElements()
        {
            var collectionOne = new List<int> { 1 };
            var collectionTwo = new List<int> { 2};
            Assert.AreEqual(-1, EnumerableExtensions.CompareTo(collectionOne, collectionTwo));
        }

        [TestMethod]
        public void ShallReturnOneIfSameLengthButGreaterElements()
        {
            var collectionOne = new List<int> { 2 };
            var collectionTwo = new List<int> { 1 };
            Assert.AreEqual(1, EnumerableExtensions.CompareTo(collectionOne, collectionTwo));
        }

        [TestMethod]
        public void ShallReturnOneIfContainingMoreElementsButOtherwiseEqual()
        {
            var collectionOne = new List<int> { 1, 2 };
            var collectionTwo = new List<int> { 1 };
            Assert.AreEqual(1, EnumerableExtensions.CompareTo(collectionOne, collectionTwo));
        }

        [TestMethod]
        public void ShallReturnMinusOneIfContainingLessElementsButOtherwiseEqual()
        {
            var collectionOne = new List<int> { 1 };
            var collectionTwo = new List<int> { 1, 2 };
            Assert.AreEqual(-1, EnumerableExtensions.CompareTo(collectionOne, collectionTwo));
        }
    }
}
