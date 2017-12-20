using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Utils.HelperMethods
{
    [TestClass]
    public class NumericHelperMethodsUnitTest
    {

        [TestMethod]
        public void IsInRangeInt_ReturnsTrue_IfValueEqualToLowLimit()
        {
            int value = 1;
            Assert.IsTrue(value.IsInRange(1, 2));
        }

        [TestMethod]
        public void IsInRangeInt_ReturnsTrue_IfValueEqualToUpLimit()
        {
            int value = 2;
            Assert.IsTrue(value.IsInRange(1, 2));
        }

        [TestMethod]
        public void IsInRangeInt_ReturnsTrue_IfValueBetweenLimits()
        {
            int value = 2;
            Assert.IsTrue(value.IsInRange(1, 3));
        }

        [TestMethod]
        public void IsInRangeInt_ReturnsTrue_IfValueBetweenLimitsAndLimitsInverted()
        {
            int value = 2;
            Assert.IsTrue(value.IsInRange(3, 1));
        }

        [TestMethod]
        public void IsInRangeInt_ReturnsFalse_IfValueLowerThanLowLimit()
        {
            int value = 1;
            Assert.IsFalse(value.IsInRange(2, 3));
        }

        [TestMethod]
        public void IsInRangeInt_ReturnsFalse_IfValueLowerThanLowLimitWhenLimitsInverted()
        {
            int value = 1;
            Assert.IsFalse(value.IsInRange(3, 2));
        }

        [TestMethod]
        public void IsInRangeInt_ReturnsFalse_IfValueHigherThanUpLimit()
        {
            int value = 4;
            Assert.IsFalse(value.IsInRange(2, 3));
        }

        [TestMethod]
        public void IsInRangeInt_ReturnsFalse_IfValueHigherThanUpLimitWhenLimitsInverted()
        {
            int value = 4;
            Assert.IsFalse(value.IsInRange(3, 2));
        }



        [TestMethod]
        public void IsInRangeDouble_ReturnsTrue_IfValueEqualToLowLimit()
        {
            double value = 1.0d;
            Assert.IsTrue(value.IsInRange(1.0d, 2.0d));
        }

        [TestMethod]
        public void IsInRangeDouble_ReturnsTrue_IfValueEqualToUpLimit()
        {
            double value = 2.0d;
            Assert.IsTrue(value.IsInRange(1.0d, 2.0d));
        }

        [TestMethod]
        public void IsInRangeDouble_ReturnsTrue_IfValueBetweenLimits()
        {
            double value = 2.0d;
            Assert.IsTrue(value.IsInRange(1.0d, 3.0d));
        }

        [TestMethod]
        public void IsInRangeDouble_ReturnsTrue_IfValueBetweenLimitsAndLimitsInverted()
        {
            double value = 2.0d;
            Assert.IsTrue(value.IsInRange(3.0d, 1.0d));
        }

        [TestMethod]
        public void IsInRangeDouble_ReturnsFalse_IfValueLowerThanLowLimit()
        {
            double value = 1.0d;
            Assert.IsFalse(value.IsInRange(2.0d, 3.0d));
        }

        [TestMethod]
        public void IsInRangeDouble_ReturnsFalse_IfValueLowerThanLowLimitWhenLimitsInverted()
        {
            double value = 1.0d;
            Assert.IsFalse(value.IsInRange(3.0d, 2.0d));
        }

        [TestMethod]
        public void IsInRangeDouble_ReturnsFalse_IfValueHigherThanUpLimit()
        {
            double value = 4.0d;
            Assert.IsFalse(value.IsInRange(2.0d, 3.0d));
        }

        [TestMethod]
        public void IsInRangeDouble_ReturnsFalse_IfValueHigherThanUpLimitWhenLimitsInverted()
        {
            double value = 4.0d;
            Assert.IsFalse(value.IsInRange(3.0d, 2.0d));
        }


    }
}
