using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class AnalysisInfoUnitTests
    {

        private DateTime DEFAULT_START_DATETIME = new DateTime(2017, 3, 4, 21, 10, 0);
        private DateTime DEFAULT_END_DATETIME = new DateTime(2017, 3, 7, 21, 10, 0);
        private int DEFAULT_START_INDEX = 1;
        private int DEFAULT_END_INDEX = 100;
        private const double DEFAULT_MIN_LEVEL = 1.03245;
        private const double DEFAULT_MAX_LEVEL = 1.03845;
        private const int DEFAULT_COUNTER = 10;



        #region FROM_DTO

        [TestMethod]
        public void FromDto_ReturnsProperObject()
        {

            //Arrange
            AnalysisInfoDto dto = new AnalysisInfoDto()
            {
                StartDate = DEFAULT_START_DATETIME,
                EndDate = DEFAULT_END_DATETIME,
                StartIndex = DEFAULT_START_INDEX,
                EndIndex = DEFAULT_END_INDEX,
                MinLevel = DEFAULT_MIN_LEVEL,
                MaxLevel = DEFAULT_MAX_LEVEL,
                Counter = DEFAULT_COUNTER
            };

            //Act
            AnalysisInfo actualAnalysisInfo = AnalysisInfo.FromDto(dto);

            //Assert
            AnalysisInfo expectedAnalysisInfo = new AnalysisInfo()
            {
                StartDate = DEFAULT_START_DATETIME,
                EndDate = DEFAULT_END_DATETIME,
                StartIndex = DEFAULT_START_INDEX,
                EndIndex = DEFAULT_END_INDEX,
                MinLevel = DEFAULT_MIN_LEVEL,
                MaxLevel = DEFAULT_MAX_LEVEL,
                Counter = DEFAULT_COUNTER
            };
            Assert.AreEqual(expectedAnalysisInfo, actualAnalysisInfo);

        }

        #endregion FROM_DTO


        #region TO_DTO

        [TestMethod]
        public void ToDto_ReturnsProperPriceDtoObject()
        {

            //Arrange
            AnalysisInfo info = new AnalysisInfo()
            {
                StartDate = DEFAULT_START_DATETIME,
                EndDate = DEFAULT_END_DATETIME,
                StartIndex = DEFAULT_START_INDEX,
                EndIndex = DEFAULT_END_INDEX,
                MinLevel = DEFAULT_MIN_LEVEL,
                MaxLevel = DEFAULT_MAX_LEVEL,
                Counter = DEFAULT_COUNTER
            };

            //Act
            AnalysisInfoDto actualDto = info.ToDto();

            //Assert
            AnalysisInfoDto expectedDto = new AnalysisInfoDto()
            {
                StartDate = DEFAULT_START_DATETIME,
                EndDate = DEFAULT_END_DATETIME,
                StartIndex = DEFAULT_START_INDEX,
                EndIndex = DEFAULT_END_INDEX,
                MinLevel = DEFAULT_MIN_LEVEL,
                MaxLevel = DEFAULT_MAX_LEVEL,
                Counter = DEFAULT_COUNTER
            };

            Assert.AreEqual(expectedDto, actualDto);

        }

        #endregion TO_DTO


        #region EQUALS

        private AnalysisInfo getDefaultAnalysisInfo()
        {
            return new AnalysisInfo()
            {
                StartDate = DEFAULT_START_DATETIME,
                StartIndex = DEFAULT_START_INDEX,
                EndDate = DEFAULT_END_DATETIME,
                EndIndex = DEFAULT_END_INDEX,
                MinLevel = DEFAULT_MIN_LEVEL,
                MaxLevel = DEFAULT_MAX_LEVEL,
                Counter = DEFAULT_COUNTER
            };
        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = new { Id = 1 };

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfAllPropertiesAreEqualAndAllObjectsAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfStartDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.StartDate = ((DateTime)comparedItem.StartDate).AddMinutes(5);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfStartIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.StartIndex = comparedItem.StartIndex + 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothStartDatesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.StartDate = null;
            comparedItem.StartDate = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothStartIndexesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.StartIndex = null;
            comparedItem.StartIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseStartDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.StartDate = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseStartIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.StartIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedStartDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.StartDate = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedStartIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.StartIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEndDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.EndDate = ((DateTime)comparedItem.EndDate).AddMinutes(5);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEndIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.EndIndex = comparedItem.EndIndex + 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothEndDatesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.EndDate = null;
            comparedItem.EndDate = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothEndIndexesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.EndIndex = null;
            comparedItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseEndDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.EndDate = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseEndIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedEndDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.EndDate = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedEndIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfMinLevelIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.MinLevel = ((double)comparedItem.MinLevel) + 0.01;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothMinLevelAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.MinLevel = null;
            comparedItem.MinLevel = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseMinLevelIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.MinLevel = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedMinLevelIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.MinLevel = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }



        [TestMethod]
        public void Equals_ReturnsFalse_IfMaxLevelIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.MaxLevel = ((double)comparedItem.MaxLevel) + 0.01;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothMaxLevelAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.MaxLevel = null;
            comparedItem.MaxLevel = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseMaxLevelIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            baseItem.MaxLevel = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedMaxLevelIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.MaxLevel = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfCounterIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisInfo();
            var comparedItem = getDefaultAnalysisInfo();

            //Act
            comparedItem.Counter++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        #endregion EQUALS


    }
}
