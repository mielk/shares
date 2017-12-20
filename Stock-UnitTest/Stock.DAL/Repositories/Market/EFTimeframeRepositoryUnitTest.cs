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
    public class EFTimeframeRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string TIMEFRAMES_TABLE_NAME = "timeframes";
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_SYMBOL = "M5";
        private const int DEFAULT_PERIOD_COUNTER = 5;
        private const bool DEFAULT_IS_ACTIVE = true;
        private const string DEFAULT_PERIOD_UNIT = "MINUTE";


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


        private void insertTimeframeToTestDb(TimeframeDto timeframe)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Symbol, PeriodCounter, PeriodUnit) VALUES({2}, {3}, {4}, {5});";
            string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, TIMEFRAMES_TABLE_NAME,
                                                timeframe.Id, timeframe.Symbol.ToDbString(),
                                                timeframe.PeriodCounter, timeframe.PeriodUnit.ToDbString());

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, TIMEFRAMES_TABLE_NAME);
                context.Database.ExecuteSqlCommand(insertSql);
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        private void insertTimeframesToTestDb(IEnumerable<TimeframeDto> timeframes)
        {
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, Symbol, PeriodCounter, PeriodUnit) VALUES({2}, {3}, {4}, {5});";

            DbContext context = new UnitTestsDbContext();
            try
            {
                context.Database.BeginTransaction();
                context.ClearTable(UNIT_TEST_DB_NAME, TIMEFRAMES_TABLE_NAME);
                foreach (var timeframe in timeframes)
                {
                    string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, TIMEFRAMES_TABLE_NAME,
                                                        timeframe.Id, timeframe.Symbol.ToDbString(),
                                                        timeframe.PeriodCounter, timeframe.PeriodUnit.ToDbString());
                    context.Database.ExecuteSqlCommand(insertSql);
                }
                context.Database.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                context.Database.CurrentTransaction.Rollback();
            }

        }

        
        private TimeframeDto getDefaultTimeframeDto()
        {
            return new TimeframeDto() {
                Id = DEFAULT_ID,
                Symbol = DEFAULT_SYMBOL,
                PeriodCounter = DEFAULT_PERIOD_COUNTER,
                PeriodUnit = DEFAULT_PERIOD_UNIT
            };
        }

        private IEnumerable<TimeframeDto> getDefaultTimeframeDtos()
        {
            List<TimeframeDto> list = new List<TimeframeDto>();
            list.Add(new TimeframeDto() { Id = 1, Symbol = "M5", PeriodCounter = 5, PeriodUnit = "MINUTE" });
            list.Add(new TimeframeDto() { Id = 2, Symbol = "M15", PeriodCounter = 15, PeriodUnit = "MINUTE" });
            list.Add(new TimeframeDto() { Id = 3, Symbol = "M30", PeriodCounter = 30, PeriodUnit = "MINUTE" });
            list.Add(new TimeframeDto() { Id = 4, Symbol = "H1", PeriodCounter = 1, PeriodUnit = "HOUR" });
            list.Add(new TimeframeDto() { Id = 5, Symbol = "H4", PeriodCounter = 4, PeriodUnit = "HOUR" });
            list.Add(new TimeframeDto() { Id = 6, Symbol = "D1", PeriodCounter = 1, PeriodUnit = "DAY" });
            list.Add(new TimeframeDto() { Id = 7, Symbol = "W1", PeriodCounter = 1, PeriodUnit = "WEEK" });
            list.Add(new TimeframeDto() { Id = 8, Symbol = "MN1", PeriodCounter = 1, PeriodUnit = "MONTH" });
            return list;
        }
        
        #endregion INFRASTRUCTURE


        #region GET_TIMEFRAMES

        [TestMethod]
        public void GetAllTimeframes_returnProperDtoCollection()
        {

            //Arrange
            EFTimeframeRepository repository = new EFTimeframeRepository();
            IEnumerable<TimeframeDto> timeframeDtos = getDefaultTimeframeDtos();
            insertTimeframesToTestDb(timeframeDtos);

            //Act
            TimeframeDto[] dtos = repository.GetAllTimeframes().ToArray();

            //Assert
            bool areEqualArrays = timeframeDtos.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        #endregion GET_TIMEFRAMES


        #region GET_TIMEFRAME_BY_ID

        [TestMethod]
        public void GetTimeframeById_returnProperDto_forExistingItem()
        {

            //Arrange
            EFTimeframeRepository repository = new EFTimeframeRepository();
            TimeframeDto baseDto = getDefaultTimeframeDto();
            insertTimeframeToTestDb(baseDto);

            //Act
            TimeframeDto dto = repository.GetTimeframeById(baseDto.Id);

            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetTimeframeById_returnNull_forNonExistingItem()
        {

            //Arrange
            EFTimeframeRepository repository = new EFTimeframeRepository();
            TimeframeDto baseDto = getDefaultTimeframeDto();
            insertTimeframeToTestDb(baseDto);

            //Act
            TimeframeDto dto = repository.GetTimeframeById(baseDto.Id + 21);

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_TIMEFRAME_BY_ID


        #region GET_TIMEFRAME_BY_SYMBOL

        [TestMethod]
        public void GetTimeframeByName_returnProperDto_forExistingItem()
        {

            //Arrange
            EFTimeframeRepository repository = new EFTimeframeRepository();
            TimeframeDto baseDto = getDefaultTimeframeDto();
            insertTimeframeToTestDb(baseDto);

            //Act
            TimeframeDto dto = repository.GetTimeframeBySymbol(baseDto.Symbol);
                
            //Assert
            var areEqual = baseDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetTimeframeByName_returnNull_forNonExistingItem()
        {

            //Arrange
            EFTimeframeRepository repository = new EFTimeframeRepository();
            TimeframeDto baseDto = getDefaultTimeframeDto();
            insertTimeframeToTestDb(baseDto);

            //Act
            TimeframeDto dto = repository.GetTimeframeBySymbol(baseDto.Symbol + "a");

            //Assert
            Assert.IsNull(dto);

        }

        #endregion GET_TIMEFRAME_BY_SYMBOL


    }

}