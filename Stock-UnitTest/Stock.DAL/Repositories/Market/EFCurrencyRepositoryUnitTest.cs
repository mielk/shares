using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Repositories;
using Stock.DAL.TransferObjects;
using Stock.DAL.Infrastructure;
using Stock.DAL.Helpers;
using Stock.Utils;


namespace Stock_UnitTest.Stock.DAL.Repositories
{
    [TestClass]
    public class EFCurrencyRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string CURRENCIES_TABLE_NAME = "currencies";
        private const string FX_PAIRS_TABLE_NAME = "pairs";
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_CURRENCY_NAME = "US Dollar";
        private const string DEFAULT_CURRENCY_SYMBOL = "USD";
        private const string DEFAULT_PAIR_NAME = "EURUSD";
        private const int DEFAULT_PAIR_BASE_CURRENCY = 1;
        private const int DEFAULT_PAIR_QUOTE_CURRENCY = 2;
        private const bool DEFAULT_IS_ACTIVE = true;


        #region INFRASTRUCTURE

        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            DbContext context = new OriginalDbContext();
            context.Database.ExecuteSqlCommand("recreateDb");
        }

        [ClassCleanup()]
        public static void CleanupTestSuite()
        {
            DbContext context = new OriginalDbContext();
            context.Database.ExecuteSqlCommand("DROP DATABASE fx_unittests");
        }

        private void insertCurrencyToTestDb(CurrencyDto dto)    //int id, string name, string fullName)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, CurrencySymbol, CurrencyFullName) VALUES({2}, '{3}', '{4}');";
            string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, CURRENCIES_TABLE_NAME, dto.Id, dto.Symbol, dto.Name);

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, CURRENCIES_TABLE_NAME);
                context.Database.ExecuteSqlCommand(insertSql);
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        private void insertCurrenciesToTestDb(IEnumerable<CurrencyDto> currencies)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, CurrencySymbol, CurrencyFullName) VALUES({2}, '{3}', '{4}');";

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, CURRENCIES_TABLE_NAME);
                foreach (var currency in currencies)
                {
                    string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, CURRENCIES_TABLE_NAME, currency.Id, currency.Symbol, currency.Name);
                    context.Database.ExecuteSqlCommand(insertSql);
                }
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        private void insertFxPairToTestDb(FxPairDto pair)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Name, BaseCurrency, QuoteCurrency, IsActive) VALUE({2}, {3}, {4}, {5}, {6})";
            string insertSql = string.Format(INSERT_SQL_PATTERN, 
                                                UNIT_TEST_DB_NAME, FX_PAIRS_TABLE_NAME, 
                                                pair.Id, pair.Name.ToDbString(), 
                                                pair.BaseCurrency, pair.QuoteCurrency, 
                                                pair.IsActive.ToDbString());

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, FX_PAIRS_TABLE_NAME);
                context.Database.ExecuteSqlCommand(insertSql);
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        private void insertFxPairsToTestDb(IEnumerable<FxPairDto> pairs)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Name, BaseCurrency, QuoteCurrency, IsActive) VALUE({2}, {3}, {4}, {5}, 1)";

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, FX_PAIRS_TABLE_NAME);
                foreach (var pair in pairs)
                {
                    try
                    {
                        string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, FX_PAIRS_TABLE_NAME, pair.Id, pair.Name.ToDbString(), pair.BaseCurrency, pair.QuoteCurrency);
                        context.Database.ExecuteSqlCommand(insertSql);
                    }
                    catch (Exception ex)
                    {
                        var x = ex;
                    }
                }
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        private FxPairDto getDefaultFxPairDto()
        {
            return new FxPairDto()
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_PAIR_NAME,
                BaseCurrency = DEFAULT_PAIR_QUOTE_CURRENCY,
                QuoteCurrency = DEFAULT_PAIR_QUOTE_CURRENCY,
                IsActive= DEFAULT_IS_ACTIVE
            };
        }

        private CurrencyDto getDefaultCurrencyDto()
        {
            return new CurrencyDto() {
                Id = DEFAULT_ID,
                Name = DEFAULT_CURRENCY_NAME,
                Symbol = DEFAULT_CURRENCY_SYMBOL
            };
        }

        private IEnumerable<CurrencyDto> getDefaultCurrencyDtosForTest()
        {
            List<CurrencyDto> list = new List<CurrencyDto>();
            list.Add(new CurrencyDto() { Id = 1, Name = "US Dollar", Symbol = "USD" });
            list.Add(new CurrencyDto() { Id = 2, Name = "Euro", Symbol = "EUR" });
            list.Add(new CurrencyDto() { Id = 3, Name = "British Pound", Symbol = "GBP" });
            list.Add(new CurrencyDto() { Id = 4, Name = "Japanese Yen", Symbol = "JPY" });
            return list;
        }

        private IEnumerable<FxPairDto> GetDefaultFxPairDtosCollectionForTests()
        {
            List<FxPairDto> list = new List<FxPairDto>();
            list.Add(new FxPairDto() { Id = 1, Name = "EURUSD", BaseCurrency = 2, QuoteCurrency = 1, IsActive = true });
            list.Add(new FxPairDto() { Id = 2, Name = "AUDUSD", BaseCurrency = 5, QuoteCurrency = 1, IsActive = true });
            list.Add(new FxPairDto() { Id = 3, Name = "NZDUSD", BaseCurrency = 6, QuoteCurrency = 1, IsActive = true });
            list.Add(new FxPairDto() { Id = 4, Name = "GBPUSD", BaseCurrency = 7, QuoteCurrency = 1, IsActive = true });
            list.Add(new FxPairDto() { Id = 5, Name = "USDJPY", BaseCurrency = 1, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto() { Id = 6, Name = "EURCHF", BaseCurrency = 2, QuoteCurrency = 4, IsActive = true });
            list.Add(new FxPairDto() { Id = 7, Name = "EURJPY", BaseCurrency = 2, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto() { Id = 8, Name = "EURGBP", BaseCurrency = 2, QuoteCurrency = 7, IsActive = true });
            list.Add(new FxPairDto() { Id = 9, Name = "AUDJPY", BaseCurrency = 5, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto() { Id = 10, Name = "NZDJPY", BaseCurrency = 6, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto() { Id = 11, Name = "USDCAD", BaseCurrency = 1, QuoteCurrency = 8, IsActive = true });
            list.Add(new FxPairDto() { Id = 12, Name = "AUDCAD", BaseCurrency = 5, QuoteCurrency = 8, IsActive = true });
            list.Add(new FxPairDto() { Id = 13, Name = "CADJPY", BaseCurrency = 8, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto() { Id = 14, Name = "CHFJPY", BaseCurrency = 4, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto() { Id = 15, Name = "EURAUD", BaseCurrency = 2, QuoteCurrency = 5, IsActive = true });
            list.Add(new FxPairDto() { Id = 16, Name = "EURCAD", BaseCurrency = 2, QuoteCurrency = 8, IsActive = true });
            list.Add(new FxPairDto() { Id = 17, Name = "EURNZD", BaseCurrency = 2, QuoteCurrency = 6, IsActive = true });
            list.Add(new FxPairDto() { Id = 18, Name = "GBPJPY", BaseCurrency = 7, QuoteCurrency = 3, IsActive = true });
            list.Add(new FxPairDto() { Id = 19, Name = "USDCHF", BaseCurrency = 1, QuoteCurrency = 4, IsActive = true });
            return list;
        }


        #endregion INFRASTRUCTURE


        #region GET_CURRENCIES

        [TestMethod]
        public void GetCurrencies_returnProperDtoCollection()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            IEnumerable<CurrencyDto> currencyDtos = getDefaultCurrencyDtosForTest();
            insertCurrenciesToTestDb(currencyDtos);

            //Act
            CurrencyDto[] dtos = repository.GetCurrencies().ToArray();

            //Assert
            bool areEqualArrays = currencyDtos.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        #endregion GET_CURRENCIES


        #region GET_CURRENCY_BY_ID

        [TestMethod]
        public void GetCurrencyById_returnProperDto_forExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            CurrencyDto baseDto = getDefaultCurrencyDto();
            insertCurrencyToTestDb(baseDto);

            //Act
            CurrencyDto dto = repository.GetCurrencyById(baseDto.Id);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetCurrencyById_returnNull_forNonExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            CurrencyDto baseDto = getDefaultCurrencyDto();
            insertCurrencyToTestDb(baseDto);

            //Act
            CurrencyDto dto = repository.GetCurrencyById(baseDto.Id + 1);

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_CURRENCY_BY_ID


        #region GET_CURRENCY_BY_NAME

        [TestMethod]
        public void GetCurrencyByName_returnProperDto_forExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            CurrencyDto baseDto = getDefaultCurrencyDto();
            insertCurrencyToTestDb(baseDto);

            //Act
            CurrencyDto dto = repository.GetCurrencyByName(baseDto.Name);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetCurrencyByName_returnNull_forNonExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            CurrencyDto baseDto = getDefaultCurrencyDto();
            insertCurrencyToTestDb(baseDto);

            //Act
            CurrencyDto dto = repository.GetCurrencyByName(baseDto.Name + "a");

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_CURRENCY_BY_NAME


        #region GET_CURRENCY_BY_SYMBOL

        [TestMethod]
        public void GetCurrencyBySymbol_returnProperDto_forExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            CurrencyDto baseDto = getDefaultCurrencyDto();
            insertCurrencyToTestDb(baseDto);

            //Act
            CurrencyDto dto = repository.GetCurrencyBySymbol(baseDto.Symbol);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetCurrencyBySymbol_returnNull_forNonExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            CurrencyDto baseDto = getDefaultCurrencyDto();
            insertCurrencyToTestDb(baseDto);

            //Act
            CurrencyDto dto = repository.GetCurrencyBySymbol(baseDto.Symbol + "a");

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_CURRENCY_BY_SYMBOL



        #region GET_FX_PAIRS


        #endregion GET_FX_PAIRS


        #region FILTER_FX_PAIRS



        #endregion FILTER_FX_PAIRS


        #region GET_FX_PAIR_BY_ID

        [TestMethod]
        public void GetFxPairById_returnProperDto_forExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            FxPairDto baseDto = getDefaultFxPairDto();
            insertFxPairToTestDb(baseDto);

            //Act
            FxPairDto dto = repository.GetFxPairById(baseDto.Id);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetFxPairById_returnNull_forNonExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            FxPairDto baseDto = getDefaultFxPairDto();
            insertFxPairToTestDb(baseDto);

            //Act
            FxPairDto dto = repository.GetFxPairById(baseDto.Id + 1);

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_FX_PAIR_BY_ID


        #region GET_FX_PAIR_BY_SYMBOL

        [TestMethod]
        public void GetFxPairByName_returnProperDto_forExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            FxPairDto baseDto = getDefaultFxPairDto();
            insertFxPairToTestDb(baseDto);

            //Act
            FxPairDto dto = repository.GetFxPairBySymbol(baseDto.Name);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetFxPairByName_returnNull_forNonExistingItem()
        {

            //Arrange
            EFCurrencyRepository repository = new EFCurrencyRepository();
            FxPairDto baseDto = getDefaultFxPairDto();
            insertFxPairToTestDb(baseDto);

            //Act
            FxPairDto dto = repository.GetFxPairBySymbol(baseDto.Name + "a");

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_FX_PAIR_BY_SYMBOL

    }

}