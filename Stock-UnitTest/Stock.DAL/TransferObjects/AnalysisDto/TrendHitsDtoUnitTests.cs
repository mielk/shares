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
    public class TrendHitsDtoUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_GUID = "74017f2d-9dfe-494e-bfa0-93c09418cfb7";
        private const int DEFAULT_TRENDLINE_ID = 1; 
        private const int DEFAULT_INDEX_NUMBER = 2;
        private const int DEFAULT_EXTREMUM_TYPE = 2;
        private const double DEFAULT_DISTANCE_TO_LINE = 0.134;
        private const double DEFAULT_VALUE = 1.234;
        private const string DEFAULT_PREVIOUS_RANGE_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_NEXT_RANGE_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";

        private TrendHitDto getDefaultTrendHitDto()
        {
            return new TrendHitDto()
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID, 
                TrendlineId = DEFAULT_TRENDLINE_ID,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                ExtremumType = DEFAULT_EXTREMUM_TYPE,
                Value = DEFAULT_VALUE,
                DistanceToLine = DEFAULT_DISTANCE_TO_LINE,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
            };
        }


        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = new TrendHitDto()
            {
                Id = 1,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID + 1,
                IndexNumber = DEFAULT_INDEX_NUMBER + 1,
                ExtremumType = DEFAULT_EXTREMUM_TYPE + 1,
                Value = DEFAULT_VALUE + 1,
                DistanceToLine = DEFAULT_DISTANCE_TO_LINE + 1,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
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
            var baseItem = getDefaultTrendHitDto();
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
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();
            
            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

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
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.TrendlineId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfExtremumTypeIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.ExtremumType += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIndexNumberIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.IndexNumber += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.Value = comparedItem.Value + 2;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDistanceToLineIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.DistanceToLine += 0.1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPreviousRangeGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.PreviousRangeGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedPreviousRangeGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.PreviousRangeGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBasePreviousRangeGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            baseItem.PreviousRangeGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfNextRangeGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.NextRangeGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedNextRangeGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            comparedItem.NextRangeGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseNextRangeGuidIsNull()
        {

            //Arrange
            var baseItem = getDefaultTrendHitDto();
            var comparedItem = getDefaultTrendHitDto();

            //Act
            baseItem.NextRangeGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        #endregion EQUALS


    }

}
