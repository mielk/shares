using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities.MarketObjects.TimeframeProcessors;
using Stock.Domain.Entities;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Entities.MarketObjects.TimeframeProcessors
{
    [TestClass]
    public class MonthsProcessorUnitTests
    {

        #region GET_PROPER_DATETIME

        [TestMethod]
        public void GetProperDateTime_ReturnsTheSameValue_ForFirstDayOfMonth()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 8, 1, 0, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = baseDate;
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnsFirstDayOfMonth_ForNotFirstDayOfMonth()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 8, 13, 0, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 1, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }


        #endregion GET_PROPER_DATETIME


        #region GET_NEXT

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForFirstDayOfMonth()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2017, 5, 1, 0, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 6, 1, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForOtherThanFirstDay()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2017, 5, 7, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 6, 1, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        #endregion GET_NEXT
        

        #region COUNT_TIME_UNITS

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfTheSameDateIsGiven()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 0, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 0, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfDateInTheSameMonthIsGiven()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 8, 9);
            DateTime comparedDate = new DateTime(2016, 8, 11);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfLaterDateIsGiven()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);
            DateTime comparedDate = new DateTime(2016, 11, 1);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 7;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfComparedDateIsNotFirstDayOfMonth()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);
            DateTime comparedDate = new DateTime(2016, 11, 16);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 7;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfBaseDateIsNotFirstDayOfMonth()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 15);
            DateTime comparedDate = new DateTime(2016, 11, 20);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 7;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfBothDatesInTheSameMonthButBaseDateIsEarlier()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 15);
            DateTime comparedDate = new DateTime(2016, 4, 30);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfBothDatesInTheSameWeekButBaseDateIsLater()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 15);
            DateTime comparedDate = new DateTime(2016, 4, 2);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfBaseDateIsEarlier()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);
            DateTime comparedDate = new DateTime(2016, 4, 21);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfComparedDateIsFromNextYear()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2013, 7, 15);
            DateTime comparedDate = new DateTime(2016, 4, 21);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 33;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfComparedDateIsFromPreviousYear()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 20);
            DateTime comparedDate = new DateTime(2012, 6, 21);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -46;
            Assert.AreEqual(expected, unitsBetween);

        }

        #endregion COUNT_TIME_UNITS


        #region ADD_TIME_UNITS

        [TestMethod]
        public void AddTimeUnits_ReturnsTheSameDate_ForZeroUnits()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 1);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsTheSameDate_ForUnitsOverZero()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 9, 1);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsTheSameDate_ForUnitsUnderZero()
        {

            //Arrange
            MonthsProcessor processor = new MonthsProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -5);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 11, 1);
            Assert.AreEqual(expectedDateTime, result);

        }

        #endregion ADD_TIME_UNITS


    }
}
