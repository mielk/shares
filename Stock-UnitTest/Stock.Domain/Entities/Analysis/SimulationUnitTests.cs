using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;
using Stock.Core;
using Stock.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class SimulationUnitTests
    {
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_NAME = "Simulation";
        private const int DEFAULT_SIMULATION_ID = 1;
        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const int DEFAULT_ANALYSIS_TYPE_ID = 2;
        private DateTime? DEFAULT_LAST_DATE = new DateTime(2017, 4, 21, 16, 0, 0);
        private int? DEFAULT_LAST_INDEX = 20;



        #region CONSTRUCTOR

        [TestMethod]
        public void Constructor_newInstance_hasProperIdNameAndCurrencies()
        {

            //Act.
            var simulation = new Simulation() { Id = DEFAULT_ID, Name = DEFAULT_NAME };

            //Assert.
            Assert.AreEqual(DEFAULT_ID, simulation.Id);
            Assert.AreEqual(DEFAULT_NAME, simulation.Name);

        }

        [TestMethod]
        public void Constructor_fromDto_hasCorrectProperties()
        {

            //Act.
            SimulationDto dto = new SimulationDto
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME
            };
            var simulation = Simulation.FromDto(dto);

            //Assert.
            Assert.AreEqual(DEFAULT_ID, simulation.Id);
            Assert.AreEqual(DEFAULT_NAME, simulation.Name);

        }

        #endregion CONSTRUCTOR


        #region TO_DTO

        [TestMethod]
        public void ToDto_returnProperDto()
        {

            //Act.
            Simulation simulation = new Simulation
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME
            };
            var simulationDto = simulation.ToDto();

            //Assert.
            Assert.AreEqual(DEFAULT_ID, simulationDto.Id);
            Assert.AreEqual(DEFAULT_NAME, simulationDto.Name);

        }


        #endregion TO_DTO


        #region ADDING_LAST_UPDATES

        [TestMethod]
        public void AfterAddLastUpdate_ThisTimestampIsReturned()
        {

            //Arrange
            AnalysisTimestamp timestamp = new AnalysisTimestamp()
            {
                Id = 1,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedIndex = DEFAULT_LAST_INDEX,
                LastAnalysedItem = DEFAULT_LAST_DATE,
            };


            //Act
            Simulation simulation = new Simulation() { Id = 1, Name = "a" };
            simulation.AddLastUpdate(timestamp);

            //Assert
            AnalysisTimestamp actualLastUpdate = simulation.GetLastUpdate(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, AnalysisType.Prices);
            Assert.IsTrue(actualLastUpdate.LastAnalysedItem.IsEqual(DEFAULT_LAST_DATE));
            Assert.IsTrue(actualLastUpdate.LastAnalysedIndex.Equals(DEFAULT_LAST_INDEX));

        }

        [TestMethod]
        public void AfterAddLastUpdate_IfItemAlreadyExists_TimestampIsOverwritten()
        {

            //Arrange
            AnalysisTimestamp oldTimestamp = new AnalysisTimestamp()
            {
                Id = 1,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedIndex = DEFAULT_LAST_INDEX + 10,
                LastAnalysedItem = ((DateTime)DEFAULT_LAST_DATE).AddDays(2),
            };
            AnalysisTimestamp newTimestamp = new AnalysisTimestamp()
            {
                Id = 1,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedIndex = DEFAULT_LAST_INDEX,
                LastAnalysedItem = DEFAULT_LAST_DATE,
            };

            //Act
            Simulation simulation = new Simulation() { Id = 1, Name = "a" };
            simulation.AddLastUpdate(oldTimestamp);
            simulation.AddLastUpdate(newTimestamp);

            //Assert
            AnalysisTimestamp actualLastUpdate = simulation.GetLastUpdate(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, AnalysisType.Prices);
            Assert.IsTrue(actualLastUpdate.LastAnalysedItem.IsEqual(DEFAULT_LAST_DATE));
            Assert.IsTrue(actualLastUpdate.LastAnalysedIndex.Equals(DEFAULT_LAST_INDEX));

        }

        [TestMethod]
        public void AfterAddLastUpdateByDto_ThisTimestampIsReturned()
        {

            //Arrange
            AnalysisTimestampDto dto = new AnalysisTimestampDto()
            {
                Id = 1,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedIndex = DEFAULT_LAST_INDEX,
                LastAnalysedItem = DEFAULT_LAST_DATE,
            };

            //Act
            Simulation simulation = new Simulation() { Id = 1, Name = "a" };
            simulation.AddLastUpdate(dto);

            //Assert
            AnalysisTimestamp actualLastUpdate = simulation.GetLastUpdate(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, AnalysisType.Prices);
            Assert.IsTrue(actualLastUpdate.LastAnalysedItem.IsEqual(DEFAULT_LAST_DATE));
            Assert.IsTrue(actualLastUpdate.LastAnalysedIndex.Equals(DEFAULT_LAST_INDEX));

        }

        [TestMethod]
        public void AfterAddLastUpdateByDto_IfItemAlreadyExists_TimestampIsOverwritten()
        {


            //Arrange
            AnalysisTimestamp oldTimestamp = new AnalysisTimestamp()
            {
                Id = 1,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedIndex = DEFAULT_LAST_INDEX + 10,
                LastAnalysedItem = ((DateTime)DEFAULT_LAST_DATE).AddDays(2),
            };
            AnalysisTimestampDto newDto = new AnalysisTimestampDto()
            {
                Id = 1,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                SimulationId = DEFAULT_SIMULATION_ID,
                AnalysisTypeId = DEFAULT_ANALYSIS_TYPE_ID,
                LastAnalysedIndex = DEFAULT_LAST_INDEX,
                LastAnalysedItem = DEFAULT_LAST_DATE,
            };


            //Act
            Simulation simulation = new Simulation() { Id = 1, Name = "a" };
            simulation.AddLastUpdate(oldTimestamp);
            simulation.AddLastUpdate(newDto);


            //Assert
            AnalysisTimestamp actualLastUpdate = simulation.GetLastUpdate(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, AnalysisType.Prices);
            Assert.IsTrue(actualLastUpdate.LastAnalysedItem.IsEqual(DEFAULT_LAST_DATE));
            Assert.IsTrue(actualLastUpdate.LastAnalysedIndex.Equals(DEFAULT_LAST_INDEX));

        }


        #endregion ADDING_LAST_UPDATES





        #region EQUALS

        private Simulation getDefaultSimulation()
        {
            return new Simulation(){
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME
            };
        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultSimulation();
            var comparedItem = new { Id = 1 };

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = getDefaultSimulation();
            var comparedItem = getDefaultSimulation();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultSimulation();
            var comparedItem = getDefaultSimulation();

            //Act
            comparedItem.Id++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfNameIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultSimulation();
            var comparedItem = getDefaultSimulation();

            //Act
            comparedItem.Name += "a";
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLastUpdatesDictionariesAreDifferent()
        {

            //Arrange
            var baseItem = getDefaultSimulation();
            var comparedItem = getDefaultSimulation();

            //Act
            baseItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Prices,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 5
            });
            baseItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Macd,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 6
            });

            comparedItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Prices,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 5
            });
            comparedItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Macd,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 5
            });
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfLastUpdatesDictionariesAreEqual()
        {

            //Arrange
            var baseItem = getDefaultSimulation();
            var comparedItem = getDefaultSimulation();

            //Act
            baseItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Prices,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 5
            });
            baseItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Macd,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 6
            });

            comparedItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Prices,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 5
            });
            comparedItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Macd,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 6
            });
            
            //Assert
            var areEqual = baseItem.Equals(comparedItem);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLastUpdatesDictionariesHaveDifferentNumberOfItems()
        {

            //Arrange
            var baseItem = getDefaultSimulation();
            var comparedItem = getDefaultSimulation();

            //Act
            baseItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Prices,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 5
            });
            baseItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Macd,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 6
            });

            comparedItem.AddLastUpdate(new AnalysisTimestamp()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                AnalysisTypeId = (int)AnalysisType.Prices,
                LastAnalysedItem = new DateTime(2017, 5, 2, 12, 0, 0),
                LastAnalysedIndex = 5
            });
            
            //Assert
            var areEqual = baseItem.Equals(comparedItem);
            Assert.IsFalse(areEqual);

        }
        #endregion EQUALS



        #region GET.ANALYSIS.TIMESTAMP.DTOS


        [TestMethod]
        public void GetAnalysisTimestamps_ReturnsEmptyContainer_IfThereIsNoEntries()
        {

            //Act.
            Simulation simulation = new Simulation
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME
            };

            //Assert.
            var result = simulation.GetAnalysisTimestamps();
            var isEmpty = (result.Count() == 0);
            Assert.IsTrue(isEmpty);

        }


        [TestMethod]
        public void GetAnalysisTimestamps_ReturnsProperContainer_IfThereAreEntries()
        {

            //Arrange.
            AnalysisTimestamp timestamp1 = new AnalysisTimestamp() { Id = 1, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0), LastAnalysedIndex = 10 };
            AnalysisTimestamp timestamp2 = new AnalysisTimestamp() { Id = 2, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 5, 2, 13, 0, 0), LastAnalysedIndex = 11 };
            AnalysisTimestamp timestamp3 = new AnalysisTimestamp() { Id = 3, SimulationId = 1, AssetId = 1, TimeframeId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0), LastAnalysedIndex = 5 };
            var expectedTimestamps = new AnalysisTimestamp[] { timestamp1, timestamp2, timestamp3 };


            //Act.
            Simulation simulation = new Simulation
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME
            };
            simulation.AddLastUpdate(timestamp1);
            simulation.AddLastUpdate(timestamp2);
            simulation.AddLastUpdate(timestamp3);


            //Assert.
            var result = simulation.GetAnalysisTimestamps();
            var areEqual = expectedTimestamps.HasEqualItems(result);
            Assert.IsTrue(areEqual);

        }

        
        [TestMethod]
        public void GetAnalysisTimestampDtos_ReturnsEmptyContainer_IfThereIsNoEntries()
        {

            //Act.
            Simulation simulation = new Simulation
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME
            };

            //Assert.
            var result = simulation.GetAnalysisTimestampDtos();
            var isEmpty = (result.Count() == 0);
            Assert.IsTrue(isEmpty);

        }


        [TestMethod]
        public void GetAnalysisTimestampDtos_ReturnsProperContainer_IfThereAreEntries()
        {

            //Arrange.
            AnalysisTimestampDto timestamp1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0), LastAnalysedIndex = 10 };
            AnalysisTimestampDto timestamp2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 5, 2, 13, 0, 0), LastAnalysedIndex = 11 };
            AnalysisTimestampDto timestamp3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 1, AssetId = 1, TimeframeId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0), LastAnalysedIndex = 5 };
            var expectedDtos = new AnalysisTimestampDto[] { timestamp1, timestamp2, timestamp3 };


            //Act.
            Simulation simulation = new Simulation
            {
                Id = DEFAULT_ID,
                Name = DEFAULT_NAME
            };
            simulation.AddLastUpdate(timestamp1);
            simulation.AddLastUpdate(timestamp2);
            simulation.AddLastUpdate(timestamp3);

            //Assert.
            var result = simulation.GetAnalysisTimestampDtos();
            var areEqual = expectedDtos.HasEqualItems(result);
            Assert.IsTrue(areEqual);

        }

        #endregion GET.ANALYSIS.TIMESTAMP.DTOS


    }
}
