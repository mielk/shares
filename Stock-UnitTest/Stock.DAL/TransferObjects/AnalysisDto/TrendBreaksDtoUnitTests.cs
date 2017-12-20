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
    public class TrendBreaksDtoUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_GUID = "74017f2d-9dfe-494e-bfa0-93c09418cfb7";
        private const int DEFAULT_TRENDLINE_ID = 1;
        private const int DEFAULT_INDEX_NUMBER = 2;
        private const string DEFAULT_PREVIOUS_RANGE_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_NEXT_RANGE_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";

        private TrendBreakDto getDefaultTrendBreakDto()
        {
            return new TrendBreakDto()
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
            };
        }


        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = new TrendBreakDto()
            {
                Id = 1,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID + 1,
                IndexNumber = DEFAULT_INDEX_NUMBER + 1,
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
            var baseItem = getDefaultTrendBreakDto();
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
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

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
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

            //Act
            comparedItem.TrendlineId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIndexNumberIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

            //Act
            comparedItem.IndexNumber += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPreviousRangeGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

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
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

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
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

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
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

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
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

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
            var baseItem = getDefaultTrendBreakDto();
            var comparedItem = getDefaultTrendBreakDto();

            //Act
            baseItem.NextRangeGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        #endregion EQUALS


    }

}
