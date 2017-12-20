using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Stock.DAL.TransferObjects;
using System.Linq;
using Stock.Domain.Services;
using Stock.Utils;
using Moq;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class AssetUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_SYMBOL = "EURUSD";
        private const int DEFAULT_MARKET_ID = 1;


        #region INFRASTRUCTURE

        private IEnumerable<Asset> getAssetsCollection()
        {
            List<Asset> assets = new List<Asset>();
            assets.Add(new Asset(1, "EURUSD", 1));
            assets.Add(new Asset(2, "USDJPY", 1));
            assets.Add(new Asset(3, "EURJPY", 1));
            return assets;
        }

        private void injectMockedServiceToMarketClass()
        {
            Mock<IMarketService> mockedService = new Mock<IMarketService>();
            mockedService.Setup(s => s.GetMarketById(1)).Returns(new Market(1, "Forex", "FX"));
            mockedService.Setup(s => s.GetMarketById(2)).Returns(new Market(2, "unknown", "unknown"));
            Market.injectService(mockedService.Object);
        }

        #endregion INFRASTRUCTURE




        [TestMethod]
        public void constructor_newInstanceHasProperIdAndName()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var asset = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);

            //Assert
            Assert.AreEqual(DEFAULT_ID, asset.GetId());
            Assert.AreEqual(DEFAULT_SYMBOL, asset.GetSymbol());
            Assert.AreEqual(DEFAULT_MARKET_ID, asset.GetMarketId());

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void constructor_newInstanceHasNullMarket_IfGivenMarketIdDoesntExist()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var asset = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, 0);

            //Assert
            Assert.IsNull(asset.GetMarket());

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void assetFromDto_hasSamePropertiesAsDto()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            AssetDto dto = new AssetDto { Id = DEFAULT_ID, Symbol = DEFAULT_SYMBOL, MarketId = DEFAULT_MARKET_ID };
            Asset asset = Asset.FromDto(dto);

            //Assert
            Assert.AreEqual(DEFAULT_ID, asset.GetId());
            Assert.AreEqual(DEFAULT_SYMBOL, asset.GetSymbol());
            Assert.AreEqual(DEFAULT_MARKET_ID, asset.GetMarketId());

            //Clear
            Market.restoreDefaultService();

        }

        //[TestMethod]
        //public void constructor_new_instance_empty_list_of_assetTimeframes_is_created()
        //{
        //    const int ID = 2;
        //    const string NAME = "name";
        //    var asset = new Asset(ID, NAME);
        //    Assert.IsNotNull(asset.AssetTimeframes);
        //    Assert.IsInstanceOfType(asset.AssetTimeframes, typeof(IEnumerable<AssetTimeframe>));
        //}





        [TestMethod]
        public void Equals_returnFalse_forObjectOfOtherType()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var baseObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            var comparedObject = new { Id = 1, Value = 2 };

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifIdDifferent()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var baseObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            var comparedObject = new Asset(DEFAULT_ID + 1, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifSymbolDifferent()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var baseObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            var comparedObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL + "a", DEFAULT_MARKET_ID);

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifMarketDifferent()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var baseObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            var comparedObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID + 1);

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifAnyMarketIsNull()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var baseObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            var comparedObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, 0);

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnTrue_ifBothMarketsAreNullAndOtherPropertiesEqual()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var baseObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, 0);
            var comparedObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, 0);

            //Assert
            Assert.IsTrue(baseObject.Equals(comparedObject));

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnTrue_ifCurrenciesEqual()
        {

            //Arrange
            injectMockedServiceToMarketClass();

            //Act
            var baseObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            var comparedObject = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);

            //Assert
            Assert.IsTrue(baseObject.Equals(comparedObject));

            //Clear
            Market.restoreDefaultService();

        }




        [TestMethod]
        public void ById_returnsNull_ifNotExistInRepository()
        {

            //Arrange
            Mock<IAssetService> mockService = new Mock<IAssetService>();
            Asset nullAsset = null;
            mockService.Setup(s => s.GetAssetById(DEFAULT_ID)).Returns(nullAsset);
            Asset.InjectService(mockService.Object);

            //Act.
            Asset asset = Asset.ById(DEFAULT_ID);

            //Assert.
            Assert.IsNull(asset);

        }

        [TestMethod]
        public void ById_returnsExistingInstance()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetService> mockService = new Mock<IAssetService>();
            Asset expectedAsset = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            mockService.Setup(s => s.GetAssetById(DEFAULT_ID)).Returns(expectedAsset);
            Asset.InjectService(mockService.Object);

            //Act.
            Asset asset = Asset.ById(DEFAULT_ID);

            //Assert.
            Assert.AreSame(asset, expectedAsset);

            //Clear.
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void BySymbol_returnsExistingInstance()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetService> mockService = new Mock<IAssetService>();
            Asset expectedAsset = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            mockService.Setup(s => s.GetAssetBySymbol(DEFAULT_SYMBOL)).Returns(expectedAsset);
            Asset.InjectService(mockService.Object);

            //Act.
            Asset asset = Asset.BySymbol(DEFAULT_SYMBOL);

            //Assert.
            Assert.AreSame(asset, expectedAsset);

            //Clear.
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void ById_returnsExistingInstance_afterAddingNewItem()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetService> mockService = new Mock<IAssetService>();
            mockService.Setup(s => s.GetAssetById(1)).Returns(new Asset(1, "EURUSD", 1));
            mockService.Setup(s => s.GetAssetById(2)).Returns(new Asset(2, "USDJPY", 1));
            Asset.InjectService(mockService.Object);

            //Act
            Asset eurusd = Asset.ById(1);
            Asset usdjpy = Asset.ById(2);
            Asset eurusdAgain = Asset.ById(1);

            //Assert
            Assert.AreSame(eurusd, eurusdAgain);

            //Clear.
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAllAssets_returnsProperCollection()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetService> mockService = new Mock<IAssetService>();
            var expectedAssets = getAssetsCollection();
            mockService.Setup(s => s.GetAllAssets()).Returns(expectedAssets);
            Asset.InjectService(mockService.Object);

            //Act.
            var assets = Asset.GetAllAssets();

            //Assert.
            bool areEquivalent = assets.HasTheSameItems(expectedAssets);
            Assert.IsTrue(areEquivalent);

            //Clear.
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetFilteredAssets_returnsProperCollection()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetService> mockService = new Mock<IAssetService>();
            var expectedAssets = getAssetsCollection();
            mockService.Setup(s => s.GetAssets(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedAssets);
            Asset.InjectService(mockService.Object);

            //Act.
            var assets = Asset.GetAssets("a", 1);

            //Assert.
            bool areEquivalent = assets.HasTheSameItems(expectedAssets);
            Assert.IsTrue(areEquivalent);

            //Clear.
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAssetsForMarket_returnsProperCollection()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetService> mockService = new Mock<IAssetService>();
            var expectedAssets = getAssetsCollection();
            mockService.Setup(s => s.GetAssetsForMarket(It.IsAny<int>())).Returns(expectedAssets);
            Asset.InjectService(mockService.Object);

            //Act.
            var assets = Asset.GetAssetsForMarket(1);

            //Assert.
            bool areEquivalent = assets.HasTheSameItems(expectedAssets);
            Assert.IsTrue(areEquivalent);

            //Clear.
            Market.restoreDefaultService();

        }

    }

}
