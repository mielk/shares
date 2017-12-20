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
    public class TrendHitUnitTests
    {
        private const string DEFAULT_GUID = "74017f2d-9dfe-494e-bfa0-93c09418cfb7";
        private const int DEFAULT_ID = 1;
        private const int DEFAULT_TRENDLINE_ID = 1;
        private const int DEFAULT_INDEX_NUMBER = 2;
        private const int DEFAULT_EXTREMUM_TYPE = 2;
        private const double DEFAULT_DISTANCE_TO_LINE = 0.134;
        private const double DEFAULT_VALUE = 1.234;
        private const string DEFAULT_PREVIOUS_RANGE_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_NEXT_RANGE_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";




        #region CONSTRUCTOR

        [TestMethod]
        public void Constructor_newInstance_hasProperParameters()
        {

            //Act.
            var trendHit = new TrendHit(DEFAULT_TRENDLINE_ID, DEFAULT_INDEX_NUMBER, DEFAULT_EXTREMUM_TYPE);

            //Assert.
            Assert.IsNotNull(trendHit.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendHit.TrendlineId);
            Assert.AreEqual(DEFAULT_INDEX_NUMBER, trendHit.IndexNumber);
            Assert.AreEqual(DEFAULT_EXTREMUM_TYPE, (int)trendHit.ExtremumType);

        }

        [TestMethod]
        public void Constructor_fromDto_hasCorrectProperties()
        {

            //Act.
            var trendHitDto = new TrendHitDto()
            { 
                Id = DEFAULT_ID,
                TrendlineId = DEFAULT_TRENDLINE_ID,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                ExtremumType = DEFAULT_EXTREMUM_TYPE,
                Value = DEFAULT_VALUE,
                DistanceToLine = DEFAULT_DISTANCE_TO_LINE,
                Guid = DEFAULT_GUID,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
            };

            var trendHit = TrendHit.FromDto(trendHitDto);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, trendHit.Id);
            Assert.AreEqual(DEFAULT_GUID, trendHit.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendHit.TrendlineId);
            Assert.AreEqual(DEFAULT_INDEX_NUMBER, trendHit.IndexNumber);
            Assert.AreEqual(DEFAULT_EXTREMUM_TYPE, (int)trendHit.ExtremumType);
            Assert.AreEqual(DEFAULT_VALUE, trendHit.Value);
            Assert.AreEqual(DEFAULT_DISTANCE_TO_LINE, trendHit.DistanceToLine);
            Assert.AreEqual(DEFAULT_PREVIOUS_RANGE_GUID, trendHit.PreviousRangeGuid);
            Assert.AreEqual(DEFAULT_NEXT_RANGE_GUID, trendHit.NextRangeGuid);

        }

        #endregion CONSTRUCTOR



        #region TO_DTO

        [TestMethod]
        public void ToDto_returnProperDto()
        {

            //Act
            var trendHit = new TrendHit(DEFAULT_TRENDLINE_ID, DEFAULT_INDEX_NUMBER, DEFAULT_EXTREMUM_TYPE)
            {
                Id = DEFAULT_ID,
                Value = DEFAULT_VALUE,
                DistanceToLine = DEFAULT_DISTANCE_TO_LINE,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = null
            };
            var guid = trendHit.Guid;
            var trendHitDto = trendHit.ToDto();

            //Assert.
            Assert.AreEqual(DEFAULT_ID, trendHitDto.Id);
            Assert.AreEqual(guid, trendHitDto.Guid);
            Assert.AreEqual(DEFAULT_TRENDLINE_ID, trendHitDto.TrendlineId);
            Assert.AreEqual(DEFAULT_INDEX_NUMBER, trendHitDto.IndexNumber);
            Assert.AreEqual(DEFAULT_EXTREMUM_TYPE, trendHitDto.ExtremumType);
            Assert.AreEqual(DEFAULT_VALUE, trendHitDto.Value);
            Assert.AreEqual(DEFAULT_DISTANCE_TO_LINE, trendHitDto.DistanceToLine);
            Assert.AreEqual(DEFAULT_PREVIOUS_RANGE_GUID, trendHitDto.PreviousRangeGuid);
            Assert.IsNull(trendHitDto.NextRangeGuid);
        }

        #endregion TO_DTO



        #region EQUALS


        private TrendHit getDefaultTrendline()
        {
            var trendHit = new TrendHit(DEFAULT_TRENDLINE_ID, DEFAULT_INDEX_NUMBER, DEFAULT_EXTREMUM_TYPE)
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                Value = DEFAULT_VALUE,
                DistanceToLine = DEFAULT_DISTANCE_TO_LINE,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
            };
            return trendHit;

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultTrendline();
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
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfGuidIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

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
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

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
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

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
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

            //Act
            comparedItem.IndexNumber += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfExtremumTypeIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

            //Act
            comparedItem.ExtremumType = (ExtremumType)(DEFAULT_EXTREMUM_TYPE + 1);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

            //Act
            comparedItem.Value = comparedItem.Value + 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDistanceToLineLevelIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

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
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

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
            var baseItem = getDefaultTrendline();
            var comparedItem = getDefaultTrendline();

            //Act
            comparedItem.NextRangeGuid = System.Guid.NewGuid().ToString();
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        #endregion EQUALS


    }
}
