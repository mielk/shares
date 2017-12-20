using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Utils.HelperMethods
{
    [TestClass]
    public class DateTimeHelperMethodsUnitTest
    {

        #region MODIFIED_DAY_OF_WEEK

        [TestMethod]
        public void DayOfWeekTm_Returns7_ForSunday()
        {

            //Arrange
            DateTime date = new DateTime(2017, 5, 7, 11, 0, 0);

            //Act
            int dayOfWeek = date.DayOfWeekTm();

            //Assert
            int expectedValue = 7;
            Assert.AreEqual(expectedValue, dayOfWeek);
        }

        [TestMethod]
        public void DayOfWeekTm_ReturnsProperValue_ForOtherThanSunday()
        {

            //Arrange
            DateTime date = new DateTime(2017, 5, 4, 0, 0, 0);

            //Act
            int dayOfWeek = date.DayOfWeekTm();

            //Assert
            int expectedValue = 4;
            Assert.AreEqual(expectedValue, dayOfWeek);
        }


        #endregion MODIFIED_DAY_OF_WEEK


        #region MIDNIGHT

        [TestMethod]
        public void Midnight_ReturnsProperValue()
        {

            //Arrange
            DateTime date = new DateTime(2017, 5, 7, 11, 12, 13);

            //Act
            DateTime midnight = date.Midnight();

            //Assert
            DateTime expectedValue = new DateTime(2017, 5, 7, 0, 0, 0);
            Assert.AreEqual(expectedValue, midnight);
        }


        #endregion MIDNIGHT


        #region IS_WEEKEND

        [TestMethod]
        public void IsWeekend_ReturnsTrue_ForSaturdayMidnight()
        {

            //Arrange
            DateTime date = new DateTime(2017, 4, 29, 0, 0, 0);

            //Assert
            Assert.IsTrue(date.IsWeekend());
        }

        [TestMethod]
        public void IsWeekend_ReturnsFalse_ForFriday235959()
        {

            //Arrange
            DateTime date = new DateTime(2017, 4, 28, 23, 59, 59);

            //Assert
            Assert.IsFalse(date.IsWeekend());
        }

        [TestMethod]
        public void IsWeekend_ReturnsTrue_ForSunday235959()
        {

            //Arrange
            DateTime date = new DateTime(2017, 4, 30, 23, 59, 59);

            //Assert
            Assert.IsTrue(date.IsWeekend());
        }

        [TestMethod]
        public void IsWeekend_ReturnsFalse_ForMondayMidnight()
        {

            //Arrange
            DateTime date = new DateTime(2017, 5, 1, 0, 0, 0);

            //Assert
            Assert.IsFalse(date.IsWeekend());

        }

        [TestMethod]
        public void IsWeekend_ReturnsFalse_ForTuesdayDate()
        {

            //Arrange
            DateTime date = new DateTime(2017, 5, 2, 11, 0, 0);

            //Assert
            Assert.IsFalse(date.IsWeekend());

        }

        [TestMethod]
        public void IsWeekend_ReturnsFalse_ForWednesdayDate()
        {

            //Arrange
            DateTime date = new DateTime(2017, 5, 3, 2, 0, 0);

            //Assert
            Assert.IsFalse(date.IsWeekend());

        }

        [TestMethod]
        public void IsWeekend_ReturnsFalse_ForThursdayDate()
        {

            //Arrange
            DateTime date = new DateTime(2017, 5, 4, 11, 0, 0);

            //Assert
            Assert.IsFalse(date.IsWeekend());

        }

        #endregion IS_WEEKEND


        #region GET_WEEKEND_START

        [TestMethod]
        public void GetWeekendStart_ReturnsProperValue_ForMidweekValue()
        {

            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 3, 11, 15, 0);

            //Act
            DateTime result = baseDate.GetWeekendStart();

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);
        }

        [TestMethod]
        public void GetWeekendStart_ReturnsProperValue_ForWeekendValue()
        {

            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 6, 14, 0, 0);

            //Act
            DateTime result = baseDate.GetWeekendStart();

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);
        }


        [TestMethod]
        public void GetWeekendStart_ReturnsProperValue_ForSunday235959()
        {

            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 7, 23, 59, 59);

            //Act
            DateTime result = baseDate.GetWeekendStart();

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);
        }


        [TestMethod]
        public void GetWeekendStart_ReturnsProperValue_ForMonday000000()
        {

            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 0, 0, 0);

            //Act
            DateTime result = baseDate.GetWeekendStart();

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 6, 0, 0, 0);
            Assert.AreEqual(expectedDate, result);
        }



        #endregion GET_WEEKEND_START


        #region DAY_BEFORE

        [TestMethod]
        public void DayBefore_ReturnsProperDate()
        {
            
            //Arrange
            DateTime baseDate = new DateTime(2017, 4, 30, 15, 30, 12);

            //Act
            DateTime result = baseDate.DayBefore();

            //Assert
            DateTime expectedDate = new DateTime(2017, 4, 29, 15, 30, 12);
            Assert.AreEqual(expectedDate, result);

        }

        #endregion DAY_BEFORE


        #region SET_TIME
        
        [TestMethod]
        public void SetTime_ReturnProperValue()
        {

            //Arrange
            DateTime baseDate = new DateTime(2017, 4, 30, 15, 30, 12);
            TimeSpan span = new TimeSpan(12, 43, 18);

            //Act
            DateTime result = baseDate.SetTime(span);

            //Assert
            DateTime expectedDate = new DateTime(2017, 4, 30, 12, 43, 18);
            Assert.AreEqual(expectedDate, result);

        }

        #endregion SET_TIME


        #region LATER & EARLIER

        [TestMethod]
        public void IsLaterThan_ReturnsTrue_IfComparedDateIsNull()
        {
            //Arrange
            DateTime? baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime? comparedDate = null;

            //Assert
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.IsTrue(isLater);

        }

        [TestMethod]
        public void IsLaterThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNotNull()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = null;

            //Assert
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.IsFalse(isLater);

        }

        [TestMethod]
        public void IsLaterThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNullToo()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.IsFalse(isLater);
        }


        [TestMethod]
        public void IsLaterThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 13, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isLaterNullAllowed = baseDateNullAllowed.IsLaterThan(comparedDateNullAllowed);
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.AreEqual(isLater, isLaterNullAllowed);

        }

        [TestMethod]
        public void IsLaterThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEqual()
        {

            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isLaterNullAllowed = baseDateNullAllowed.IsLaterThan(comparedDateNullAllowed);
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.AreEqual(isLater, isLaterNullAllowed);

        }

        [TestMethod]
        public void IsLaterThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsLater()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 15, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isLaterNullAllowed = baseDateNullAllowed.IsLaterThan(comparedDateNullAllowed);
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.AreEqual(isLater, isLaterNullAllowed);

        }




        [TestMethod]
        public void IsLaterThan_ReturnsTrue_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 13, 14, 0);

            //Assert
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.IsTrue(isLater);

        }

        [TestMethod]
        public void IsLaterThan_ReturnsFalse_IfComparedDateIsEqual()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.IsFalse(isLater);

        }

        [TestMethod]
        public void IsLaterThan_ReturnsFalse_IfComparedDateIsLater()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 15, 14, 0);

            //Assert
            bool isLater = baseDate.IsLaterThan(comparedDate);
            Assert.IsFalse(isLater);

        }






        [TestMethod]
        public void IsEarlierThan_ReturnsFalse_IfComparedDateIsNull()
        {
            //Arrange
            DateTime? baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime? comparedDate = null;

            //Assert
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);

        }

        [TestMethod]
        public void IsEarlierThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNotNull()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = null;

            //Assert
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);

        }

        [TestMethod]
        public void IsEarlierThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNullToo()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);
        }


        [TestMethod]
        public void IsEarlierThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsLater()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 15, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isEarlierNullAllowed = baseDateNullAllowed.IsEarlierThan(comparedDateNullAllowed);
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.AreEqual(isEarlier, isEarlierNullAllowed);

        }

        [TestMethod]
        public void IsEarlierThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEqual()
        {

            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isEarlierNullAllowed = baseDateNullAllowed.IsEarlierThan(comparedDateNullAllowed);
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.AreEqual(isEarlier, isEarlierNullAllowed);

        }

        [TestMethod]
        public void IsEarlierThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 13, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isEarlierNullAllowed = baseDateNullAllowed.IsEarlierThan(comparedDateNullAllowed);
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.AreEqual(isEarlier, isEarlierNullAllowed);

        }


        [TestMethod]
        public void IsEarlierThan_ReturnsTrue_IfComparedDateIsLater()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 15, 14, 0);

            //Assert
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.IsTrue(isEarlier);

        }

        [TestMethod]
        public void IsEarlierThan_ReturnsFalse_IfComparedDateIsEqual()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);

        }

        [TestMethod]
        public void IsEarlierThan_ReturnsFalse_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 13, 14, 0);

            //Assert
            bool isEarlier = baseDate.IsEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);

        }





        [TestMethod]
        public void IsNotEarlierThan_ReturnsTrue_IfComparedDateIsNull()
        {
            //Arrange
            DateTime? baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime? comparedDate = null;

            //Assert
            bool isEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsTrue(isEarlier);

        }

        [TestMethod]
        public void IsNotEarlierThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNotNull()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = null;

            //Assert
            bool isEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);

        }

        [TestMethod]
        public void IsNotEarlierThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNullToo()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);
        }


        [TestMethod]
        public void IsNotEarlierThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 13, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isNotEarlierNullAllowed = baseDateNullAllowed.IsNotEarlierThan(comparedDateNullAllowed);
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.AreEqual(isNotEarlier, isNotEarlierNullAllowed);

        }

        [TestMethod]
        public void IsNotEarlierThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEqual()
        {

            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isNotEarlierNullAllowed = baseDateNullAllowed.IsNotEarlierThan(comparedDateNullAllowed);
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.AreEqual(isNotEarlier, isNotEarlierNullAllowed);

        }

        [TestMethod]
        public void IsNotEarlierThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsLater()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 15, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isNotEarlierNullAllowed = baseDateNullAllowed.IsNotEarlierThan(comparedDateNullAllowed);
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.AreEqual(isNotEarlier, isNotEarlierNullAllowed);

        }

        [TestMethod]
        public void IsNotEarlierThan_ReturnsTrue_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 13, 14, 0);

            //Assert
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsTrue(isNotEarlier);

        }

        [TestMethod]
        public void IsNotEarlierThan_ReturnsTrue_IfComparedDateIsEqual()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsTrue(isNotEarlier);

        }

        [TestMethod]
        public void IsNotEarlierThan_ReturnsFalse_IfComparedDateIsLater()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 15, 14, 0);

            //Assert
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsFalse(isNotEarlier);

        }





        [TestMethod]
        public void IsNotLaterThan_ReturnsFalse_IfComparedDateIsNull()
        {
            //Arrange
            DateTime? baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime? comparedDate = null;

            //Assert
            bool isEarlier = baseDate.IsNotLaterThan(comparedDate);
            Assert.IsFalse(isEarlier);

        }

        [TestMethod]
        public void IsNotLaterThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNotNull()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = null;

            //Assert
            bool isEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);

        }

        [TestMethod]
        public void IsNotLaterThan_ReturnsFalse_IfBaseDateIsNullAndComparedIsNullToo()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.IsFalse(isEarlier);
        }


        [TestMethod]
        public void IsNotLaterThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsLater()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 13, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isNotEarlierNullAllowed = baseDateNullAllowed.IsNotEarlierThan(comparedDateNullAllowed);
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.AreEqual(isNotEarlier, isNotEarlierNullAllowed);

        }

        [TestMethod]
        public void IsNotLaterThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEqual()
        {

            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isNotEarlierNullAllowed = baseDateNullAllowed.IsNotEarlierThan(comparedDateNullAllowed);
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.AreEqual(isNotEarlier, isNotEarlierNullAllowed);

        }

        [TestMethod]
        public void IsNotLaterThan_ReturnsTheSameValueForNullAllowedVersion_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime? baseDateNullAllowed = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime baseDate = (DateTime)baseDateNullAllowed;
            DateTime? comparedDateNullAllowed = new DateTime(2017, 5, 1, 15, 14, 0);
            DateTime comparedDate = (DateTime)comparedDateNullAllowed;

            //Assert
            bool isNotEarlierNullAllowed = baseDateNullAllowed.IsNotEarlierThan(comparedDateNullAllowed);
            bool isNotEarlier = baseDate.IsNotEarlierThan(comparedDate);
            Assert.AreEqual(isNotEarlier, isNotEarlierNullAllowed);

        }


        [TestMethod]
        public void IsNotLaterThan_ReturnsTrue_IfComparedDateIsLater()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 15, 14, 0);

            //Assert
            bool isNotLater = baseDate.IsNotLaterThan(comparedDate);
            Assert.IsTrue(isNotLater);

        }

        [TestMethod]
        public void IsNotLaterThan_ReturnsTrue_IfComparedDateIsEqual()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 14, 15, 9);

            //Assert
            bool isNotLater = baseDate.IsNotLaterThan(comparedDate);
            Assert.IsTrue(isNotLater);

        }

        [TestMethod]
        public void IsNotLaterThan_ReturnsFalse_IfComparedDateIsEarlier()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 1, 13, 14, 0);

            //Assert
            bool isNotLater = baseDate.IsNotLaterThan(comparedDate);
            Assert.IsFalse(isNotLater);

        }


        #endregion LATER & EARLIER


        #region ARE_THE_SAME_WEEK

        [TestMethod]
        public void AreTheSameWeek_ReturnsTrue_IfFirstDateIsEarlierInTheSameWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 2, 13, 14, 0);

            //Assert
            bool isInTheSameWeek = baseDate.AreTheSameWeek(comparedDate);
            Assert.IsTrue(isInTheSameWeek);

        }

        [TestMethod]
        public void AreTheSameWeek_ReturnsTrue_IfFirstDateIsLaterInTheSameWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 6, 21, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 2, 13, 14, 0);

            //Assert
            bool isInTheSameWeek = baseDate.AreTheSameWeek(comparedDate);
            Assert.IsTrue(isInTheSameWeek);

        }

        [TestMethod]
        public void AreTheSameWeek_ReturnsTrue_ForMonday000000AndSunday235959()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 0, 0, 0);
            DateTime comparedDate = new DateTime(2017, 5, 7, 23, 59, 59);

            //Assert
            bool isInTheSameWeek = baseDate.AreTheSameWeek(comparedDate);
            Assert.IsTrue(isInTheSameWeek);

        }

        [TestMethod]
        public void AreTheSameWeek_ReturnsFalse_IfFirstDateIsInEarlierWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 4, 30, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 2, 13, 14, 0);

            //Assert
            bool isInTheSameWeek = baseDate.AreTheSameWeek(comparedDate);
            Assert.IsFalse(isInTheSameWeek);

        }

        [TestMethod]
        public void AreTheSameWeek_ReturnsTrue_IfFirstDateIsInLaterWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 8, 14, 15, 9);
            DateTime comparedDate = new DateTime(2017, 5, 2, 13, 14, 0);

            //Assert
            bool isInTheSameWeek = baseDate.AreTheSameWeek(comparedDate);
            Assert.IsFalse(isInTheSameWeek);

        }


        #endregion ARE_THE_SAME_WEEK


        #region WEEK_START

        [TestMethod]
        public void WeekStart_ReturnsEarlierMonday000000_ForSunday235959()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 7, 23, 59, 59);

            //Act
            DateTime startWeek = baseDate.WeekStart();

            //Assert
            DateTime expectedDate = new DateTime(2017, 5, 1);
            Assert.AreEqual(expectedDate, startWeek);

        }

        [TestMethod]
        public void WeekStart_ReturnsMonday000000_ForMonday000000()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 1, 0, 0, 0);

            //Act
            DateTime startWeek = baseDate.WeekStart();

            //Assert
            DateTime expectedDate = new DateTime(baseDate.Ticks);
            Assert.AreEqual(expectedDate, startWeek);

        }

        #endregion WEEK_START


        #region GET_WEEKS_DIFFERENCE

        [TestMethod]
        public void GetWeeksDifference_ReturnsZero_IfDatesIsEarlierInTheSameWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 7, 23, 59, 59);
            DateTime comparedDate = new DateTime(2017, 5, 6, 12, 23, 12);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = 0;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        [TestMethod]
        public void GetWeeksDifference_ReturnsZero_IfDatesIsLaterInTheSameWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2017, 5, 4, 23, 59, 59);
            DateTime comparedDate = new DateTime(2017, 5, 6, 12, 23, 12);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = 0;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        [TestMethod]
        public void GetWeeksDifference_ReturnsProperValue_IfComparedDateIsFewWeeksEarlier()
        {
            //Arrange
            DateTime baseDate = new DateTime(2016, 4, 20);
            DateTime comparedDate = new DateTime(2016, 3, 7);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = -6;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        [TestMethod]
        public void GetWeeksDifference_ReturnsProperValue_IfComparedDateIsOneWeekEarlierAndOnLaterDayOfWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2016, 4, 20);
            DateTime comparedDate = new DateTime(2016, 4, 16);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = -1;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        [TestMethod]
        public void GetWeeksDifference_ReturnsProperValue_IfComparedDateIsOneWeekEarlierAndOnEarlierDayOfWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2016, 4, 20);
            DateTime comparedDate = new DateTime(2016, 4, 11);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = -1;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        [TestMethod]
        public void GetWeeksDifference_ReturnsProperValue_IfComparedDateIsOneWeekLaterAndOnEarlierDayOfWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2016, 8, 11);
            DateTime comparedDate = new DateTime(2016, 8, 16);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = 1;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        [TestMethod]
        public void GetWeeksDifference_ReturnsProperValue_IfComparedDateIsOneWeekLaterAndOnLaterDayOfWeek()
        {
            //Arrange
            DateTime baseDate = new DateTime(2016, 8, 11);
            DateTime comparedDate = new DateTime(2016, 8, 19);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = 1;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        [TestMethod]
        public void GetWeeksDifference_ReturnsProperValue_IfComparedDateIsFewWeeksLater()
        {
            //Arrange
            DateTime baseDate = new DateTime(2016, 4, 20);
            DateTime comparedDate = new DateTime(2016, 6, 20);

            //Act
            int weeksDifference = baseDate.GetWeeksDifferenceTo(comparedDate);

            //Assert
            int expectedDifference = 9;
            Assert.AreEqual(expectedDifference, weeksDifference);

        }

        #endregion GET_WEEKS_DIFFERENCE


        #region NULLABLE_DATETIME_IS_EQUAL


        [TestMethod]
        public void NullableDateTimeIsEqual_ReturnsTrue_IfBothDatesAreNull()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = null;

            //Assert
            var result = baseDate.IsEqual(comparedDate);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void NullableDateTimeIsEqual_ReturnsFalse_IfOnlyBaseDateIsNull()
        {
            //Arrange
            DateTime? baseDate = null;
            DateTime? comparedDate = new DateTime(2017, 5, 4, 12, 0, 0);

            //Assert
            var result = baseDate.IsEqual(comparedDate);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void NullableDateTimeIsEqual_ReturnsFalse_IfOnlyComparedDateIsNull()
        {
            //Arrange
            DateTime? baseDate = new DateTime(2017, 5, 4, 12, 0, 0);
            DateTime? comparedDate = null;

            //Assert
            var result = baseDate.IsEqual(comparedDate);
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void NullableDateTimeIsEqual_ReturnsFalse_IfGivenDatesAreDifferent()
        {
            //Arrange
            DateTime? baseDate = new DateTime(2017, 5, 4, 12, 0, 0);
            DateTime? comparedDate = new DateTime(2017, 5, 4, 13, 0, 0);

            //Assert
            var result = baseDate.IsEqual(comparedDate);
            Assert.IsFalse(result);

        }


        [TestMethod]
        public void NullableDateTimeIsEqual_ReturnsTrue_IfGivenDatesAreEqual()
        {
            //Arrange
            DateTime? baseDate = new DateTime(2017, 5, 4, 12, 0, 0);
            DateTime? comparedDate = new DateTime(2017, 5, 4, 12, 0, 0);

            //Assert
            var result = baseDate.IsEqual(comparedDate);
            Assert.IsTrue(result);

        }

        #endregion NULLABLE_DATETIME_IS_EQUAL

    }
}