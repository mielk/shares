using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Moq;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class AssetTimeframeUnitTest
    {

        private const int DEFAULT_ASSET_ID = 1;
        private const string DEFAULT_ASSET_SYMBOL = "EURUSD";
        private const int DEFAULT_ASSET_MARKET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const string DEFAULT_TIMEFRAME_NAME = "M5";
        private const TimeframeUnit DEFAULT_TIMEFRAME_UNIT_TYPE = TimeframeUnit.Minutes;
        private const int DEFAULT_TIMEFRAME_UNIT_COUNT = 5;


        #region INFRASTRUCTURE

        private Asset defaultAsset()
        {
            Market market = new Market(DEFAULT_ASSET_MARKET_ID, "Forex", "FX");
            return new Asset(DEFAULT_ASSET_ID, DEFAULT_ASSET_SYMBOL, market);
        }

        private Timeframe defaultTimeframe()
        {
            return new Timeframe(DEFAULT_TIMEFRAME_ID, DEFAULT_TIMEFRAME_NAME, DEFAULT_TIMEFRAME_UNIT_TYPE, DEFAULT_TIMEFRAME_UNIT_COUNT); 
        }

        #endregion INFRASTRUCTURE


        #region IS.VALID

        [TestMethod]
        public void IsValid_ReturnsTrue_IfBothAssetAndTimeframeAreSet()
        {

            //Arrange
            Asset asset = defaultAsset();
            Timeframe timeframe = defaultTimeframe();
            AssetTimeframe assetTimeframe = new AssetTimeframe(asset, timeframe);

            //Act
            bool isValid = assetTimeframe.IsValid();

            //Assert
            Assert.IsTrue(isValid);

        }

        [TestMethod]
        public void IsValid_ReturnsFalse_IfAssetIsNull()
        {

            //Arrange
            Asset asset = null;
            Timeframe timeframe = defaultTimeframe();
            AssetTimeframe assetTimeframe = new AssetTimeframe(asset, timeframe);

            //Act
            bool isValid = assetTimeframe.IsValid();

            //Assert
            Assert.IsFalse(isValid);

        }

        [TestMethod]
        public void IsValid_ReturnsFalse_IfTimeframeIsNull()
        {

            //Arrange
            Asset asset = defaultAsset();
            Timeframe timeframe = null;
            AssetTimeframe assetTimeframe = new AssetTimeframe(asset, timeframe);

            //Act
            bool isValid = assetTimeframe.IsValid();

            //Assert
            Assert.IsFalse(isValid);

        }

        #endregion IS.VALID


        #region GET.SYMBOL

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Asset or timeframe is null")]
        public void GetSymbol_ThrowsException_IfAssetIsNull()
        {

            //Arrange
            Asset asset = null;
            Timeframe timeframe = defaultTimeframe();
            AssetTimeframe assetTimeframe = new AssetTimeframe(asset, timeframe);

            //Act
            string symbol = assetTimeframe.GetSymbol();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Asset or timeframe is null")]
        public void GetSymbol_ThrowsException_IfTimeframeIsNull()
        {

            //Arrange
            Asset asset = defaultAsset();
            Timeframe timeframe = null;
            AssetTimeframe assetTimeframe = new AssetTimeframe(asset, timeframe);

            //Act
            string symbol = assetTimeframe.GetSymbol();

        }

        [TestMethod]
        public void GetSymbol_ReturnsProperSymbol_IfAssetAndTimeframeAreSet()
        {

            //Arrange
            Asset asset = defaultAsset();
            Timeframe timeframe = defaultTimeframe();
            AssetTimeframe assetTimeframe = new AssetTimeframe(asset, timeframe);

            //Act
            string actualSymbol = assetTimeframe.GetSymbol();

            //Assert
            string expectedSymbol = "EURUSD_M5";
            Assert.AreEqual(expectedSymbol, actualSymbol);

        }

        #endregion GET.SYMBOL


    }
}
