using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Stock.DAL.TransferObjects;
using System.Linq;
using Moq;
using Stock.Domain.Services;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class FxPairUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_SYMBOL = "EURUSD";
        private const int DEFAULT_BASE_CURRENCY_ID = 1;
        private const int DEFAULT_QUOTE_CURRENCY_ID = 2;
        private const bool DEFAULT_IS_ACTIVE = true;



        #region INFRASTRUCTURE

        private FxPair defaultFxPair()
        {
            Currency baseCurrency = new Currency(1, "USD", "US Dollar");
            Currency quoteCurrency = new Currency(2, "EUR", "Euro");
            FxPair pair = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, baseCurrency, quoteCurrency);
            return pair;
        }

        private IEnumerable<FxPair> defaultFxPairsCollection()
        {

            injectMockedServiceToCurrency();

            List<FxPair> fxPairs = new List<FxPair>();
            fxPairs.Add(new FxPair(1, "EURUSD", 2, 1));
            fxPairs.Add(new FxPair(2, "USDJPY", 1, 3));
            fxPairs.Add(new FxPair(3, "EURJPY", 2, 3));
            return fxPairs;

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
        public void Equals_returnFalse_forObjectOfOtherType()
        {

            //Arrange
            injectMockedServiceToCurrency();

            //Act
            var baseObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);
            var comparedObject = new { Id = 1, Value = 2 };
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsFalse(areEqual);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifIdDifferent()
        {
            
            //Arrange
            injectMockedServiceToCurrency();

            //Act
            var baseObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);
            var comparedObject = new FxPair(DEFAULT_ID + 1, DEFAULT_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);
            bool areEqual = baseObject.Equals(comparedObject);
            
            //Assert
            Assert.IsFalse(areEqual);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifSymbolDifferent()
        {
            
            //Arrange
            injectMockedServiceToCurrency();

            //Act
            var baseObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);
            var comparedObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL + 'a', DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);
            bool areEqual = baseObject.Equals(comparedObject);
            
            //Assert
            Assert.IsFalse(areEqual);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifBaseCurrencyIsDifferent()
        {

            //Arrange
            injectMockedServiceToCurrency();

            //Act
            var baseObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, 1, 3);
            var comparedObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, 2, 3);
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsFalse(areEqual);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnFalse_ifQuoteCurrencyIsDifferent()
        {

            //Arrange
            injectMockedServiceToCurrency();

            //Act
            var baseObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, 3, 1);
            var comparedObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, 3, 2);
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsFalse(areEqual);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void Equals_returnTrue_ifCurrenciesEqual()
        {

            //Arrange
            injectMockedServiceToCurrency();

            //Act
            var baseObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, 1, 2);
            var comparedObject = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, 1, 2);
            bool areEqual = baseObject.Equals(comparedObject);

            //Assert
            Assert.IsTrue(areEqual);

            //Clear
            Currency.RestoreDefaultService();

        }




        [TestMethod]
        public void constructor_newInstance_hasProperIdNameAndCurrencies()
        {

            //Arrange.
            injectMockedServiceToCurrency();

            //Act.
            var baseCurrency = Currency.ById(DEFAULT_BASE_CURRENCY_ID);
            var quoteCurrency = Currency.ById(DEFAULT_QUOTE_CURRENCY_ID);
            var pair = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, baseCurrency, quoteCurrency);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, pair.GetId());
            Assert.AreEqual(DEFAULT_SYMBOL, pair.GetName());
            Assert.AreSame(baseCurrency, pair.GetBaseCurrency());
            Assert.AreSame(quoteCurrency, pair.GetQuoteCurrency());

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void constructor_hasProperCurrencies_ifCurrencyIdsAreGiven()
        {

            //Arrange.
            injectMockedServiceToCurrency();

            //Act.
            var pair = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_QUOTE_CURRENCY_ID);

            //Assert.
            Assert.AreSame(Currency.ById(DEFAULT_BASE_CURRENCY_ID), pair.GetBaseCurrency());
            Assert.AreSame(Currency.ById(DEFAULT_QUOTE_CURRENCY_ID), pair.GetQuoteCurrency());

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given currencies cannot be the same")]
        public void constructor_ThrowsException_ifSameCurrencyIdsAreGiven()
        {

            //Arrange.
            injectMockedServiceToCurrency();

            //Act.
            var pair = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_BASE_CURRENCY_ID, DEFAULT_BASE_CURRENCY_ID);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given currencies cannot be the same")]
        public void constructor_ThrowException_ifSameCurrenciesAreGiven()
        {

            //Arrange.
            injectMockedServiceToCurrency();

            //Act.
            Currency currency = Currency.ById(1);
            var pair = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, currency, currency);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "One of the given currencies is null")]
        public void constructor_ThrowException_ifNonExistingBaseCurrencyIsGiven()
        {
            
            //Arrange.
            injectMockedServiceToCurrency();

            //Act.
            var pair = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, 0, DEFAULT_QUOTE_CURRENCY_ID);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "One of the given currencies is null")]
        public void constructor_ThrowException_ifNonExistingQuoteCurrencyIsGiven()
        {

            //Arrange.
            injectMockedServiceToCurrency();

            //Act.
            var pair = new FxPair(DEFAULT_ID, DEFAULT_SYMBOL, DEFAULT_BASE_CURRENCY_ID, 0);

            //Clear
            Currency.RestoreDefaultService();

        }

        [TestMethod]
        public void fxPair_fromDto_hasCorrectProperties()
        {

            //Arrange.
            injectMockedServiceToCurrency();

            //Act.
            FxPairDto dto = new FxPairDto { 
                Id = DEFAULT_ID, 
                Name = DEFAULT_SYMBOL, 
                BaseCurrency = DEFAULT_BASE_CURRENCY_ID, 
                QuoteCurrency = DEFAULT_QUOTE_CURRENCY_ID, 
                IsActive = DEFAULT_IS_ACTIVE 
            };
            var pair = FxPair.FromDto(dto);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, pair.GetId());
            Assert.AreEqual(DEFAULT_SYMBOL, pair.GetName());
            Assert.AreSame(Currency.ById(1), pair.GetBaseCurrency());
            Assert.AreSame(Currency.ById(2), pair.GetQuoteCurrency());

            //Clear
            Currency.RestoreDefaultService();

        }



        [TestMethod]
        public void ById_returnsNull_ifNotExistInRepository()
        {

            //Arrange
            Mock<ICurrencyService> mockService = new Mock<ICurrencyService>();
            FxPair nullFxPair = null;
            mockService.Setup(c => c.GetFxPairById(DEFAULT_ID)).Returns(nullFxPair);
            FxPair.injectService(mockService.Object);

            //Act.
            FxPair pair = FxPair.ById(DEFAULT_ID);

            //Assert.
            Assert.IsNull(pair);

        }

        [TestMethod]
        public void ById_returnsExistingInstance()
        {

            //Arrange
            Mock<ICurrencyService> mockService = new Mock<ICurrencyService>();
            FxPair expectedPair = defaultFxPair();
            mockService.Setup(c => c.GetFxPairById(DEFAULT_ID)).Returns(expectedPair);
            FxPair.injectService(mockService.Object);

            //Act.
            FxPair pair = FxPair.ById(DEFAULT_ID);

            //Assert.
            Assert.AreSame(pair, expectedPair);

        }

        [TestMethod]
        public void BySymbol_returnsExistingInstance()
        {

            //Arrange
            Mock<ICurrencyService> mockService = new Mock<ICurrencyService>();
            FxPair expectedPair = defaultFxPair();
            mockService.Setup(c => c.GetFxPairBySymbol(DEFAULT_SYMBOL)).Returns(expectedPair);
            FxPair.injectService(mockService.Object);

            //Act.
            FxPair pair = FxPair.BySymbol(DEFAULT_SYMBOL);

            //Assert.
            Assert.AreSame(pair, expectedPair);

        }

        [TestMethod]
        public void ById_returnsTheSameInstance_afterAddingNewItem()
        {

            //Arrange
            Mock<ICurrencyService> mockService = new Mock<ICurrencyService>();
            FxPair expectedPair = defaultFxPair();
            mockService.Setup(c => c.GetFxPairById(DEFAULT_ID)).Returns(expectedPair);
            FxPair.injectService(mockService.Object);

            //Act
            FxPair pair1 = FxPair.ById(1);
            FxPair pair2 = FxPair.ById(1);

            //Assert
            Assert.AreSame(pair1, pair2);

        }

        [TestMethod]
        public void GetAllFxPairs_returnsProperCollection()
        {

            //Arrange
            Mock<ICurrencyService> mockService = new Mock<ICurrencyService>();
            var expectedPairs = defaultFxPairsCollection();
            mockService.Setup(c => c.GetFxPairs()).Returns(expectedPairs);
            FxPair.injectService(mockService.Object);

            //Act.
            IEnumerable<FxPair> pairs = FxPair.GetFxPairs();

            //Assert.
            bool areEqual = pairs.HasTheSameItems(expectedPairs);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetFilteredFxPairs_returnsProperCollection()
        {

            //Arrange
            Mock<ICurrencyService> mockService = new Mock<ICurrencyService>();
            var expectedPairs = defaultFxPairsCollection();
            mockService.Setup(c => c.GetFxPairs(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedPairs);
            FxPair.injectService(mockService.Object);

            //Act.
            IEnumerable<FxPair> pairs = FxPair.GetFxPairs("a", 1);

            //Assert.
            bool areEqual = pairs.HasTheSameItems(expectedPairs);
            Assert.IsTrue(areEqual);

        }

    }
}