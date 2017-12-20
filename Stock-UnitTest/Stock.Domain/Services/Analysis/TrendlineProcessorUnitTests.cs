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
using Stock_UnitTest.Helpers;

namespace Stock_UnitTest.Stock.Domain.Services
{
    [TestClass]
    public class TrendlineProcessorUnitTests
    {

        private UTFactory utf = new UTFactory();


        #region CAN_CREATE_TRENDLINES

        [TestMethod]
        public void CanCreateTrendlines_ReturnFalse_IfDistanceBetweenExtremaIsGreaterThanLimit()
        {

            //Arrange
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();

            Price basePrice = utf.getPrice(100);
            Extremum baseMaster = new Extremum(basePrice, ExtremumType.PeakByClose);
            ExtremumGroup baseExtremumGroup = new ExtremumGroup(baseMaster, null);

            Price footholdPrice = utf.getPrice(500);
            Extremum footholdMaster = new Extremum(footholdPrice, ExtremumType.PeakByClose);
            ExtremumGroup footholdExtremumGroup = new ExtremumGroup(footholdMaster, null);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);
            processor.MaxDistanceBetweenExtrema = 100;

            //Assert
            var result = processor.CanCreateTrendline(baseExtremumGroup, footholdExtremumGroup);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void CanCreateTrendlines_ReturnFalse_IfDistanceBetweenExtremaIsLessThanMinLimit()
        {

            //Arrange
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            Price basePrice = utf.getPrice(100);
            Extremum baseMaster = new Extremum(basePrice, ExtremumType.PeakByClose);
            ExtremumGroup baseExtremumGroup = new ExtremumGroup(baseMaster, null);

            Price footholdPrice = utf.getPrice(102);
            Extremum footholdMaster = new Extremum(footholdPrice, ExtremumType.PeakByClose);
            ExtremumGroup footholdExtremumGroup = new ExtremumGroup(footholdMaster, null);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);
            processor.MaxDistanceBetweenExtrema = 3;

            //Assert
            var result = processor.CanCreateTrendline(baseExtremumGroup, footholdExtremumGroup);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanCreateTrendlines_ReturnTrue_IfDistanceIsSuitable()
        {

            //Arrange
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            Price basePrice = utf.getPrice(100);
            Extremum baseMaster = new Extremum(basePrice, ExtremumType.PeakByClose);
            ExtremumGroup baseExtremumGroup = new ExtremumGroup(baseMaster, null);

            Price footholdPrice = utf.getPrice(200);
            Extremum footholdMaster = new Extremum(footholdPrice, ExtremumType.PeakByClose);
            ExtremumGroup footholdExtremumGroup = new ExtremumGroup(footholdMaster, null);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);
            processor.MaxDistanceBetweenExtrema = 150;

            //Assert
            var result = processor.CanCreateTrendline(baseExtremumGroup, footholdExtremumGroup);
            Assert.IsTrue(result);
        }

        #endregion CAN_CREATE_TRENDLINES



        #region GET_CHART_POINTS

        [TestMethod]
        public void GetChartPoints_IfOnlyMasterPeakExtremumWithLongShadow_OnlyLimitedNumberOfPointsIsCreated()
        {

            //Arrange
            DataSet ds5 = utf.getDataSet(5);
            Price basePrice = utf.getPrice(ds5);
            Extremum master = new Extremum(basePrice, ExtremumType.PeakByClose);
            Quotation quotation5 = new Quotation(ds5) { Id = 5, Open = 1.09193, High = 1.09307, Low = 1.09165, Close = 1.09207, Volume = 1819 };
            ExtremumGroup extremumGroup = new ExtremumGroup(master, null);
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            mockManager.Setup(m => m.GetDataSet(5)).Returns(ds5);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);

            //Assert
            processor.MaxChartPointsForExtremumGroup = 6;
            processor.MinDistanceBetweenChartPoints = 0.0001;
            var result = processor.GetChartPoints(extremumGroup);
            var expectedResult = new List<ChartPoint>();
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09207));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09227));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09247));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09267));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09287));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09307));
            var isEqual = expectedResult.HasEqualItems(result);
            Assert.IsTrue(isEqual);

        }

        [TestMethod]
        public void GetChartPoints_IfOnlyMasterPeakExtremumWithShortShadow_PointsDifferByOnePipBetweenThemselves()
        {

            //Arrange
            DataSet ds5 = utf.getDataSet(5);
            Price price5 = utf.getPrice(ds5);
            Quotation quotation5 = new Quotation(ds5) { Id = 5, Open = 1.09193, High = 1.09209, Low = 1.09165, Close = 1.09207, Volume = 1819 };
            Extremum master = new Extremum(price5, ExtremumType.PeakByClose);
            ExtremumGroup extremumGroup = new ExtremumGroup(master, null);
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            mockManager.Setup(m => m.GetDataSet(5)).Returns(ds5);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);
            processor.MinDistanceBetweenChartPoints = 1;

            //Assert
            processor.MaxChartPointsForExtremumGroup = 6;
            processor.MinDistanceBetweenChartPoints = 0.0001;
            var result = processor.GetChartPoints(extremumGroup);
            var expectedResult = new List<ChartPoint>();
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09207));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09209));
            var isEqual = expectedResult.HasEqualItems(result);
            Assert.IsTrue(isEqual);

        }

        [TestMethod]
        public void GetChartPoints_IfOnlySlavePeakExtremumWithLongShadow_PointsFromShadowTopToHighestAdjacentShadow()
        {

            //Arrange
            DataSet ds5 = utf.getDataSet(5);
            Price price5 = utf.getPrice(ds5);
            Quotation quotation5 = new Quotation(ds5) { Id = 5, Open = 1.09193, High = 1.0927, Low = 1.09165, Close = 1.09207, Volume = 1819 };
            Extremum slave = new Extremum(price5, ExtremumType.PeakByHigh);
            ExtremumGroup extremumGroup = new ExtremumGroup(null, slave);
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            mockManager.Setup(m => m.GetDataSet(5)).Returns(ds5);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);
            processor.MinDistanceBetweenChartPoints = 1;

            //Assert
            processor.MaxChartPointsForExtremumGroup = 6;
            processor.MinDistanceBetweenChartPoints = 0.0001;
            var result = processor.GetChartPoints(extremumGroup);
            var expectedResult = new List<ChartPoint>();
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.0927));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09207));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.092574));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.092196));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.092448));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.092322));
            var isEqual = expectedResult.HasEqualItems(result);
            Assert.IsTrue(isEqual);

        }

        [TestMethod]
        public void GetChartPoints_IfOnlySlavePeakExtremumWithShortShadow_PointsFromShadowTopToHighestAdjacentShadow()
        {

            //Arrange
            DataSet ds5 = utf.getDataSet(5);
            Price price5 = utf.getPrice(ds5);
            Quotation quotation5 = new Quotation(ds5) { Id = 5, Open = 1.09193, High = 1.09209, Low = 1.09165, Close = 1.09207, Volume = 1819 };
            Extremum slave = new Extremum(price5, ExtremumType.PeakByHigh);
            ExtremumGroup extremumGroup = new ExtremumGroup(null, slave);
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            mockManager.Setup(m => m.GetDataSet(5)).Returns(ds5);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);
            processor.MinDistanceBetweenChartPoints = 1;

            //Assert
            processor.MaxChartPointsForExtremumGroup = 6;
            processor.MinDistanceBetweenChartPoints = 0.0001;
            var result = processor.GetChartPoints(extremumGroup);
            var expectedResult = new List<ChartPoint>();
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09207));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09209));
            var isEqual = expectedResult.HasEqualItems(result);
            Assert.IsTrue(isEqual);

        }

        [TestMethod]
        public void GetChartPoints_IfBothPeakExtremaAndSlaveEarlier_PointsAreProperlyGenerated()
        {

            //Arrange
            DataSet ds5 = utf.getDataSet(5);
            Price price5 = utf.getPrice(ds5);
            Quotation quotation5 = new Quotation(ds5) { Id = 5, Open = 1.09012, High = 1.09307, Low = 1.08947, Close = 1.09127, Volume = 1819 };
            Extremum slave = new Extremum(price5, ExtremumType.PeakByHigh);

            DataSet ds6 = utf.getDataSet(6);
            Price price6 = utf.getPrice(ds6);
            Quotation quotation6 = new Quotation(ds6) { Id = 6, Open = 1.09127, High = 1.09235, Low = 1.09095, Close = 1.09207, Volume = 1519 };
            Extremum master = new Extremum(price6, ExtremumType.PeakByClose);

            ExtremumGroup extremumGroup = new ExtremumGroup(master, slave);
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            mockManager.Setup(m => m.GetDataSet(5)).Returns(ds5);
            mockManager.Setup(m => m.GetDataSet(6)).Returns(ds6);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);

            //Assert
            processor.MaxChartPointsForExtremumGroup = 6;
            processor.MinDistanceBetweenChartPoints = 0.0001;
            var result = processor.GetChartPoints(extremumGroup);
            var expectedResult = new List<ChartPoint>();
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09207));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09227));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09247));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09267));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09287));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09307));
            var isEqual = expectedResult.HasEqualItems(result);
            Assert.IsTrue(isEqual);

        }

        [TestMethod]
        public void GetChartPoints_IfBothPeakExtremaAndSlaveLater_PointsAreProperlyGenerated()
        {

            //Arrange
            DataSet ds5 = utf.getDataSet(5);
            Price price5 = utf.getPrice(ds5);
            Quotation quotation5 = new Quotation(ds5) { Id = 5, Open = 1.09127, High = 1.09235, Low = 1.09095, Close = 1.09207, Volume = 1519 };
            Extremum master = new Extremum(price5, ExtremumType.PeakByClose);

            DataSet ds6 = utf.getDataSet(6);
            Price price6 = utf.getPrice(ds6);
            Quotation quotation6 = new Quotation(ds6) { Id = 6, Open = 1.09207, High = 1.09307, Low = 1.08947, Close = 1.09067, Volume = 1819 };
            Extremum slave = new Extremum(price6, ExtremumType.PeakByHigh);


            ExtremumGroup extremumGroup = new ExtremumGroup(master, slave);
            Mock<IProcessManager> mockManager = new Mock<IProcessManager>();
            mockManager.Setup(m => m.GetDataSet(5)).Returns(ds5);
            mockManager.Setup(m => m.GetDataSet(6)).Returns(ds6);

            //Act
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);

            //Assert
            processor.MaxChartPointsForExtremumGroup = 6;
            processor.MinDistanceBetweenChartPoints = 0.0001;
            var result = processor.GetChartPoints(extremumGroup);
            var expectedResult = new List<ChartPoint>();
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09207));
            expectedResult.Add(new ChartPoint(master.GetIndexNumber(), 1.09227));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09247));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09267));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09287));
            expectedResult.Add(new ChartPoint(slave.GetIndexNumber(), 1.09307));
            var isEqual = expectedResult.HasEqualItems(result);
            Assert.IsTrue(isEqual);

        }




        [Ignore]
        [TestMethod]
        public void GetChartPoints_IfOnlyMasterTroughExtremumWithLongShadow_OnlyLimitedNumberOfPointsIsCreated()
        {
            Assert.Fail("Not implemented");
        }

        [Ignore]
        [TestMethod]
        public void GetChartPoints_IfOnlyMasterTroughExtremumWithShortShadow_PointsDifferByOnePipBetweenThemselves()
        {
            Assert.Fail("Not implemented");
        }

        [Ignore]
        [TestMethod]
        public void GetChartPoints_IfOnlySlaveTroughExtremumWithLongShadow_PointsFromShadowTopToHighestAdjacentShadow()
        {
            Assert.Fail("Not implemented");
        }

        [Ignore]
        [TestMethod]
        public void GetChartPoints_IfOnlySlaveTroughExtremumWithShortShadow_PointsFromShadowTopToHighestAdjacentShadow()
        {
            Assert.Fail("Not implemented");
        }

        [Ignore]
        [TestMethod]
        public void GetChartPoints_IfBothTroughExtremaAndSlaveEarlier_PointsAreProperlyGenerated()
        {
            Assert.Fail("Not implemented");
        }

        [Ignore]
        [TestMethod]
        public void GetChartPoints_IfBothTroughExtremaAndSlaveLater_PointsAreProperlyGenerated()
        {
            Assert.Fail("Not implemented");
        }





        #endregion GET_CHART_POINTS



        #region PROCESS
        [Ignore]
        [TestMethod]
        public void Process_IfNewTrendlineAndStartIndexLaterThanGivenItem_NothingHappens()
        {

            //Arrange
            Mock<IProcessManager> mockManager = null; // getDefaultMockManager(4);
            //Extremum baseMaster = new Extremum(getDefaultAtsSettings(), ExtremumType.PeakByClose, 4);
            //Extremum secondMaster = new Extremum(getDefaultAtsSettings(), ExtremumType.PeakByClose, 10);

            //Act
            Trendline trendline = new Trendline(null);
            TrendlineProcessor processor = new TrendlineProcessor(mockManager.Object);

            //processor.Process(trendline, 

            //Assert
            Assert.Fail("Not implemented yet");

        }

        [Ignore]
        [TestMethod]
        public void Process_IfNewTrendlineAndGivenItemIsStartIndex_PeakObjectIsCreated()
        {
            Assert.Fail("Not implemented yet");
        }

        [Ignore]
        [TestMethod]
        public void Process_IfSlaveIndexOFExtremumIsProcessed_ItIsCalculatedLikeRegularQuotation()
        {
            Assert.Fail("Not implemented yet");
        }

        [Ignore]
        [TestMethod]
        public void Process_AfterNormalQuotationProcessed_CounterIsLargerByOne()
        {
            Assert.Fail("Not implemented yet");
        }

        [Ignore]
        [TestMethod]
        public void Process_AfterNormalQuotationProcessed_TotalDistanceFromTrendlineIsProper()
        {
            Assert.Fail("Not implemented yet");
        }

        [Ignore]
        [TestMethod]
        public void Process_AfterBreakIsFound_TrendlineTypeIsChanged()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_AfterBreakIsFound_CurrentRangeIsClosedWithNextBreakObjectSet()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_AfterBreakIsFound_IfNextQuotationIsBackToTrend_SetThisTrendBreakAsSingleSession()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_AfterBreakIsFoundAsSingleSession_IfSecondQuotationIsProcessedItIsSkipped()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_AfterBreakIsFound_FromNextQuoationNextRangeIsStarted()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_IfExtremumTooFarFromTrendline_ItIsCalculatedAsRegularQuotation()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_IfExtremumCloseEnoughToTrendline_NewTrendHitIsCreated()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_IfExtremumCloseEnoughToTrendline_CurrentRangeIsClosed()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_IfExtremumCloseEnoughToTrendline_IfSlaveIndexProcessedAfterward_ItIsSkipped()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_IfTrendHitIsSet_IfNextQuotationAfterSlaveIndexIsProcessed_NewRangeIsCreated()
        {
            Assert.Fail("Not implemented yet");
        }
        [Ignore]
        [TestMethod]
        public void Process_IfTrendHitIsSet_()
        {
            Assert.Fail("Not implemented yet");
        }

        #endregion PROCESS

    }
}
