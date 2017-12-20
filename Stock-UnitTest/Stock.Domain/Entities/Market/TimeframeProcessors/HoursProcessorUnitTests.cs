using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities.MarketObjects.TimeframeProcessors;
using Stock.Domain.Entities;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Entities.MarketObjects.TimeframeProcessors
{
    [TestClass]
    public class HoursProcessorUnitTests
    {


        #region GET_PROPER_DATETIME

        //[TestMethod]
        public void GetProperDateTime_ReturnsTheSameValue_IfProperValueIsPassed()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 4);

            //Assert
            DateTime expectedDateTime = baseDate;
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnLastValueInWeek_ForWeekendDatetime()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 13, 16, 15, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 20, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValue()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2016, 1, 1, 0, 0, 0));
            DateTime baseDate = new DateTime(2016, 1, 1, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 12, 31, 20, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValueIfTwoPreviousDaysAreAlsoHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2016, 1, 1, 0, 0, 0));
            processor.AddHoliday(new DateTime(2015, 12, 31, 0, 0, 0));
            processor.AddHoliday(new DateTime(2015, 12, 30, 0, 0, 0));
            DateTime baseDate = new DateTime(2016, 1, 1, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 12, 29, 20, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValueIfPreviousDayIsWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 1, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 1, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 4, 28, 23, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForDayBeforeHoliday230000()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 4, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 3, 23, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 3, 20, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForWeekendIfFridayWasHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 4, 7, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 4, 8, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 4, 6, 20, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsDateTimeRoundedDown_ForTimeBetweenFullPeriods()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2017, 5, 8, 16, 14, 27);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 16, 00, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsProperDateTime_ForTimeOnEdgeOfFullPeriod()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2017, 5, 8, 16, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        #endregion GET_PROPER_DATETIME



        #region GET_NEXT

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForTimestampBetweenFullPeriods()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 13, 21);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 16, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForTimestampOnPeriodEdge()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 17, 20, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2017, 4, 28, 23, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 1, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForWeekendValue()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 20, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 22, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 3));
            DateTime baseDate = new DateTime(2017, 5, 2, 20, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 4);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeHolidayInFriday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2017, 5, 4, 21, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastWeekQuotationIfMondayIsHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 8, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 5, 23, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

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
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 15, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 5);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsZero_IfDateInTheSamePeriodIsGiven()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 14, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 14, 21, 53);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsZero_IfDateInTheSamePeriodIsGiven()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 14, 21, 53);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_IfThereIsNoTimeOffBetweenComparedDates()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 12, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 52;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_IfThereIsNoTimeOffBetweenComparedDates()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 12, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 13;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_IfThereIsWeekendBetweenComparedDates()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 16, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 56;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_IfThereIsWeekendBetweenComparedDates()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 16, 20, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 14;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_IfThereAreFewWeekendsBetweenComparedDates()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 22, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 188;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_IfThereAreFewWeekendsBetweenComparedDates()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 16, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 22, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 47;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 23, 8, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 212;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 23, 8, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 53;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 25, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 268;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 10, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 25, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 67;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateLaterInTheSameWeekButAfterHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 3, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 66;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateLaterInTheSameWeekButAfterHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2013, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 1, 3, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 17;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateAfterHolidayAndAfterWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2014, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2015, 1, 6, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 90;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateAfterHolidayAndAfterWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2014, 12, 30, 16, 0, 0);
            DateTime comparedDate = new DateTime(2015, 1, 6, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 23;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateAfterHolidayInWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2012, 1, 1));
            DateTime baseDate = new DateTime(2011, 12, 28, 12, 0, 0);
            DateTime comparedDate = new DateTime(2012, 1, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 100;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateAfterHolidayInWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2012, 1, 1));
            DateTime baseDate = new DateTime(2011, 12, 28, 12, 0, 0);
            DateTime comparedDate = new DateTime(2012, 1, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 25;
            Assert.AreEqual(expected, unitsBetween);

        }



        private HoursProcessor getProcessorForAfterManyHolidaysInDifferentYears()
        {
            HoursProcessor processor = new HoursProcessor();
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
        public void CountTimeUnits_H1_ReturnsProperValue_AfterManyHolidaysInDifferentYears()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2013, 10, 15, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 11, 11, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 19120;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_AfterManyHolidaysInDifferentYears()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2013, 10, 15, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 11, 11, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = 4783;
            Assert.AreEqual(expected, unitsBetween);

        }




        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateFewDaysEarlierInTheSameWeek()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 10, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -44;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateFewDaysEarlierInTheSameWeek()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 10, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -11;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateFewDaysEarlierBeforeWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -96;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateFewDaysEarlierBeforeWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -24;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 16, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -212;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 16, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -53;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 19, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -284;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 19, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 3, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -71;
            Assert.AreEqual(expected, unitsBetween);

        }


        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateFewWeekendsEarlier()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 7, 21, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -356;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateFewWeekendsEarlier()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 7, 21, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -89;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateEarlierInTheSameWeekBeforeHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 2, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 12, 31, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -26;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateEarlierInTheSameWeekBeforeHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 2, 16, 0, 0);
            DateTime comparedDate = new DateTime(2014, 12, 31, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -7;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateBeforeWeekendAndBeforeHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2014, 1, 2, 12, 0, 0);
            DateTime comparedDate = new DateTime(2013, 12, 27, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -66;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateBeforeWeekendAndBeforeHoliday()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2014, 1, 2, 12, 0, 0);
            DateTime comparedDate = new DateTime(2013, 12, 27, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -17;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateBeforeHolidayInWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 3, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 12, 29, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -72;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateBeforeHolidayInWeekend()
        {

            //Arrange
            HoursProcessor processor = new HoursProcessor();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 3, 12, 0, 0);
            DateTime comparedDate = new DateTime(2016, 12, 29, 12, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -18;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_H1_ReturnsProperValue_ForDateFewHolidaysEarlier()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 26, 16, 0, 0);
            DateTime comparedDate = new DateTime(2012, 10, 23, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -25928;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_H4_ReturnsProperValue_ForDateFewHolidaysEarlier()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 12, 26, 16, 0, 0);
            DateTime comparedDate = new DateTime(2012, 10, 23, 16, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 4);

            //Assert
            int expected = -6486;
            Assert.AreEqual(expected, unitsBetween);

        }



        #endregion COUNT_TIME_UNITS


        #region ADD_TIME_UNITS


        [TestMethod]
        public void AddTimeUnits_H1_ReturnsTheSameDate_IfUnitsZero()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2017, 4, 21, 15, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 0);

            //Assert
            DateTime expectedDate = new DateTime(2017, 4, 21, 15, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsTheSameDate_IfUnitsZero()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2017, 4, 21, 15, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 0);

            //Assert
            DateTime expectedDate = new DateTime(2017, 4, 21, 15, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_PositiveUnitsWithoutDayOff()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 9, 8, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 76);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_PositiveUnitsWithoutDayOff()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 9, 8, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 19);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_PositiveUnitsWithWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 2, 8, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 196);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_PositiveUnitsWithWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 2, 8, 0, 0);
            
            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 49);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 12, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }




        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_PositiveUnitsWithHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2014, 12, 29, 8, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 150);

            //Assert
            DateTime expectedDate = new DateTime(2015, 1, 7, 16, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_PositiveUnitsWithHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2014, 12, 19, 8, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 68);

            //Assert
            DateTime expectedDate = new DateTime(2015, 1, 7, 16, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_PositiveUnitsWithHolidayAtWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2016, 12, 28, 8, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 152);

            //Assert
            DateTime expectedDate = new DateTime(2017, 1, 5, 16, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_PositiveUnitsWithHolidayAtWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2016, 12, 28, 8, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 38);

            //Assert
            DateTime expectedDate = new DateTime(2017, 1, 5, 16, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_PositiveWithTwoHolidays()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2015, 12, 22, 16, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 184);

            //Assert
            DateTime expectedDate = new DateTime(2016, 1, 5, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_PositiveWithTwoHolidays()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2016, 1, 1));
            processor.AddHoliday(new DateTime(2015, 12, 25));
            DateTime baseDate = new DateTime(2015, 12, 22, 16, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 47);

            //Assert
            DateTime expectedDate = new DateTime(2016, 1, 5, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_PositiveAfterFridayHolidayAndWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2015, 12, 24, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 6);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 28, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_PositiveAfterFridayHolidayAndWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2015, 12, 24, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 2);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 28, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_PositiveAfterWeekendAndMondayHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 22, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 4);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 26, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_PositiveAfterWeekendAndMondayHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 22, 20, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, 1);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 26, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_NegativeUnitsWithoutDayOff()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -76);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 9, 8, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_NegativeUnitsWithoutDayOff()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 8, 12, 12, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, -19);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 9, 8, 0, 0);
            Assert.AreEqual(expectedDate, result);
        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_NegativeUnitsWithWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 7, 26, 4, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -168);

            //Assert
            DateTime expectedDate = new DateTime(2016, 7, 15, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_NegativeUnitsWithWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2016, 7, 26, 4, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, -42);

            //Assert
            DateTime expectedDate = new DateTime(2016, 7, 15, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_NegativeUnitsWithHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 6, 12, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -102);

            //Assert
            DateTime expectedDate = new DateTime(2014, 12, 30, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_NegativeUnitsWithHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 6, 12, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, -26);

            //Assert
            DateTime expectedDate = new DateTime(2014, 12, 30, 4, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_NegativeUnitsWithHolidayAtWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 6, 16, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -196);

            //Assert
            DateTime expectedDate = new DateTime(2016, 12, 27, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_NegativeUnitsWithHolidayAtWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 6, 16, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, -49);

            //Assert
            DateTime expectedDate = new DateTime(2016, 12, 27, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_NegativeWithMoreHolidays()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 1, 7, 12, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -6424);

            //Assert
            DateTime expectedDate = new DateTime(2013, 12, 23, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_NegativeWithMoreHolidays()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 1, 7, 12, 0, 0);
            
            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, -1608);

            //Assert
            DateTime expectedDate = new DateTime(2013, 12, 23, 12, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }




        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_NegativeBeforeMondayHolidayAndWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 12, 25));
            DateTime baseDate = new DateTime(2017, 12, 26, 0, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -4);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 22, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_NegativeBeforeMondayHolidayAndWeekend()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 26, 0, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, -1);

            //Assert
            DateTime expectedDate = new DateTime(2017, 12, 22, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }



        [TestMethod]
        public void AddTimeUnits_H1_ReturnsProperDate_NegativeBeforeWeekendAndFridayHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2015, 12, 28, 4, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -6);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 24, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }
        
        [TestMethod]
        public void AddTimeUnits_H4_ReturnsProperDate_NegativeBeforeWeekendAndFridayHoliday()
        {

            //Arrange
            HoursProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2015, 12, 25));
            DateTime baseDate = new DateTime(2015, 12, 28, 4, 0, 0);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 4, -2);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 24, 20, 0, 0);
            Assert.AreEqual(expectedDate, result);

        }


        #endregion ADD_TIME_UNITS




    }
}
