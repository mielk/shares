using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Moq;
using Stock.Domain.Services;
using Stock.DAL.TransferObjects;
using System.Linq;
using Stock.Utils;
using System.Linq.Expressions;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class MarketUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_NAME = "Forex";
        private const string DEFAULT_SHORT_NAME = "FX";


        #region INFRASTRUCTURE

        private Mock<IMarketService> mockedServiceForTests()
        {
            Mock<IMarketService> mockService = new Mock<IMarketService>();
            var expectedMarkets = defaultMarketsCollection().ToArray();
            var expectedMarket = expectedMarkets.SingleOrDefault(m => m.GetId() == DEFAULT_ID);
            mockService.Setup(s => s.GetMarketById(DEFAULT_ID)).Returns(expectedMarket);
            mockService.Setup(s => s.GetMarketByName(DEFAULT_NAME)).Returns(expectedMarket);
            mockService.Setup(s => s.GetMarketBySymbol(DEFAULT_SHORT_NAME)).Returns(expectedMarket);
            mockService.Setup(s => s.GetMarkets()).Returns(expectedMarkets);
            return mockService;
        }

        private Market defaultMarket()
        {
            Market market = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            return market;
        }

        private IEnumerable<Market> defaultMarketsCollection()
        {

            List<Market> markets = new List<Market>();
            markets.Add(new Market(1, "Forex", "FX"));
            markets.Add(new Market(2, "USA", "US"));
            markets.Add(new Market(3, "United Kingdom", "UK"));
            return markets;

        }

        private IEnumerable<Asset> getAssetsCollectionForUnitTest(Market market)
        {
            List<Asset> assets = new List<Asset>();
            assets.Add(new Asset(1, "USD", market));
            assets.Add(new Asset(2, "EUR", market));
            assets.Add(new Asset(3, "JPY", market));
            return assets;
        }

        #endregion INFRASTRUCTURE


        #region EQUALS

        [TestMethod]
        public void Equals_returnFalse_forObjectOfOtherType()
        {

            //Act
            var baseObject = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            var comparedObject = new { Id = 1, Value = 2 };
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_returnFalse_ifIdDifferent()
        {

            //Act
            var baseObject = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            var comparedObject = new Market(DEFAULT_ID + 1, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_returnFalse_ifNameDifferent()
        {

            //Act
            var baseObject = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            var comparedObject = new Market(DEFAULT_ID, DEFAULT_NAME + "a", DEFAULT_SHORT_NAME);
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_returnFalse_ifSymbolDifferent()
        {

            //Act
            var baseObject = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            var comparedObject = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME + "a"); 
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_returnTrue_ifCurrenciesEqual()
        {

            //Act
            var baseObject = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            var comparedObject = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsTrue(areEqual);

        }

        #endregion EQUALS


        #region CONSTRUCTOR

        [TestMethod]
        public void Constructor_newInstance_hasProperIdNameAndCurrencies()
        {

            //Act.
            var market = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SHORT_NAME);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, market.GetId());
            Assert.AreEqual(DEFAULT_NAME, market.GetName());
            Assert.AreEqual(DEFAULT_SHORT_NAME, market.GetSymbol());

        }

        [TestMethod]
        public void Constructor_fromDto_hasCorrectProperties()
        {
            
            //Act.
            MarketDto dto = new MarketDto
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME,
                ShortName = DEFAULT_SHORT_NAME
            };
            var market = Market.FromDto(dto);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, market.GetId());
            Assert.AreEqual(DEFAULT_NAME, market.GetName());
            Assert.AreEqual(DEFAULT_SHORT_NAME, market.GetSymbol());

        }

        #endregion CONSTRUCTOR


        #region IS.OPEN
        ////[TestMethod]
        ////public void isOpen_returns_false_if_before_open_time()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_false_if_after_close_time()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_true_if_equal_to_open_time()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_true_if_equal_to_close_time()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_true_if_between_open_and_close_time()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_true_if_midnight_for_247_market()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_false_if_saturday()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_false_if_sunday()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}

        ////[TestMethod]
        ////public void isOpen_returns_false_if_newYear()
        ////{
        ////    Assert.Fail("Not implemented");
        ////}
        #endregion IS.OPEN


        #region GET.INSTANCE

        [TestMethod]
        public void ById_returnsNull_ifNotExistInRepository()
        {

            //Arrange
            Mock<IMarketService> mockService = new Mock<IMarketService>();
            Market nullMarket = null;
            int id = 123;
            mockService.Setup(c => c.GetMarketById(id)).Returns(nullMarket);
            Market.injectService(mockService.Object);

            //Act.
            Market market = Market.ById(id);

            //Assert.
            Assert.IsNull(market);

        }

        [TestMethod]
        public void ById_returnsExistingInstance()
        {

            //Arrange
            Mock<IMarketService> mockService = new Mock<IMarketService>();
            Market expectedMarket = defaultMarket();
            mockService.Setup(s => s.GetMarketById(DEFAULT_ID)).Returns(expectedMarket);
            Market.injectService(mockService.Object);

            //Act.
            Market pair = Market.ById(DEFAULT_ID);

            //Assert.
            Assert.AreSame(pair, expectedMarket);

        }

        [TestMethod]
        public void ById_returnsTheSameInstance_afterAddingNewItem()
        {

            //Arrange
            Mock<IMarketService> mockService = new Mock<IMarketService>();
            Market expectedMarket = defaultMarket();
            mockService.Setup(s => s.GetMarketById(DEFAULT_ID)).Returns(expectedMarket);
            Market.injectService(mockService.Object);

            //Act
            Market market1 = Market.ById(1);
            Market market2 = Market.ById(1);

            //Assert
            Assert.AreSame(market1, market2);

        }

        [TestMethod]
        public void ByName_returnsExistingInstance()
        {

            //Arrange
            Mock<IMarketService> mockService = new Mock<IMarketService>();
            Market expectedMarket = defaultMarket();
            mockService.Setup(s => s.GetMarketByName(DEFAULT_NAME)).Returns(expectedMarket);
            Market.injectService(mockService.Object);

            //Act.
            Market market = Market.ByName(DEFAULT_NAME);

            //Assert.
            Assert.AreSame(market, expectedMarket);

        }

        [TestMethod]
        public void BySymbol_returnsExistingInstance()
        {

            //Arrange
            Mock<IMarketService> mockService = new Mock<IMarketService>();
            Market expectedMarket = defaultMarket();
            mockService.Setup(s => s.GetMarketBySymbol(DEFAULT_SHORT_NAME)).Returns(expectedMarket);
            Market.injectService(mockService.Object);

            //Act.
            Market market = Market.BySymbol(DEFAULT_SHORT_NAME);

            //Assert.
            Assert.AreSame(market, expectedMarket);

        }

        [TestMethod]
        public void GetMarkets_returnsProperCollection()
        {

            //Arrange
            Mock<IMarketService> mockService = new Mock<IMarketService>();
            var expectedMarkets = defaultMarketsCollection();
            mockService.Setup(s => s.GetMarkets()).Returns(expectedMarkets);
            Market.injectService(mockService.Object);

            //Act.
            IEnumerable<Market> markets = Market.GetMarkets();

            //Assert.
            bool areEqual = markets.HasTheSameItems(expectedMarkets);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetMarkets_returnsSameInstanceForAlreadyExistingItems()
        {

            //Arrange
            Market.injectService(mockedServiceForTests().Object);

            //Act.
            Market market = Market.ById(DEFAULT_ID);
            IEnumerable<Market> markets = Market.GetMarkets();

            //Assert.
            Market comparedMarket = markets.SingleOrDefault(m => m.GetId() == DEFAULT_ID);
            Assert.AreSame(market, comparedMarket);

        }

        #endregion GET.INSTANCE


        #region GET.ASSETS

        [TestMethod]
        public void GetAssets_ReturnsProperCollectionOfAssets()
        {

            //Arrange
            Market market = defaultMarket();
            IEnumerable<Asset> expectedAssets = getAssetsCollectionForUnitTest(market);

            Mock<IAssetService> mockedService = new Mock<IAssetService>();
            mockedService.Setup(s => s.GetAssetsForMarket(DEFAULT_ID)).Returns(expectedAssets);
            Asset.InjectService(mockedService.Object);

            //Act
            IEnumerable<Asset> assets = market.GetAssets();

            //Assert
            bool areEqual = assets.HasTheSameItems(expectedAssets);
            Assert.IsTrue(areEqual);

            //Clear
            Asset.RestoreDefaultService();

        }

        #endregion GET.ASSETS


    }

}