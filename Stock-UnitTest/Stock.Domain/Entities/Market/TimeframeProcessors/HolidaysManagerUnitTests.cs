using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities.MarketObjects.TimeframeProcessors;
using System.Collections.Generic;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Domain.Entities.MarketObjects.TimeframeProcessors
{
    [TestClass]
    public class HolidaysManagerUnitTests
    {

        #region LOADING_HOLIDAYS

        [TestMethod]
        public void IsHoliday_ReturnsTrue_IfDateIsAddedAsHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime holiday = new DateTime(2017, 5, 1);
            DateTime checkedDate = new DateTime(2017, 5, 1);
            
            //Act
            manager.AddHoliday(holiday);

            //Assert
            bool isHoliday = manager.IsHoliday(checkedDate);
            Assert.IsTrue(isHoliday);

        }

        [TestMethod]
        public void IsHoliday_ReturnsTrue_ForDateAddedToHolidaysInTheMiddleOfDay()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime holiday = new DateTime(2017, 5, 1);
            DateTime checkedDate = new DateTime(2017, 5, 1, 13, 14, 21);

            //Act
            manager.AddHoliday(holiday);

            //Assert
            bool isHoliday = manager.IsHoliday(checkedDate);
            Assert.IsTrue(isHoliday);

        }

        [TestMethod]
        public void IsHoliday_ReturnsFalse_ForDateNotAddedToHolidays()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime holiday = new DateTime(2017, 5, 1);
            DateTime checkedDate = new DateTime(2017, 5, 2);

            //Act
            manager.AddHoliday(holiday);

            //Assert
            bool isHoliday = manager.IsHoliday(checkedDate);
            Assert.IsFalse(isHoliday);

        }

        [TestMethod]
        public void LoadHolidays_ReturnsTrue_ForDateAddedAsListItem()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            List<DateTime> holidays = new List<DateTime>();
            holidays.Add(new DateTime(2017, 5, 1));
            holidays.Add(new DateTime(2017, 5, 3));
            holidays.Add(new DateTime(2017, 11, 11));
            DateTime checkedDate = new DateTime(2017, 5, 2);

            //Act
            manager.LoadHolidays(holidays);

            //Assert
            bool isHoliday = manager.IsHoliday(checkedDate);
            Assert.IsFalse(isHoliday);

        }

        #endregion LOADING_HOLIDAYS


        #region IS_DAY_BEFORE_HOLIDAY

        [TestMethod]
        public void IsDayBeforeHoliday_ReturnsTrue_IfNextDayAfterGivenDateIsHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 11, 23, 34);

            //Act
            manager.AddHoliday(new DateTime(2017, 5, 3));

            //Assert
            bool isHoliday = manager.IsDayBeforeHoliday(checkedDate);
            Assert.IsTrue(isHoliday);

        }

        [TestMethod]
        public void IsDayBeforeHoliday_ReturnsFalse_IfNextDayAfterGivenDateIsWeekDay()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 4, 11, 23, 34);

            //Act
            manager.AddHoliday(new DateTime(2017, 5, 3));

            //Assert
            bool isHoliday = manager.IsDayBeforeHoliday(checkedDate);
            Assert.IsFalse(isHoliday);

        }

        [TestMethod]
        public void IsDayBeforeHoliday_ReturnsFalse_IfGivenDayIsHolidayButNotNextOne()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 3, 11, 23, 34);

            //Act
            manager.AddHoliday(new DateTime(2017, 5, 3));

            //Assert
            bool isHoliday = manager.IsDayBeforeHoliday(checkedDate);
            Assert.IsFalse(isHoliday);

        }

        [TestMethod]
        public void IsDayBeforeHoliday_ReturnsFalse_IfNextDayIsRegularSunday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 6, 11, 23, 34);

            //Act
            manager.AddHoliday(new DateTime(2017, 5, 3));

            //Assert
            bool isHoliday = manager.IsDayBeforeHoliday(checkedDate);
            Assert.IsFalse(isHoliday);

        }

        [TestMethod]
        public void IsDayBeforeHoliday_ReturnsFalse_IfNextDayIsRegularSaturday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 5, 11, 23, 34);

            //Act
            manager.AddHoliday(new DateTime(2017, 5, 3));

            //Assert
            bool isHoliday = manager.IsDayBeforeHoliday(checkedDate);
            Assert.IsFalse(isHoliday);

        }

        #endregion IS_DAY_BEFORE_HOLIDAY


        #region IS_WORKING_DAY

        [TestMethod]
        public void IsWorkingDay_ReturnsTrue_ForWorkingDay()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 11, 15, 0);

            //Act

            //Assert
            bool isWorkingDay = manager.IsWorkingDay(checkedDate);
            Assert.IsTrue(isWorkingDay);

        }

        [TestMethod]
        public void IsWorkingDay_ReturnsFalse_ForWeekDayHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 11, 15, 0);

            //Act
            manager.AddHoliday(checkedDate.Midnight());

            //Assert
            bool isWorkingDay = manager.IsWorkingDay(checkedDate);
            Assert.IsFalse(isWorkingDay);

        }

        [TestMethod]
        public void IsWorkingDay_ReturnsFalse_ForWeekend()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 6, 11, 15, 0);

            //Act

            //Assert
            bool isWorkingDay = manager.IsWorkingDay(checkedDate);
            Assert.IsFalse(isWorkingDay);

        }

        #endregion IS_WORKING_DAY


        #region IS_WORKING_TIME

        [TestMethod]
        public void IsWorkingTime_ReturnsTrue_ForWorkingDay()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 11, 15, 0);

            //Act

            //Assert
            bool isWorkingTime = manager.IsWorkingTime(checkedDate);
            Assert.IsTrue(isWorkingTime);

        }

        [TestMethod]
        public void IsWorkingTime_ReturnsFalse_ForWeekDayHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 11, 15, 0);

            //Act
            manager.AddHoliday(checkedDate.Midnight());

            //Assert
            bool isWorkingTime = manager.IsWorkingTime(checkedDate);
            Assert.IsFalse(isWorkingTime);

        }

        [TestMethod]
        public void IsWorkingTime_ReturnsFalse_ForWeekend()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 6, 11, 15, 0);

            //Act

            //Assert
            bool isWorkingTime = manager.IsWorkingTime(checkedDate);
            Assert.IsFalse(isWorkingTime);

        }

        [TestMethod]
        public void IsWorkingTime_ReturnsFalse_ForWorkingDayBeforeHolidayAfterEveningBreak()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 3, 21, 15, 0);

            //Act
            manager.AddHoliday(new DateTime(2017, 5, 4));
            manager.SetHolidayEveBreak(new TimeSpan(3, 0, 0));

            //Assert
            bool isWorkingTime = manager.IsWorkingTime(checkedDate);
            Assert.IsFalse(isWorkingTime);

        }

        #endregion IS_WORKING_TIME


        #region IS_DAY_OFF

        [TestMethod]
        public void IsDayOff_ReturnsFalse_ForWorkingDay()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 11, 15, 0);

            //Act

            //Assert
            bool isDayOff = manager.IsDayOff(checkedDate);
            Assert.IsFalse(isDayOff);

        }

        [TestMethod]
        public void IsDayOff_ReturnsTrue_ForWeekDayHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 11, 15, 0);

            //Act
            manager.AddHoliday(checkedDate.Midnight());

            //Assert
            bool isDayOff = manager.IsDayOff(checkedDate);
            Assert.IsTrue(isDayOff);

        }

        [TestMethod]
        public void IsDayOff_ReturnsTrue_ForWeekend()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 6, 11, 15, 0);

            //Act

            //Assert
            bool isDayOff = manager.IsDayOff(checkedDate);
            Assert.IsTrue(isDayOff);

        }

        #endregion IS_DAY_OFF


        #region IS_TIME_AFTER_HOLIDAY_EVE_CLOSE

        [TestMethod]
        public void isTimeAfterHolidayEveMarketClose_ReturnsFalse_IfNextDayIsWorkingDay()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 2, 23, 15, 0);

            //Act

            //Assert
            bool isTimeAfterHolidayEveMarketClose = manager.IsHolidayEveAfterMarketClose(checkedDate);
            Assert.IsFalse(isTimeAfterHolidayEveMarketClose);

        }

        [TestMethod]
        public void isTimeAfterHolidayEveMarketClose_ReturnsFalse_IfNextDayIsSaturday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 5, 23, 0, 0);

            //Act

            //Assert
            bool isTimeAfterHolidayEveMarketClose = manager.IsHolidayEveAfterMarketClose(checkedDate);
            Assert.IsFalse(isTimeAfterHolidayEveMarketClose);

        }

        [TestMethod]
        public void isTimeAfterHolidayEveMarketClose_ReturnsFalse_IfNextDayIsSunday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime checkedDate = new DateTime(2017, 5, 6, 23, 15, 0);

            //Act

            //Assert
            bool isTimeAfterHolidayEveMarketClose = manager.IsHolidayEveAfterMarketClose(checkedDate);
            Assert.IsFalse(isTimeAfterHolidayEveMarketClose);

        }

        [TestMethod]
        public void isTimeAfterHolidayEveMarketClose_ReturnsFalse_IfNextDayIsHolidayButItIsBeforeBreak()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 3));
            manager.SetHolidayEveBreak(new TimeSpan(2, 0, 0));

            //Act
            DateTime checkedDate = new DateTime(2017, 5, 2, 21, 15, 0);

            //Assert
            bool isTimeAfterHolidayEveMarketClose = manager.IsHolidayEveAfterMarketClose(checkedDate);
            Assert.IsFalse(isTimeAfterHolidayEveMarketClose);

        }

        [TestMethod]
        public void isTimeAfterHolidayEveMarketClose_ReturnsTrue_IfNextDayIsHolidayAndItIsAfterBreak()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 3));
            manager.SetHolidayEveBreak(new TimeSpan(3, 0, 0));

            //Act
            DateTime checkedDate = new DateTime(2017, 5, 2, 23, 0, 0);

            //Assert
            bool isTimeAfterHolidayEveMarketClose = manager.IsHolidayEveAfterMarketClose(checkedDate);
            Assert.IsTrue(isTimeAfterHolidayEveMarketClose);

        }


        #endregion IS_TIME_AFTER_HOLIDAY_EVE_CLOSE


        #region GET_NEXT_WORKING_DAY

        [TestMethod]
        public void GetNextWorkingDay_ReturnsNextDay_IfNextDayIsWorking()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 3));
            DateTime baseDate = new DateTime(2017, 5, 1);

            //Act
            DateTime date = manager.GetNextWorkingDay(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 2);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetNextWorkingDay_ReturnsNextMonday_IfNextDayIsSaturday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime baseDate = new DateTime(2017, 5, 5);

            //Act
            DateTime date = manager.GetNextWorkingDay(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 8);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetNextWorkingDay_ReturnsTwoDaysLater_IfNextDayIsMiddleWeekHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 3));
            DateTime baseDate = new DateTime(2017, 5, 2);

            //Act
            DateTime date = manager.GetNextWorkingDay(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 4);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetNextWorkingDay_ReturnsNextMonday_IfNextDayIsFridayHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 5));
            DateTime baseDate = new DateTime(2017, 5, 4);

            //Act
            DateTime date = manager.GetNextWorkingDay(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 8);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetNextWorkingDay_ReturnsNextTuesday_IfItIsFridayAndMondayIsHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 8));
            DateTime baseDate = new DateTime(2017, 5, 5);

            //Act
            DateTime date = manager.GetNextWorkingDay(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 9);
            Assert.AreEqual(expectedDate, date);

        }

        #endregion GET_NEXT_WORKING_DAY


        #region GET_NEXT_HOLIDAY

        [TestMethod]
        public void GetNextHoliday_ReturnsClosestRegisteredHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 1));
            manager.AddHoliday(new DateTime(2017, 5, 3));
            manager.AddHoliday(new DateTime(2017, 11, 11));
            DateTime baseDate = new DateTime(2017, 5, 2);

            //Act
            DateTime? date = manager.GetNextHoliday(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 3);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetNextHoliday_ReturnsNull_IfThereIsNoMoreHolidays()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 3));
            DateTime baseDate = new DateTime(2017, 5, 4);

            //Act
            DateTime? result = manager.GetNextHoliday(baseDate);

            //Assert
            Assert.IsNull(result);

        }


        [TestMethod]
        public void GetNextHolidayWithEndDate_ReturnsClosestRegisteredHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 1));
            manager.AddHoliday(new DateTime(2017, 5, 3));
            manager.AddHoliday(new DateTime(2017, 11, 11));
            DateTime startDate = new DateTime(2017, 5, 2);
            DateTime endDate = new DateTime(2017, 5, 8);

            //Act
            DateTime? date = manager.GetNextHoliday(startDate, endDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 3);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetNextHolidayWithEndDate_ReturnsNull_IfThereIsNoMoreHolidays()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 3));
            DateTime startDate = new DateTime(2017, 5, 4);
            DateTime endDate = new DateTime(2017, 5, 8);

            //Act
            DateTime? result = manager.GetNextHoliday(startDate, endDate);

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void GetNextHolidayWithEndDate_ReturnsNull_IfThereAreHolidaysOnlyAfterEndDate()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 3));
            DateTime startDate = new DateTime(2017, 4, 2);
            DateTime endDate = new DateTime(2017, 4, 21);

            //Act
            DateTime? result = manager.GetNextHoliday(startDate, endDate);

            //Assert
            Assert.IsNull(result);

        }

        #endregion GET_NEXT_HOLIDAY


        #region GET_NEXT_TIME_OFF

        [TestMethod]
        public void GetNextTimeOff_ReturnsClosestRegisteredHoliday_IfItIsEarlierThanWeekend()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 1));
            manager.AddHoliday(new DateTime(2017, 5, 3));
            manager.AddHoliday(new DateTime(2017, 11, 11));
            DateTime baseDate = new DateTime(2017, 5, 2, 16, 0, 0);

            //Act
            DateTime date = manager.GetNextTimeOff(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 2, 21, 0, 0);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetNextTimeOff_ReturnsWeekendStart_IfItIsEarlierThanNextHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 8));
            DateTime baseDate = new DateTime(2017, 5, 4);

            //Act
            DateTime date = manager.GetNextTimeOff(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, date);

        }


        [TestMethod]
        public void GetNextTimeOff_ReturnsWeekendStart_IfThereIsNoFutureHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime baseDate = new DateTime(2017, 5, 4);

            //Act
            DateTime date = manager.GetNextTimeOff(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, date);

        }

        #endregion GET_NEXT_TIME_OFF



        #region GET_PREVIOUS_TIME_OFF

        [TestMethod]
        public void GetPreviousTimeOff_ReturnsClosestRegisteredHoliday_IfItIsLaterThanPreviousWeekend()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 1));
            manager.AddHoliday(new DateTime(2017, 5, 3));
            manager.AddHoliday(new DateTime(2017, 11, 11));
            DateTime baseDate = new DateTime(2017, 5, 4, 16, 0, 0);

            //Act
            DateTime date = manager.GetPreviousTimeOff(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 2, 21, 0, 0);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetPreviousTimeOff_ReturnsPreviousWeekendStart_IfItIsLaterThanPreviousHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            manager.AddHoliday(new DateTime(2017, 5, 4));
            DateTime baseDate = new DateTime(2017, 5, 8);

            //Act
            DateTime date = manager.GetPreviousTimeOff(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, date);

        }

        [TestMethod]
        public void GetPreviousTimeOff_ReturnsPreviousWeekendStart_IfThereIsNoPreviousHoliday()
        {

            //Arrange
            HolidaysManager manager = new HolidaysManager();
            DateTime baseDate = new DateTime(2017, 5, 9);

            //Act
            DateTime date = manager.GetPreviousTimeOff(baseDate);

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, date);

        }

        #endregion GET_PREVIOUS_TIME_OFF


    }
}
