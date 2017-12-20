using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities.MarketObjects.TimeframeProcessors;
using Stock.Domain.Entities;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Entities.MarketObjects.TimeframeProcessors
{
    [TestClass]
    public class WeeksProcessorUnitTests
    {


        #region GET_PROPER_DATETIME
        
        [TestMethod]
        public void GetProperDateTime_ReturnsTheSameDateMidnight_ForSunday()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 17);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = baseDate;
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void GetProperDateTime_ReturnsPreviousSundayMidnight_ForNonSunday()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21, 15, 14, 52);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 17, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }
        
        #endregion GET_PROPER_DATETIME



        #region GET_NEXT

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForMiddleWeekTimestamp()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 13, 21);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 7, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForSunday()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2017, 5, 7, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 14, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForSaturday()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2017, 5, 6, 23, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 7, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        #endregion GET_NEXT



        #region COUNT_TIME_UNITS

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfTheSameDateIsGiven()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 0, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 0, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfDateInTheSameWeekIsGiven()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
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
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);
            DateTime comparedDate = new DateTime(2016, 8, 1);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 18;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfComparedDateIsNotMonday()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 8, 1);
            DateTime comparedDate = new DateTime(2016, 8, 11);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 1;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfBaseDateIsNotMonday()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 15);
            DateTime comparedDate = new DateTime(2016, 4, 20);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 1;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfBothDatesInTheSameWeekButBaseDateIsEarlier()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 19);
            DateTime comparedDate = new DateTime(2016, 4, 21);

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
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21);
            DateTime comparedDate = new DateTime(2016, 4, 19);

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
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 1);
            DateTime comparedDate = new DateTime(2016, 4, 21);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 3;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfComparedDateIsFromNextYear()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2015, 12, 5);
            DateTime comparedDate = new DateTime(2016, 4, 21);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 20;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfComparedDateIsFromPreviousYear()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21);
            DateTime comparedDate = new DateTime(2015, 12, 5);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -20;
            Assert.AreEqual(expected, unitsBetween);

        }

        #endregion COUNT_TIME_UNITS



        #region ADD_TIME_UNITS

        [TestMethod]
        public void AddTimeUnits_ReturnsTheSameDate_ForZeroUnits()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 17);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 17);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsTheSameDate_ForUnitsOverZero()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 17);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 5, 22);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsTheSameDate_ForUnitsUnderZero()
        {

            //Arrange
            WeeksProcessor processor = new WeeksProcessor();
            DateTime baseDate = new DateTime(2016, 4, 17);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -2);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 3);
            Assert.AreEqual(expectedDateTime, result);

        }

        #endregion ADD_TIME_UNITS

    }
}
