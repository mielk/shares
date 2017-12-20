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
    public class TrendBreakUnitTests
    {

        private const string DEFAULT_GUID = "74017f2d-9dfe-494e-bfa0-93c09418cfb7";
        private const int DEFAULT_ID = 1;
        private const int DEFAULT_TRENDLINE_ID = 1;
        private const int DEFAULT_INDEX_NUMBER = 2;
        private const string DEFAULT_PREVIOUS_RANGE_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_NEXT_RANGE_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";



        #region CONSTRUCTOR

        [TestMethod]
        public void Constructor_newInstance_hasProperParameters()
        {

            //Act.
            var trendRange = new TrendBreak(DEFAULT_TRENDLINE_ID, DEFAULT_INDEX_NUMBER);

            //Assert.
            Assert.IsNotNull(trendRange.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendRange.TrendlineId);
            Assert.AreEqual(DEFAULT_INDEX_NUMBER, trendRange.IndexNumber);

        }

        [TestMethod]
        public void Constructor_fromDto_hasCorrectProperties()
        {

            //Act.
            var TrendBreakDto = new TrendBreakDto()
            {
                Id = DEFAULT_ID,
                TrendlineId = DEFAULT_TRENDLINE_ID,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                Guid = DEFAULT_GUID,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
            };

            var trendRange = TrendBreak.FromDto(TrendBreakDto);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, trendRange.Id);
            Assert.AreEqual(DEFAULT_GUID, trendRange.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendRange.TrendlineId);
            Assert.AreEqual(DEFAULT_INDEX_NUMBER, trendRange.IndexNumber);
            Assert.AreEqual(DEFAULT_PREVIOUS_RANGE_GUID, trendRange.PreviousRangeGuid);
            Assert.AreEqual(DEFAULT_NEXT_RANGE_GUID, trendRange.NextRangeGuid);

        }

        #endregion CONSTRUCTOR



        #region TO_DTO

        [TestMethod]
        public void ToDto_returnProperDto()
        {

            //Act
            var TrendBreak = new TrendBreak(DEFAULT_TRENDLINE_ID, DEFAULT_INDEX_NUMBER)
            {
                Id = DEFAULT_ID,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = null
            };
            var guid = TrendBreak.Guid;
            var TrendBreakDto = TrendBreak.ToDto();

            //Assert.
            Assert.AreEqual(DEFAULT_ID, TrendBreakDto.Id);
            Assert.AreEqual(guid, TrendBreakDto.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, TrendBreakDto.TrendlineId);
            Assert.AreEqual(DEFAULT_INDEX_NUMBER, TrendBreakDto.IndexNumber);
            Assert.AreEqual(DEFAULT_PREVIOUS_RANGE_GUID, TrendBreakDto.PreviousRangeGuid);
            Assert.IsNull(TrendBreakDto.NextRangeGuid);
        }

        #endregion TO_DTO



        #region EQUALS


        private TrendBreak getDefaultTrendBreak()
        {
            var TrendBreak = new TrendBreak(DEFAULT_TRENDLINE_ID, DEFAULT_INDEX_NUMBER)
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
            };
            return TrendBreak;

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultTrendBreak();
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
            var baseItem = getDefaultTrendBreak();
            var comparedItem = getDefaultTrendBreak();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendBreak();
            var comparedItem = getDefaultTrendBreak();

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
            var baseItem = getDefaultTrendBreak();
            var comparedItem = getDefaultTrendBreak();

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
            var baseItem = getDefaultTrendBreak();
            var comparedItem = getDefaultTrendBreak();

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
            var baseItem = getDefaultTrendBreak();
            var comparedItem = getDefaultTrendBreak();

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
            var baseItem = getDefaultTrendBreak();
            var comparedItem = getDefaultTrendBreak();

            //Act
            comparedItem.PreviousRangeGuid = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfNextRangeGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendBreak();
            var comparedItem = getDefaultTrendBreak();

            //Act
            comparedItem.NextRangeGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        #endregion EQUALS


    }
}
