using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Repositories;
using Moq;
using Stock.Domain.Entities;
using Stock.DAL.TransferObjects;
using Stock.Domain.Services;
using System.Collections.Generic;
using System.Linq;

namespace Stock_UnitTest.Stock.Domain.Services.MarketServices
{
    [TestClass]
    public class AssetServiceUnitTest
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_SYMBOL = "USD";
        private const int DEFAULT_MARKET_ID = 1;



        #region INFRASTRUCTURE

        private IAssetService testServiceInstance(Mock<IAssetRepository> mockedRepository)
        {
            IAssetService service = new AssetService(mockedRepository.Object);
            return service;
        }

        private AssetDto[] getAssetDtos()
        {
            List<AssetDto> list = new List<AssetDto>();
            list.Add(new AssetDto { Id = 1, Symbol = "EURUSD", MarketId = 1 });
            list.Add(new AssetDto { Id = 2, Symbol = "USDJPY", MarketId = 1 });
            list.Add(new AssetDto { Id = 3, Symbol = "EURJPY", MarketId = 1 });
            return list.ToArray();
        }

        private AssetDto defaultAssetDto()
        {
            return new AssetDto
            {
                Id = DEFAULT_ID,
                Symbol = DEFAULT_SYMBOL,
                MarketId = DEFAULT_MARKET_ID
            };
        }

        private Mock<IAssetRepository> mockedAssetRepositoryForUnitTests()
        {
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto dto = defaultAssetDto();
            mockedRepository.Setup(s => s.GetAssetById(DEFAULT_ID)).Returns(dto);
            mockedRepository.Setup(s => s.GetAssetBySymbol(DEFAULT_SYMBOL)).Returns(dto);
            return mockedRepository;
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
        public void GetAssetById_ReturnsAsset_IfExists()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetRepository> mockedRepository = mockedAssetRepositoryForUnitTests();
            IAssetService service = testServiceInstance(mockedRepository);

            //Act
            Asset expectedAsset = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            Asset asset = service.GetAssetById(DEFAULT_ID);

            //Assert
            Assert.AreEqual(expectedAsset, asset);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAssetById_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto dto = null;
            mockedRepository.Setup(c => c.GetAssetById(DEFAULT_ID)).Returns(dto);

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset = service.GetAssetById(DEFAULT_ID);

            //Assert
            Assert.IsNull(asset);

        }

        [TestMethod]
        public void GetAssetId_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<IAssetRepository> mockedRepository = mockedAssetRepositoryForUnitTests();
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset1 = service.GetAssetById(DEFAULT_ID);
            Asset asset2 = service.GetAssetById(DEFAULT_ID);

            //Assert
            Assert.AreSame(asset1, asset2);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAssetBySymbol_ReturnsAsset_IfExists()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetRepository> mockedRepository = mockedAssetRepositoryForUnitTests();
            IAssetService service = testServiceInstance(mockedRepository);

            //Act
            Asset expectedAsset = new Asset(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_MARKET_ID);
            Asset asset = service.GetAssetBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreEqual(expectedAsset, asset);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAssetBySymbol_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            injectMockedServiceToMarketClass();
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto dto = null;
            mockedRepository.Setup(s => s.GetAssetBySymbol(DEFAULT_SYMBOL)).Returns(dto);

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset = service.GetAssetBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.IsNull(asset);

        }

        [TestMethod]
        public void GetAssetBySymbol_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<IAssetRepository> mockedRepository = mockedAssetRepositoryForUnitTests();
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset1 = service.GetAssetBySymbol(DEFAULT_SYMBOL);
            Asset asset2 = service.GetAssetBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreSame(asset1, asset2);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAsset_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<IAssetRepository> mockedRepository = mockedAssetRepositoryForUnitTests();
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset1 = service.GetAssetById(DEFAULT_ID);
            Asset asset2 = service.GetAssetBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreSame(asset1, asset2);

            //Clear
            Market.restoreDefaultService();

        }



        [TestMethod]
        public void GetAllAssets_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto[] dtos = getAssetDtos();
            mockedRepository.Setup(r => r.GetAllAssets()).Returns(dtos);
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            IEnumerable<Asset> assets = service.GetAllAssets();

            //Assert
            Assert.AreEqual(dtos.Length, ((List<Asset>)assets).Count);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAllAssets_AlreadyExistingCurrencyInstancesAreUsed()
        {
            //Arrange
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto[] dtos = getAssetDtos();
            AssetDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetAssetById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetAllAssets()).Returns(dtos);
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset = service.GetAssetById(dto.Id);
            IEnumerable<Asset> assets = service.GetAllAssets();

            //Assert
            Asset fromResultCollection = assets.SingleOrDefault(a => a.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, asset);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void FilterAssets_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto[] dtos = getAssetDtos();
            mockedRepository.Setup(r => r.GetAssets(It.IsAny<string>(), It.IsAny<int>())).Returns(dtos);
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            IEnumerable<Asset> currencies = service.GetAssets("a", 3);

            //Assert
            Assert.AreEqual(dtos.Length, ((List<Asset>)currencies).Count);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void FilterAssets_AlreadyExistingCurrencyInstancesAreUsed()
        {
            //Arrange
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto[] dtos = getAssetDtos();
            AssetDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetAssetById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetAssets(It.IsAny<string>(), It.IsAny<int>())).Returns(dtos);
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset = service.GetAssetById(dto.Id);
            IEnumerable<Asset> assets = service.GetAssets("a", 3);

            //Assert
            Asset fromResultCollection = assets.SingleOrDefault(a => a.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, asset);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAssetsForMarket_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto[] dtos = getAssetDtos();
            mockedRepository.Setup(r => r.GetAssetsForMarket(It.IsAny<int>())).Returns(dtos);
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            IEnumerable<Asset> currencies = service.GetAssetsForMarket(3);

            //Assert
            Assert.AreEqual(dtos.Length, ((List<Asset>)currencies).Count);

            //Clear
            Market.restoreDefaultService();

        }

        [TestMethod]
        public void GetAssetsForMarket_AlreadyExistingCurrencyInstancesAreUsed()
        {
            //Arrange
            Mock<IAssetRepository> mockedRepository = new Mock<IAssetRepository>();
            AssetDto[] dtos = getAssetDtos();
            AssetDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetAssetById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetAssetsForMarket(It.IsAny<int>())).Returns(dtos);
            injectMockedServiceToMarketClass();

            //Act
            IAssetService service = testServiceInstance(mockedRepository);
            Asset asset = service.GetAssetById(dto.Id);
            IEnumerable<Asset> assets = service.GetAssetsForMarket(3);

            //Assert
            Asset fromResultCollection = assets.SingleOrDefault(a => a.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, asset);

            //Clear
            Market.restoreDefaultService();

        }


    }

}