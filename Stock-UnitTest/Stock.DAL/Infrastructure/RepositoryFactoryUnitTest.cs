using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Repositories;
using Stock.DAL.Infrastructure;

namespace Stock_UnitTest.Stock.DAL.Infrastructure
{
    [TestClass]
    public class RepositoryFactoryUnitTest
    {

        [TestMethod]
        public void GetMarketRepository_alwaysReturnsSingletonInstance()
        {
            IMarketRepository repository = RepositoryFactory.GetMarketRepository();
            IMarketRepository repository2 = RepositoryFactory.GetMarketRepository();
            Assert.AreSame(repository, repository2);
        }

        [TestMethod]
        public void GetCurrencyRepository_alwaysReturnsSingletonInstance()
        {
            ICurrencyRepository repository = RepositoryFactory.GetCurrencyRepository();
            ICurrencyRepository repository2 = RepositoryFactory.GetCurrencyRepository();
            Assert.AreSame(repository, repository2);
        }
        
        [TestMethod]
        public void GetTimeframeRepository_alwaysReturnsSingletonInstance()
        {
            ITimeframeRepository repository = RepositoryFactory.GetTimeframeRepository();
            ITimeframeRepository repository2 = RepositoryFactory.GetTimeframeRepository();
            Assert.AreSame(repository, repository2);
        }

        [TestMethod]
        public void GetAssetRepository_alwaysReturnsSingletonInstance()
        {
            IAssetRepository repository = RepositoryFactory.GetAssetRepository();
            IAssetRepository repository2 = RepositoryFactory.GetAssetRepository();
            Assert.AreSame(repository, repository2);
        }

        [TestMethod]
        public void GetQuotationRepository_alwaysReturnsSingletonInstance()
        {
            IQuotationRepository repository = RepositoryFactory.GetQuotationRepository();
            IQuotationRepository repository2 = RepositoryFactory.GetQuotationRepository();
            Assert.AreSame(repository, repository2);
        }

        [TestMethod]
        public void GetPriceRepository_alwaysReturnsSingletonInstance()
        {
            IPriceRepository repository = RepositoryFactory.GetPriceRepository();
            IPriceRepository repository2 = RepositoryFactory.GetPriceRepository();
            Assert.AreSame(repository, repository2);
        }

        [TestMethod]
        public void GetSimulationRepository_alwaysReturnsSingletonInstance()
        {
            ISimulationRepository repository = RepositoryFactory.GetSimulationRepository();
            ISimulationRepository repository2 = RepositoryFactory.GetSimulationRepository();
            Assert.AreSame(repository, repository2);
        }

        [TestMethod]
        public void GetAnalysisRepository_alwaysReturnsSingletonInstance()
        {
            IAnalysisRepository repository = RepositoryFactory.GetAnalysisRepository();
            IAnalysisRepository repository2 = RepositoryFactory.GetAnalysisRepository();
            Assert.AreSame(repository, repository2);
        }

        [TestMethod]
        public void GetTrendlineRepository_alwaysReturnsSingletonInstance()
        {
            ITrendlineRepository repository = RepositoryFactory.GetTrendlineRepository();
            ITrendlineRepository repository2 = RepositoryFactory.GetTrendlineRepository();
            Assert.AreSame(repository, repository2);
        }

    }
}