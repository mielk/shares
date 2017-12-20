using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Stock.DAL.TransferObjects;
using System.Linq;
using Stock.Domain.Services;
using Stock.Utils;
using Stock.Domain.Enums;
using Moq;

namespace Stock_UnitTest.Stock.Domain.Enums
{
    [TestClass]
    public class HelperMethodsUnitTests
    {

        #region TO_TIMEFRAME_UNIT

        [TestMethod]
        public void ToTimeframeUnit_ReturnTimeframeUnitMinutes_ForMinutes()
        {

            //Act
            string value = "MINUTES";
            TimeframeUnit result = value.ToTimeframeUnit();

            //Assert
            TimeframeUnit expected = TimeframeUnit.Minutes;
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void ToTimeframeUnit_ReturnTimeframeUnitHours_ForHours()
        {

            //Act
            string value = "HOURS";
            TimeframeUnit result = value.ToTimeframeUnit();

            //Assert
            TimeframeUnit expected = TimeframeUnit.Hours;
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void ToTimeframeUnit_ReturnTimeframeUnitDays_ForDays()
        {

            //Act
            string value = "Days";
            TimeframeUnit result = value.ToTimeframeUnit();

            //Assert
            TimeframeUnit expected = TimeframeUnit.Days;
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void ToTimeframeUnit_ReturnTimeframeUnitWeeks_ForWeeks()
        {

            //Act
            string value = "weeks";
            TimeframeUnit result = value.ToTimeframeUnit();

            //Assert
            TimeframeUnit expected = TimeframeUnit.Weeks;
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void ToTimeframeUnit_ReturnTimeframeUnitMonths_ForMonths()
        {

            //Act
            string value = "months";
            TimeframeUnit result = value.ToTimeframeUnit();

            //Assert
            TimeframeUnit expected = TimeframeUnit.Months;
            Assert.AreEqual(expected, result);

        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Unknown timeframe unit: unknown")]
        public void ToTimeframeUnit_ThrowsException_ForUnknownTimeframeName()
        {

            //Act
            string value = "unknown";
            TimeframeUnit result = value.ToTimeframeUnit();

        }

        #endregion TO_TIMEFRAME_UNIT


    }

}
