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
    public class EFMarketRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string MARKETS_TABLE_NAME = "markets";
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_NAME = "Forex";
        private const string DEFAULT_SYMBOL = "FX";


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


        private void insertMarketToTestDb(MarketDto market)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Name, ShortName) VALUES({2}, {3}, {4});";
            string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, MARKETS_TABLE_NAME, market.Id, market.Name.ToDbString(), market.ShortName.ToDbString());

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, MARKETS_TABLE_NAME);
                context.Database.ExecuteSqlCommand(insertSql);
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        private void insertMarketsToTestDb(IEnumerable<MarketDto> markets)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Name, ShortName) VALUES({2}, {3}, {4});";

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, MARKETS_TABLE_NAME);
                foreach (var market in markets)
                {
                    string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, MARKETS_TABLE_NAME, market.Id, market.Name.ToDbString(), market.ShortName.ToDbString());
                    context.Database.ExecuteSqlCommand(insertSql);
                }
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        
        private MarketDto getDefaultMarketDto()
        {
            return new MarketDto() {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME,
                ShortName = DEFAULT_SYMBOL
            };
        }

        private IEnumerable<MarketDto> getDefaultMarketDtos()
        {
            List<MarketDto> list = new List<MarketDto>();
            list.Add(new MarketDto() { Id = 1, Name = "Forex", ShortName = "FX" });
            list.Add(new MarketDto() { Id = 2, Name = "USA", ShortName = "US" });
            return list;
        }
        
        #endregion INFRASTRUCTURE


        #region GET_MARKETS

        [TestMethod]
        public void GetAllMarkets_returnProperDtoCollection()
        {

            //Arrange
            EFMarketRepository repository = new EFMarketRepository();
            IEnumerable<MarketDto> marketDtos = getDefaultMarketDtos();
            insertMarketsToTestDb(marketDtos);

            //Act
            MarketDto[] dtos = repository.GetMarkets().ToArray();

            //Assert
            bool areEqualArrays = marketDtos.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        #endregion GET_MARKETS


        #region FILTER_MARKETS



        #endregion FILTER_MARKETS


        #region GET_MARKET_BY_ID

        [TestMethod]
        public void GetMarketById_returnProperDto_forExistingItem()
        {

            //Arrange
            EFMarketRepository repository = new EFMarketRepository();
            MarketDto baseDto = getDefaultMarketDto();
            insertMarketToTestDb(baseDto);

            //Act
            MarketDto dto = repository.GetMarketById(baseDto.Id);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetMarketById_returnNull_forNonExistingItem()
        {

            //Arrange
            EFMarketRepository repository = new EFMarketRepository();
            MarketDto baseDto = getDefaultMarketDto();
            insertMarketToTestDb(baseDto);

            //Act
            MarketDto dto = repository.GetMarketById(baseDto.Id + 1);

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_MARKET_BY_ID



        #region GET_MARKET_BY_NAME

        [TestMethod]
        public void GetMarketByName_returnProperDto_forExistingItem()
        {

            //Arrange
            EFMarketRepository repository = new EFMarketRepository();
            MarketDto baseDto = getDefaultMarketDto();
            insertMarketToTestDb(baseDto);

            //Act
            MarketDto dto = repository.GetMarketByName(baseDto.Name);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetMarketByName_returnNull_forNonExistingItem()
        {

            //Arrange
            EFMarketRepository repository = new EFMarketRepository();
            MarketDto baseDto = getDefaultMarketDto();
            insertMarketToTestDb(baseDto);

            //Act
            MarketDto dto = repository.GetMarketByName(baseDto.Name + "a");

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_MARKET_BY_NAME




        #region GET_MARKET_BY_SYMBOL

        [TestMethod]
        public void GetMarketBySymbol_returnProperDto_forExistingItem()
        {

            //Arrange
            EFMarketRepository repository = new EFMarketRepository();
            MarketDto baseDto = getDefaultMarketDto();
            insertMarketToTestDb(baseDto);

            //Act
            MarketDto dto = repository.GetMarketBySymbol(baseDto.ShortName);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetMarketBySymbol_returnNull_forNonExistingItem()
        {

            //Arrange
            EFMarketRepository repository = new EFMarketRepository();
            MarketDto baseDto = getDefaultMarketDto();
            insertMarketToTestDb(baseDto);

            //Act
            MarketDto dto = repository.GetMarketBySymbol(baseDto.ShortName + "a");

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_MARKET_BY_SYMBOL



    }

}