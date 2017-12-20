using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;
using Stock.Core;
using Stock.Utils;
using System.Collections.Generic;
using System.Linq;
using Stock_UnitTest.Helpers;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class TrendlineUnitTests
    {

        private UTFactory utf = new UTFactory();


        #region CONSTRUCTOR

        [TestMethod]
        public void Constructor_newInstance_hasProperParameters()
        {

            //Act.
            var trendline = utf.getDefaultTrendline();

            //Assert.
            Assert.AreEqual(UTDefaulter.DEFAULT_ID, trendline.Id);
            Assert.AreEqual(UTDefaulter.DEFAULT_ASSET_ID, trendline.AssetId);
            Assert.AreEqual(UTDefaulter.DEFAULT_TIMEFRAME_ID, trendline.TimeframeId);
            Assert.AreEqual(UTDefaulter.DEFAULT_SIMULATION_ID, trendline.SimulationId);
            Assert.AreEqual(UTDefaulter.DEFAULT_START_INDEX, trendline.StartIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_START_LEVEL, trendline.StartLevel);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_INDEX, trendline.FootholdIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_LEVEL, trendline.FootholdLevel);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_SLAVE_INDEX, trendline.FootholdSlaveIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_IS_PEAK, trendline.FootholdIsPeak);
            Assert.AreEqual(UTDefaulter.DEFAULT_VALUE, trendline.Value);
            Assert.AreEqual(UTDefaulter.DEFAULT_INITIAL_IS_PEAK, trendline.InitialIsPeak);
            Assert.AreEqual(UTDefaulter.DEFAULT_LAST_UPDATE_INDEX, trendline.LastUpdateIndex);

        }

        [TestMethod]
        public void Constructor_fromDto_hasCorrectProperties()
        {

            //Act.
            var trendlineDto = new TrendlineDto() { 
                Id = UTDefaulter.DEFAULT_ID, 
                AssetId = UTDefaulter.DEFAULT_ASSET_ID, 
                TimeframeId = UTDefaulter.DEFAULT_TIMEFRAME_ID, 
                SimulationId = UTDefaulter.DEFAULT_SIMULATION_ID, 
                StartIndex = UTDefaulter.DEFAULT_START_INDEX, 
                StartLevel = UTDefaulter.DEFAULT_START_LEVEL, 
                FootholdIndex = UTDefaulter.DEFAULT_FOOTHOLD_INDEX, 
                FootholdLevel = UTDefaulter.DEFAULT_FOOTHOLD_LEVEL,
                FootholdSlaveIndex = UTDefaulter.DEFAULT_FOOTHOLD_SLAVE_INDEX,
                FootholdIsPeak = UTDefaulter.DEFAULT_FOOTHOLD_IS_PEAK,
                EndIndex = null,
                Value = UTDefaulter.DEFAULT_VALUE,
                LastUpdateIndex = UTDefaulter.DEFAULT_LAST_UPDATE_INDEX
            };

            var trendline = new Trendline(trendlineDto);

            //Assert.
            Assert.AreEqual(UTDefaulter.DEFAULT_ID, trendline.Id);
            Assert.AreEqual(UTDefaulter.DEFAULT_ASSET_ID, trendline.AssetId);
            Assert.AreEqual(UTDefaulter.DEFAULT_TIMEFRAME_ID, trendline.TimeframeId);
            Assert.AreEqual(UTDefaulter.DEFAULT_SIMULATION_ID, trendline.SimulationId);
            Assert.AreEqual(UTDefaulter.DEFAULT_START_INDEX, trendline.StartIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_START_LEVEL, trendline.StartLevel);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_INDEX, trendline.FootholdIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_LEVEL, trendline.FootholdLevel);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_SLAVE_INDEX, trendlineDto.FootholdSlaveIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_IS_PEAK, trendlineDto.FootholdIsPeak);
            Assert.AreEqual(UTDefaulter.DEFAULT_VALUE, trendline.Value);
            Assert.IsNull(trendline.EndIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_LAST_UPDATE_INDEX, trendline.LastUpdateIndex);

        }

        #endregion CONSTRUCTOR



        #region TO_DTO


        [TestMethod]
        public void ToDto_returnProperDto()
        {

            //Act

            var trendline = utf.getDefaultTrendline();
            var trendlineDto = trendline.ToDto();

            //Assert.
            Assert.AreEqual(UTDefaulter.DEFAULT_ID, trendlineDto.Id);
            Assert.AreEqual(UTDefaulter.DEFAULT_ASSET_ID, trendlineDto.AssetId);
            Assert.AreEqual(UTDefaulter.DEFAULT_TIMEFRAME_ID, trendlineDto.TimeframeId);
            Assert.AreEqual(UTDefaulter.DEFAULT_SIMULATION_ID, trendlineDto.SimulationId);
            Assert.AreEqual(UTDefaulter.DEFAULT_START_INDEX, trendlineDto.StartIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_START_LEVEL, trendlineDto.StartLevel);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_INDEX, trendlineDto.FootholdIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_LEVEL, trendlineDto.FootholdLevel);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_SLAVE_INDEX, trendlineDto.FootholdSlaveIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_FOOTHOLD_IS_PEAK, trendlineDto.FootholdIsPeak);
            Assert.AreEqual(UTDefaulter.DEFAULT_VALUE, trendlineDto.Value);
            Assert.IsNull(trendlineDto.EndIndex);
            Assert.AreEqual(UTDefaulter.DEFAULT_LAST_UPDATE_INDEX, trendlineDto.LastUpdateIndex);

        }


        #endregion TO_DTO



        #region EQUALS


        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
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
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIdIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.Id++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.AssetId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTimeframeIdIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.TimeframeId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfSimulationIdIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.SimulationId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfStartIndexIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.StartIndex = comparedItem.StartIndex + 2;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        public void Equals_ReturnsFalse_IfStartLevelIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.StartLevel += 0.1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        public void Equals_ReturnsFalse_IfFootholdIndexIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.FootholdIndex = comparedItem.FootholdIndex++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        public void Equals_ReturnsFalse_IfFootholdLevelIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.FootholdLevel += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.Value += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        public void Equals_ReturnsFalse_IfLastUpdateIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.LastUpdateIndex = comparedItem.LastUpdateIndex + 5;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        #endregion EQUALS



        [TestMethod]
        public void Equals_ReturnsTrue_IfEndIndexIsEqual()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            baseItem.EndIndex = 100;
            comparedItem.EndIndex = 100;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEndIndexIsDifferent()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            baseItem.EndIndex = 50;
            comparedItem.EndIndex = 100;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseItemHasEndIndexNull()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            baseItem.EndIndex = null;
            comparedItem.EndIndex = 100;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedItemHasEndIndexNull()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            baseItem.EndIndex = 100;
            comparedItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothItemHasEndIndexNull()
        {

            //Arrange
            var baseItem = utf.getDefaultTrendline();
            var comparedItem = utf.getDefaultTrendline();

            //Act
            comparedItem.EndIndex = null;
            baseItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }
    }
}
