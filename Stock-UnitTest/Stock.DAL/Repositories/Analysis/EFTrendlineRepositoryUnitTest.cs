using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Infrastructure;
using Stock.DAL.TransferObjects;
using Stock.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.DAL.Helpers;
using Stock.Utils;
using Stock.Core;

namespace Stock_UnitTest.Stock.DAL.Repositories.Data
{
    [TestClass]
    public class EFTrendlineRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string TRENDLINES_TABLE_NAME = "trendlines";
        private const string TREND_HITS_TABLE_NAME = "trend_hits";
        private const string TREND_BREAKS_TABLE_NAME = "trend_breaks";
        private const string TREND_RANGES_TABLE_NAME = "trend_ranges";
        private const int DEFAULT_ID = 1;
        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const int DEFAULT_SIMULATION_ID = 1;
        private const int DEFAULT_START_INDEX = 87;
        private int? DEFAULT_END_INDEX = null;
        private const double DEFAULT_START_LEVEL = 1.1654;
        private const int DEFAULT_FOOTHOLD_INDEX = 100;
        private const double DEFAULT_FOOTHOLD_LEVEL = 1.1754;
        private const double DEFAULT_VALUE = 1.234;
        private const int DEFAULT_LAST_UPDATE_INDEX = 104;
        //TrendHit
        private const string DEFAULT_GUID = "74017f2d-9dfe-494e-bfa0-93c09418cfb7";
        private const int DEFAULT_TRENDLINE_ID = 1;
        private const int DEFAULT_INDEX_NUMBER = 2;
        private const int DEFAULT_EXTREMUM_TYPE = 2;
        private const double DEFAULT_DISTANCE_TO_LINE = 0.134;
        private const string DEFAULT_PREVIOUS_RANGE_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_NEXT_RANGE_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";
        //TrendRange
        private const int DEFAULT_QUOTATIONS_COUNTER = 11;
        private const double DEFAULT_TOTAL_DISTANCE = 0.12d;
        private const string DEFAULT_PREVIOUS_BREAK_GUID = "45e223ec-cd32-4eca-8d38-0d96d3ee121b";
        private const string DEFAULT_PREVIOUS_HIT_TYPE = null;
        private const string DEFAULT_NEXT_BREAK_TYPE = null;
        private const string DEFAULT_NEXT_HIT_GUID = "a9139a25-6d38-4c05-bbc7-cc413d6feee9";





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

        private void clearTrendlinesTables()
        {
            DbContext context = new UnitTestsDbContext();
            context.ClearTable(UNIT_TEST_DB_NAME, TRENDLINES_TABLE_NAME);
        }

        private void clearTrendHitsTables()
        {
            DbContext context = new UnitTestsDbContext();
            context.ClearTable(UNIT_TEST_DB_NAME, TREND_HITS_TABLE_NAME);
        }

        private void clearTrendBreaksTables()
        {
            DbContext context = new UnitTestsDbContext();
            context.ClearTable(UNIT_TEST_DB_NAME, TREND_BREAKS_TABLE_NAME);
        }

        private void clearTrendRangesTables()
        {
            DbContext context = new UnitTestsDbContext();
            context.ClearTable(UNIT_TEST_DB_NAME, TREND_RANGES_TABLE_NAME);
        }


        private TrendlineDto getDefaultTrendlineDto()
        {
            return new TrendlineDto() {
                Id = DEFAULT_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                StartIndex = DEFAULT_START_INDEX,
                StartLevel = DEFAULT_START_LEVEL,
                FootholdIndex = DEFAULT_FOOTHOLD_INDEX,
                FootholdLevel = DEFAULT_FOOTHOLD_LEVEL,
                EndIndex = DEFAULT_END_INDEX,
                Value = DEFAULT_VALUE,
                LastUpdateIndex = DEFAULT_LAST_UPDATE_INDEX
            };
        }

        private TrendHitDto getDefaultTrendHitDto()
        {
            return new TrendHitDto()
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                ExtremumType = DEFAULT_EXTREMUM_TYPE,
                Value = DEFAULT_VALUE,
                DistanceToLine = DEFAULT_DISTANCE_TO_LINE,
                PreviousRangeGuid = DEFAULT_PREVIOUS_RANGE_GUID,
                NextRangeGuid = DEFAULT_NEXT_RANGE_GUID
            };
        }

        #endregion INFRASTRUCTURE




        #region UPDATE_TRENDLINES

        [TestMethod]
        public void UpdateTrendlines_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            TrendlineDto dto1 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 5, StartLevel = 1.2345, FootholdIndex = 26, FootholdLevel = 1.3456, Value = 1.234, LastUpdateIndex = 31 };
            TrendlineDto dto2 = new TrendlineDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 6, StartLevel = 1.4567, FootholdIndex = 23, FootholdLevel = 1.5678, Value = 1.345, LastUpdateIndex = 29 };
            TrendlineDto dto3 = new TrendlineDto() { Id = 3, AssetId = 1, TimeframeId = 1, SimulationId = 2, StartIndex = 12, StartLevel = 1.5678, FootholdIndex = 45, FootholdLevel = 1.6789, Value = 1.567, LastUpdateIndex = 47 };
            TrendlineDto dto4 = new TrendlineDto() { Id = 4, AssetId = 1, TimeframeId = 1, SimulationId = 2, StartIndex = 8, StartLevel = 1.6789, FootholdIndex = 21, FootholdLevel = 1.7891, Value = 1.678, LastUpdateIndex = 29 };
            trendlines.AddRange(new TrendlineDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearTrendlinesTables();
            repository.UpdateTrendlines(trendlines);
            IEnumerable<TrendlineDto> actualRecords = repository.GetTrendlines(1, 1, 1);

            //Assert
            Assert.IsTrue(repository.GetTrendlines(1, 1, 1).HasEqualItems(new TrendlineDto[] { dto1, dto2 }));
            Assert.IsTrue(repository.GetTrendlines(1, 1, 2).HasEqualItems(new TrendlineDto[] { dto3, dto4 }));

        }

        [TestMethod]
        public void UpdateTrendlines_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            TrendlineDto dto1 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 5, StartLevel = 1.2345, FootholdIndex = 26, FootholdLevel = 1.3456, Value = 1.234, LastUpdateIndex = 31 };
            TrendlineDto dto2 = new TrendlineDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 6, StartLevel = 1.4567, FootholdIndex = 23, FootholdLevel = 1.5678, Value = 1.345, LastUpdateIndex = 29 };
            TrendlineDto dto3 = new TrendlineDto() { Id = 3, AssetId = 1, TimeframeId = 1, SimulationId = 2, StartIndex = 12, StartLevel = 1.5678, FootholdIndex = 45, FootholdLevel = 1.6789, Value = 1.567, LastUpdateIndex = 47 };
            TrendlineDto dto4 = new TrendlineDto() { Id = 4, AssetId = 1, TimeframeId = 1, SimulationId = 2, StartIndex = 8, StartLevel = 1.6789, FootholdIndex = 21, FootholdLevel = 1.7891, Value = 1.678, LastUpdateIndex = 29 };
            trendlines.AddRange(new TrendlineDto[] { dto1, dto2, dto3, dto4 });
            clearTrendlinesTables();
            repository.UpdateTrendlines(trendlines);

            //Act
            dto1.Value = 2.345;
            dto2.Value = 3.456;
            dto3.Value = 4.567;
            repository.UpdateTrendlines(new TrendlineDto[] { dto1, dto2, dto3, dto4 });

            //Assert
            Assert.IsTrue(repository.GetTrendlines(1, 1, 1).HasEqualItems(new TrendlineDto[] { dto1, dto2 }));
            Assert.IsTrue(repository.GetTrendlines(1, 1, 2).HasEqualItems(new TrendlineDto[] { dto3, dto4 }));

        }

        [TestMethod]
        public void UpdateTrendlines_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            TrendlineDto dto1 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 5, StartLevel = 1.2345, FootholdIndex = 26, FootholdLevel = 1.3456, Value = 1.234, LastUpdateIndex = 31 };
            TrendlineDto dto2 = new TrendlineDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 6, StartLevel = 1.4567, FootholdIndex = 23, FootholdLevel = 1.5678, Value = 1.345, LastUpdateIndex = 29 };
            TrendlineDto dto3 = new TrendlineDto() { Id = 3, AssetId = 1, TimeframeId = 1, SimulationId = 2, StartIndex = 12, StartLevel = 1.5678, FootholdIndex = 45, FootholdLevel = 1.6789, Value = 1.567, LastUpdateIndex = 47 };
            TrendlineDto dto4 = new TrendlineDto() { Id = 4, AssetId = 1, TimeframeId = 1, SimulationId = 2, StartIndex = 8, StartLevel = 1.6789, FootholdIndex = 21, FootholdLevel = 1.7891, Value = 1.678, LastUpdateIndex = 29 };
            trendlines.AddRange(new TrendlineDto[] { dto1, dto2 });
            clearTrendlinesTables();
            repository.UpdateTrendlines(trendlines);

            //Act
            dto1.Value = 2.345;
            dto2.LastUpdateIndex += 2;
            dto2.Value += 0.250;
            dto2.LastUpdateIndex = 32;
            repository.UpdateTrendlines(new TrendlineDto[] { dto1, dto2, dto3, dto4 });

            //Assert
            Assert.IsTrue(repository.GetTrendlines(1, 1, 1).HasEqualItems(new TrendlineDto[] { dto1, dto2 }));
            Assert.IsTrue(repository.GetTrendlines(1, 1, 2).HasEqualItems(new TrendlineDto[] { dto3, dto4 }));

        }

        #endregion UPDATE_TRENDLINES


        #region GET_TRENDLINES

        [TestMethod]
        public void GetTrendlines_returnProperDtoCollection()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            TrendlineDto dto1 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 5, StartLevel = 1.2573, FootholdIndex = 27, FootholdLevel = 1.2871, Value = 1.54, LastUpdateIndex = 70 };
            TrendlineDto dto2 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 2, SimulationId = 1, StartIndex = 7, StartLevel = 1.0123, FootholdIndex = 52, FootholdLevel = 1.4865, Value = 1.54, LastUpdateIndex = 70 };
            TrendlineDto dto3 = new TrendlineDto() { Id = 1, AssetId = 2, TimeframeId = 1, SimulationId = 1, StartIndex = 7, StartLevel = 1.1234, FootholdIndex = 60, FootholdLevel = 1.4564, Value = 1.54, LastUpdateIndex = 70 };
            TrendlineDto dto4 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 25, StartLevel = 1.3456, FootholdIndex = 48, FootholdLevel = 1.4564, Value = 1.54, LastUpdateIndex = 70 };
            trendlines.AddRange(new TrendlineDto[] { dto1, dto2, dto3, dto4 });
            clearTrendlinesTables(); 
            repository.UpdateTrendlines(trendlines);

            //Act
            IEnumerable<TrendlineDto> dtos = repository.GetTrendlines(1, 1, 1).ToArray();

            //Assert
            IEnumerable<TrendlineDto> expected = new TrendlineDto[] { dto1, dto4 };
            bool areEqualArrays = expected.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetTrendlineById_ReturnsNull_IfThereIsNoTrendlineWithSuchId()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            trendlines.AddRange(new TrendlineDto[] { getDefaultTrendlineDto() });
            clearTrendlinesTables();
            repository.UpdateTrendlines(trendlines);

            //Act
            TrendlineDto resultDto = repository.GetTrendlineById(2);

            //Assert
            Assert.IsNull(resultDto);

        }

        [TestMethod]
        public void GetTrendlineById_ReturnsProperTrendlineDto_IfExists()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            TrendlineDto expectedDto = getDefaultTrendlineDto();
            trendlines.AddRange(new TrendlineDto[] { expectedDto });
            clearTrendlinesTables();
            repository.UpdateTrendlines(trendlines);

            //Act
            TrendlineDto dto = repository.GetTrendlineById(expectedDto.Id);

            //Assert
            var areEqual = expectedDto.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        #endregion GET_TRENDLINES


        #region REMOVE_TRENDLINES

        [TestMethod]
        public void RemoveTrendlines_AfterRemovingTrendline_ItDoesntExistInDatabaseAnymore()
        {
            
            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            TrendlineDto dto1 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 5, StartLevel = 1.2345, FootholdIndex = 26, FootholdLevel = 1.3456, Value = 1.234, LastUpdateIndex = 31 };
            TrendlineDto dto2 = new TrendlineDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 6, StartLevel = 1.4567, FootholdIndex = 23, FootholdLevel = 1.5678, Value = 1.345, LastUpdateIndex = 29 };
            TrendlineDto dto3 = new TrendlineDto() { Id = 3, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 12, StartLevel = 1.5678, FootholdIndex = 45, FootholdLevel = 1.6789, Value = 1.567, LastUpdateIndex = 47 };
            TrendlineDto dto4 = new TrendlineDto() { Id = 4, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 8, StartLevel = 1.6789, FootholdIndex = 21, FootholdLevel = 1.7891, Value = 1.678, LastUpdateIndex = 29 };
            trendlines.AddRange(new TrendlineDto[] { dto1, dto2, dto3, dto4 });
            clearTrendlinesTables();
            repository.UpdateTrendlines(trendlines);
            IEnumerable<TrendlineDto> actualRecords = repository.GetTrendlines(1, 1, 1);
            if (actualRecords.Count() != 4)
            {
                throw new Exception("Test failed while preparing environment");
            }

            //Act
            repository.RemoveTrendlines(new TrendlineDto[] { dto1, dto3 });

            //Assert
            IEnumerable<TrendlineDto> recordsAfterRemoving = repository.GetTrendlines(1, 1, 1);
            IEnumerable<TrendlineDto> expectedRecords = new TrendlineDto[] { dto2, dto4 };
            Assert.IsTrue(recordsAfterRemoving.HasEqualItems(expectedRecords));

        }

        [TestMethod]
        public void RemoveTrendlines_IfNotExistingTrendlineIsPassed_NothingHappens()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendlineDto> trendlines = new List<TrendlineDto>();
            TrendlineDto dto1 = new TrendlineDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 5, StartLevel = 1.2345, FootholdIndex = 26, FootholdLevel = 1.3456, Value = 1.234, LastUpdateIndex = 31 };
            TrendlineDto dto2 = new TrendlineDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 6, StartLevel = 1.4567, FootholdIndex = 23, FootholdLevel = 1.5678, Value = 1.345, LastUpdateIndex = 29 };
            TrendlineDto dto3 = new TrendlineDto() { Id = 3, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 12, StartLevel = 1.5678, FootholdIndex = 45, FootholdLevel = 1.6789, Value = 1.567, LastUpdateIndex = 47 };
            TrendlineDto dto4 = new TrendlineDto() { Id = 4, AssetId = 1, TimeframeId = 1, SimulationId = 1, StartIndex = 8, StartLevel = 1.6789, FootholdIndex = 21, FootholdLevel = 1.7891, Value = 1.678, LastUpdateIndex = 29 };
            trendlines.AddRange(new TrendlineDto[] { dto1, dto2, dto3 });
            clearTrendlinesTables();
            repository.UpdateTrendlines(trendlines);
            IEnumerable<TrendlineDto> actualRecords = repository.GetTrendlines(1, 1, 1);
            if (actualRecords.Count() != 3)
            {
                throw new Exception("Test failed while preparing environment");
            }

            //Act
            repository.RemoveTrendlines(new TrendlineDto[] { dto4 });

            //Assert
            IEnumerable<TrendlineDto> recordsAfterRemoving = repository.GetTrendlines(1, 1, 1);
            IEnumerable<TrendlineDto> expectedRecords = new TrendlineDto[] { dto1, dto2, dto3 };
            Assert.IsTrue(recordsAfterRemoving.HasEqualItems(expectedRecords));

        }

        #endregion REMOVE_TRENDLINES



        #region UPDATE_TREND_HITS

        [TestMethod]
        public void UpdateTrendHits_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendHitDto> trendlines = new List<TrendHitDto>();
            TrendHitDto dto1 = new TrendHitDto() { Id = 1, Guid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", TrendlineId = 1, IndexNumber = 2, ExtremumType = 1, Value = 1.234, DistanceToLine = 0.0004, PreviousRangeGuid = null, NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendHitDto dto2 = new TrendHitDto() { Id = 2, Guid = "89BFF378-F310-4A28-B753-00A0FF9A852C", TrendlineId = 1, IndexNumber = 9, ExtremumType = 2, Value = 1.345, DistanceToLine = 0.0007, PreviousRangeGuid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", NextRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58" };
            TrendHitDto dto3 = new TrendHitDto() { Id = 3, Guid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", TrendlineId = 1, IndexNumber = 18, ExtremumType = 2, Value = 1.567, DistanceToLine = 0.0002, PreviousRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C", NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendHitDto dto4 = new TrendHitDto() { Id = 4, Guid = "562BED90-29F8-423E-8D00-DE699C1D14C3", TrendlineId = 2, IndexNumber = 21, ExtremumType = 3, Value = 1.678, DistanceToLine = 0.0001, PreviousRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", NextRangeGuid = null };
            trendlines.AddRange(new TrendHitDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearTrendHitsTables();
            repository.UpdateTrendHits(trendlines);
            IEnumerable<TrendHitDto> actualRecords = repository.GetTrendHits(1);

            //Assert
            Assert.IsTrue(repository.GetTrendHits(1).HasEqualItems(new TrendHitDto[] { dto1, dto2, dto3 }));
            Assert.IsTrue(repository.GetTrendHits(2).HasEqualItems(new TrendHitDto[] { dto4 }));

        }

        [TestMethod]
        public void UpdateTrendHits_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendHitDto> trendlines = new List<TrendHitDto>();
            TrendHitDto dto1 = new TrendHitDto() { Id = 1, Guid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", TrendlineId = 1, IndexNumber = 2, ExtremumType = 1, Value = 1.234, DistanceToLine = 0.0004, PreviousRangeGuid = null, NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendHitDto dto2 = new TrendHitDto() { Id = 2, Guid = "89BFF378-F310-4A28-B753-00A0FF9A852C", TrendlineId = 1, IndexNumber = 9, ExtremumType = 2, Value = 1.345, DistanceToLine = 0.0007, PreviousRangeGuid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", NextRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58" };
            TrendHitDto dto3 = new TrendHitDto() { Id = 3, Guid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", TrendlineId = 1, IndexNumber = 18, ExtremumType = 2, Value = 1.567, DistanceToLine = 0.0002, PreviousRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C", NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendHitDto dto4 = new TrendHitDto() { Id = 4, Guid = "562BED90-29F8-423E-8D00-DE699C1D14C3", TrendlineId = 2, IndexNumber = 21, ExtremumType = 3, Value = 1.678, DistanceToLine = 0.0001, PreviousRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", NextRangeGuid = null };
            trendlines.AddRange(new TrendHitDto[] { dto1, dto2, dto3, dto4 });
            clearTrendHitsTables();
            repository.UpdateTrendHits(trendlines);

            //Act
            dto1.Value = 2.345;
            dto2.Value = 3.456;
            dto3.Value = 4.567;
            repository.UpdateTrendHits(new TrendHitDto[] { dto1, dto2, dto3, dto4 });

            //Assert
            Assert.IsTrue(repository.GetTrendHits(1).HasEqualItems(new TrendHitDto[] { dto1, dto2, dto3 }));
            Assert.IsTrue(repository.GetTrendHits(2).HasEqualItems(new TrendHitDto[] { dto4 }));

        }

        [TestMethod]
        public void UpdateTrendHits_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendHitDto> trendlines = new List<TrendHitDto>();
            TrendHitDto dto1 = new TrendHitDto() { Id = 1, Guid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", TrendlineId = 1, IndexNumber = 2, ExtremumType = 1, Value = 1.234, DistanceToLine = 0.0004, PreviousRangeGuid = null, NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendHitDto dto2 = new TrendHitDto() { Id = 2, Guid = "89BFF378-F310-4A28-B753-00A0FF9A852C", TrendlineId = 1, IndexNumber = 9, ExtremumType = 2, Value = 1.345, DistanceToLine = 0.0007, PreviousRangeGuid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", NextRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58" };
            TrendHitDto dto3 = new TrendHitDto() { Id = 3, Guid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", TrendlineId = 1, IndexNumber = 18, ExtremumType = 2, Value = 1.567, DistanceToLine = 0.0002, PreviousRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C", NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendHitDto dto4 = new TrendHitDto() { Id = 4, Guid = "562BED90-29F8-423E-8D00-DE699C1D14C3", TrendlineId = 2, IndexNumber = 21, ExtremumType = 3, Value = 1.678, DistanceToLine = 0.0001, PreviousRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", NextRangeGuid = null };
            trendlines.AddRange(new TrendHitDto[] { dto1, dto2, dto3, dto4 });
            clearTrendHitsTables();
            repository.UpdateTrendHits(trendlines);

            //Act
            dto1.Value = 2.345;
            dto1.DistanceToLine += 0.002;
            dto2.Value += 0.250;
            dto1.DistanceToLine -= 0.002;
            repository.UpdateTrendHits(new TrendHitDto[] { dto1, dto2, dto3, dto4 });

            //Assert
            Assert.IsTrue(repository.GetTrendHits(1).HasEqualItems(new TrendHitDto[] { dto1, dto2, dto3 }));
            Assert.IsTrue(repository.GetTrendHits(2).HasEqualItems(new TrendHitDto[] { dto4 }));

        }

        #endregion UPDATE_TREND_HITS


        #region GET_TREND_HITS

        [TestMethod]
        public void GetTrendHits_returnProperDtoCollection()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendHitDto> trendHits = new List<TrendHitDto>();
            TrendHitDto dto1 = new TrendHitDto() { Id = 1, Guid = "0BF8E6BD-0C8D-43B4-A9A0-C8B2745502B0", TrendlineId = 1, IndexNumber = 10, ExtremumType = 1, DistanceToLine = 0.0005, PreviousRangeGuid = null, NextRangeGuid = "07ACE1F3-89B4-49C5-A6F9-155B78E33836" };
            TrendHitDto dto2 = new TrendHitDto() { Id = 2, Guid = "07ACE1F3-89B4-49C5-A6F9-155B78E33836", TrendlineId = 1, IndexNumber = 31, ExtremumType = 2, DistanceToLine = 0.0003, PreviousRangeGuid = "0BF8E6BD-0C8D-43B4-A9A0-C8B2745502B0", NextRangeGuid = "15AB46D1-615A-46FA-9533-A2DC3EA4F340" };
            TrendHitDto dto3 = new TrendHitDto() { Id = 3, Guid = "15AB46D1-615A-46FA-9533-A2DC3EA4F340", TrendlineId = 1, IndexNumber = 31, ExtremumType = 2, DistanceToLine = 0.0003, PreviousRangeGuid = "07ACE1F3-89B4-49C5-A6F9-155B78E33836", NextRangeGuid = "B49C3C3E-0D42-451F-B2FD-F28A1679BF50" };
            TrendHitDto dto4 = new TrendHitDto() { Id = 4, Guid = "B49C3C3E-0D42-451F-B2FD-F28A1679BF50", TrendlineId = 2, IndexNumber = 31, ExtremumType = 2, DistanceToLine = 0.0003, PreviousRangeGuid = "15AB46D1-615A-46FA-9533-A2DC3EA4F341", NextRangeGuid = null };
            trendHits.AddRange(new TrendHitDto[] { dto1, dto2, dto3, dto4 });
            clearTrendHitsTables();
            repository.UpdateTrendHits(trendHits);

            //Act
            IEnumerable<TrendHitDto> dtos = repository.GetTrendHits(1).ToArray();

            //Assert
            IEnumerable<TrendHitDto> expected = new TrendHitDto[] { dto1, dto2, dto3 };
            bool areEqualArrays = expected.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetTrendHitById_ReturnsNull_IfThereIsNoTrendlineWithSuchId()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendHitDto> trendHits = new List<TrendHitDto>();
            trendHits.AddRange(new TrendHitDto[] { getDefaultTrendHitDto() });
            clearTrendHitsTables();
            repository.UpdateTrendHits(trendHits);

            //Act
            TrendHitDto resultDto = repository.GetTrendHitById(DEFAULT_ID + 1);

            //Assert
            Assert.IsNull(resultDto);

        }

        [TestMethod]
        public void GetTrendHitById_ReturnsProperTrendlineDto_IfExists()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendHitDto> trendHits = new List<TrendHitDto>();
            TrendHitDto expectedDto = getDefaultTrendHitDto();
            trendHits.AddRange(new TrendHitDto[] { expectedDto });
            clearTrendHitsTables();
            repository.UpdateTrendHits(trendHits);

            //Act
            TrendHitDto resultDto = repository.GetTrendHitById(expectedDto.Id);

            //Assert
            var areEqual = expectedDto.Equals(resultDto);
            Assert.IsTrue(areEqual);

        }

        #endregion GET_TREND_HITS




        #region UPDATE TREND BREAKS

        private TrendBreakDto getDefaultTrendBreakDto()
        {
            return new TrendBreakDto()
            {
                Id = DEFAULT_ID,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = DEFAULT_TRENDLINE_ID,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                PreviousRangeGuid = System.Guid.NewGuid().ToString(),
                NextRangeGuid = System.Guid.NewGuid().ToString()
            };
        }

        private TrendBreakDto[] getDefaultTrendBreakDtosArray()
        {
            TrendBreakDto[] arr = new TrendBreakDto[4];
            arr[0] = new TrendBreakDto()
            {
                Id = 1,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 1,
                IndexNumber = 5,
                PreviousRangeGuid = System.Guid.NewGuid().ToString(),
                NextRangeGuid = System.Guid.NewGuid().ToString()
            };
            arr[1] = new TrendBreakDto()
            {
                Id = 2,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 1,
                IndexNumber = 12,
                PreviousRangeGuid = System.Guid.NewGuid().ToString(),
                NextRangeGuid = System.Guid.NewGuid().ToString()
            };
            arr[2] = new TrendBreakDto()
            {
                Id = 3,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 1,
                IndexNumber = 156,
                PreviousRangeGuid = System.Guid.NewGuid().ToString(),
                NextRangeGuid = System.Guid.NewGuid().ToString()
            };
            arr[3] = new TrendBreakDto()
            {
                Id = 4,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 2,
                IndexNumber = 187,
                PreviousRangeGuid = System.Guid.NewGuid().ToString(),
                NextRangeGuid = null
            };
            return arr;
        }

        [TestMethod]
        public void UpdateTrendBreaks_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendBreakDto> trendBreaks = new List<TrendBreakDto>();
            TrendBreakDto dto1 = new TrendBreakDto() { Id = 1, Guid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", TrendlineId = 1, IndexNumber = 2, PreviousRangeGuid = null, NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendBreakDto dto2 = new TrendBreakDto() { Id = 2, Guid = "89BFF378-F310-4A28-B753-00A0FF9A852C", TrendlineId = 1, IndexNumber = 9, PreviousRangeGuid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", NextRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58" };
            TrendBreakDto dto3 = new TrendBreakDto() { Id = 3, Guid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", TrendlineId = 1, IndexNumber = 18, PreviousRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C", NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendBreakDto dto4 = new TrendBreakDto() { Id = 4, Guid = "562BED90-29F8-423E-8D00-DE699C1D14C3", TrendlineId = 2, IndexNumber = 21, PreviousRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", NextRangeGuid = null };
            trendBreaks.AddRange(new TrendBreakDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearTrendBreaksTables();
            repository.UpdateTrendBreaks(trendBreaks);
            IEnumerable<TrendBreakDto> actualRecords = repository.GetTrendBreaks(1);

            //Assert
            Assert.IsTrue(repository.GetTrendBreaks(1).HasEqualItems(new TrendBreakDto[] { dto1, dto2, dto3 }));
            Assert.IsTrue(repository.GetTrendBreaks(2).HasEqualItems(new TrendBreakDto[] { dto4 }));

        }

        [TestMethod]
        public void UpdateTrendBreaks_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();

            List<TrendBreakDto> trendBreaks = new List<TrendBreakDto>();
            TrendBreakDto dto1 = new TrendBreakDto() { Id = 1, Guid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", TrendlineId = 1, IndexNumber = 2, PreviousRangeGuid = null, NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendBreakDto dto2 = new TrendBreakDto() { Id = 2, Guid = "89BFF378-F310-4A28-B753-00A0FF9A852C", TrendlineId = 1, IndexNumber = 9, PreviousRangeGuid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", NextRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58" };
            TrendBreakDto dto3 = new TrendBreakDto() { Id = 3, Guid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", TrendlineId = 1, IndexNumber = 18, PreviousRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C", NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendBreakDto dto4 = new TrendBreakDto() { Id = 4, Guid = "562BED90-29F8-423E-8D00-DE699C1D14C3", TrendlineId = 2, IndexNumber = 21, PreviousRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", NextRangeGuid = null };
            trendBreaks.AddRange(new TrendBreakDto[] { dto1, dto2, dto3, dto4 });
            clearTrendBreaksTables();
            repository.UpdateTrendBreaks(trendBreaks);

            //Act
            dto1.NextRangeGuid = System.Guid.NewGuid().ToString();
            dto2.NextRangeGuid = System.Guid.NewGuid().ToString();
            dto3.NextRangeGuid = System.Guid.NewGuid().ToString();
            repository.UpdateTrendBreaks(new TrendBreakDto[] { dto1, dto2, dto3, dto4 });

            //Assert
            Assert.IsTrue(repository.GetTrendBreaks(1).HasEqualItems(new TrendBreakDto[] { dto1, dto2, dto3 }));
            Assert.IsTrue(repository.GetTrendBreaks(2).HasEqualItems(new TrendBreakDto[] { dto4 }));

        }

        [TestMethod]
        public void UpdateTrendBreaks_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();

            List<TrendBreakDto> trendBreaks = new List<TrendBreakDto>();
            TrendBreakDto dto1 = new TrendBreakDto() { Id = 1, Guid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", TrendlineId = 1, IndexNumber = 2, PreviousRangeGuid = null, NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendBreakDto dto2 = new TrendBreakDto() { Id = 2, Guid = "89BFF378-F310-4A28-B753-00A0FF9A852C", TrendlineId = 1, IndexNumber = 9, PreviousRangeGuid = "AC180C9B-E6D2-4138-8E0A-BE31FCE8626D", NextRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58" };
            TrendBreakDto dto3 = new TrendBreakDto() { Id = 3, Guid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", TrendlineId = 1, IndexNumber = 18, PreviousRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C", NextRangeGuid = "89BFF378-F310-4A28-B753-00A0FF9A852C" };
            TrendBreakDto dto4 = new TrendBreakDto() { Id = 4, Guid = "562BED90-29F8-423E-8D00-DE699C1D14C3", TrendlineId = 2, IndexNumber = 21, PreviousRangeGuid = "A62DB207-FDDA-45B4-94F6-AE16F4CA9A58", NextRangeGuid = null };
            trendBreaks.AddRange(new TrendBreakDto[] { dto1, dto2 });
            clearTrendBreaksTables();
            repository.UpdateTrendBreaks(trendBreaks);

            //Act
            dto1.NextRangeGuid = System.Guid.NewGuid().ToString();
            dto2.NextRangeGuid = System.Guid.NewGuid().ToString();
            repository.UpdateTrendBreaks(new TrendBreakDto[] { dto1, dto2, dto3, dto4 });

            //Assert
            Assert.IsTrue(repository.GetTrendBreaks(1).HasEqualItems(new TrendBreakDto[] { dto1, dto2, dto3 }));
            Assert.IsTrue(repository.GetTrendBreaks(2).HasEqualItems(new TrendBreakDto[] { dto4 }));

        }


        #endregion UPDATE TREND BREAKS


        #region GET TREND BREAKS


        [TestMethod]
        public void GetTrendBreaks_returnProperDtoCollection()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            TrendBreakDto[] breaks = getDefaultTrendBreakDtosArray();
            clearTrendBreaksTables();
            repository.UpdateTrendBreaks(breaks);

            //Act
            IEnumerable<TrendBreakDto> dtos = repository.GetTrendBreaks(1).ToArray();

            //Assert
            IEnumerable<TrendBreakDto> expected = new TrendBreakDto[] { breaks[0], breaks[1], breaks[2] };
            bool areEqualArrays = expected.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetTrendBreakById_ReturnsNull_IfThereIsNoTrendlineWithSuchId()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendBreakDto> trendBreaks = new List<TrendBreakDto>();
            trendBreaks.AddRange(new TrendBreakDto[] { getDefaultTrendBreakDto() });
            clearTrendBreaksTables();
            repository.UpdateTrendBreaks(trendBreaks);

            //Act
            TrendBreakDto resultDto = repository.GetTrendBreakById(50);

            //Assert
            Assert.IsNull(resultDto);

        }

        [TestMethod]
        public void GetTrendBreakById_ReturnsProperTrendlineDto_IfExists()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendBreakDto> trendBreaks = new List<TrendBreakDto>();
            TrendBreakDto expectedDto = getDefaultTrendBreakDto();
            trendBreaks.AddRange(new TrendBreakDto[] { expectedDto });
            clearTrendBreaksTables();
            repository.UpdateTrendBreaks(trendBreaks);

            //Act
            TrendBreakDto resultDto = repository.GetTrendBreakById(expectedDto.Id);

            //Assert
            var areEqual = expectedDto.Equals(resultDto);
            Assert.IsTrue(areEqual);

        }


        #endregion GET TREND BREAKS




        #region UPDATE TREND RANGES

        private TrendRangeDto getDefaultTrendRangeDto()
        {
            return new TrendRangeDto()
            {
                Id = DEFAULT_ID,
                Guid = DEFAULT_GUID,
                TrendlineId = DEFAULT_TRENDLINE_ID,
                StartIndex = DEFAULT_START_INDEX,
                EndIndex = DEFAULT_FOOTHOLD_INDEX,
                QuotationsCounter = DEFAULT_QUOTATIONS_COUNTER,
                TotalDistance = DEFAULT_TOTAL_DISTANCE,
                PreviousBreakGuid = DEFAULT_PREVIOUS_BREAK_GUID,
                PreviousHitGuid = DEFAULT_PREVIOUS_HIT_TYPE,
                NextBreakGuid = DEFAULT_NEXT_BREAK_TYPE,
                NextHitGuid = DEFAULT_NEXT_HIT_GUID
            };
        }

        private TrendRangeDto[] getDefaultTrendRangeDtosArray()
        {
            TrendRangeDto[] arr = new TrendRangeDto[4];
            arr[0] = new TrendRangeDto()
            {
                Id = 1,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 1,
                StartIndex = 2,
                EndIndex = 9,
                QuotationsCounter = 8,
                TotalDistance = 1.23,
                Value = 21.04,
                PreviousBreakGuid = null,
                PreviousHitGuid = System.Guid.NewGuid().ToString(),
                NextBreakGuid = System.Guid.NewGuid().ToString(),
                NextHitGuid = null
            };
            arr[1] = new TrendRangeDto()
            {
                Id = 2,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 1,
                StartIndex = 10,
                EndIndex = 18,
                QuotationsCounter = 8,
                TotalDistance = 1.34,
                Value = 22.04,
                PreviousBreakGuid = System.Guid.NewGuid().ToString(),
                PreviousHitGuid = null,
                NextHitGuid = System.Guid.NewGuid().ToString(),
                NextBreakGuid = null
            };
            arr[2] = new TrendRangeDto()
            {
                Id = 3,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 1,
                StartIndex = 19,
                EndIndex = null,
                QuotationsCounter = 10,
                TotalDistance = 1.23,
                Value = 23.04,
                PreviousBreakGuid = null,
                PreviousHitGuid = System.Guid.NewGuid().ToString(),
                NextBreakGuid = System.Guid.NewGuid().ToString(),
                NextHitGuid = null
            };
            arr[3] = new TrendRangeDto()
            {
                Id = 4,
                Guid = System.Guid.NewGuid().ToString(),
                TrendlineId = 2,
                StartIndex = 10,
                EndIndex = 18,
                QuotationsCounter = 8,
                TotalDistance = 1.34,
                Value = 22.04,
                PreviousBreakGuid = System.Guid.NewGuid().ToString(),
                PreviousHitGuid = null,
                NextHitGuid = System.Guid.NewGuid().ToString(),
                NextBreakGuid = null
            };
            return arr;
        }

        [TestMethod]
        public void UpdateTrendRanges_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            TrendRangeDto[] trendRanges = getDefaultTrendRangeDtosArray();

            //Act
            clearTrendRangesTables();
            repository.UpdateTrendRanges(trendRanges);
            IEnumerable<TrendRangeDto> actualRecords = repository.GetTrendRanges(1);

            //Assert
            Assert.IsTrue(repository.GetTrendRanges(1).HasEqualItems(new TrendRangeDto[] { trendRanges[0], trendRanges[1], trendRanges[2] }));
            Assert.IsTrue(repository.GetTrendRanges(2).HasEqualItems(new TrendRangeDto[] { trendRanges[3] }));

        }

        [TestMethod]
        public void UpdateTrendRanges_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();

            TrendRangeDto[] trendRanges = getDefaultTrendRangeDtosArray();
            clearTrendRangesTables();
            repository.UpdateTrendRanges(trendRanges);

            //Act
            trendRanges[0].NextBreakGuid = System.Guid.NewGuid().ToString();
            trendRanges[1].NextBreakGuid = System.Guid.NewGuid().ToString();
            trendRanges[2].NextHitGuid = System.Guid.NewGuid().ToString();
            repository.UpdateTrendRanges(trendRanges);

            //Assert
            Assert.IsTrue(repository.GetTrendRanges(1).HasEqualItems(new TrendRangeDto[] { trendRanges[0], trendRanges[1], trendRanges[2] }));
            Assert.IsTrue(repository.GetTrendRanges(2).HasEqualItems(new TrendRangeDto[] { trendRanges[3] }));

        }

        [TestMethod]
        public void UpdateTrendRanges_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFTrendlineRepository repository = new EFTrendlineRepository();

            TrendRangeDto[] trendRanges = getDefaultTrendRangeDtosArray();
            clearTrendRangesTables();
            repository.UpdateTrendRanges(new TrendRangeDto[] { trendRanges[0], trendRanges[1] });

            //Act
            trendRanges[0].NextBreakGuid = System.Guid.NewGuid().ToString();
            trendRanges[1].NextBreakGuid = System.Guid.NewGuid().ToString();
            repository.UpdateTrendRanges(trendRanges);

            //Assert
            Assert.IsTrue(repository.GetTrendRanges(1).HasEqualItems(new TrendRangeDto[] { trendRanges[0], trendRanges[1], trendRanges[2] }));
            Assert.IsTrue(repository.GetTrendRanges(2).HasEqualItems(new TrendRangeDto[] { trendRanges[3] }));

        }



        #endregion UPDATE TREND RANGES


        #region GET TREND RANGES



        [TestMethod]
        public void GetTrendRanges_returnProperDtoCollection()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            TrendRangeDto[] ranges = getDefaultTrendRangeDtosArray();
            clearTrendRangesTables();
            repository.UpdateTrendRanges(ranges);

            //Act
            IEnumerable<TrendRangeDto> dtos = repository.GetTrendRanges(1).ToArray();

            //Assert
            IEnumerable<TrendRangeDto> expected = new TrendRangeDto[] { ranges[0], ranges[1], ranges[2] };
            bool areEqualArrays = expected.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetTrendRangeById_ReturnsNull_IfThereIsNoTrendlineWithSuchId()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            TrendRangeDto[] ranges = getDefaultTrendRangeDtosArray();
            clearTrendRangesTables();
            repository.UpdateTrendRanges(ranges);

            //Act
            TrendRangeDto resultDto = repository.GetTrendRangeById(50);

            //Assert
            Assert.IsNull(resultDto);

        }

        [TestMethod]
        public void GetTrendRangeById_ReturnsProperTrendlineDto_IfExists()
        {

            //Arrange
            EFTrendlineRepository repository = new EFTrendlineRepository();
            List<TrendRangeDto> trendBreaks = new List<TrendRangeDto>();
            TrendRangeDto expectedDto = getDefaultTrendRangeDto();
            trendBreaks.AddRange(new TrendRangeDto[] { expectedDto });
            clearTrendRangesTables();
            repository.UpdateTrendRanges(trendBreaks);

            //Act
            TrendRangeDto resultDto = repository.GetTrendRangeById(expectedDto.Id);

            //Assert
            var areEqual = expectedDto.Equals(resultDto);
            Assert.IsTrue(areEqual);

        }


        #endregion GET TREND RANGES



    }
}

