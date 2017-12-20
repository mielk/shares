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
using Stock.Domain.Enums;
using Stock.Domain.Services;
using Stock.Utils;
using Stock.Core;

namespace Stock_UnitTest.Stock.Domain.Services
{
    [TestClass]
    public class AnalysisTimestampServiceUnitTests
    {

        private const int DEFAULT_SIMULATION_ID = 1;
        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;


        #region HELPER_METHODS

        private IEnumerable<AnalysisTimestampDto> getDefaultAnalysisTimestampDtos(int simulationId)
        {
            List<AnalysisTimestampDto> list = new List<AnalysisTimestampDto>();
            list.Add(new AnalysisTimestampDto() { AnalysisTypeId = 1, Id = 7, SimulationId = simulationId, LastAnalysedItem = new DateTime(2017, 6, 5, 12, 0, 0) });
            list.Add(new AnalysisTimestampDto() { AnalysisTypeId = 2, Id = 54, SimulationId = simulationId, LastAnalysedItem = new DateTime(2017, 6, 4, 13, 0, 0) });
            list.Add(new AnalysisTimestampDto() { AnalysisTypeId = 3, Id = 54, SimulationId = simulationId, LastAnalysedItem = new DateTime(2017, 6, 4, 13, 0, 0) });
            return list;
        }

        #endregion HELPER_METHODS


        [TestMethod]
        public void GetLastAnalyzedIndexes_ReturnsEmptyDictionary_IfNothingIsFound()
        {

            //Arrange
            Mock<ISimulationRepository> mockedRepository = new Mock<ISimulationRepository>();
            mockedRepository.Setup(m => m.GetAnalysisTimestampsForSimulation(DEFAULT_SIMULATION_ID)).Returns(new List<AnalysisTimestampDto>());

            //Act
            AnalysisTimestampService service = new AnalysisTimestampService(mockedRepository.Object);

            //Assert
            var result = service.GetLastAnalyzedIndexes(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, DEFAULT_SIMULATION_ID);
            Assert.AreEqual(0, result.Count);

        }


        [TestMethod]
        public void GetLastAnalyzedIndexes_ReturnsDictionaryWithProperItems()
        {

            //Arrange
            Mock<ISimulationRepository> mockedRepository = new Mock<ISimulationRepository>();
            int lastQuotationIndex = 100;
            int lastPriceIndex = 84;
            int lastMacdIndex = 72;
            List<AnalysisTimestampDto> list = new List<AnalysisTimestampDto>();
            list.Add(new AnalysisTimestampDto() { AssetId = 1, TimeframeId = 1, AnalysisTypeId = (int)AnalysisType.Quotations, Id = 7, SimulationId = DEFAULT_SIMULATION_ID, LastAnalysedItem = new DateTime(2017, 6, 4, 13, 0, 0), LastAnalysedIndex = lastQuotationIndex });
            list.Add(new AnalysisTimestampDto() { AssetId = 1, TimeframeId = 2, AnalysisTypeId = (int)AnalysisType.Quotations, Id = 10, SimulationId = DEFAULT_SIMULATION_ID, LastAnalysedItem = new DateTime(2017, 6, 4, 15, 0, 0), LastAnalysedIndex = lastQuotationIndex + 2 });
            list.Add(new AnalysisTimestampDto() { AssetId = 1, TimeframeId = 1, AnalysisTypeId = (int)AnalysisType.Prices, Id = 54, SimulationId = DEFAULT_SIMULATION_ID, LastAnalysedItem = new DateTime(2017, 6, 4, 11, 0, 0), LastAnalysedIndex = lastPriceIndex });
            list.Add(new AnalysisTimestampDto() { AssetId = 1, TimeframeId = 1, AnalysisTypeId = (int)AnalysisType.Macd, Id = 57, SimulationId = DEFAULT_SIMULATION_ID, LastAnalysedItem = new DateTime(2017, 6, 4, 10, 0, 0), LastAnalysedIndex = lastMacdIndex });
            mockedRepository.Setup(m => m.GetAnalysisTimestampsForSimulation(DEFAULT_SIMULATION_ID)).Returns(list);

            //Act
            AnalysisTimestampService service = new AnalysisTimestampService(mockedRepository.Object);

            //Assert
            var result = service.GetLastAnalyzedIndexes(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, DEFAULT_SIMULATION_ID);
            Assert.AreEqual(3, result.Count);

            int? returnedQuotationIndex;
            int? returnedPriceIndex;
            int? returnedMacdIndex;
            result.TryGetValue(AnalysisType.Quotations, out returnedQuotationIndex);
            result.TryGetValue(AnalysisType.Prices, out returnedPriceIndex);
            result.TryGetValue(AnalysisType.Macd, out returnedMacdIndex);

            Assert.AreEqual(lastQuotationIndex, returnedQuotationIndex);
            Assert.AreEqual(lastPriceIndex, returnedPriceIndex);
            Assert.AreEqual(lastMacdIndex, returnedMacdIndex);

        }

    }
}
