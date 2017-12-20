using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Stock.DAL.TransferObjects;
using Stock.Domain.Services;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class TrendRangesDtoUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_GUID = "74017f2d-9dfe-494e-bfa0-93c09418cfb7";
        private const int DEFAULT_TRENDLINE_ID = 1;
        private const int DEFAULT_START_INDEX = 2;
        private const int DEFAULT_END_INDEX = 12;
        private const int DEFAULT_QUOTATIONS_COUNTER = 11;
        private const double DEFAULT_TOTAL_DISTANCE = 0.12d;
        private const string DEFAULT_PREVIOUS_BREAK_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_PREVIOUS_HIT_TYPE = null;
        private const string DEFAULT_NEXT_BREAK_TYPE = null;
        private const string DEFAULT_NEXT_HIT_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";
        private const double DEFAULT_VALUE = 21.04;



        private TrendRangeDto getDefaultTrendRangeDto()
        {
            return new TrendRangeDto()
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID,
                StartIndex = DEFAULT_START_INDEX,
                EndIndex = DEFAULT_END_INDEX,
                QuotationsCounter = DEFAULT_QUOTATIONS_COUNTER,
                TotalDistance = DEFAULT_TOTAL_DISTANCE,
                PreviousBreakGuid = DEFAULT_PREVIOUS_BREAK_GUID,
                PreviousHitGuid = DEFAULT_PREVIOUS_HIT_TYPE,
                NextBreakGuid = DEFAULT_NEXT_BREAK_TYPE,
                NextHitGuid = DEFAULT_NEXT_HIT_GUID
            };
        }


        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = new TrendRangeDto()
            {
                Id = 1,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID + 1,
                StartIndex = DEFAULT_START_INDEX + 1,
                EndIndex = DEFAULT_END_INDEX + 1,
                QuotationsCounter = DEFAULT_QUOTATIONS_COUNTER + 10,
                TotalDistance = DEFAULT_TOTAL_DISTANCE + 1,
                PreviousBreakGuid = System.Guid.NewGuid().ToString(),
                PreviousHitGuid = System.Guid.NewGuid().ToString(),
                NextBreakGuid = System.Guid.NewGuid().ToString(),
                NextHitGuid = System.Guid.NewGuid().ToString()
            };

            //Act
            comparedItem.CopyProperties(baseItem);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        #endregion COPY_PROPERTIES



        #region EQUALS

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = new { Id = 1 };

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.Guid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTrendlineIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.TrendlineId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfStartIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.StartIndex += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEndIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.EndIndex += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedEndIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseEndIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            baseItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfQuotationsCounterIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.QuotationsCounter = comparedItem.QuotationsCounter + 2;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTotalDistanceIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.TotalDistance += 0.1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfPreviousHitGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.PreviousHitGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedPreviousHitGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.PreviousHitGuid = null;
            baseItem.PreviousHitGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBasePreviousHitGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            baseItem.PreviousHitGuid = null;
            comparedItem.PreviousHitGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPreviousBreakGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.PreviousBreakGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedPreviousBreakGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.PreviousBreakGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBasePreviousEventGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            baseItem.PreviousBreakGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfNextHitGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.NextHitGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedNextHitGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.NextHitGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseNextHitGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            baseItem.NextHitGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfNextBreakGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.NextBreakGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedNextBreakGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            comparedItem.NextBreakGuid = null;
            baseItem.NextBreakGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseNextBreakGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            baseItem.NextBreakGuid = null;
            comparedItem.NextBreakGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRangeDto();
            var comparedItem = getDefaultTrendRangeDto();

            //Act
            baseItem.Value += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        #endregion EQUALS


    }

}
