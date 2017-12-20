using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Core;
using Stock.Utils;


namespace Stock_UnitTest.Stock.Domain.Services
{
    [TestClass]
    public class AnalysisDataQueryDefinitionUnitTests
    {

        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;


        [TestMethod]
        public void Clone_Afterward_NewInstanceHasEqualAllProperties()
        {
            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID)
            {
                Limit = 100,
                StartIndex = 10,
                EndIndex = 150,
                SimulationId = 1,
                AnalysisTypes = new AnalysisType[] { AnalysisType.Prices }
            };

            //Act
            AnalysisDataQueryDefinition clone = queryDef.Clone();

            //Assert
            Assert.IsTrue(queryDef.AssetId == clone.AssetId);
            Assert.IsTrue(queryDef.TimeframeId == clone.TimeframeId);
            Assert.IsTrue(queryDef.SimulationId == clone.SimulationId);
            Assert.IsTrue(queryDef.Limit == clone.Limit);
            Assert.IsTrue(queryDef.StartDate == clone.StartDate);
            Assert.IsTrue(queryDef.StartIndex == clone.StartIndex);
            Assert.IsTrue(queryDef.EndDate == clone.EndDate);
            Assert.IsTrue(queryDef.EndIndex == clone.EndIndex);
            Assert.IsTrue(queryDef.AnalysisTypes.HasEqualItems(clone.AnalysisTypes));

        }

        [TestMethod]
        public void Clone_Afterward_NewInstanceIsDifferentObjectThanOriginal()
        {
            
            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID);

            //Act
            AnalysisDataQueryDefinition clone = queryDef.Clone();

            //Assert
            Assert.IsFalse(queryDef == clone);

        }


    }
}
