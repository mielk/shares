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
    public class EFSimulationRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string SIMULATIONS_TABLE_NAME = "simulations";
        private const string LASTUPDATES_TABLE_NAME = "last_updates";


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

        private void clearSimulationsTables()
        {
            DbContext context = new UnitTestsDbContext();
            context.ClearTable(UNIT_TEST_DB_NAME, SIMULATIONS_TABLE_NAME);
            context.ClearTable(UNIT_TEST_DB_NAME, LASTUPDATES_TABLE_NAME);
        }

        #endregion INFRASTRUCTURE


        #region UPDATE_SIMULATIONS

        [TestMethod]
        public void UpdateSimulations_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<SimulationDto> simulations = new List<SimulationDto>();
            SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a" };
            SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b" };
            SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c" };
            SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d" };
            simulations.AddRange(new SimulationDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearSimulationsTables();
            repository.UpdateSimulations(simulations);
            IEnumerable<SimulationDto> actualRecords = repository.GetSimulations();

            //Assert
            bool areEqual = simulations.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateSimulations_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFSimulationRepository repository = new EFSimulationRepository();
            List<SimulationDto> simulations = new List<SimulationDto>();
            SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a" };
            SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b" };
            SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c" };
            SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d" };
            simulations.AddRange(new SimulationDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateSimulations(simulations);

            //Act
            dto1.Name = "x";
            dto2.Name = "y";
            dto3.Name = "z";
            repository.UpdateSimulations(new SimulationDto[] { dto1, dto2, dto3 });
            IEnumerable<SimulationDto> actualRecords = repository.GetSimulations();

            //Assert
            bool areEqual = simulations.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateSimulations_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFSimulationRepository repository = new EFSimulationRepository();
            List<SimulationDto> simulations = new List<SimulationDto>();
            SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a" };
            SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b" };
            SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c" };
            SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d" };
            simulations.AddRange(new SimulationDto[] { dto1, dto2, dto3 });
            clearSimulationsTables();
            repository.UpdateSimulations(simulations);

            //Act
            dto1.Name = "x";
            dto2.Name += "b";
            
            IEnumerable<SimulationDto> expectedRecords = new SimulationDto[] { dto1, dto2, dto3, dto4 };
            repository.UpdateSimulations(expectedRecords);
            IEnumerable<SimulationDto> actualRecords = repository.GetSimulations();

            //Assert
            bool areEqual = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        #endregion UPDATE_SIMULATIONS


        #region UPDATE_ANALYSIS_TIMESTAMP

        [TestMethod]
        public void UpdateAnalysisTimestamps_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);
            IEnumerable<AnalysisTimestampDto> actualRecords = repository.GetAnalysisTimestamps();

            //Assert
            bool areEqual = timestamps.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateAnalysisTimestamps_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            dto1.SimulationId++;
            dto2.LastAnalysedItem = new DateTime(2017, 1, 2, 1, 0, 0);
            dto3.AnalysisTypeId += 4;
            repository.UpdateAnalysisTimestamps(new AnalysisTimestampDto[] { dto1, dto2, dto3 });
            IEnumerable<AnalysisTimestampDto> actualRecords = repository.GetAnalysisTimestamps();

            //Assert
            bool areEqual = timestamps.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateAnalysisTimestamps_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            dto3.AnalysisTypeId += 4;
            dto2.SimulationId++;

            IEnumerable<AnalysisTimestampDto> expectedRecords = new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 };
            repository.UpdateAnalysisTimestamps(expectedRecords);
            IEnumerable<AnalysisTimestampDto> actualRecords = repository.GetAnalysisTimestamps();

            //Assert
            bool areEqual = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        #endregion UPDATE_ANALYSIS_TIMESTAMP


        #region GET_SIMULATIONS

        [TestMethod]
        public void GetSimulations_returnProperDtoCollection()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<SimulationDto> simulations = new List<SimulationDto>();
            SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "a" };
            SimulationDto dto2 = new SimulationDto() { Id = 2, Name = "b" };
            SimulationDto dto3 = new SimulationDto() { Id = 3, Name = "c" };
            SimulationDto dto4 = new SimulationDto() { Id = 4, Name = "d" };
            simulations.AddRange(new SimulationDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables(); 
            repository.UpdateSimulations(simulations);

            //Act
            IEnumerable<SimulationDto> dtos = repository.GetSimulations().ToArray();

            //Assert
            bool areEqualArrays = simulations.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetSimulationById_ReturnsNull_IfThereIsNoSimulationWithSuchId()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<SimulationDto> simulations = new List<SimulationDto>();
            SimulationDto dto0 = new SimulationDto() { Id = 1, Name = "a" };
            SimulationDto dto1 = new SimulationDto() { Id = 1, Name = "b" };
            simulations.AddRange(new SimulationDto[] { dto0, dto1 });
            clearSimulationsTables();
            repository.UpdateSimulations(simulations);

            //Act
            SimulationDto dto = repository.GetSimulationById(dto1.Id + 1);

            //Assert
            Assert.IsNull(dto);

        }

        [TestMethod]
        public void GetSimulationById_ReturnsProperSimulationDto_IfExists()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<SimulationDto> simulations = new List<SimulationDto>();
            SimulationDto expected = new SimulationDto() { Id = 1, Name = "a" };
            simulations.AddRange(new SimulationDto[] { expected });
            clearSimulationsTables();
            repository.UpdateSimulations(simulations);

            //Act
            SimulationDto dto = repository.GetSimulationById(expected.Id);

            //Assert
            var areEqual = expected.Equals(dto);
            Assert.IsTrue(areEqual);

        }


        #endregion GET_SIMULATIONS


        #region GET_ANALYSIS_TIMESTAMP

        [TestMethod]
        public void GetAnalysisTimestamps_returnProperDtoCollection()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            IEnumerable<AnalysisTimestampDto> dtos = repository.GetAnalysisTimestamps().ToArray();

            //Assert
            bool areEqualArrays = timestamps.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetAnalysisTimestampsForSimulation_returnProperDtoCollection()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 2, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            IEnumerable<AnalysisTimestampDto> dtos = repository.GetAnalysisTimestampsForSimulation(1).ToArray();

            //Assert
            IEnumerable<AnalysisTimestampDto> expectedDtos = new AnalysisTimestampDto[] { dto1, dto2, dto3 };
            bool areEqualArrays = expectedDtos.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetAnalysisTimestampsForSimulation_ReturnsEmptyCollection_IfThereIsNoTimestampsForGivenSimulation()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            IEnumerable<AnalysisTimestampDto> dtos = repository.GetAnalysisTimestampsForSimulation(3).ToArray();

            //Assert
            IEnumerable<AnalysisTimestampDto> expectedDtos = new AnalysisTimestampDto[] { };
            bool areEqualArrays = expectedDtos.HasEqualItems(dtos);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetAnalysisTimestampsForSimulation_ReturnsNull_IfThereIsNoSuchTimestamp()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            AnalysisTimestampDto dto = repository.GetAnalysisTimestamp(1, 1, 1, 4);

            //Assert
            Assert.IsNull(dto);

        }

        [TestMethod]
        public void GetAnalysisTimestampsForSimulation_ReturnsProperDto_IfThereIsSuchTimestamp()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            AnalysisTimestampDto dto = repository.GetAnalysisTimestamp(1, 1, 2, 2);

            //Assert
            var areEqual = dto4.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetAnalysisTimestampsForSimulation_ReturnsProperDto_IfAnalysisTypeGivenByEnum()
        {

            //Arrange
            EFSimulationRepository repository = new EFSimulationRepository();
            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto dto1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            AnalysisTimestampDto dto2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = null, LastAnalysedIndex = null };
            AnalysisTimestampDto dto4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2017, 2, 4, 14, 15, 0), LastAnalysedIndex = 100 };
            timestamps.AddRange(new AnalysisTimestampDto[] { dto1, dto2, dto3, dto4 });
            clearSimulationsTables();
            repository.UpdateAnalysisTimestamps(timestamps);

            //Act
            AnalysisTimestampDto dto = repository.GetAnalysisTimestamp(1, 1, 2, AnalysisType.Prices);

            //Assert
            var areEqual = dto4.Equals(dto);
            Assert.IsTrue(areEqual);

        }

        #endregion GET_ANALYSIS_TIMESTAMP

        
    }
}

