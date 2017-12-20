using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;
using Stock.Core;
using Stock.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class TrendRangeUnitTests
    {

        private const string DEFAULT_GUID = "74017f2d-9dfe-494e-bfa0-93c09418cfb7";
        private const int DEFAULT_ID = 1;
        private const int DEFAULT_TRENDLINE_ID = 1;
        private const int DEFAULT_START_INDEX = 15;
        private int? DEFAULT_END_INDEX = null;
        private const int DEFAULT_QUOTATIONS_COUNTER = 19;
        private const double DEFAULT_TOTAL_DISTANCE = 2.14;
        private const string DEFAULT_PREVIOUS_BREAK_GUID = null;
        private const string DEFAULT_PREVIOUS_HIT_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_NEXT_BREAK_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";
        private const string DEFAULT_NEXT_HIT_GUID = null;
        private const double DEFAULT_VALUE = 21.04;



        #region CONSTRUCTOR

        [TestMethod]
        public void Constructor_newInstance_hasProperParameters()
        {

            //Act.
            var trendRange = new TrendRange(DEFAULT_TRENDLINE_ID, DEFAULT_START_INDEX);

            //Assert.
            Assert.IsNotNull(trendRange.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendRange.TrendlineId);
            Assert.AreEqual(DEFAULT_START_INDEX, trendRange.StartIndex);

        }

        [TestMethod]
        public void Constructor_fromDto_hasCorrectProperties()
        {

            //Act.
            var TrendRangeDto = new TrendRangeDto()
            { 
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID,
                StartIndex = DEFAULT_START_INDEX,
                EndIndex = DEFAULT_END_INDEX,
                QuotationsCounter = DEFAULT_QUOTATIONS_COUNTER,
                TotalDistance = DEFAULT_TOTAL_DISTANCE,
                Value = DEFAULT_VALUE,
                PreviousBreakGuid = DEFAULT_PREVIOUS_BREAK_GUID,
                PreviousHitGuid = DEFAULT_PREVIOUS_HIT_GUID,
                NextBreakGuid = DEFAULT_NEXT_BREAK_GUID,
                NextHitGuid = DEFAULT_NEXT_HIT_GUID
            };

            var trendRange = TrendRange.FromDto(TrendRangeDto);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, trendRange.Id);
            Assert.AreEqual(DEFAULT_GUID, trendRange.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendRange.TrendlineId);
            Assert.AreEqual(DEFAULT_START_INDEX, trendRange.StartIndex);
            Assert.IsTrue(trendRange.EndIndex.IsEqual(DEFAULT_END_INDEX));
            Assert.AreEqual(DEFAULT_VALUE, trendRange.Value);
            Assert.AreEqual(DEFAULT_QUOTATIONS_COUNTER, trendRange.QuotationsCounter);
            Assert.AreEqual(DEFAULT_TOTAL_DISTANCE, trendRange.TotalDistance);
            Assert.IsTrue(trendRange.PreviousBreakGuid.Compare(DEFAULT_PREVIOUS_BREAK_GUID));
            Assert.IsTrue(trendRange.PreviousHitGuid.Compare(DEFAULT_PREVIOUS_HIT_GUID));
            Assert.IsTrue(trendRange.NextBreakGuid.Compare(DEFAULT_NEXT_BREAK_GUID));
            Assert.IsTrue(trendRange.NextHitGuid.Compare(DEFAULT_NEXT_HIT_GUID));
        }

        #endregion CONSTRUCTOR



        #region TO_DTO

        [TestMethod]
        public void ToDto_returnProperDto()
        {

            //Act
            var trendRange = new TrendRange(DEFAULT_TRENDLINE_ID, DEFAULT_START_INDEX)
            {
                Guid = DEFAULT_GUID,
                Id = DEFAULT_ID,
                EndIndex = DEFAULT_END_INDEX,
                QuotationsCounter = DEFAULT_QUOTATIONS_COUNTER,
                TotalDistance = DEFAULT_TOTAL_DISTANCE,
                PreviousBreakGuid = DEFAULT_PREVIOUS_BREAK_GUID,
                PreviousHitGuid = DEFAULT_PREVIOUS_HIT_GUID,
                NextBreakGuid = DEFAULT_NEXT_BREAK_GUID,
                NextHitGuid = DEFAULT_NEXT_HIT_GUID,
                Value = DEFAULT_VALUE
            };
            var guid = trendRange.Guid;
            var trendRangeDto = trendRange.ToDto();

            //Assert.
            Assert.AreEqual(DEFAULT_ID, trendRange.Id);
            Assert.AreEqual(DEFAULT_GUID, trendRange.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendRange.TrendlineId);
            Assert.AreEqual(DEFAULT_START_INDEX, trendRange.StartIndex);
            Assert.IsTrue(trendRange.EndIndex.IsEqual(DEFAULT_END_INDEX));
            Assert.AreEqual(DEFAULT_VALUE, trendRange.Value);
            Assert.AreEqual(DEFAULT_QUOTATIONS_COUNTER, trendRange.QuotationsCounter);
            Assert.AreEqual(DEFAULT_TOTAL_DISTANCE, trendRange.TotalDistance);
            Assert.IsTrue(trendRange.PreviousBreakGuid.Compare(DEFAULT_PREVIOUS_BREAK_GUID));
            Assert.IsTrue(trendRange.PreviousHitGuid.Compare(DEFAULT_PREVIOUS_HIT_GUID));
            Assert.IsTrue(trendRange.NextBreakGuid.Compare(DEFAULT_NEXT_BREAK_GUID));
            Assert.IsTrue(trendRange.NextHitGuid.Compare(DEFAULT_NEXT_HIT_GUID));
        }

        #endregion TO_DTO



        #region EQUALS


        private TrendRange getDefaultTrendRange()
        {
            var trendRange = new TrendRange(DEFAULT_TRENDLINE_ID, DEFAULT_START_INDEX)
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                EndIndex = DEFAULT_END_INDEX,
                PreviousBreakGuid = DEFAULT_PREVIOUS_BREAK_GUID,
                PreviousHitGuid = DEFAULT_PREVIOUS_HIT_GUID,
                NextBreakGuid = DEFAULT_NEXT_BREAK_GUID,
                NextHitGuid = DEFAULT_NEXT_HIT_GUID,
                Value = DEFAULT_VALUE
            };
            return trendRange;
        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.Guid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.Id++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTrendlineIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.EndIndex = 1;
            baseItem.EndIndex = 2;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedEndIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.EndIndex = null;
            baseItem.EndIndex = 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseEndIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            baseItem.EndIndex = 1;
            comparedItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfQuotationsCounterIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.PreviousHitGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBasePreviousHitGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            baseItem.PreviousHitGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPreviousBreakGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.PreviousBreakGuid = null;
            baseItem.PreviousBreakGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBasePreviousEventGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            baseItem.PreviousBreakGuid = null;
            comparedItem.PreviousBreakGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfNextHitGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.NextHitGuid = null;
            baseItem.NextHitGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseNextHitGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.NextHitGuid = System.Guid.NewGuid().ToString();
            baseItem.NextHitGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfNextBreakGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

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
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            comparedItem.NextBreakGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseNextBreakGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            baseItem.NextBreakGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendRange();
            var comparedItem = getDefaultTrendRange();

            //Act
            baseItem.Value += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }



        #endregion EQUALS


    }
}
