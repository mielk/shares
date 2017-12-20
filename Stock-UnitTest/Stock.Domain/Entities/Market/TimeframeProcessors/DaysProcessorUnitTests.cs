using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities.MarketObjects.TimeframeProcessors;
using Stock.Domain.Entities;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Entities.MarketObjects.TimeframeProcessors
{
    [TestClass]
    public class DaysProcessorUnitTests
    {


        #region GET_PROPER_DATETIME

        [TestMethod]
        public void GetProperDateTime_ReturnsTheSameValue_IfProperValueIsPassed()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 0, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = baseDate;
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnsFriday_ForWeekendDatetime()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 8, 13, 0, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2016, 8, 12, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValue()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2016, 1, 1, 0, 0, 0));
            DateTime baseDate = new DateTime(2016, 1, 1, 0, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 12, 31, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValueIfTwoPreviousDaysAreAlsoHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2016, 1, 1, 0, 0, 0));
            processor.AddHoliday(new DateTime(2015, 12, 31, 0, 0, 0));
            processor.AddHoliday(new DateTime(2015, 12, 30, 0, 0, 0));
            DateTime baseDate = new DateTime(2016, 1, 1, 0, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2015, 12, 29, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForHolidayValueIfPreviousDayIsWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 1, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 1, 0, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 4, 28, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsLastValidValueBefore_ForWeekendIfFridayWasHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2017, 4, 7, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 4, 8, 0, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 4, 6, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }


        [TestMethod]
        public void GetProperDateTime_ReturnsDateTimeRoundedDown_ForTimeBetweenFullPeriods()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 5, 8, 16, 14, 27);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        [TestMethod]
        public void GetProperDateTime_ReturnsProperDateTime_ForTimeOnEdgeOfFullPeriod()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 5, 8, 0, 0, 0);

            //Act
            DateTime result = processor.GetProperDateTime(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 8, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, result);

        }

        #endregion GET_PROPER_DATETIME



        #region GET_NEXT

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForTimestampBetweenFullPeriods()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 15, 13, 21);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 5, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForTimestampOnPeriodEdge()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 5, 3, 0, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 4, 28, 0, 0, 0);

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
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 4, 30, 16, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 1, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 3));
            DateTime baseDate = new DateTime(2017, 5, 2, 0, 0, 0);

            //Act
            DateTime actualDateTime = processor.GetNext(baseDate, 1);

            //Assert
            DateTime expectedDateTime = new DateTime(2017, 5, 4, 0, 0, 0);
            Assert.AreEqual(expectedDateTime, actualDateTime);

        }

        [TestMethod]
        public void GetNext_ReturnsProperValue_ForLastQuotationBeforeHolidayInFriday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2017, 5, 4, 0, 0, 0);

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
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2017, 5, 8, 0, 0, 0));
            DateTime baseDate = new DateTime(2017, 5, 5, 0, 0, 0);

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
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 5, 4, 0, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 4, 0, 0, 0);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsZero_IfDateInTheSamePeriodIsGiven()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 8, 11, 14, 0, 0);
            DateTime comparedDate = new DateTime(2016, 8, 11, 14, 21, 53);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 0;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfThereIsNoTimeOffBetweenComparedDates()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 19);
            DateTime comparedDate = new DateTime(2016, 4, 21);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 2;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfThereIsWeekendBetweenComparedDates()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 19);
            DateTime comparedDate = new DateTime(2016, 4, 28);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 7;
            Assert.AreEqual(expected, unitsBetween);

        }
        
        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_IfThereAreFewWeekendsBetweenComparedDates()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 19);
            DateTime comparedDate = new DateTime(2016, 6, 8);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 36;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 15);
            DateTime comparedDate = new DateTime(2016, 4, 28);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 9;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateTwoWeeksLaterWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 12);
            DateTime comparedDate = new DateTime(2016, 4, 28);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 12;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateLaterInTheSameWeekButAfterHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();            
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2014, 12, 30);
            DateTime comparedDate = new DateTime(2015, 1, 2);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 2;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateAfterHolidayAndAfterWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2014, 12, 30);
            DateTime comparedDate = new DateTime(2015, 1, 6);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 4;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateAfterHolidayInWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2016, 12, 29);
            DateTime comparedDate = new DateTime(2017, 1, 4);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 4;
            Assert.AreEqual(expected, unitsBetween);

        }

        private DaysProcessor getProcessorForAfterManyHolidaysInDifferentYears()
        {
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2010, 1, 1));
            processor.AddHoliday(new DateTime(2010, 12, 25));
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
        public void CountTimeUnits_ReturnsProperValue_AfterManyHolidaysInDifferentYears()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2010, 5, 21);
            DateTime comparedDate = new DateTime(2015, 5, 20);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = 1297;
            Assert.AreEqual(expected, unitsBetween);

        }



        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateFewDaysEarlierInTheSameWeek()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 21);
            DateTime comparedDate = new DateTime(2016, 4, 19);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -2;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateFewDaysEarlierBeforeWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 28);
            DateTime comparedDate = new DateTime(2016, 4, 19);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -7;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekLaterThanBaseDate()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 28);
            DateTime comparedDate = new DateTime(2016, 4, 15);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -9;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateTwoWeeksEarlierWithDayOfWeekEarlierThanBaseDate()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 4, 28);
            DateTime comparedDate = new DateTime(2016, 4, 12);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -12;
            Assert.AreEqual(expected, unitsBetween);

        }
        
        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateFewWeekendsEarlier()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 6, 8);
            DateTime comparedDate = new DateTime(2016, 4, 19);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -36;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateEarlierInTheSameWeekBeforeHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 2);
            DateTime comparedDate = new DateTime(2014, 12, 30);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -2;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateBeforeWeekendAndBeforeHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2015, 1, 6);
            DateTime comparedDate = new DateTime(2014, 12, 30);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -4;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateBeforeHolidayInWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 4);
            DateTime comparedDate = new DateTime(2016, 12, 29);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -4;
            Assert.AreEqual(expected, unitsBetween);

        }

        [TestMethod]
        public void CountTimeUnits_ReturnsProperValue_ForDateFewHolidaysEarlier()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2015, 5, 20);
            DateTime comparedDate = new DateTime(2010, 5, 21);

            //Act
            int unitsBetween = processor.CountTimeUnits(baseDate, comparedDate, 1);

            //Assert
            int expected = -1297;
            Assert.AreEqual(expected, unitsBetween);

        }

        #endregion COUNT_TIME_UNITS



        #region ADD_TIME_UNITS


        [TestMethod]
        public void AddTimeUnits_ReturnsTheSameDate_IfUnitsZero()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 4, 21);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 0);

            //Assert
            DateTime expectedDate = new DateTime(2017, 4, 21);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_PositiveUnitsWithoutDayOff()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2017, 4, 18);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 3);

            //Assert
            DateTime expectedDate = new DateTime(2017, 4, 21);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_PositiveUnitsWithWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 1, 21);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 157);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 29);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_PositiveUnitsWithHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2015, 1, 1));
            DateTime baseDate = new DateTime(2014, 12, 31);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 1);

            //Assert
            DateTime expectedDate = new DateTime(2015, 1, 2);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_PositiveUnitsWithMoreHolidays()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2012, 1, 25);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 1451);

            //Assert
            DateTime expectedDate = new DateTime(2017, 8, 29);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_WhenAddOneDayAndBaseDayIsFridayBeforeHolidayMonday()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2018, 1, 1));
            DateTime baseDate = new DateTime(2017, 12, 29);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 1);

            //Assert
            DateTime expectedDate = new DateTime(2018, 1, 2);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_WhenAddOneDayAndBaseDayIsThursdayBeforeHolidayFriday()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2016, 1, 1));
            DateTime baseDate = new DateTime(2015, 12, 31);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 1);

            //Assert
            DateTime expectedDate = new DateTime(2016, 1, 4);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_PositiveUnitsWithHolidayAtWeekend()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2016, 12, 28);
            
            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, 4);

            //Assert
            DateTime expectedDate = new DateTime(2017, 1, 3);
            Assert.AreEqual(expectedDate, result);

        }


        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_NegativeUnitsWithoutDayOff()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17);
            
            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -2);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 15);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_NegativeUnitsWithWeekend()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            DateTime baseDate = new DateTime(2016, 8, 17);
            
            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -9);

            //Assert
            DateTime expectedDate = new DateTime(2016, 8, 4);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_NegativeUnitsWithHoliday()
        {

            //Arrange
            DaysProcessor processor = new DaysProcessor();
            processor.AddHoliday(new DateTime(2014, 1, 1));
            DateTime baseDate = new DateTime(2014, 1, 3);
            
            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -2);

            //Assert
            DateTime expectedDate = new DateTime(2013, 12, 31);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_NegativeUnitsWithMoreHolidays()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            DateTime baseDate = new DateTime(2017, 8, 29);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -1451);

            //Assert
            DateTime expectedDate = new DateTime(2012, 1, 25);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_WhenSubtractOneDayAndBaseDayIsMondayAfterHolidayFriday()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2016, 1, 1));
            DateTime baseDate = new DateTime(2016, 1, 4);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -1);

            //Assert
            DateTime expectedDate = new DateTime(2015, 12, 31);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_WhenSubtractOneDayAndBaseDayIsTuesdayAfterHolidayMonday()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 5, 1));
            DateTime baseDate = new DateTime(2017, 5, 2);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -1);

            //Assert
            DateTime expectedDate = new DateTime(2017, 4, 28);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_NegativeUnitsWithHolidayAtWeekend()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 1, 1));
            DateTime baseDate = new DateTime(2017, 1, 3);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -2);

            //Assert
            DateTime expectedDate = new DateTime(2016, 12, 30);
            Assert.AreEqual(expectedDate, result);

        }

        [TestMethod]
        public void AddTimeUnits_ReturnsProperDate_NegativeUnitsWithHolidayAndWeekend()
        {

            //Arrange
            DaysProcessor processor = getProcessorForAfterManyHolidaysInDifferentYears();
            processor.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2017, 5, 9);

            //Act
            DateTime result = processor.AddTimeUnits(baseDate, 1, -2);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 4);
            Assert.AreEqual(expectedDate, result);

        }


        #endregion ADD_TIME_UNITS


    }
}



