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
    public class CurrencyServiceUnitTest
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_NAME = "US Dollar";
        private const string DEFAULT_SYMBOL = "USD";
        private const string DEFAULT_PAIR_SYMBOL = "EURUSD";
        private const int DEFAULT_BASE_CURRENCY_ID = 1;
        private const int DEFAULT_QUOTE_CURRENCY_ID = 2;
        private const bool DEFAULT_IS_ACTIVE = true;



        #region INFRASTRUCTURE

        private ICurrencyService testServiceInstance(Mock<ICurrencyRepository> mockedRepository)
        {
            ICurrencyService service = new CurrencyService(mockedRepository.Object);
            return service;
        }

        private CurrencyDto[] getCurrencyDtos()
        {
            List<CurrencyDto> list = new List<CurrencyDto>();
            list.Add(new CurrencyDto { Id = 1, Name = "US Dollar", Symbol = "USD" });
            list.Add(new CurrencyDto { Id = 2, Name = "Euro", Symbol = "EUR" });
            list.Add(new CurrencyDto { Id = 3, Name = "Japanese Yen", Symbol = "JPY" });
            return list.ToArray();
        }

        private FxPairDto[] getFxPairsDtos()
        {
            List<FxPairDto> list = new List<FxPairDto>();
            list.Add(new FxPairDto { Id = 1, Name = "EURUSD", BaseCurrency = 2, QuoteCurrency = 1, IsActive = true });
            list.Add(new FxPairDto { Id = 2, Name = "USDJPY", BaseCurrency = 1, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto { Id = 3, Name = "EURJPY", BaseCurrency = 2, QuoteCurrency = 3, IsActive = true });
            return list.ToArray();
        }

        private FxPairDto defaultFxPairDto()
        {
            return new FxPairDto
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_PAIR_SYMBOL,
                BaseCurrency = DEFAULT_BASE_CURRENCY_ID,
                QuoteCurrency = DEFAULT_QUOTE_CURRENCY_ID,
                IsActive = DEFAULT_IS_ACTIVE
            };
        }

        private Mock<ICurrencyRepository> mockedCurrencyRepositoryForFxPairUnitTests()
        {
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            FxPairDto dto = defaultFxPairDto();
            mockedRepository.Setup(c => c.GetFxPairById(DEFAULT_ID)).Returns(dto);
            mockedRepository.Setup(c => c.GetFxPairBySymbol(DEFAULT_PAIR_SYMBOL)).Returns(dto);
            return mockedRepository;
        }

        private void injectMockedServiceToCurrency()
        {
            Mock<ICurrencyService> mockedService = new Mock<ICurrencyService>();
            mockedService.Setup(s => s.GetCurrencyById(1)).Returns(new Currency(1, "USD", "US Dollar"));
            mockedService.Setup(s => s.GetCurrencyById(2)).Returns(new Currency(2, "EUR", "Euro"));
            mockedService.Setup(s => s.GetCurrencyById(3)).Returns(new Currency(3, "JPY", "Japanese Yen"));
            Currency.InjectService(mockedService.Object);
        }

        #endregion INFRASTRUCTURE



        [TestMethod]
        public void GetCurrencyById_ReturnsCurrency_IfExists()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = new CurrencyDto { Id = DEFAULT_ID, Name = DEFAULT_NAME, Symbol = DEFAULT_SYMBOL };
            mockedRepository.Setup(c => c.GetCurrencyById(DEFAULT_ID)).Returns(dto);
            Currency expectedCurrency = new Currency(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_NAME);
            
            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency = service.GetCurrencyById(DEFAULT_ID);

            //Assert
            Assert.AreEqual(expectedCurrency, currency);

        }

        [TestMethod]
        public void GetCurrencyById_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = null;
            mockedRepository.Setup(c => c.GetCurrencyById(DEFAULT_ID)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency = service.GetCurrencyById(DEFAULT_ID);

            //Assert
            Assert.IsNull(currency);

        }

        [TestMethod]
        public void GetCurrencyById_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = new CurrencyDto { Id = DEFAULT_ID, Name = DEFAULT_NAME, Symbol = DEFAULT_SYMBOL };
            mockedRepository.Setup(c => c.GetCurrencyById(DEFAULT_ID)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency1 = service.GetCurrencyById(DEFAULT_ID);
            Currency currency2 = service.GetCurrencyById(DEFAULT_ID);

            //Assert
            Assert.AreSame(currency1, currency2);

        }



        [TestMethod]
        public void GetCurrencyByName_ReturnsCurrency_IfExists()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = new CurrencyDto { Id = DEFAULT_ID, Name = DEFAULT_NAME, Symbol = DEFAULT_SYMBOL };
            mockedRepository.Setup(c => c.GetCurrencyByName(DEFAULT_NAME)).Returns(dto);
            Currency expectedCurrency = new Currency(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_NAME);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency = service.GetCurrencyByName(DEFAULT_NAME);

            //Assert
            Assert.AreEqual(expectedCurrency, currency);

        }

        [TestMethod]
        public void GetCurrencyByName_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = null;
            mockedRepository.Setup(c => c.GetCurrencyByName(DEFAULT_NAME)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency = service.GetCurrencyByName(DEFAULT_NAME);

            //Assert
            Assert.IsNull(currency);

        }

        [TestMethod]
        public void GetCurrencyByName_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = new CurrencyDto { Id = DEFAULT_ID, Name = DEFAULT_NAME, Symbol = DEFAULT_SYMBOL };
            mockedRepository.Setup(c => c.GetCurrencyByName(DEFAULT_NAME)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency1 = service.GetCurrencyByName(DEFAULT_NAME);
            Currency currency2 = service.GetCurrencyByName(DEFAULT_NAME);

            //Assert
            Assert.AreSame(currency1, currency2);

        }



        [TestMethod]
        public void GetCurrencyBySymbol_ReturnsCurrency_IfExists()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = new CurrencyDto { Id = DEFAULT_ID, Name = DEFAULT_NAME, Symbol = DEFAULT_SYMBOL };
            mockedRepository.Setup(c => c.GetCurrencyBySymbol(DEFAULT_SYMBOL)).Returns(dto);
            Currency expectedCurrency = new Currency(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_NAME);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency = service.GetCurrencyBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreEqual(expectedCurrency, currency);

        }

        [TestMethod]
        public void GetCurrencyBySymbol_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = null;
            mockedRepository.Setup(c => c.GetCurrencyBySymbol(DEFAULT_SYMBOL)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency = service.GetCurrencyBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.IsNull(currency);

        }

        [TestMethod]
        public void GetCurrencyBySymbol_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = new CurrencyDto { Id = DEFAULT_ID, Name = DEFAULT_NAME, Symbol = DEFAULT_SYMBOL };
            mockedRepository.Setup(c => c.GetCurrencyBySymbol(DEFAULT_SYMBOL)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency1 = service.GetCurrencyBySymbol(DEFAULT_SYMBOL);
            Currency currency2 = service.GetCurrencyBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreSame(currency1, currency2);

        }

        [TestMethod]
        public void GetCurrency_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto dto = new CurrencyDto { Id = DEFAULT_ID, Name = DEFAULT_NAME, Symbol = DEFAULT_SYMBOL };
            mockedRepository.Setup(r => r.GetCurrencyById(DEFAULT_ID)).Returns(dto);
            mockedRepository.Setup(r => r.GetCurrencyBySymbol(DEFAULT_SYMBOL)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency1 = service.GetCurrencyById(DEFAULT_ID);
            Currency currency2 = service.GetCurrencyBySymbol(DEFAULT_SYMBOL);

            //Assert
            Assert.AreSame(currency1, currency2);

        }



        [TestMethod]
        public void GetAllCurrencies_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto[] dtos = getCurrencyDtos();
            mockedRepository.Setup(r => r.GetCurrencies()).Returns(dtos);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            IEnumerable<Currency> currencies = service.GetAllCurrencies();

            //Assert
            Assert.AreEqual(dtos.Length, ((List<Currency>)currencies).Count);

        }

        [TestMethod]
        public void GetAllCurrencies_AlreadyExistingCurrencyInstancesAreUsed()
        {
            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            CurrencyDto[] dtos = getCurrencyDtos();
            CurrencyDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetCurrencyById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetCurrencies()).Returns(dtos);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            Currency currency = service.GetCurrencyById(dto.Id);
            IEnumerable<Currency> currencies = service.GetAllCurrencies();

            //Assert
            Currency fromResultCollection = currencies.SingleOrDefault(c => c.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, currency);

        }





        [TestMethod]
        public void GetFxPairById_ReturnsFxPair_IfExists()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = mockedCurrencyRepositoryForFxPairUnitTests();
            injectMockedServiceToCurrency();

            //Act
            FxPair expectedPair = new FxPair(DEFAULT_ID, DEFAULT_PAIR_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair = service.GetFxPairById(DEFAULT_ID);

            //Assert
            Assert.AreEqual(expectedPair, pair);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void GetFxPairById_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            FxPairDto dto = null;
            mockedRepository.Setup(c => c.GetFxPairById(DEFAULT_ID)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair = service.GetFxPairById(DEFAULT_ID);

            //Assert
            Assert.IsNull(pair);

        }

        [TestMethod]
        public void GetFxPairById_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = mockedCurrencyRepositoryForFxPairUnitTests();
            injectMockedServiceToCurrency();

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair1 = service.GetFxPairById(DEFAULT_ID);
            FxPair pair2 = service.GetFxPairById(DEFAULT_ID);

            //Assert
            Assert.AreSame(pair1, pair2);

            //Clear
            Currency.RestoreDefaultService();

        }



        [TestMethod]
        public void GetFxPairBySymbol_ReturnsFxPair_IfExists()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = mockedCurrencyRepositoryForFxPairUnitTests();
            injectMockedServiceToCurrency();

            //Act
            FxPair expectedPair = new FxPair(DEFAULT_ID, DEFAULT_PAIR_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair = service.GetFxPairBySymbol(DEFAULT_PAIR_SYMBOL);

            //Assert
            Assert.AreEqual(expectedPair, pair);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void GetFxPairBySymbol_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            FxPairDto dto = null;
            mockedRepository.Setup(c => c.GetFxPairBySymbol(DEFAULT_PAIR_SYMBOL)).Returns(dto);

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair = service.GetFxPairBySymbol(DEFAULT_PAIR_SYMBOL);

            //Assert
            Assert.IsNull(pair);

        }

        [TestMethod]
        public void GetFxPairBySymbol_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = mockedCurrencyRepositoryForFxPairUnitTests();
            injectMockedServiceToCurrency();

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair1 = service.GetFxPairBySymbol(DEFAULT_PAIR_SYMBOL);
            FxPair pair2 = service.GetFxPairBySymbol(DEFAULT_PAIR_SYMBOL);

            //Assert
            Assert.AreSame(pair1, pair2);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void GetFxPair_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ICurrencyRepository> mockedRepository = mockedCurrencyRepositoryForFxPairUnitTests();
            injectMockedServiceToCurrency();

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair1 = service.GetFxPairById(DEFAULT_ID);
            FxPair pair2 = service.GetFxPairBySymbol(DEFAULT_PAIR_SYMBOL);

            //Assert
            Assert.AreSame(pair1, pair2);

            //Clear
            Currency.RestoreDefaultService();

        }



        [TestMethod]
        public void GetAllFxPairs_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            FxPairDto[] dtos = getFxPairsDtos();
            mockedRepository.Setup(r => r.GetFxPairs()).Returns(dtos);
            injectMockedServiceToCurrency();

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            IEnumerable<FxPair> currencies = service.GetFxPairs();

            //Assert
            Assert.AreEqual(dtos.Length, ((List<FxPair>)currencies).Count);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void GetAllFxPairs_AlreadyExistingCurrencyInstancesAreUsed()
        {
            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            FxPairDto[] dtos = getFxPairsDtos();
            FxPairDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetFxPairById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetFxPairs()).Returns(dtos);
            injectMockedServiceToCurrency();

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair = service.GetFxPairById(dto.Id);
            IEnumerable<FxPair> pairs = service.GetFxPairs();

            //Assert
            FxPair fromResultCollection = pairs.SingleOrDefault(p => p.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, pair);

            //Clear
            Currency.RestoreDefaultService();

        }



        [TestMethod]
        public void FilterFxPairs_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            FxPairDto[] dtos = getFxPairsDtos();
            mockedRepository.Setup(r => r.GetFxPairs(It.IsAny<string>(), It.IsAny<int>())).Returns(dtos);
            injectMockedServiceToCurrency();

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            IEnumerable<FxPair> currencies = service.GetFxPairs("a", 3);

            //Assert
            Assert.AreEqual(dtos.Length, ((List<FxPair>)currencies).Count);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void FilterFxPairs_AlreadyExistingCurrencyInstancesAreUsed()
        {
            //Arrange
            Mock<ICurrencyRepository> mockedRepository = new Mock<ICurrencyRepository>();
            FxPairDto[] dtos = getFxPairsDtos();
            FxPairDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetFxPairById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetFxPairs(It.IsAny<string>(), It.IsAny<int>())).Returns(dtos);
            injectMockedServiceToCurrency();

            //Act
            ICurrencyService service = testServiceInstance(mockedRepository);
            FxPair pair = service.GetFxPairById(dto.Id);
            IEnumerable<FxPair> pairs = service.GetFxPairs("a", 3);

            //Assert
            FxPair fromResultCollection = pairs.SingleOrDefault(p => p.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, pair);

            //Clear
            Currency.RestoreDefaultService();

        }


    }

}