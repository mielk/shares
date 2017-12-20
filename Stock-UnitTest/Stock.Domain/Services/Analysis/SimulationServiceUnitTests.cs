using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Repositories;
using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Stock.Domain.Entities;
using Stock.Domain.Services;
using Stock.Utils;
using Stock.Core;   

namespace Stock_UnitTest.Stock.Domain.Services
{
    [TestClass]
    public class SimulationServiceUnitTests
    {

        #region GET_SIMULATIONS

        [TestMethod]
        public void GetSimulations_ReturnsProperCollectionOfSimulations_IncludingLastUpdateTimestamps()
        {

            //Arrange
            Mock<ISimulationRepository> mockedRepository = new Mock<ISimulationRepository>();
            List<SimulationDto> dtos = new List<SimulationDto>();
            SimulationDto simulationDto1 = new SimulationDto() { Id = 1, Name = "a" };
            SimulationDto simulationDto2 = new SimulationDto() { Id = 2, Name = "b" };
            SimulationDto simulationDto3 = new SimulationDto() { Id = 3, Name = "c" };
            dtos.AddRange(new SimulationDto[] { simulationDto1, simulationDto2, simulationDto3 });

            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto timestamp1 = new AnalysisTimestampDto() { Id = 1, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0) };
            AnalysisTimestampDto timestamp2 = new AnalysisTimestampDto() { Id = 2, AssetId = 1, TimeframeId = 1, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 5, 2, 13, 0, 0) };
            AnalysisTimestampDto timestamp3 = new AnalysisTimestampDto() { Id = 3, AssetId = 1, TimeframeId = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 5, 2, 14, 0, 0) };
            AnalysisTimestampDto timestamp4 = new AnalysisTimestampDto() { Id = 4, AssetId = 1, TimeframeId = 1, SimulationId = 2, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0) };
            AnalysisTimestampDto timestamp5 = new AnalysisTimestampDto() { Id = 5, AssetId = 1, TimeframeId = 1, SimulationId = 2, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 5, 2, 13, 0, 0) };
            AnalysisTimestampDto timestamp6 = new AnalysisTimestampDto() { Id = 6, AssetId = 1, TimeframeId = 2, SimulationId = 2, AnalysisTypeId = 4, LastAnalysedItem = new DateTime(2016, 5, 2, 14, 0, 0) };
            timestamps.AddRange(new AnalysisTimestampDto[] { timestamp1, timestamp2, timestamp3, timestamp4, timestamp5, timestamp6 });

            mockedRepository.Setup(r => r.GetSimulations()).Returns(dtos);
            mockedRepository.Setup(r => r.GetAnalysisTimestampsForSimulation(1)).Returns(new AnalysisTimestampDto[] { timestamp1, timestamp2, timestamp3 });
            mockedRepository.Setup(r => r.GetAnalysisTimestampsForSimulation(2)).Returns(new AnalysisTimestampDto[] { timestamp4, timestamp5, timestamp6 });

            //Act
            SimulationService service = new SimulationService(mockedRepository.Object);
            var actualSimultations = service.GetSimulations();

            //Assert
            List<Simulation> expectedSimulations = new List<Simulation>();
            Simulation simulation1 = new Simulation() { Id = 1, Name = "a" };
            simulation1.AddLastUpdate(timestamp1);
            simulation1.AddLastUpdate(timestamp2);
            simulation1.AddLastUpdate(timestamp3);
            Simulation simulation2 = new Simulation() { Id = 2, Name = "b" };
            simulation2.AddLastUpdate(timestamp4);
            simulation2.AddLastUpdate(timestamp5);
            simulation2.AddLastUpdate(timestamp6);
            Simulation simulation3 = new Simulation() { Id = 3, Name = "c" };
            expectedSimulations.AddRange(new Simulation[] { simulation1, simulation2, simulation3 });
            bool areEqual = expectedSimulations.HasEqualItems(actualSimultations);
            Assert.IsTrue(areEqual);
            
        }

        [TestMethod]
        public void GetSimulationById_ReturnsNull_IfThereIsNoItemWithGivenCombination()
        {

            //Arrange
            Mock<ISimulationRepository> mockedRepository = new Mock<ISimulationRepository>();
            SimulationDto returnedDto = null;
            mockedRepository.Setup(r => r.GetSimulationById(1)).Returns(returnedDto);

            //Act
            SimulationService service = new SimulationService(mockedRepository.Object);

            //Assert
            Simulation baseSimulation = service.GetSimulationById(1);
            Assert.IsNull(baseSimulation);

        }

        [TestMethod]
        public void GetSimulationById_ReturnsProperItem_IfItemExists()
        {

            //Arrange
            Mock<ISimulationRepository> mockedRepository = new Mock<ISimulationRepository>();
            SimulationDto simulationDto = new SimulationDto() { Id = 1, Name = "a" };

            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto timestamp1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0) };
            AnalysisTimestampDto timestamp2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 5, 2, 13, 0, 0) };
            AnalysisTimestampDto timestamp3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 4, LastAnalysedItem = new DateTime(2016, 5, 2, 14, 0, 0) };
            timestamps.AddRange(new AnalysisTimestampDto[] { timestamp1, timestamp2, timestamp3 });

            mockedRepository.Setup(r => r.GetSimulationById(1)).Returns(simulationDto);
            mockedRepository.Setup(r => r.GetAnalysisTimestampsForSimulation(1)).Returns(new AnalysisTimestampDto[] { timestamp1, timestamp2, timestamp3 });

            //Act
            SimulationService service = new SimulationService(mockedRepository.Object);
            var actualSimulation = service.GetSimulationById(1);

            //Assert
            Simulation expectedSimulation = new Simulation() { Id = 1, Name = "a" };
            expectedSimulation.AddLastUpdate(timestamp1);
            expectedSimulation.AddLastUpdate(timestamp2);
            expectedSimulation.AddLastUpdate(timestamp3);
            bool areEqual = expectedSimulation.Equals(actualSimulation);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetSimulationById_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ISimulationRepository> mockedRepository = new Mock<ISimulationRepository>();
            List<SimulationDto> dtos = new List<SimulationDto>();
            SimulationDto simulationDto1 = new SimulationDto() { Id = 1, Name = "a" };
            dtos.AddRange(new SimulationDto[] { simulationDto1 });

            List<AnalysisTimestampDto> timestamps = new List<AnalysisTimestampDto>();
            AnalysisTimestampDto timestamp1 = new AnalysisTimestampDto() { Id = 1, SimulationId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0) };
            AnalysisTimestampDto timestamp2 = new AnalysisTimestampDto() { Id = 2, SimulationId = 1, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 5, 2, 13, 0, 0) };
            AnalysisTimestampDto timestamp3 = new AnalysisTimestampDto() { Id = 3, SimulationId = 1, AnalysisTypeId = 4, LastAnalysedItem = new DateTime(2016, 5, 2, 14, 0, 0) };
            timestamps.AddRange(new AnalysisTimestampDto[] { timestamp1, timestamp2, timestamp3 });

            mockedRepository.Setup(r => r.GetSimulations()).Returns(dtos);
            mockedRepository.Setup(r => r.GetAnalysisTimestampsForSimulation(1)).Returns(new AnalysisTimestampDto[] { timestamp1, timestamp2, timestamp3 });

            //Act
            SimulationService service = new SimulationService(mockedRepository.Object);

            //Assert
            Simulation baseSimulation = service.GetSimulationById(1);
            Simulation comparedSimulation = service.GetSimulationById(1);

            bool areTheSame = (baseSimulation == comparedSimulation);
            Assert.IsTrue(areTheSame);

        }

        #endregion GET_SIMULATIONS



        #region UPDATE_SIMULATIONS

        [TestMethod]
        public void Update_AllItemsPassedToMethodArePassedToRepository()
        {

            //Arrange
            IEnumerable<SimulationDto> simulationDtos = null;
            IEnumerable<AnalysisTimestampDto> timestampDtos = null;
            Simulation simulation = new Simulation() { Id = 1, Name = "a" };
            AnalysisTimestamp timestamp1 = new AnalysisTimestamp() { Id = 1, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 2, LastAnalysedItem = new DateTime(2016, 5, 2, 12, 0, 0), LastAnalysedIndex = 15 };
            AnalysisTimestamp timestamp2 = new AnalysisTimestamp() { Id = 2, SimulationId = 1, AssetId = 1, TimeframeId = 1, AnalysisTypeId = 3, LastAnalysedItem = new DateTime(2016, 6, 2, 12, 0, 0), LastAnalysedIndex = 19 };
            simulation.AddLastUpdate(timestamp1);
            simulation.AddLastUpdate(timestamp2);
            Mock<ISimulationRepository> mockedRepository = new Mock<ISimulationRepository>();
            mockedRepository.
                Setup(r => r.UpdateSimulations(It.IsAny<IEnumerable<SimulationDto>>())).
                Callback<IEnumerable<SimulationDto>>((col) => simulationDtos = col).Verifiable();
            mockedRepository.
                Setup(r => r.UpdateAnalysisTimestamps(It.IsAny<IEnumerable<AnalysisTimestampDto>>())).
                Callback<IEnumerable<AnalysisTimestampDto>>((col) => timestampDtos = col).Verifiable();

            //Act
            SimulationService service = new SimulationService(mockedRepository.Object);
            service.Update(simulation);

            //Assert
            IEnumerable<SimulationDto> expectedSimulationDtos = new SimulationDto[] { simulation.ToDto() };
            IEnumerable<AnalysisTimestampDto> expectedTimestampDtos = new AnalysisTimestampDto[] { timestamp1.ToDto(), timestamp2.ToDto() };
            mockedRepository.Verify(r => r.UpdateSimulations(It.IsAny<IEnumerable<SimulationDto>>()), Times.Exactly(1));
            mockedRepository.Verify(r => r.UpdateAnalysisTimestamps(It.IsAny<IEnumerable<AnalysisTimestampDto>>()), Times.Exactly(1));
            Assert.IsTrue(simulationDtos.HasEqualItems(expectedSimulationDtos));
            Assert.IsTrue(timestampDtos.HasEqualItems(expectedTimestampDtos));

        }

        #endregion UPDATE_SIMULATIONS


    }
}
