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
    public class AnalysisTimestampUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const int DEFAULT_SIMULATION_ID = 1;
        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const int DEFAULT_ANALYSIS_TYPE_ID = 1;
        private DateTime? DEFAULT_LAST_DATE = new DateTime(2017, 4, 21, 16, 0, 0);
        private int? DEFAULT_LAST_INDEX = 20;



        #region CONSTRUCTOR

        [TestMethod]
        public void Constructor_newInstance_hasProperIdNameAndCurrencies()
        {

            //Act.
            var analysisTimestamp = new AnalysisTimestamp() { 
                Id = DEFAULT_ID, 
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedItem = DEFAULT_LAST_DATE,
                LastAnalysedIndex = DEFAULT_LAST_INDEX
            };

            //Assert.
            Assert.AreEqual(DEFAULT_ID, analysisTimestamp.Id);
            Assert.AreEqual(DEFAULT_SIMULATION_ID, analysisTimestamp.SimulationId);
            Assert.AreEqual(DEFAULT_ASSET_ID, analysisTimestamp.AssetId);
            Assert.AreEqual(DEFAULT_TIMEFRAME_ID, analysisTimestamp.TimeframeId);
            Assert.AreEqual(DEFAULT_ANALYSIS_TYPE_ID, analysisTimestamp.AnalysisTypeId);
            Assert.IsTrue(analysisTimestamp.LastAnalysedItem.IsEqual(DEFAULT_LAST_DATE));
            Assert.AreEqual(DEFAULT_LAST_INDEX, (int) analysisTimestamp.LastAnalysedIndex);

        }

        [TestMethod]
        public void Constructor_fromDto_hasCorrectProperties()
        {

            //Act.
            AnalysisTimestampDto dto = new AnalysisTimestampDto
            {
                Id = DEFAULT_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedItem = DEFAULT_LAST_DATE,
                LastAnalysedIndex = DEFAULT_LAST_INDEX
            };
            var analysisTimestamp = AnalysisTimestamp.FromDto(dto);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, analysisTimestamp.Id);
            Assert.AreEqual(DEFAULT_SIMULATION_ID, analysisTimestamp.SimulationId);
            Assert.AreEqual(DEFAULT_ASSET_ID, analysisTimestamp.AssetId);
            Assert.AreEqual(DEFAULT_TIMEFRAME_ID, analysisTimestamp.TimeframeId);
            Assert.AreEqual(DEFAULT_ANALYSIS_TYPE_ID, analysisTimestamp.AnalysisTypeId);
            Assert.IsTrue(analysisTimestamp.LastAnalysedItem.IsEqual(DEFAULT_LAST_DATE));
            Assert.AreEqual(DEFAULT_LAST_INDEX, (int)analysisTimestamp.LastAnalysedIndex);

        }

        #endregion CONSTRUCTOR



        #region TO_DTO

        [TestMethod]
        public void ToDto_returnProperDto()
        {

            //Act.
            AnalysisTimestamp analysisTimestamp = new AnalysisTimestamp
            {
                Id = DEFAULT_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedItem = DEFAULT_LAST_DATE,
                LastAnalysedIndex = DEFAULT_LAST_INDEX
            };
            var analysisTimestampDto = analysisTimestamp.ToDto();

            //Assert.
            Assert.AreEqual(DEFAULT_ID, analysisTimestampDto.Id);
            Assert.AreEqual(DEFAULT_SIMULATION_ID, analysisTimestampDto.SimulationId);
            Assert.AreEqual(DEFAULT_ASSET_ID, analysisTimestampDto.AssetId);
            Assert.AreEqual(DEFAULT_TIMEFRAME_ID, analysisTimestampDto.TimeframeId);
            Assert.AreEqual(DEFAULT_ANALYSIS_TYPE_ID, analysisTimestampDto.AnalysisTypeId);
            Assert.IsTrue(analysisTimestampDto.LastAnalysedItem.IsEqual(DEFAULT_LAST_DATE));
            Assert.AreEqual(DEFAULT_LAST_INDEX, (int)analysisTimestampDto.LastAnalysedIndex);

        }

        #endregion TO_DTO



        #region EQUALS

        private AnalysisTimestamp getDefaultAnalysisTimestamp()
        {
            return new AnalysisTimestamp()
            {
                Id = DEFAULT_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedItem = DEFAULT_LAST_DATE,
                LastAnalysedIndex = DEFAULT_LAST_INDEX
            };
        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
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
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfSimulationIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.SimulationId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.AssetId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTimeframeIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.TimeframeId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAnalysisTypeIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.AnalysisTypeId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLastItemDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.LastAnalysedItem = ((DateTime)comparedItem.LastAnalysedItem).AddMinutes(30);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseLastItemDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            baseItem.LastAnalysedItem = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedLastItemDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.LastAnalysedItem = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothLastItemDatesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            baseItem.LastAnalysedItem = null;
            comparedItem.LastAnalysedItem = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfLastItemIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.LastAnalysedIndex = comparedItem.LastAnalysedIndex + 5;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseLastItemIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            baseItem.LastAnalysedIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedLastItemIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            comparedItem.LastAnalysedIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothLastItemIndexesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestamp();
            var comparedItem = getDefaultAnalysisTimestamp();

            //Act
            baseItem.LastAnalysedIndex = null;
            comparedItem.LastAnalysedIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }



        #endregion EQUALS



    }
}
