using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Utils.HelperMethods
{
    [TestClass]
    public class StringHelperMethodsUnitTest
    {

        [TestMethod]
        public void CompareStrings_ReturnsTrue_IfBothValuesAreNull()
        {
            string baseStr = null;
            string comparedStr = null;
            Assert.IsTrue(baseStr.Compare(comparedStr));
        }

        [TestMethod]
        public void CompareStrings_ReturnsFalse_IfOnlyBaseValueIsNull()
        {
            string baseStr = null;
            string comparedStr = "a";
            Assert.IsFalse(baseStr.Compare(comparedStr));
        }

        [TestMethod]
        public void CompareStrings_ReturnsFalse_IfOnlyComparedValueIsNull()
        {
            string baseStr = "a";
            string comparedStr = null;
            Assert.IsFalse(baseStr.Compare(comparedStr));
        }

        [TestMethod]
        public void CompareStrings_ReturnsFalse_IfValuesAreNotNullButDifferent()
        {
            string baseStr = "a";
            string comparedStr = "b";
            Assert.IsFalse(baseStr.Compare(comparedStr));
        }

        [TestMethod]
        public void CompareStrings_ReturnsFalse_IfValuesAreEqual()
        {
            string baseStr = "a";
            string comparedStr = "a";
            Assert.IsTrue(baseStr.Compare(comparedStr));
        }

    }
}
