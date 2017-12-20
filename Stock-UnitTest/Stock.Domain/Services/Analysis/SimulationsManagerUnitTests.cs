using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Stock.Domain.Services;
using Stock.Domain.Entities;
using Stock.Core;

namespace Stock_UnitTest.Stock.Domain.Services.Analysis
{
    [TestClass]
    public class SimulationManagerUnitTests
    {


        #region HELPER_METHODS

        private Simulation defaultSimulation()
        {
            return new Simulation()
            {
                Id = 1, 
                Name = "a"
            };
        }

        #endregion HELPER_METHODS


        #region GET_ANALYSIS_LAST_UPDATED_INDEX

        [Ignore]
        [TestMethod]
        public void SimulationManager_WszystkieTesty()
        {
            Assert.Fail("Not implemented yet");
        }

        #endregion GET_ANALYSIS_LAST_UPDATED_INDEX


    }

}
