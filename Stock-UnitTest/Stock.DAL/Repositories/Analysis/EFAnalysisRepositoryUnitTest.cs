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
    public class EFAnalysisRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string SIMULATIONS_TABLE_NAME = "simulations";
        private const string LASTUPDATES_TABLE_NAME = "last_updates";


        [Ignore]
        [TestMethod]
        public void EFAnalysisRepositoryUnitTest_WszystkieTesty()
        {
            Assert.Fail("Not implemented yet");
        }


        //#region INFRASTRUCTURE

        //[ClassInitialize()]
        //public static void InitTestSuite(TestContext testContext)
        //{
        //    DbContext context = new OriginalDbContext();
        //    context.Database.ExecuteSqlCommand("recreateDb");
        //}

        //[ClassCleanup()]
        //public static void CleanupTestSuite()
        //{
        //    DbContext context = new OriginalDbContext();
        //    context.Database.ExecuteSqlCommand("DROP DATABASE fx_unittests");
        //}

        //private void clearTrendlinesTables()
        //{
        //    DbContext context = new UnitTestsDbContext();
        //    context.ClearTable(UNIT_TEST_DB_NAME, SIMULATIONS_TABLE_NAME);
        //    context.ClearTable(UNIT_TEST_DB_NAME, LASTUPDATES_TABLE_NAME);
        //}

        //#endregion INFRASTRUCTURE


        //#region UPDATE_SIMULATIONS

        //[TestMethod]
        //public void UpdateSimulations_WorksProperly_IfItemsAreOnlyAdded()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<SimulationDto> ranges = new List<SimulationDto>();
        //    SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d", AssetId = 1, TimeframeId = 1 };
        //    ranges.AddRange(new SimulationDto[] { dto1, dto2, dto3, dto4 });

        //    //Act
        //    clearTrendlinesTables();
        //    repository.UpdateSimulations(ranges);
        //    IEnumerable<SimulationDto> actualRecords = repository.GetSimulations();

        //    //Assert
        //    bool areEqual = ranges.HasEqualItems(actualRecords);
        //    Assert.IsTrue(areEqual);

        //}

        //[TestMethod]
        //public void UpdateSimulations_WorksProperly_IfItemsAreOnlyUpdated()
        //{

        //    //Arrange
        //    AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<SimulationDto> ranges = new List<SimulationDto>();
        //    SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d", AssetId = 1, TimeframeId = 1 };
        //    ranges.AddRange(new SimulationDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateSimulations(ranges);

        //    //Act
        //    dto1.Name += "a";
        //    dto2.TimeframeId++;
        //    dto3.AssetId++;
        //    repository.UpdateSimulations(new SimulationDto[] { dto1, dto2, dto3 });
        //    IEnumerable<SimulationDto> actualRecords = repository.GetSimulations();

        //    //Assert
        //    bool areEqual = ranges.HasEqualItems(actualRecords);
        //    Assert.IsTrue(areEqual);

        //}

        //[TestMethod]
        //public void UpdateSimulations_WorksProperly_IfItemsAreAddedAndUpdated()
        //{

        //    //Arrange
        //    AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<SimulationDto> ranges = new List<SimulationDto>();
        //    SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d", AssetId = 1, TimeframeId = 1 };
        //    ranges.AddRange(new SimulationDto[] { dto1, dto2, dto3 });
        //    clearTrendlinesTables();
        //    repository.UpdateSimulations(ranges);

        //    //Act
        //    dto1.Name += "a";
        //    dto2.TimeframeId++;
            
        //    IEnumerable<SimulationDto> expectedRecords = new SimulationDto[] { dto1, dto2, dto3, dto4 };
        //    repository.UpdateSimulations(expectedRecords);
        //    IEnumerable<SimulationDto> actualRecords = repository.GetSimulations();

        //    //Assert
        //    bool areEqual = expectedRecords.HasEqualItems(actualRecords);
        //    Assert.IsTrue(areEqual);

        //}

        //#endregion UPDATE_SIMULATIONS


        //#region UPDATE_ANALYSIS_TIMESTAMP

        //[TestMethod]
        //public void UpdateAnalysisTimestamps_WorksProperly_IfItemsAreOnlyAdded()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 1, AnalysisTypeId = 4, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });

        //    //Act
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);
        //    IEnumerable<AnalysisTimestampDto> actualRecords = repository.GetAnalysisTimestamps();

        //    //Assert
        //    bool areEqual = timestamps.HasEqualItems(actualRecords);
        //    Assert.IsTrue(areEqual);

        //}

        //[TestMethod]
        //public void UpdateAnalysisTimestamps_WorksProperly_IfItemsAreOnlyUpdated()
        //{

        //    //Arrange
        //    AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 1, AnalysisTypeId = 4, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    dto1.SimulationId++;
        //    dto2.LastAnalysedItem = new DateTime(2017, 1, 2, 1, 0, 0);
        //    dto3.AnalysisTypeId += 4;
        //    repository.UpdateAnalysisTimestamps(new AnalysisTimestampDto[] { dto1, dto2, dto3 });
        //    IEnumerable<AnalysisTimestampDto> actualRecords = repository.GetAnalysisTimestamps();

        //    //Assert
        //    bool areEqual = timestamps.HasEqualItems(actualRecords);
        //    Assert.IsTrue(areEqual);

        //}

        //[TestMethod]
        //public void UpdateAnalysisTimestamps_WorksProperly_IfItemsAreAddedAndUpdated()
        //{

        //    //Arrange
        //    AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 1, AnalysisTypeId = 4, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    dto3.AnalysisTypeId += 4;
        //    dto2.SimulationId++;

        //    IEnumerable<AnalysisTimestampDto> expectedRecords = new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 };
        //    repository.UpdateAnalysisTimestamps(expectedRecords);
        //    IEnumerable<AnalysisTimestampDto> actualRecords = repository.GetAnalysisTimestamps();

        //    //Assert
        //    bool areEqual = expectedRecords.HasEqualItems(actualRecords);
        //    Assert.IsTrue(areEqual);

        //}

        //#endregion UPDATE_ANALYSIS_TIMESTAMP


        //#region GET_SIMULATIONS

        //[TestMethod]
        //public void GetSimulations_returnProperDtoCollection()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<SimulationDto> ranges = new List<SimulationDto>();
        //    SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c", AssetId = 1, TimeframeId = 1 };
        //    SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d", AssetId = 1, TimeframeId = 1 };
        //    ranges.AddRange(new SimulationDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables(); 
        //    repository.UpdateSimulations(ranges);

        //    //Act
        //    IEnumerable<SimulationDto> dtos = repository.GetSimulations().ToArray();

        //    //Assert
        //    bool areEqualArrays = ranges.HasEqualItems(dtos);
        //    Assert.IsTrue(areEqualArrays);

        //}

        //[TestMethod]
        //public void GetSimulation_ReturnsNull_IfThereIsNoSuchSimulation()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<SimulationDto> ranges = new List<SimulationDto>();
        //    SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a", AssetId = 1, TimeframeId = 1 };
        //    ranges.AddRange(new SimulationDto[] { dto1 });
        //    clearTrendlinesTables();
        //    repository.UpdateSimulations(ranges);

        //    //Act
        //    SimulationDto resultDto = repository.GetSimulationByNameAssetTimeframe("a", 1, 2);

        //    //Assert
        //    Assert.IsNull(resultDto);

        //}

        //[TestMethod]
        //public void GetSimulationByNameAssetTimeframe_ReturnsProperSimulationDto_IfExists()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<SimulationDto> ranges = new List<SimulationDto>();
        //    SimulationDto expected = new SimulationDto() { Id = 1, Name = "a", AssetId = 1, TimeframeId = 1 };
        //    ranges.AddRange(new SimulationDto[] { expected });
        //    clearTrendlinesTables();
        //    repository.UpdateSimulations(ranges);

        //    //Act
        //    SimulationDto resultDto = repository.GetSimulationByNameAssetTimeframe("a", 1, 1);

        //    //Assert
        //    var areEqual = expected.Equals(resultDto);
        //    Assert.IsTrue(areEqual);

        //}

        //#endregion GET_SIMULATIONS


        //#region GET_ANALYSIS_TIMESTAMP

        //[TestMethod]
        //public void GetAnalysisTimestamps_returnProperDtoCollection()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 1, AnalysisTypeId = 4, LastAnalysedItem = null, LastAnalysedIndex = null };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    IEnumerable<AnalysisTimestampDto> dtos = repository.GetAnalysisTimestamps().ToArray();

        //    //Assert
        //    bool areEqualArrays = timestamps.HasEqualItems(dtos);
        //    Assert.IsTrue(areEqualArrays);

        //}

        //[TestMethod]
        //public void GetAnalysisTimestampsForSimulation_returnProperDtoCollection()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 1, 12, 15, 0), LastAnalysedIndex = 50 };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 2, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 50 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    IEnumerable<AnalysisTimestampDto> dtos = repository.GetAnalysisTimestampsForSimulation(1).ToArray();

        //    //Assert
        //    IEnumerable<AnalysisTimestampDto> expectedTimestamps = new AnalysisTimestampDto[] { dto1, dto2 };
        //    bool areEqualArrays = expectedTimestamps.HasEqualItems(dtos);
        //    Assert.IsTrue(areEqualArrays);

        //}

        //[TestMethod]
        //public void GetAnalysisTimestampsForSimulation_ReturnsEmptyCollection_IfThereIsNoTimestampsForGivenSimulation()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 1, 12, 15, 0), LastAnalysedIndex = 50 };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 2, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 50 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    IEnumerable<AnalysisTimestampDto> dtos = repository.GetAnalysisTimestampsForSimulation(3).ToArray();

        //    //Assert
        //    IEnumerable<AnalysisTimestampDto> expectedTimestamps = new AnalysisTimestampDto[] { };
        //    bool areEqualArrays = expectedTimestamps.HasEqualItems(dtos);
        //    Assert.IsTrue(areEqualArrays);

        //}

        //[TestMethod]
        //public void GetAnalysisTimestampsForSimulation_ReturnsNull_IfThereIsNoSuchTimestamp()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 1, 12, 15, 0), LastAnalysedIndex = 50 };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 2, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    AnalysisTimestampDto resultDto = repository.GetAnalysisTimestamp(3, 4);

        //    //Assert
        //    Assert.IsNull(resultDto);

        //}

        //[TestMethod]
        //public void GetAnalysisTimestampsForSimulation_ReturnsProperDto_IfThereIsSuchTimestamp()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 1, 12, 15, 0), LastAnalysedIndex = 50 };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 2, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 1000 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    AnalysisTimestampDto resultDto = repository.GetAnalysisTimestamp(2, 5);

        //    //Assert
        //    var areEqual = dto4.Equals(resultDto);
        //    Assert.IsTrue(areEqual);

        //}

        //[TestMethod]
        //public void GetAnalysisTimestampsForSimulation_ReturnsProperDto_IfAnalysisTypeGivenByEnum()
        //{

        //    //Arrange
        //    EFSimulationRepository repository = new EFSimulationRepository();
        //    List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
        //    AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 1, 12, 15, 0), LastAnalysedIndex = 50 };
        //    AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, SimulationId = 2, AnalysisTypeId = 5, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
        //    timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
        //    clearTrendlinesTables();
        //    repository.UpdateAnalysisTimestamps(timestamps);

        //    //Act
        //    AnalysisTimestampDto resultDto = repository.GetAnalysisTimestamp(2, AnalysisType.Prices);

        //    //Assert
        //    var areEqual = dto3.Equals(resultDto);
        //    Assert.IsTrue(areEqual);

        //}

        //#endregion GET_ANALYSIS_TIMESTAMP

        
    }
}

