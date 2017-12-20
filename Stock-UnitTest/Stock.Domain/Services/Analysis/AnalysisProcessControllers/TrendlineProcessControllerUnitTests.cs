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
    public class TrendlineProcessControllerUnitTests
    {


        [TestMethod]
        public void Run_IfLastUpdateIsEqualToLastQuotation_ProcessorIsNotRun()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();

            mockedManager.Setup(m => m.GetAnalysisLastUpdatedIndex(AnalysisType.Trendlines)).Returns(10);
            mockedManager.Setup(m => m.GetAnalysisLastUpdatedIndex(AnalysisType.Quotations)).Returns(10);

            Mock<ITrendlineProcessor> mockedProcessor = new Mock<ITrendlineProcessor>();
            //mockedProcessor.Setup(m => m.Process(It.IsAny<DataSet>())).Verifiable();

            //Act
            TrendlineProcessController controller = new TrendlineProcessController();
            controller.InjectTrendlineProcessor(mockedProcessor.Object);
            controller.Run(mockedManager.Object);

            //Assert
            //mockedProcessor.Verify(m => m.Process(It.IsAny<DataSet>()), Times.Exactly(0));

        }


        //[TestMethod]
        //public void Run_AllItemsFromLastUpdatedMinusFixedNumberOfQuotationsAreChecked_IfThereIsNotEnoughItems()
        //{
        //    //Arrange
        //    Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
        //    DataSet[] dataSets = new DataSet[13];
        //    dataSets[1] = new DataSet(new Quotation() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, Open = 1.09191, High = 1.09218, Low = 1.09186, Close = 1.09194, Volume = 1411, IndexNumber = 1 });
        //    dataSets[2] = new DataSet(new Quotation() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, Open = 1.09193, High = 1.09256, Low = 1.09165, Close = 1.09177, Volume = 1819, IndexNumber = 2 });
        //    dataSets[3] = new DataSet(new Quotation() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, Open = 1.09176, High = 1.09182, Low = 1.09142, Close = 1.09151, Volume = 1359, IndexNumber = 3 });
        //    dataSets[4] = new DataSet(new Quotation() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, Open = 1.0915, High = 1.0916, Low = 1.09111, Close = 1.09112, Volume = 1392, IndexNumber = 4 });
        //    dataSets[5] = new DataSet(new Quotation() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 1, Open = 1.09111, High = 1.09124, Low = 1.09091, Close = 1.091, Volume = 1154, IndexNumber = 5 });
        //    dataSets[6] = new DataSet(new Quotation() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 50, 0), AssetId = 1, TimeframeId = 1, Open = 1.09101, High = 1.09132, Low = 1.09097, Close = 1.09131, Volume = 933, IndexNumber = 6 });
        //    dataSets[7] = new DataSet(new Quotation() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 55, 0), AssetId = 1, TimeframeId = 1, Open = 1.09131, High = 1.09167, Low = 1.09114, Close = 1.09165, Volume = 1079, IndexNumber = 7 });
        //    dataSets[8] = new DataSet(new Quotation() { Id = 8, Date = new DateTime(2016, 1, 15, 23, 0, 0), AssetId = 1, TimeframeId = 1, Open = 1.09164, High = 1.09183, Low = 1.0915, Close = 1.09177, Volume = 1009, IndexNumber = 8 });
        //    dataSets[9] = new DataSet(new Quotation() { Id = 9, Date = new DateTime(2016, 1, 15, 23, 5, 0), AssetId = 1, TimeframeId = 1, Open = 1.09178, High = 1.09189, Low = 1.09143, Close = 1.09149, Volume = 657, IndexNumber = 9 });
        //    dataSets[10] = new DataSet(new Quotation() { Id = 10, Date = new DateTime(2016, 1, 15, 23, 10, 0), AssetId = 1, TimeframeId = 1, Open = 1.0915, High = 1.09164, Low = 1.09144, Close = 1.09148, Volume = 414, IndexNumber = 10 });
        //    dataSets[11] = new DataSet(new Quotation() { Id = 11, Date = new DateTime(2016, 1, 15, 23, 15, 0), AssetId = 1, TimeframeId = 1, Open = 1.09149, High = 1.09156, Low = 1.09095, Close = 1.091, Volume = 419, IndexNumber = 11 });
        //    dataSets[12] = new DataSet(new Quotation() { Id = 12, Date = new DateTime(2016, 1, 15, 23, 20, 0), AssetId = 1, TimeframeId = 1, Open = 1.09098, High = 1.09118, Low = 1.09091, Close = 1.09108, Volume = 341, IndexNumber = 12 });
        //    for (int indexNumber = 1; indexNumber <= 12; indexNumber++)
        //    {
        //        mockedManager.Setup(m => m.GetDataSet(indexNumber)).Returns(dataSets[indexNumber]);
        //    }

        //    mockedManager.Setup(m => m.GetAnalysisLastUpdatedIndex(AnalysisType.Prices)).Returns(4);
        //    mockedManager.Setup(m => m.GetAnalysisLastUpdatedIndex(AnalysisType.Quotations)).Returns(12);

        //    Mock<IPriceProcessor> mockedProcessor = new Mock<IPriceProcessor>();
        //    mockedProcessor.Setup(m => m.Process(It.IsAny<DataSet>())).Verifiable();

        //    //Act
        //    PriceProcessController controller = new PriceProcessController();
        //    controller.InjectPriceProcessor(mockedProcessor.Object);
        //    controller.HowManyItemsBeforeInclude = 5;
        //    controller.Run(mockedManager.Object);

        //    //Assert
        //    mockedProcessor.Verify(m => m.Process(It.IsAny<DataSet>()), Times.Exactly(12));
        //    for (var indexNumber = 1; indexNumber <= 12; indexNumber++)
        //    {
        //        DataSet getPrice = dataSets[indexNumber];
        //        mockedProcessor.Verify(m => m.Process(getPrice), Times.Once());
        //    }

        //}


        //[TestMethod]
        //public void Run_ItemsBeforeLastUpdatedMinusFixedNumberOfQuotationsAreNotChecked()
        //{
        //    //Arrange
        //    Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
        //    DataSet[] dataSets = new DataSet[13];
        //    dataSets[1] = new DataSet(new Quotation() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, Open = 1.09191, High = 1.09218, Low = 1.09186, Close = 1.09194, Volume = 1411, IndexNumber = 1 });
        //    dataSets[2] = new DataSet(new Quotation() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, Open = 1.09193, High = 1.09256, Low = 1.09165, Close = 1.09177, Volume = 1819, IndexNumber = 2 });
        //    dataSets[3] = new DataSet(new Quotation() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, Open = 1.09176, High = 1.09182, Low = 1.09142, Close = 1.09151, Volume = 1359, IndexNumber = 3 });
        //    dataSets[4] = new DataSet(new Quotation() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, Open = 1.0915, High = 1.0916, Low = 1.09111, Close = 1.09112, Volume = 1392, IndexNumber = 4 });
        //    dataSets[5] = new DataSet(new Quotation() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 1, Open = 1.09111, High = 1.09124, Low = 1.09091, Close = 1.091, Volume = 1154, IndexNumber = 5 });
        //    dataSets[6] = new DataSet(new Quotation() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 50, 0), AssetId = 1, TimeframeId = 1, Open = 1.09101, High = 1.09132, Low = 1.09097, Close = 1.09131, Volume = 933, IndexNumber = 6 });
        //    dataSets[7] = new DataSet(new Quotation() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 55, 0), AssetId = 1, TimeframeId = 1, Open = 1.09131, High = 1.09167, Low = 1.09114, Close = 1.09165, Volume = 1079, IndexNumber = 7 });
        //    dataSets[8] = new DataSet(new Quotation() { Id = 8, Date = new DateTime(2016, 1, 15, 23, 0, 0), AssetId = 1, TimeframeId = 1, Open = 1.09164, High = 1.09183, Low = 1.0915, Close = 1.09177, Volume = 1009, IndexNumber = 8 });
        //    dataSets[9] = new DataSet(new Quotation() { Id = 9, Date = new DateTime(2016, 1, 15, 23, 5, 0), AssetId = 1, TimeframeId = 1, Open = 1.09178, High = 1.09189, Low = 1.09143, Close = 1.09149, Volume = 657, IndexNumber = 9 });
        //    dataSets[10] = new DataSet(new Quotation() { Id = 10, Date = new DateTime(2016, 1, 15, 23, 10, 0), AssetId = 1, TimeframeId = 1, Open = 1.0915, High = 1.09164, Low = 1.09144, Close = 1.09148, Volume = 414, IndexNumber = 10 });
        //    dataSets[11] = new DataSet(new Quotation() { Id = 11, Date = new DateTime(2016, 1, 15, 23, 15, 0), AssetId = 1, TimeframeId = 1, Open = 1.09149, High = 1.09156, Low = 1.09095, Close = 1.091, Volume = 419, IndexNumber = 11 });
        //    dataSets[12] = new DataSet(new Quotation() { Id = 12, Date = new DateTime(2016, 1, 15, 23, 20, 0), AssetId = 1, TimeframeId = 1, Open = 1.09098, High = 1.09118, Low = 1.09091, Close = 1.09108, Volume = 341, IndexNumber = 12 });
        //    for (int indexNumber = 1; indexNumber <= 12; indexNumber++)
        //    {
        //        mockedManager.Setup(m => m.GetDataSet(indexNumber)).Returns(dataSets[indexNumber]);
        //    }


        //    mockedManager.Setup(m => m.GetAnalysisLastUpdatedIndex(AnalysisType.Prices)).Returns(10);
        //    mockedManager.Setup(m => m.GetAnalysisLastUpdatedIndex(AnalysisType.Quotations)).Returns(12);

        //    Mock<IPriceProcessor> mockedProcessor = new Mock<IPriceProcessor>();
        //    mockedProcessor.Setup(m => m.Process(It.IsAny<DataSet>())).Verifiable();

        //    //Act
        //    PriceProcessController controller = new PriceProcessController();
        //    controller.InjectPriceProcessor(mockedProcessor.Object);
        //    controller.HowManyItemsBeforeInclude = 5;
        //    controller.Run(mockedManager.Object);

        //    //Assert
        //    mockedProcessor.Verify(m => m.Process(It.IsAny<DataSet>()), Times.Exactly(8));
        //    for (var indexNumber = 0; indexNumber <= 4; indexNumber++)
        //    {
        //        DataSet getPrice = dataSets[indexNumber];
        //        mockedProcessor.Verify(m => m.Process(getPrice), Times.Never());
        //    }

        //    for (var indexNumber = 5; indexNumber <= 12; indexNumber++)
        //    {
        //        DataSet getPrice = dataSets[indexNumber];
        //        mockedProcessor.Verify(m => m.Process(getPrice), Times.Once());
        //    }

        //}



    }
}
