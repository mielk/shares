using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Stock.DAL.Repositories;
using Stock.DAL.TransferObjects;
using Stock.Domain.Entities;
using Stock.Domain.Services;

namespace Stock_UnitTest.Stock.Domain.Services.MarketServices
{
    [TestClass]
    public class MarketServiceUnitTest
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_NAME = "Forex";
        private const string DEFAULT_SYMBOL = "FX";


        #region INFRASTRUCTURE

        private IMarketService testServiceInstance(Mock<IMarketRepository> mockedRepository)
        {
            IMarketService service = new MarketService(mockedRepository.Object);
            return service;
        }

        private MarketDto[] getMarketDtos()
        {
            List<MarketDto> list = new List<MarketDto>();
            list.Add(new MarketDto { Id = 1, Name = "Forex", ShortName = "FX", StartTime = "00:00:00", EndTime = "23:00:00", IsAroundClock = true });
            list.Add(new MarketDto { Id = 2, Name = "United States", ShortName = "US", StartTime = "14:00:00", EndTime = "22:00:00", IsAroundClock = false });
            list.Add(new MarketDto { Id = 3, Name = "United Kingdom", ShortName = "UK", StartTime = "09:30:00", EndTime = "17:30:00", IsAroundClock = false });
            return list.ToArray();
        }

        private MarketDto defaultMarketDto()
        {
            return new MarketDto
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME,
                ShortName = DEFAULT_SYMBOL
            };
        }

        private Mock<IMarketRepository> mockedMarketRepositoryForUnitTests()
        {
            Mock<IMarketRepository> mockedRepository = new Mock<IMarketRepository>();
            MarketDto dto = defaultMarketDto();
            mockedRepository.Setup(s => s.GetMarketById(DEFAULT_ID)).Returns(dto);
            mockedRepository.Setup(s => s.GetMarketBySymbol(DEFAULT_SYMBOL)).Returns(dto);
            return mockedRepository;
        }

        #endregion INFRASTRUCTURE



        [TestMethod]
        public void GetMarketById_ReturnsAsset_IfExists()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = mockedMarketRepositoryForUnitTests();
            IMarketService service = testServiceInstance(mockedRepository);

            //Act
            Market expectedMarket = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SYMBOL);
            Market market = service.GetMarketById(DEFAULT_ID);

            //Assert
            Assert.AreEqual(expectedMarket, market);

        }

        [TestMethod]
        public void GetMarketById_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = new Mock<IMarketRepository>();
            MarketDto dto = null;
            mockedRepository.Setup(c => c.GetMarketById(DEFAULT_ID)).Returns(dto);

            //Act
            IMarketService service = testServiceInstance(mockedRepository);
            Market market = service.GetMarketById(DEFAULT_ID);

            //Assert
            Assert.IsNull(market);

        }

        [TestMethod]
        public void GetMarketId_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = mockedMarketRepositoryForUnitTests();

            //Act
            IMarketService service = testServiceInstance(mockedRepository);
            Market market1 = service.GetMarketById(DEFAULT_ID);
            Market market2 = service.GetMarketById(DEFAULT_ID);

            //Assert
            Assert.AreSame(market1, market2);

        }

        [TestMethod]
        public void GetMarketBySymbol_ReturnsAsset_IfExists()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = mockedMarketRepositoryForUnitTests();
            IMarketService service = testServiceInstance(mockedRepository);

            //Act
            Market expectedMarket = new Market(DEFAULT_ID, DEFAULT_NAME, DEFAULT_SYMBOL);
            Market market = service.GetMarketBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreEqual(expectedMarket, market);

        }

        [TestMethod]
        public void GetMarketBySymbol_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = new Mock<IMarketRepository>();
            MarketDto dto = null;
            mockedRepository.Setup(s => s.GetMarketBySymbol(DEFAULT_SYMBOL)).Returns(dto);

            //Act
            IMarketService service = testServiceInstance(mockedRepository);
            Market market = service.GetMarketBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.IsNull(market);

        }

        [TestMethod]
        public void GetMarketBySymbol_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = mockedMarketRepositoryForUnitTests();

            //Act
            IMarketService service = testServiceInstance(mockedRepository);
            Market market1 = service.GetMarketBySymbol(DEFAULT_SYMBOL);
            Market market2 = service.GetMarketBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreSame(market1, market2);

        }

        [TestMethod]
        public void GetMarket_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = mockedMarketRepositoryForUnitTests();

            //Act
            IMarketService service = testServiceInstance(mockedRepository);
            Market market1 = service.GetMarketById(DEFAULT_ID);
            Market market2 = service.GetMarketBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreSame(market1, market2);

        }



        [TestMethod]
        public void GetMarkets_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<IMarketRepository> mockedRepository = new Mock<IMarketRepository>();
            MarketDto[] dtos = getMarketDtos();
            mockedRepository.Setup(r => r.GetMarkets()).Returns(dtos);

            //Act
            IMarketService service = testServiceInstance(mockedRepository);
            IEnumerable<Market> markets = service.GetMarkets();

            //Assert
            Assert.AreEqual(dtos.Length, ((List<Market>)markets).Count);

        }

        [TestMethod]
        public void GetMarkets_AlreadyExistingCurrencyInstancesAreUsed()
        {

            //Arrange
            Mock<IMarketRepository> mockedRepository = new Mock<IMarketRepository>();
            MarketDto[] dtos = getMarketDtos();
            MarketDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetMarketById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetMarkets()).Returns(dtos);

            //Act
            IMarketService service = testServiceInstance(mockedRepository);
            Market market = service.GetMarketById(dto.Id);
            IEnumerable<Market> markets = service.GetMarkets();

            //Assert
            Market fromResultCollection = markets.SingleOrDefault(m => m.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, market);

        }


    }

}