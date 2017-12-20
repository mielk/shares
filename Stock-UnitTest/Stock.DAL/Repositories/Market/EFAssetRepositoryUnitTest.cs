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
    public class EFAssetRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string ASSETS_TABLE_NAME = "assets";
        private const string MARKETS_TABLE_NAME = "markets";
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_SYMBOL = "EURUSD";
        private const int DEFAULT_MARKET_ID = 1;


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

        private IEnumerable<MarketDto> getDefaultMarketDtosCollection()
        {
            List<MarketDto> list = new List<MarketDto>();
            list.Add(new MarketDto() { Id = 1, Name = "Forex", ShortName = "FX" });
            list.Add(new MarketDto() { Id = 2, Name = "USA", ShortName = "US" });
            return list;
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

        private void insertMarketsToTestDb()
        {
            IEnumerable<MarketDto> markets = getDefaultMarketDtosCollection();
            insertMarketsToTestDb(markets);
        }

        private void insertAssetToTestDb(AssetDto dto)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Symbol, MarketId) VALUES({2}, {3}, {4});";
            string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, ASSETS_TABLE_NAME, dto.Id, dto.Symbol.ToDbString(), dto.MarketId);

            DbContext context = new UnitTestsDbContext();
            try
            {
                insertMarketsToTestDb();
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, ASSETS_TABLE_NAME);
                context.Database.ExecuteSqlCommand(insertSql);
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        private void insertAssetsToTestDb(IEnumerable<AssetDto> assets)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Symbol, MarketId) VALUES({2}, {3}, {4});";

            DbContext context = new UnitTestsDbContext();
            try
            {
                insertMarketsToTestDb();
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, ASSETS_TABLE_NAME);
                foreach (var asset in assets)
                {
                    string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, ASSETS_TABLE_NAME, asset.Id, asset.Symbol.ToDbString(), asset.MarketId);
                    context.Database.ExecuteSqlCommand(insertSql);
                }
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                if (context.Database.CurrentTransaction != null)
                {
                    context.Database.CurrentTransaction.Rollback();
                }
            }

        }

        private AssetDto getDefaultAssetDto()
        {
            return new AssetDto() {
                Id = DEFAULT_ID,
                Symbol = DEFAULT_SYMBOL,
                MarketId = DEFAULT_MARKET_ID
            };
        }

        private IEnumerable<AssetDto> getDefaultAssetDtos()
        {
            List<AssetDto> list = new List<AssetDto>();
            list.Add(new AssetDto() { Id = 1, Symbol = "EURUSD", MarketId = 1 });
            list.Add(new AssetDto() { Id = 2, Symbol = "EURCHF", MarketId = 1 });
            list.Add(new AssetDto() { Id = 3, Symbol = "USDJPY", MarketId = 1 });
            list.Add(new AssetDto() { Id = 4, Symbol = "AUDUSD", MarketId = 1 });
            list.Add(new AssetDto() { Id = 6, Symbol = "EURJPY", MarketId = 1 });
            list.Add(new AssetDto() { Id = 7, Symbol = "GBPUSD", MarketId = 1 });
            list.Add(new AssetDto() { Id = 8, Symbol = "USDCAD", MarketId = 1 });
            list.Add(new AssetDto() { Id = 9, Symbol = "Microsoft", MarketId = 2 });
            list.Add(new AssetDto() { Id = 10, Symbol = "Apple", MarketId = 2 });
            return list;
        }
        
        #endregion INFRASTRUCTURE


        #region GET_ASSETS

        [TestMethod]
        public void GetAllAssets_returnProperDtoCollection()
        {

            //Arrange
            EFAssetRepository repository = new EFAssetRepository();
            IEnumerable<AssetDto> assetDtos = getDefaultAssetDtos();
            insertAssetsToTestDb(assetDtos);

            //Act
            AssetDto[] dtos = repository.GetAllAssets().ToArray();

            //Assert
            bool areEqualArrays = assetDtos.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        #endregion GET_ASSETS


        #region GET_ASSETS_F0R_MARKET

        [TestMethod]
        public void GetAssetsForMarket_returnEmptyCollection_IfThereIsNoItemForGivenMarketId()
        {

            //Arrange
            EFAssetRepository repository = new EFAssetRepository();
            IEnumerable<AssetDto> assetDtos = getDefaultAssetDtos();
            insertAssetsToTestDb(assetDtos);

            //Act
            int marketId = 3;
            AssetDto[] dtos = repository.GetAssetsForMarket(marketId).ToArray();

            //Assert
            bool isEmptyArray = (dtos.Length == 0);
            Assert.IsTrue(isEmptyArray);

        }


        [TestMethod]
        public void GetAssetsForMarket_returnProperDtoCollection()
        {

            //Arrange
            EFAssetRepository repository = new EFAssetRepository();
            IEnumerable<AssetDto> assetDtos = getDefaultAssetDtos();
            insertAssetsToTestDb(assetDtos);

            //Act
            int marketId = 2;
            AssetDto[] dtos = repository.GetAssetsForMarket(marketId).ToArray();

            //Assert
            IEnumerable<AssetDto> expectedDtos = assetDtos.Where(a => a.MarketId == marketId);
            bool areEqualArrays = expectedDtos.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }


        #endregion GET_ASSETS_F0R_MARKET


        #region FILTER_ASSETS



        #endregion FILTER_ASSETS


        #region GET_ASSET_BY_ID

        [TestMethod]
        public void GetAssetById_returnProperDto_forExistingItem()
        {

            //Arrange
            EFAssetRepository repository = new EFAssetRepository();
            AssetDto baseDto = getDefaultAssetDto();
            insertAssetToTestDb(baseDto);

            //Act
            AssetDto dto = repository.GetAssetById(baseDto.Id);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetAssetById_returnNull_forNonExistingItem()
        {

            //Arrange
            EFAssetRepository repository = new EFAssetRepository();
            AssetDto baseDto = getDefaultAssetDto();
            insertAssetToTestDb(baseDto);

            //Act
            AssetDto dto = repository.GetAssetById(baseDto.Id + 1);

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_ASSET_BY_ID


        #region GET_ASSET_BY_SYMBOL

        [TestMethod]
        public void GetAssetBySymbol_returnProperDto_forExistingItem()
        {

            //Arrange
            EFAssetRepository repository = new EFAssetRepository();
            AssetDto baseDto = getDefaultAssetDto();
            insertAssetToTestDb(baseDto);

            //Act
            AssetDto dto = repository.GetAssetBySymbol(baseDto.Symbol);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetAssetBySymbol_returnNull_forNonExistingItem()
        {

            //Arrange
            EFAssetRepository repository = new EFAssetRepository();
            AssetDto baseDto = getDefaultAssetDto();
            insertAssetToTestDb(baseDto);

            //Act
            AssetDto dto = repository.GetAssetBySymbol(baseDto.Symbol + "a");

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_ASSET_BY_SYMBOL



    }

}