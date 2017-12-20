using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities.MarketObjects.TimeframeProcessors;
using Stock.Domain.Entities;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Entities.MarketObjects.TimeframeProcessors
{
    [TestClass]
    public class MinutesProcessorUnitTests
    {


        #region GET_PROPER_DATETIME

        [TestMethod]
        public void GetProperDateTime_M5_ReturnsTheSameValue_IfProperValueIsPassed()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = baseDate;
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetProperDateTime_M15_ReturnsTheSameValue_IfProperValueIsPassed()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 15);

            //Assert
            DateTime expectedDateTime = baseDate;
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetProperDateTime_M30_ReturnsTheSameValue_IfProperValueIsPassed()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 30);

            //Assert
            DateTime expectedDateTime = baseDate;
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void GetProperDateTime_M5_ReturnLastValueInWeek_ForWeekendDatetime()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 13, 16, 15, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 23, 55, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetProperDateTime_M15_ReturnLastValueInWeek_ForWeekendDatetime()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 13, 16, 15, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 15);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 23, 45, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetProperDateTime_M30_ReturnLastValueInWeek_ForWeekendDatetime()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 13, 16, 15, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 30);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 23, 30, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValue()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2016, 1, 1, 0, 0, 0));
            DateTime baseDate = new DateTime(2016, 1, 1, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 12, 31, 21, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValueIfTwoPreviousDaysAreAlsoHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2016, 1, 1, 0, 0, 0));
            processor.AddHoliday(new DateTime(2015, 12, 31, 0, 0, 0));
            processor.AddHoliday(new DateTime(2015, 12, 30, 0, 0, 0));
            DateTime baseDate = new DateTime(2016, 1, 1, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 12, 29, 21, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValueIfPreviousDayIsWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 1, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 1, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 4, 28, 23, 55, 0);
            Assert.AreEqual(expectedDateTime, result);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForDayBeforeHoliday230000()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 4, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 3, 23, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 3, 21, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForWeekendIfFridayWasHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 4, 7, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 4, 8, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 4, 6, 21, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsDateTimeRoundedDown_ForTimeBetweenFullPeriods()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 8, 16, 14, 27);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 16, 10, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsProperDateTime_ForTimeOnEdgeOfFullPeriod()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 8, 16, 15, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 16, 15, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        #endregion GET_PROPER_DATETIME



        #region GET_NEXT

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForTimestampBetweenFullPeriods()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 13, 21);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 15);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 15, 15, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForTimestampOnPeriodEdge()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17, 15, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 30);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 17, 15, 30, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 4, 28, 23, 30, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 30);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 1, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForWeekendValue()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 20, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 30);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 22, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 3));
            DateTime baseDate = new DateTime(2017, 5, 2, 21, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 30);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeHolidayInFriday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2017, 5, 4, 21, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 30);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastWeekQuotationIfMondayIsHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 8, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 5, 23, 55, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 5);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 9, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        #endregion GET_NEXT



        #region COUNT_TIME_UNITS


        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfTheSameDateIsGiven()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 15, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }


        [TestMethod]
        public void CountTimeUnits_M5_ReturnsZero_IfDateInTheSamePeriodIsGiven()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 15, 4, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsZero_IfDateInTheSamePeriodIsGiven()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 15, 14, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsZero_IfDateInTheSamePeriodIsGiven()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 15, 27, 12);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }


        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_IfThereIsNoTimeOffBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 12, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 624;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_IfThereIsNoTimeOffBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 12, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 208;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_IfThereIsNoTimeOffBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 12, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 104;
            Assert.AreEqual(expected, unitsBetween);

        }


        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_IfThereIsWeekendBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 16, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 672;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_IfThereIsWeekendBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 16, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 224;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_IfThereIsWeekendBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 16, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 112;
            Assert.AreEqual(expected, unitsBetween);

        }


        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_IfThereAreFewWeekendsBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 22, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 2256;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_IfThereAreFewWeekendsBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 22, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 752;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_IfThereAreFewWeekendsBetweenComparedDates()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 22, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 376;
            Assert.AreEqual(expected, unitsBetween);

        }


        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 23, 8, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 2544;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 23, 8, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 848;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 23, 8, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 424;
            Assert.AreEqual(expected, unitsBetween);

        }


        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 25, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 3216;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 25, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 1072;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 25, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 536;
            Assert.AreEqual(expected, unitsBetween);

        }

        
        
        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateLaterInTheSameWeekButAfterHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 3, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 781;
            Assert.AreEqual(expected, unitsBetween);

        }
        
        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateLaterInTheSameWeekButAfterHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 3, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 261;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateLaterInTheSameWeekButAfterHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 3, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 131;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateAfterHolidayAndAfterWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 6, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 1069;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateAfterHolidayAndAfterWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 6, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 357;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateAfterHolidayAndAfterWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 6, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 179;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateAfterHolidayInWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2012, 1, 1));
            DateTime baseDate = new DateTime(2011, 12, 28, 12, 0, 0);
            DateTime comparedDate = new DateTime(2012, 1, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 1200;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateAfterHolidayInWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2012, 1, 1));
            DateTime baseDate = new DateTime(2011, 12, 28, 12, 0, 0);
            DateTime comparedDate = new DateTime(2012, 1, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 400;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateAfterHolidayInWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2012, 1, 1));
            DateTime baseDate = new DateTime(2011, 12, 28, 12, 0, 0);
            DateTime comparedDate = new DateTime(2012, 1, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 200;
            Assert.AreEqual(expected, unitsBetween);

        }



        private MinutesProcessor getProcessorForAfterManyHolidaysInDifferentYears()
        {
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2011, 1, 1));
            processor.AddHoliday(new DateTime(2011, 12, 25));
            processor.AddHoliday(new DateTime(2012, 1, 1));
            processor.AddHoliday(new DateTime(2012, 12, 25));
            processor.AddHoliday(new DateTime(2013, 1, 1));
            processor.AddHoliday(new DateTime(2013, 12, 25));
            processor.AddHoliday(new DateTime(2014, 1, 1));
            processor.AddHoliday(new DateTime(2014, 12, 25));
            processor.AddHoliday(new DateTime(2015, 1, 1));
            processor.AddHoliday(new DateTime(2015, 12, 25));
            processor.AddHoliday(new DateTime(2016, 1, 1));
            processor.AddHoliday(new DateTime(2016, 12, 25));
            processor.AddHoliday(new DateTime(2017, 1, 1));
            processor.AddHoliday(new DateTime(2017, 12, 25));
            return processor;
        }

        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_AfterManyHolidaysInDifferentYears()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2013, 10, 15, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 11, 11, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 229374;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_AfterManyHolidaysInDifferentYears()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2013, 10, 15, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 11, 11, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = 76462;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_AfterManyHolidaysInDifferentYears()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2013, 10, 15, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 11, 11, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = 38234;
            Assert.AreEqual(expected, unitsBetween);

        }




        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateFewDaysEarlierInTheSameWeek()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 10, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -528;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateFewDaysEarlierInTheSameWeek()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 10, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -176;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateFewDaysEarlierInTheSameWeek()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 10, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -88;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateFewDaysEarlierBeforeWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -1152;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateFewDaysEarlierBeforeWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -384;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateFewDaysEarlierBeforeWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -192;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 16, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -2544;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 16, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -848;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 16, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -424;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 19, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -3408;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 19, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -1136;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 19, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -568;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateFewWeekendsEarlier()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 7, 21, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -4272;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateFewWeekendsEarlier()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 7, 21, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -1424;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateFewWeekendsEarlier()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 7, 21, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -712;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateEarlierInTheSameWeekBeforeHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 2, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 12, 31, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -301;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateEarlierInTheSameWeekBeforeHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 2, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 12, 31, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -101;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateEarlierInTheSameWeekBeforeHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 2, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 12, 31, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -51;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateBeforeWeekendAndBeforeHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2014, 1, 2, 12, 0, 0);
            DateTime comparedDate = new DateTime(2013, 12, 27, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -781;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateBeforeWeekendAndBeforeHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2014, 1, 2, 12, 0, 0);
            DateTime comparedDate = new DateTime(2013, 12, 27, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -261;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateBeforeWeekendAndBeforeHoliday()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2014, 1, 2, 12, 0, 0);
            DateTime comparedDate = new DateTime(2013, 12, 27, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -131;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateBeforeHolidayInWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 3, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 12, 29, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -864;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateBeforeHolidayInWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 3, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 12, 29, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -288;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateBeforeHolidayInWeekend()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 3, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 12, 29, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -144;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_M5_ReturnsProperValue_ForDateFewHolidaysEarlier()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 26, 16, 0, 0);
            DateTime comparedDate = new DateTime(2012, 10, 23, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = -311048;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M15_ReturnsProperValue_ForDateFewHolidaysEarlier()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 26, 16, 0, 0);
            DateTime comparedDate = new DateTime(2012, 10, 23, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 15);

            //Assert
            int expected = -103688;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_M30_ReturnsProperValue_ForDateFewHolidaysEarlier()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 26, 16, 0, 0);
            DateTime comparedDate = new DateTime(2012, 10, 23, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 30);

            //Assert
            int expected = -51848;
            Assert.AreEqual(expected, unitsBetween);

        }


        #endregion COUNT_TIME_UNITS



        #region ADD_TIME_UNITS


        [TestMethod]
        public void AddTimeUnits_M5_ReturnsTheSameDate_IfUnitsIsZero()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 5, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 15, 5, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsTheSameDate_IfUnitsIsZero()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 21, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsTheSameDate_IfUnitsIsZero()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 21, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsNormalizedDate_IfUnitsIsZeroAndDateBetweenPeriods()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 8, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 15, 5, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsNormalizedDate_IfUnitsIsZeroAndDateBetweenPeriods()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21, 12, 11, 34);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 21, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsNormalizedDate_IfUnitsIsZeroAndDateBetweenPeriods()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21, 12, 51, 34);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, 0);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 4, 21, 12, 30, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForPositiveNumberOfItemsInTheMiddleOfWeek()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 9, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, 912);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForPositiveNumberOfItemsInTheMiddleOfWeek()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 9, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, 304);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForPositiveNumberOfItemsInTheMiddleOfWeek()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 9, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, 152);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForPositiveNumberOfItemsWithWeekBreak()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 2, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, 2352);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForPositiveNumberOfItemsWithWeekBreak()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 2, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, 784);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForPositiveNumberOfItemsWithWeekBreak()
        {

            //Arrange
            MinutesProcessor processor = new MinutesProcessor();
            DateTime baseDate = new DateTime(2016, 8, 2, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, 392);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForPositiveNumberOfItemsWithHolidayAtWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2014, 12, 29, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, 1789);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 1, 7, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForPositiveNumberOfItemsWithHolidayAtWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2014, 12, 29, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, 597);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 1, 7, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForPositiveNumberOfItemsWithHolidayAtWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2014, 12, 29, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, 299);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 1, 7, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForPositiveNumberOfItemsWithHolidayAtWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 28, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, 1824);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 1, 5, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForPositiveNumberOfItemsWithHolidayAtWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 28, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, 608);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 1, 5, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForPositiveNumberOfItemsWithHolidayAtWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 28, 8, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, 304);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 1, 5, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForPositiveNumberOfItemsWithTwoHolidays()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 12, 22, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, 2186);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 1, 5, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForPositiveNumberOfItemsWithTwoHolidays()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 12, 22, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, 730);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 1, 5, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForPositiveNumberOfItemsWithTwoHolidays()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 12, 22, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, 366);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 1, 5, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }




        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperDate_PositiveAfterFridayHolidayAndWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 12, 24, 20, 0, 0);
            
            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 5, 61);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 28, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperDate_PositiveAfterFridayHolidayAndWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 12, 24, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 15, 21);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 28, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperDate_PositiveAfterFridayHolidayAndWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 12, 24, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 30, 11);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 28, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperDate_PositiveAfterWeekendAndMondayHoliday()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 22, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 5, 48);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 26, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperDate_PositiveAfterWeekendAndMondayHoliday()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 22, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 15, 16);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 26, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperDate_PositiveAfterWeekendAndMondayHoliday()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 22, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 30, 8);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 26, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }




        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForNegativeNumberOfItemsInTheMiddleOfWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            
            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, -912);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 9, 8, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForNegativeNumberOfItemsInTheMiddleOfWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, -304);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 9, 8, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForNegativeNumberOfItemsInTheMiddleOfWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, -152);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 9, 8, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForNegativeNumberOfItemsWithWeekBreak()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 7, 26, 4, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, -2016);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 7, 15, 4, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForNegativeNumberOfItemsWithWeekBreak()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 7, 26, 4, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, -672);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 7, 15, 4, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForNegativeNumberOfItemsWithWeekBreak()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 7, 26, 4, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, -336);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 7, 15, 4, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForNegativeNumberOfItemsWithHolidayAtWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 6, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, -1213);

            //Assert
            DateTime expectedDateTime = new DateTime(2014, 12, 30, 4, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForNegativeNumberOfItemsWithHolidayAtWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 6, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, -405);

            //Assert
            DateTime expectedDateTime = new DateTime(2014, 12, 30, 4, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForNegativeNumberOfItemsWithHolidayAtWeek()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 6, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, -203);

            //Assert
            DateTime expectedDateTime = new DateTime(2014, 12, 30, 4, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForNegativeNumberOfItemsWithHolidayAtWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 6, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, -2352);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 12, 27, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForNegativeNumberOfItemsWithHolidayAtWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 6, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, -784);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 12, 27, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForNegativeNumberOfItemsWithHolidayAtWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 6, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, -392);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 12, 27, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperValue_ForNegativeNumberOfItemsWithTwoHolidays()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 1, 7, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 5, -77044);

            //Assert
            DateTime expectedDateTime = new DateTime(2013, 12, 23, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperValue_ForNegativeNumberOfItemsWithTwoHolidays()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 1, 7, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 15, -25684);

            //Assert
            DateTime expectedDateTime = new DateTime(2013, 12, 23, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperValue_ForNegativeNumberOfItemsWithTwoHolidays()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 1, 7, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.AddTimeUnits(baseDate, 30, -12844);

            //Assert
            DateTime expectedDateTime = new DateTime(2013, 12, 23, 12, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }




        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperDate_NegativeBeforeWeekendAndFridayHoliday()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2015, 12, 28, 4, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 5, -61);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 24, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperDate_NegativeBeforeWeekendAndFridayHoliday()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2015, 12, 28, 4, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 15, -21);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 24, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperDate_NegativeBeforeWeekendAndFridayHoliday()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2015, 12, 28, 4, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate,30, -11);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 24, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_M5_ReturnsProperDate_NegativeBeforeMondayHolidayAndWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2017, 12, 26, 0, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 5, -48);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 22, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M15_ReturnsProperDate_NegativeBeforeMondayHolidayAndWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 26, 0, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 15, -16);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 22, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_M30_ReturnsProperDate_NegativeBeforeMondayHolidayAndWeekend()
        {

            //Arrange
            MinutesProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 26, 0, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 30, -8);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 22, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }


        #endregion ADD_TIME_UNITS



    }

}