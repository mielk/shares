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
    public class ExtremumProcessorUnitTests
    {

        private double MAX_DOUBLE_COMPARISON_DIFFERENCE = 0.00000001;

        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const int DEFAULT_SIMULATION_ID = 1;
        private DateTime DEFAULT_BASE_DATE = new DateTime(2016, 1, 15, 22, 25, 0);



        #region INFRASTRUCTURE

        private DataSet getDataSetWithQuotation(int indexNumber, double open, double high, double low, double close, double volume)
        {
            return getDataSetWithQuotation(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber, open, high, low, close, volume);
        }

        private DataSet getDataSetWithQuotation(int assetId, int timeframeId, int indexNumber, double open, double high, double low, double close, double volume)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            Quotation q = new Quotation(ds) { Open = open, High = high, Low = low, Close = close, Volume = volume };
            return ds;

        }

        private Timeframe getTimeframe(int timeframeId)
        {
            return new Timeframe(timeframeId, "5M", TimeframeUnit.Minutes, 5);
        }

        private DataSet getDataSetWithQuotationAndPrice(int indexNumber, double open, double high, double low, double close, double volume)
        {
            return getDataSetWithQuotationAndPrice(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber, open, high, low, close, volume);
        }

        private DataSet getDataSetWithQuotationAndPrice(int assetId, int timeframeId, int indexNumber, double open, double high, double low, double close, double volume)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            Quotation q = new Quotation(ds) { Open = open, High = high, Low = low, Close = close, Volume = volume };
            Price p = new Price(ds);
            return ds;

        }


        #endregion INFRASTRUCTURE



        #region ISUPDATED_FLAG

        [Ignore]
        [TestMethod]
        public void AfterBeingProcessed_ItemExtremumHasItsUpdatedFlagSetToTrue()
        {
            Assert.Fail("Zastanowić się nad tym testem, czy powinien być w tym miejscu.");
            Assert.Fail("Na pewno trzeba to sprawdzić, ale chyba z poziomu klasy wywołującej");
            Assert.Fail("indexNumber z bardziej skomplikowanymi parametrami wejściowymi.");
            Assert.Fail("Not implemented yet");
        }

        #endregion ISUPDATED_FLAG


        #region PEAK_BY_CLOSE.IS_EXTREMUM

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByClose_IfProcessedDataSetIndexIsLessOrEqualToMinimumRequiredLowerQuotations()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotation(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotation(2, 1.09193, 1.09256, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotation(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotation(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotation(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotation(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotation(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet3, ExtremumType.PeakByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByClose_IfAtLeastOneClosePriceInThePreviousThreeQuotationsIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet123 = getDataSetWithQuotation(123, 1.08965, 1.08971, 1.08889, 1.08933, 845);
            DataSet dataSet124 = getDataSetWithQuotation(124, 1.08935, 1.08964, 1.08885, 1.089, 993);
            DataSet dataSet125 = getDataSetWithQuotation(125, 1.08897, 1.08921, 1.08862, 1.08889, 681);
            DataSet dataSet126 = getDataSetWithQuotation(126, 1.08889, 1.08903, 1.0886, 1.08889, 876);
            DataSet dataSet127 = getDataSetWithQuotation(127, 1.08891, 1.08922, 1.0886, 1.08894, 923);
            DataSet dataSet128 = getDataSetWithQuotation(128, 1.08893, 1.08893, 1.08771, 1.08805, 1743);
            DataSet dataSet129 = getDataSetWithQuotation(129, 1.08807, 1.08845, 1.08749, 1.08765, 1291);
            DataSet dataSet130 = getDataSetWithQuotation(130, 1.08769, 1.0881, 1.08731, 1.08752, 1385);
            DataSet dataSet131 = getDataSetWithQuotation(131, 1.08752, 1.08829, 1.08747, 1.08825, 1337);
            DataSet dataSet132 = getDataSetWithQuotation(132, 1.08824, 1.08849, 1.0881, 1.08829, 1084);
            DataSet dataSet133 = getDataSetWithQuotation(133, 1.08831, 1.08872, 1.08817, 1.08853, 980);
            mockedManager.Setup(m => m.GetDataSet(123)).Returns(dataSet123);
            mockedManager.Setup(m => m.GetDataSet(124)).Returns(dataSet124);
            mockedManager.Setup(m => m.GetDataSet(125)).Returns(dataSet125);
            mockedManager.Setup(m => m.GetDataSet(126)).Returns(dataSet126);
            mockedManager.Setup(m => m.GetDataSet(127)).Returns(dataSet127);
            mockedManager.Setup(m => m.GetDataSet(128)).Returns(dataSet128);
            mockedManager.Setup(m => m.GetDataSet(129)).Returns(dataSet129);
            mockedManager.Setup(m => m.GetDataSet(130)).Returns(dataSet130);
            mockedManager.Setup(m => m.GetDataSet(131)).Returns(dataSet131);
            mockedManager.Setup(m => m.GetDataSet(132)).Returns(dataSet132);
            mockedManager.Setup(m => m.GetDataSet(133)).Returns(dataSet133);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet127, ExtremumType.PeakByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByClose_IfAtLeastOneClosePriceInThePreviousThreeQuotationsIsEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet123 = getDataSetWithQuotation(123, 1.08965, 1.08971, 1.08889, 1.08933, 845);
            DataSet dataSet124 = getDataSetWithQuotation(124, 1.08935, 1.08964, 1.08885, 1.08894, 993);
            DataSet dataSet125 = getDataSetWithQuotation(125, 1.08897, 1.08921, 1.08862, 1.08889, 681);
            DataSet dataSet126 = getDataSetWithQuotation(126, 1.08889, 1.08903, 1.0886, 1.08889, 876);
            DataSet dataSet127 = getDataSetWithQuotation(127, 1.08891, 1.08922, 1.0886, 1.08894, 923);
            DataSet dataSet128 = getDataSetWithQuotation(128, 1.08893, 1.08893, 1.08771, 1.08805, 1743);
            DataSet dataSet129 = getDataSetWithQuotation(129, 1.08807, 1.08845, 1.08749, 1.08765, 1291);
            DataSet dataSet130 = getDataSetWithQuotation(130, 1.08769, 1.0881, 1.08731, 1.08752, 1385);
            DataSet dataSet131 = getDataSetWithQuotation(131, 1.08752, 1.08829, 1.08747, 1.08825, 1337);
            DataSet dataSet132 = getDataSetWithQuotation(132, 1.08824, 1.08849, 1.0881, 1.08829, 1084);
            DataSet dataSet133 = getDataSetWithQuotation(133, 1.08831, 1.08872, 1.08817, 1.08853, 980);
            mockedManager.Setup(m => m.GetDataSet(123)).Returns(dataSet123);
            mockedManager.Setup(m => m.GetDataSet(124)).Returns(dataSet124);
            mockedManager.Setup(m => m.GetDataSet(125)).Returns(dataSet125);
            mockedManager.Setup(m => m.GetDataSet(126)).Returns(dataSet126);
            mockedManager.Setup(m => m.GetDataSet(127)).Returns(dataSet127);
            mockedManager.Setup(m => m.GetDataSet(128)).Returns(dataSet128);
            mockedManager.Setup(m => m.GetDataSet(129)).Returns(dataSet129);
            mockedManager.Setup(m => m.GetDataSet(130)).Returns(dataSet130);
            mockedManager.Setup(m => m.GetDataSet(131)).Returns(dataSet131);
            mockedManager.Setup(m => m.GetDataSet(132)).Returns(dataSet132);
            mockedManager.Setup(m => m.GetDataSet(133)).Returns(dataSet133);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet127, ExtremumType.PeakByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByClose_IfAtLeastOneClosePriceInTheNextThreeQuotationsIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet142 = getDataSetWithQuotation(142, 1.08852, 1.08856, 1.08798, 1.08825, 2227);
            DataSet dataSet143 = getDataSetWithQuotation(143, 1.08825, 1.0885, 1.08795, 1.08809, 1904);
            DataSet dataSet144 = getDataSetWithQuotation(144, 1.08806, 1.08827, 1.0879, 1.08816, 1484);
            DataSet dataSet145 = getDataSetWithQuotation(145, 1.08816, 1.08843, 1.08738, 1.08756, 1690);
            DataSet dataSet146 = getDataSetWithQuotation(146, 1.08759, 1.08849, 1.08754, 1.08836, 1813);
            DataSet dataSet147 = getDataSetWithQuotation(147, 1.08838, 1.08843, 1.08797, 1.0883, 1487);
            DataSet dataSet148 = getDataSetWithQuotation(148, 1.08829, 1.08848, 1.08788, 1.08836, 1635);
            DataSet dataSet149 = getDataSetWithQuotation(149, 1.08838, 1.08872, 1.08829, 1.08865, 1337);
            DataSet dataSet150 = getDataSetWithQuotation(150, 1.08865, 1.08906, 1.08848, 1.08906, 1383);
            DataSet dataSet151 = getDataSetWithQuotation(151, 1.08905, 1.08935, 1.08875, 1.08887, 1410);
            mockedManager.Setup(m => m.GetDataSet(142)).Returns(dataSet142);
            mockedManager.Setup(m => m.GetDataSet(143)).Returns(dataSet143);
            mockedManager.Setup(m => m.GetDataSet(144)).Returns(dataSet144);
            mockedManager.Setup(m => m.GetDataSet(145)).Returns(dataSet145);
            mockedManager.Setup(m => m.GetDataSet(146)).Returns(dataSet146);
            mockedManager.Setup(m => m.GetDataSet(147)).Returns(dataSet147);
            mockedManager.Setup(m => m.GetDataSet(148)).Returns(dataSet148);
            mockedManager.Setup(m => m.GetDataSet(149)).Returns(dataSet149);
            mockedManager.Setup(m => m.GetDataSet(150)).Returns(dataSet150);
            mockedManager.Setup(m => m.GetDataSet(151)).Returns(dataSet151);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet146, ExtremumType.PeakByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForPeakByClose_EvenIsSomeClosePricesInTheNextThreeQuotationsAreEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet142 = getDataSetWithQuotation(142, 1.08852, 1.08856, 1.08798, 1.08825, 2227);
            DataSet dataSet143 = getDataSetWithQuotation(143, 1.08825, 1.0885, 1.08795, 1.08809, 1904);
            DataSet dataSet144 = getDataSetWithQuotation(144, 1.08806, 1.08827, 1.0879, 1.08816, 1484);
            DataSet dataSet145 = getDataSetWithQuotation(145, 1.08816, 1.08843, 1.08738, 1.08756, 1690);
            DataSet dataSet146 = getDataSetWithQuotation(146, 1.08759, 1.08849, 1.08754, 1.08836, 1813);
            DataSet dataSet147 = getDataSetWithQuotation(147, 1.08838, 1.08843, 1.08797, 1.0883, 1487);
            DataSet dataSet148 = getDataSetWithQuotation(148, 1.08829, 1.08848, 1.08788, 1.08836, 1635);
            DataSet dataSet149 = getDataSetWithQuotation(149, 1.08838, 1.08872, 1.08829, 1.08825, 1337);
            DataSet dataSet150 = getDataSetWithQuotation(150, 1.08865, 1.08906, 1.08848, 1.08906, 1383);
            DataSet dataSet151 = getDataSetWithQuotation(151, 1.08905, 1.08935, 1.08875, 1.08887, 1410);
            mockedManager.Setup(m => m.GetDataSet(142)).Returns(dataSet142);
            mockedManager.Setup(m => m.GetDataSet(143)).Returns(dataSet143);
            mockedManager.Setup(m => m.GetDataSet(144)).Returns(dataSet144);
            mockedManager.Setup(m => m.GetDataSet(145)).Returns(dataSet145);
            mockedManager.Setup(m => m.GetDataSet(146)).Returns(dataSet146);
            mockedManager.Setup(m => m.GetDataSet(147)).Returns(dataSet147);
            mockedManager.Setup(m => m.GetDataSet(148)).Returns(dataSet148);
            mockedManager.Setup(m => m.GetDataSet(149)).Returns(dataSet149);
            mockedManager.Setup(m => m.GetDataSet(150)).Returns(dataSet150);
            mockedManager.Setup(m => m.GetDataSet(151)).Returns(dataSet151);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet146, ExtremumType.PeakByClose);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForPeakByClose_IfAtLeastThreeEarlierAndThreeLaterQuotationsHaveLowerClosePrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotation(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotation(2, 1.09193, 1.09256, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotation(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotation(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotation(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotation(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotation(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet8, ExtremumType.PeakByClose);
            Assert.IsTrue(result);

        }

        #endregion PEAK_BY_CLOSE.IS_EXTREMUM


        #region PEAK_BY_HIGH.IS_EXTREMUM

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByHigh_IfProcessedDataSetIndexIsLessOrEqualToMinimumRequiredLowerQuotations()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotation(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotation(2, 1.09193, 1.09256, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotation(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotation(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet3, ExtremumType.PeakByHigh);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByHigh_IfAtLeastOneHighPriceInThePreviousThreeQuotationsIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotation(1, 1.09181, 1.09188, 1.09176, 1.09184, 1411);
            DataSet dataSet2 = getDataSetWithQuotation(2, 1.09193, 1.09256, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotation(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotation(4, 1.0915, 1.0919, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09147, 1.09114, 1.09145, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09144, 1.09149, 1.0915, 1.09147, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet4, ExtremumType.PeakByHigh);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByHigh_IfAtLeastOneHighPriceInThePreviousThreeQuotationsIsEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotation(1, 1.09181, 1.09188, 1.09176, 1.09184, 1411);
            DataSet dataSet2 = getDataSetWithQuotation(2, 1.09183, 1.0919, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotation(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotation(4, 1.0915, 1.0919, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09147, 1.09114, 1.09145, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09144, 1.09149, 1.0915, 1.09147, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);



            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet3, ExtremumType.PeakByHigh);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForPeakByHigh_IfAtLeastOneHighPriceInTheNextThreeQuotationsIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotation(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotation(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotation(11, 1.09149, 1.09196, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet9, ExtremumType.PeakByHigh);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForPeakByHigh_EvenIsSomeHighPricesInTheNextThreeQuotationsAreEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotation(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotation(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotation(11, 1.09149, 1.09189, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet9, ExtremumType.PeakByHigh);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForPeakByHigh_IfAtLeastThreeEarlierAndThreeLaterQuotationsHaveLowerHighPrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotation(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotation(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotation(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet9, ExtremumType.PeakByHigh);
            Assert.IsTrue(result);

        }

        #endregion PEAK_BY_HIGH.IS_EXTREMUM


        #region TROUGH_BY_CLOSE.IS_EXTREMUM

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByClose_IfProcessedDataSetIndexIsLessOrEqualToMinimumRequiredLowerQuotations()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotation(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotation(2, 1.09193, 1.09256, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotation(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotation(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotation(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotation(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotation(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet3, ExtremumType.TroughByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByClose_IfAtLeastOneClosePriceInThePreviousThreeQuotationsIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet269 = getDataSetWithQuotation(269, 1.08883, 1.08906, 1.08881, 1.08892, 362);
            DataSet dataSet270 = getDataSetWithQuotation(270, 1.08891, 1.0894, 1.0889, 1.08924, 414);
            DataSet dataSet271 = getDataSetWithQuotation(271, 1.08922, 1.0896, 1.08916, 1.08952, 419);
            DataSet dataSet272 = getDataSetWithQuotation(272, 1.08952, 1.08973, 1.0895, 1.08951, 1090);
            DataSet dataSet273 = getDataSetWithQuotation(273, 1.08951, 1.08953, 1.08925, 1.08929, 869);
            DataSet dataSet274 = getDataSetWithQuotation(274, 1.08928, 1.08936, 1.08926, 1.08936, 151);
            DataSet dataSet275 = getDataSetWithQuotation(275, 1.08927, 1.08945, 1.08926, 1.08936, 155);
            DataSet dataSet276 = getDataSetWithQuotation(276, 1.08938, 1.0894, 1.08932, 1.08935, 223);
            DataSet dataSet277 = getDataSetWithQuotation(277, 1.08937, 1.08939, 1.08928, 1.08938, 237);
            mockedManager.Setup(m => m.GetDataSet(269)).Returns(dataSet269);
            mockedManager.Setup(m => m.GetDataSet(270)).Returns(dataSet270);
            mockedManager.Setup(m => m.GetDataSet(271)).Returns(dataSet271);
            mockedManager.Setup(m => m.GetDataSet(272)).Returns(dataSet272);
            mockedManager.Setup(m => m.GetDataSet(273)).Returns(dataSet273);
            mockedManager.Setup(m => m.GetDataSet(274)).Returns(dataSet274);
            mockedManager.Setup(m => m.GetDataSet(275)).Returns(dataSet275);
            mockedManager.Setup(m => m.GetDataSet(276)).Returns(dataSet276);
            mockedManager.Setup(m => m.GetDataSet(277)).Returns(dataSet277);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet273, ExtremumType.TroughByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByClose_IfAtLeastOneClosePriceInThePreviousThreeQuotationsIsEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet269 = getDataSetWithQuotation(269, 1.08883, 1.08906, 1.08881, 1.0895, 362);
            DataSet dataSet270 = getDataSetWithQuotation(270, 1.08891, 1.0894, 1.0889, 1.08929, 414);
            DataSet dataSet271 = getDataSetWithQuotation(271, 1.08922, 1.0896, 1.08916, 1.08952, 419);
            DataSet dataSet272 = getDataSetWithQuotation(272, 1.08952, 1.08973, 1.0895, 1.08951, 1090);
            DataSet dataSet273 = getDataSetWithQuotation(273, 1.08951, 1.08953, 1.08925, 1.08929, 869);
            DataSet dataSet274 = getDataSetWithQuotation(274, 1.08928, 1.08936, 1.08926, 1.08936, 151);
            DataSet dataSet275 = getDataSetWithQuotation(275, 1.08927, 1.08945, 1.08926, 1.08936, 155);
            DataSet dataSet276 = getDataSetWithQuotation(276, 1.08938, 1.0894, 1.08932, 1.08935, 223);
            DataSet dataSet277 = getDataSetWithQuotation(277, 1.08937, 1.08939, 1.08928, 1.08938, 237);
            mockedManager.Setup(m => m.GetDataSet(269)).Returns(dataSet269);
            mockedManager.Setup(m => m.GetDataSet(270)).Returns(dataSet270);
            mockedManager.Setup(m => m.GetDataSet(271)).Returns(dataSet271);
            mockedManager.Setup(m => m.GetDataSet(272)).Returns(dataSet272);
            mockedManager.Setup(m => m.GetDataSet(273)).Returns(dataSet273);
            mockedManager.Setup(m => m.GetDataSet(274)).Returns(dataSet274);
            mockedManager.Setup(m => m.GetDataSet(275)).Returns(dataSet275);
            mockedManager.Setup(m => m.GetDataSet(276)).Returns(dataSet276);
            mockedManager.Setup(m => m.GetDataSet(277)).Returns(dataSet277);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet273, ExtremumType.TroughByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByClose_IfAtLeastOneClosePriceInTheNextThreeQuotationsIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotation(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotation(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotation(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotation(18, 1.09099, 1.09129, 1.09092, 1.0905, 745);
            DataSet dataSet19 = getDataSetWithQuotation(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotation(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotation(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet16, ExtremumType.TroughByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForTroughByClose_EvenIsSomeClosePricesInTheNextThreeQuotationsAreEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotation(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotation(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotation(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotation(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotation(19, 1.09111, 1.09197, 1.09088, 1.0906, 1140);
            DataSet dataSet20 = getDataSetWithQuotation(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotation(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet16, ExtremumType.TroughByClose);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForTroughByClose_IfAtLeastThreeEarlierAndThreeLaterQuotationsHaveHigherClosePrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotation(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotation(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotation(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotation(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotation(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotation(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotation(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet16, ExtremumType.TroughByClose);
            Assert.IsTrue(result);

        }

        #endregion TROUGH_BY_CLOSE.IS_EXTREMUM
        

        #region TROUGH_BY_LOW.IS_EXTREMUM

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByLow_IfProcessedDataSetIndexIsLessOrEqualToMinimumRequiredLowerQuotations()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotation(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotation(2, 1.09193, 1.09256, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotation(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotation(4, 1.0915, 1.0916, 1.09091, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotation(5, 1.09111, 1.09124, 1.09111, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotation(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotation(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotation(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotation(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotation(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotation(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet3, ExtremumType.TroughByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByLow_IfAtLeastOneClosePriceInThePreviousThreeQuotationsIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet202 = getDataSetWithQuotation(202, 1.08935, 1.08963, 1.089, 1.089, 1444);
            DataSet dataSet203 = getDataSetWithQuotation(203, 1.08901, 1.08958, 1.08896, 1.08954, 1189);
            DataSet dataSet204 = getDataSetWithQuotation(204, 1.08954, 1.08975, 1.0893, 1.08932, 1027);
            DataSet dataSet205 = getDataSetWithQuotation(205, 1.08929, 1.08938, 1.08912, 1.08921, 959);
            DataSet dataSet206 = getDataSetWithQuotation(206, 1.0892, 1.08943, 1.08901, 1.08939, 1284);
            DataSet dataSet207 = getDataSetWithQuotation(207, 1.08938, 1.08955, 1.08922, 1.08946, 1217);
            DataSet dataSet208 = getDataSetWithQuotation(208, 1.08948, 1.08949, 1.08921, 1.08937, 1082);
            DataSet dataSet209 = getDataSetWithQuotation(209, 1.08936, 1.08969, 1.08919, 1.08951, 1142);
            DataSet dataSet210 = getDataSetWithQuotation(210, 1.0895, 1.08953, 1.08929, 1.08946, 850);
            DataSet dataSet211 = getDataSetWithQuotation(211, 1.08947, 1.08964, 1.08922, 1.08928, 1177);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet206, ExtremumType.TroughByLow);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByLow_IfAtLeastOneClosePriceInThePreviousThreeQuotationsIsEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet202 = getDataSetWithQuotation(202, 1.08935, 1.08963, 1.0891, 1.089, 1444);
            DataSet dataSet203 = getDataSetWithQuotation(203, 1.08901, 1.08958, 1.08901, 1.08954, 1189);
            DataSet dataSet204 = getDataSetWithQuotation(204, 1.08954, 1.08975, 1.0893, 1.08932, 1027);
            DataSet dataSet205 = getDataSetWithQuotation(205, 1.08929, 1.08938, 1.08912, 1.08921, 959);
            DataSet dataSet206 = getDataSetWithQuotation(206, 1.0892, 1.08943, 1.08901, 1.08939, 1284);
            DataSet dataSet207 = getDataSetWithQuotation(207, 1.08938, 1.08955, 1.08922, 1.08946, 1217);
            DataSet dataSet208 = getDataSetWithQuotation(208, 1.08948, 1.08949, 1.08921, 1.08937, 1082);
            DataSet dataSet209 = getDataSetWithQuotation(209, 1.08936, 1.08969, 1.08919, 1.08951, 1142);
            DataSet dataSet210 = getDataSetWithQuotation(210, 1.0895, 1.08953, 1.08929, 1.08946, 850);
            DataSet dataSet211 = getDataSetWithQuotation(211, 1.08947, 1.08964, 1.08922, 1.08928, 1177);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet206, ExtremumType.TroughByLow);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsFalseForTroughByLow_IfAtLeastOneClosePriceInTheNextThreeQuotationsIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotation(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotation(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotation(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotation(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotation(19, 1.09111, 1.09197, 1.09038, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotation(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotation(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotation(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotation(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet17, ExtremumType.TroughByClose);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForTroughByLow_EvenIsSomeClosePricesInTheNextThreeQuotationsAreEqual()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotation(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotation(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotation(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotation(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotation(19, 1.09111, 1.09197, 1.09041, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotation(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotation(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotation(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotation(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet16, ExtremumType.TroughByClose);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsExtremum_ReturnsTrueForTroughByLow_IfAtLeastThreeEarlierAndThreeLaterQuotationsHaveHigherClosePrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet12 = getDataSetWithQuotation(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotation(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotation(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotation(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotation(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotation(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotation(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotation(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotation(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotation(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotation(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotation(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.IsExtremum(dataSet16, ExtremumType.TroughByClose);
            Assert.IsTrue(result);

        }

        #endregion TROUGH_BY_LOW.IS_EXTREMUM


        #region CALCULATE_EARLIER_AMPLITUDE

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForPeakByClose_IfThereIsNoHigherClosePriceEarlier()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09193, 1.09256, 1.09085, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum);
            double expectedResult = 0.00086;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForPeakByClose_IfThereIsHigherClosePriceEarlierAndLowPriceAtThisQuotationIsLowerThanProcessedClosePrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09193, 1.09256, 1.09039, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09169, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09167, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum20 = new Extremum(dataSet20.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum20);
            double expectedResult = 0.0013;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForPeakByClose_WhenLookingForLastHigherBeforeIgnoresQuotationsWithHigherHighPrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09193, 1.09256, 1.09039, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum20 = new Extremum(dataSet20.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum20);
            double expectedResult = 0.0013;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForPeakByClose_IfExtremumHasMoreEarlierMinorsThanMaxSerie()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet54 = getDataSetWithQuotationAndPrice(54, 1.09162, 1.09178, 1.09148, 1.09153, 981);
            DataSet dataSet55 = getDataSetWithQuotationAndPrice(55, 1.09152, 1.09152, 1.09094, 1.09114, 1151);
            DataSet dataSet56 = getDataSetWithQuotationAndPrice(56, 1.09113, 1.09121, 1.09069, 1.09086, 1219);
            DataSet dataSet57 = getDataSetWithQuotationAndPrice(57, 1.09092, 1.09092, 1.09031, 1.09032, 1155);
            DataSet dataSet58 = getDataSetWithQuotationAndPrice(58, 1.09034, 1.09055, 1.09019, 1.0905, 1304);
            DataSet dataSet59 = getDataSetWithQuotationAndPrice(59, 1.09048, 1.09055, 1.08928, 1.08961, 2252);
            DataSet dataSet60 = getDataSetWithQuotationAndPrice(60, 1.08963, 1.09008, 1.08958, 1.08977, 1971);
            DataSet dataSet61 = getDataSetWithQuotationAndPrice(61, 1.08978, 1.09055, 1.08974, 1.09047, 2171);
            DataSet dataSet62 = getDataSetWithQuotationAndPrice(62, 1.09047, 1.09075, 1.09023, 1.09062, 1654);
            DataSet dataSet63 = getDataSetWithQuotationAndPrice(63, 1.09063, 1.09072, 1.09024, 1.09056, 1589);
            DataSet dataSet64 = getDataSetWithQuotationAndPrice(64, 1.09055, 1.09055, 1.08983, 1.09001, 1299);
            DataSet dataSet65 = getDataSetWithQuotationAndPrice(65, 1.08997, 1.09017, 1.08951, 1.08984, 1636);
            DataSet dataSet66 = getDataSetWithQuotationAndPrice(66, 1.08984, 1.09011, 1.08976, 1.0898, 1355);
            DataSet dataSet67 = getDataSetWithQuotationAndPrice(67, 1.08981, 1.09002, 1.08956, 1.08977, 1205);
            DataSet dataSet68 = getDataSetWithQuotationAndPrice(68, 1.08978, 1.09008, 1.08968, 1.08982, 1155);
            DataSet dataSet69 = getDataSetWithQuotationAndPrice(69, 1.0898, 1.09017, 1.08974, 1.09008, 893);
            DataSet dataSet70 = getDataSetWithQuotationAndPrice(70, 1.09009, 1.09022, 1.08996, 1.08996, 1013);
            DataSet dataSet71 = getDataSetWithQuotationAndPrice(71, 1.08998, 1.09022, 1.08984, 1.09015, 1077);
            DataSet dataSet72 = getDataSetWithQuotationAndPrice(72, 1.09011, 1.09037, 1.09009, 1.0903, 814);
            DataSet dataSet73 = getDataSetWithQuotationAndPrice(73, 1.09031, 1.09037, 1.0901, 1.09031, 905);
            DataSet dataSet74 = getDataSetWithQuotationAndPrice(74, 1.09031, 1.09069, 1.0902, 1.09069, 901);
            mockedManager.Setup(m => m.GetDataSet(54)).Returns(dataSet54);
            mockedManager.Setup(m => m.GetDataSet(55)).Returns(dataSet55);
            mockedManager.Setup(m => m.GetDataSet(56)).Returns(dataSet56);
            mockedManager.Setup(m => m.GetDataSet(57)).Returns(dataSet57);
            mockedManager.Setup(m => m.GetDataSet(58)).Returns(dataSet58);
            mockedManager.Setup(m => m.GetDataSet(59)).Returns(dataSet59);
            mockedManager.Setup(m => m.GetDataSet(60)).Returns(dataSet60);
            mockedManager.Setup(m => m.GetDataSet(61)).Returns(dataSet61);
            mockedManager.Setup(m => m.GetDataSet(62)).Returns(dataSet62);
            mockedManager.Setup(m => m.GetDataSet(63)).Returns(dataSet63);
            mockedManager.Setup(m => m.GetDataSet(64)).Returns(dataSet64);
            mockedManager.Setup(m => m.GetDataSet(65)).Returns(dataSet65);
            mockedManager.Setup(m => m.GetDataSet(66)).Returns(dataSet66);
            mockedManager.Setup(m => m.GetDataSet(67)).Returns(dataSet67);
            mockedManager.Setup(m => m.GetDataSet(68)).Returns(dataSet68);
            mockedManager.Setup(m => m.GetDataSet(69)).Returns(dataSet69);
            mockedManager.Setup(m => m.GetDataSet(70)).Returns(dataSet70);
            mockedManager.Setup(m => m.GetDataSet(71)).Returns(dataSet71);
            mockedManager.Setup(m => m.GetDataSet(72)).Returns(dataSet72);
            mockedManager.Setup(m => m.GetDataSet(73)).Returns(dataSet73);
            mockedManager.Setup(m => m.GetDataSet(74)).Returns(dataSet74);



            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 10;
            Extremum extremum = new Extremum(dataSet74.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum);
            double expectedResult = 0.00118;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForPeakByHigh_IfThereIsNoHigherClosePriceEarlier()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09218, 1.09186, 1.09194, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09193, 1.09256, 1.09165, 1.09177, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09176, 1.09182, 1.09142, 1.09151, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum);
            double expectedResult = 0.00098;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForPeakByHigh_IfThereIsHigherHighPriceEarlierAndLowPriceAtThisQuotationIsLowerThanProcessedHighPrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            DataSet dataSet89 = getDataSetWithQuotationAndPrice(89, 1.08893, 1.08899, 1.08863, 1.08892, 1133);
            DataSet dataSet90 = getDataSetWithQuotationAndPrice(90, 1.08896, 1.08933, 1.08893, 1.08926, 685);
            DataSet dataSet91 = getDataSetWithQuotationAndPrice(91, 1.08928, 1.08945, 1.08916, 1.08932, 774);
            DataSet dataSet92 = getDataSetWithQuotationAndPrice(92, 1.0893, 1.08939, 1.08923, 1.08932, 441);
            DataSet dataSet93 = getDataSetWithQuotationAndPrice(93, 1.08935, 1.08944, 1.08924, 1.08932, 764);
            DataSet dataSet94 = getDataSetWithQuotationAndPrice(94, 1.08932, 1.08942, 1.08908, 1.08913, 827);
            DataSet dataSet95 = getDataSetWithQuotationAndPrice(95, 1.08912, 1.08918, 1.08878, 1.0888, 805);
            DataSet dataSet96 = getDataSetWithQuotationAndPrice(96, 1.0888, 1.08966, 1.08859, 1.08904, 905);
            DataSet dataSet97 = getDataSetWithQuotationAndPrice(97, 1.08904, 1.08923, 1.08895, 1.08916, 767);
            DataSet dataSet98 = getDataSetWithQuotationAndPrice(98, 1.08915, 1.08928, 1.08902, 1.08921, 691);
            DataSet dataSet99 = getDataSetWithQuotationAndPrice(99, 1.08922, 1.08926, 1.08911, 1.08925, 675);
            DataSet dataSet100 = getDataSetWithQuotationAndPrice(100, 1.08924, 1.08959, 1.08916, 1.08956, 809);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);
            mockedManager.Setup(m => m.GetDataSet(89)).Returns(dataSet89);
            mockedManager.Setup(m => m.GetDataSet(90)).Returns(dataSet90);
            mockedManager.Setup(m => m.GetDataSet(91)).Returns(dataSet91);
            mockedManager.Setup(m => m.GetDataSet(92)).Returns(dataSet92);
            mockedManager.Setup(m => m.GetDataSet(93)).Returns(dataSet93);
            mockedManager.Setup(m => m.GetDataSet(94)).Returns(dataSet94);
            mockedManager.Setup(m => m.GetDataSet(95)).Returns(dataSet95);
            mockedManager.Setup(m => m.GetDataSet(96)).Returns(dataSet96);
            mockedManager.Setup(m => m.GetDataSet(97)).Returns(dataSet97);
            mockedManager.Setup(m => m.GetDataSet(98)).Returns(dataSet98);
            mockedManager.Setup(m => m.GetDataSet(99)).Returns(dataSet99);
            mockedManager.Setup(m => m.GetDataSet(100)).Returns(dataSet100);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum100 = new Extremum(dataSet100.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum100);
            double expectedResult = 0.00064;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForTroughByClose_IfThereIsNoLowerClosePriceEarlier()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            
            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet5.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum);
            double expectedResult = 0.00118;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForTroughByClose_IfThereIsLowerClosePriceEarlierAndHighPriceAtThisQuotationIsHigherThanProcessedClosePrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            DataSet dataSet39 = getDataSetWithQuotationAndPrice(39, 1.09153, 1.09182, 1.09149, 1.09178, 690);
            DataSet dataSet40 = getDataSetWithQuotationAndPrice(40, 1.09175, 1.09201, 1.09175, 1.09192, 546);
            DataSet dataSet41 = getDataSetWithQuotationAndPrice(41, 1.09194, 1.092, 1.09178, 1.09179, 604);
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);
            mockedManager.Setup(m => m.GetDataSet(39)).Returns(dataSet39);
            mockedManager.Setup(m => m.GetDataSet(40)).Returns(dataSet40);
            mockedManager.Setup(m => m.GetDataSet(41)).Returns(dataSet41);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum43 = new Extremum(dataSet43.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum43);
            double expectedResult = 0.00032;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForTroughByClose_WhenLookingForLastHigherBeforeIgnoresQuotationsWithHigherHighPrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
    

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum16 = new Extremum(dataSet16.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum16);
            double expectedResult = 0.00158;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForTroughByLow_IfThereIsNoLowerLowPriceEarlier()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum5 = new Extremum(dataSet5.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum5);
            double expectedResult = 0.00127;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateEarlierAmplitude_ReturnsProperValueForTroughByLow_IfThereIsLowerLowPriceEarlierAndHighPriceAtThisQuotationIsHigherThanProcessedLowPrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            DataSet dataSet39 = getDataSetWithQuotationAndPrice(39, 1.09153, 1.09182, 1.09144, 1.09178, 690);
            DataSet dataSet40 = getDataSetWithQuotationAndPrice(40, 1.09175, 1.09201, 1.09175, 1.09192, 546);
            DataSet dataSet41 = getDataSetWithQuotationAndPrice(41, 1.09194, 1.092, 1.09178, 1.09179, 604);
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            DataSet dataSet44 = getDataSetWithQuotationAndPrice(44, 1.09167, 1.09186, 1.0915, 1.09179, 1327);
            DataSet dataSet45 = getDataSetWithQuotationAndPrice(45, 1.0918, 1.09181, 1.09145, 1.0917, 1421);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);
            mockedManager.Setup(m => m.GetDataSet(39)).Returns(dataSet39);
            mockedManager.Setup(m => m.GetDataSet(40)).Returns(dataSet40);
            mockedManager.Setup(m => m.GetDataSet(41)).Returns(dataSet41);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);
            mockedManager.Setup(m => m.GetDataSet(44)).Returns(dataSet44);
            mockedManager.Setup(m => m.GetDataSet(45)).Returns(dataSet45);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum45 = new Extremum(dataSet45.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierAmplitude(extremum45);
            double expectedResult = 0.00056;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }



        #endregion CALCULATE_EARLIER_AMPLITUDE


        #region CALCULATE_EARLIER_COUNTER

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForPeakByClose_IfThereArePreviousHigherValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 4;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForPeakByClose_IfThereIsNoPreviousHigherValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09277, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 7;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForPeakByHigh_IfThereArePreviousHigherValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09189, 1.09143, 1.09149, 657);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 5;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForPeakByHigh_IfThereIsNoPreviousHigherValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 8;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForTroughByClose_IfThereArePreviousLowerValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet38.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 6;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForTroughByClose_IfThereIsNoPreviousLowerValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet5.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 4;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForTroughByLow_IfThereArePreviousLowerValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet23.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 3;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValueForTroughByLow_IfThereIsNoPreviousLowerValues()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet5.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 4;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierCounter_ReturnsProperValue_IfPreviousMinorIsEarlierThanMaxSeriesCount()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 10;
            Extremum extremum = new Extremum(dataSet17.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierCounter(extremum);
            int expectedResult = 10;
            Assert.AreEqual(expectedResult, result);

        }

        #endregion CALCULATE_EARLIER_COUNTER


        #region CALCULATE_EARLIER_CHANGE

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForPeakByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 2);
            double expectedResult = 0.00042151;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForPeakByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 5);
            double expectedResult = -0.00015569;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForPeakByClose_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 10);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForPeakByHigh_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 3);
            double expectedResult = 0.00016494;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForPeakByHigh_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 2);
            double expectedResult = -0.00014657;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForPeakByHigh_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 10);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForTroughByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 3);
            double expectedResult = -0.00016494;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForTroughByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 2);
            double expectedResult = 0.00014657;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForTroughByClose_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 10);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }


        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForTroughByLow_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet17.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 3);
            double expectedResult = -0.00011001;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForTroughByLow_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet17.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 5);
            double expectedResult = 0.00010082;
            double difference = (Math.Abs(expectedResult - result));
            Assert.IsTrue(difference < MAX_DOUBLE_COMPARISON_DIFFERENCE);

        }

        [TestMethod]
        public void CalculateEarlierChange_ReturnsProperValueForTroughByLow_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 10);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }

        #endregion CALCULATE_EARLIER_CHANGE



        #region CALCULATE_LATER_AMPLITUDE

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForPeakByClose_IfThereIsNoHigherClosePriceLater()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet3.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum);
            double expectedResult = 0.00128;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForPeakByClose_IfThereIsHigherClosePriceLaterAndLowPriceAtThisQuotationIsLowerThanProcessedClosePrice()
        {
            
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09168, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09161, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09161, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09162, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet20.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum);
            double expectedResult = 0.00071;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForPeakByClose_WhenLookingForFirstHigherAfterIgnoresQuotationsWithHigherHighPrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum20 = new Extremum(dataSet20.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum20);
            double expectedResult = 0.00071;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForPeakByClose_IfExtremumHasMoreLaterMinorsThanMaxSerie()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09038, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 10;
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum);
            double expectedResult = 0.00136;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForPeakByHigh_IfThereIsNoHigherClosePriceLater()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum);
            double expectedResult = 0.00178;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForPeakByHigh_IfThereIsHigherHighPriceLaterAndLowPriceAtThisQuotationIsLowerThanProcessedHighPrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet9.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum);
            double expectedResult = 0.00178;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForTroughByClose_IfThereIsNoLowerClosePriceLater()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet5.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum);
            double expectedResult = 0.00119;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForTroughByClose_WhenLookingForLastHigherBeforeIgnoresQuotationsWithLowerLowPriceButHigherClosePrice()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09075, 1.0912, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09318, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum5 = new Extremum(dataSet5.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum5);
            double expectedResult = 0.00218;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void CalculateLaterAmplitude_ReturnsProperValueForTroughByLow_IfThereIsNoLowerLowPriceLater()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet5.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterAmplitude(extremum);
            double expectedResult = 0.00128;
            var areEqual = Math.Abs(expectedResult - result) < MAX_DOUBLE_COMPARISON_DIFFERENCE;
            Assert.IsTrue(areEqual);

        }

        #endregion CALCULATE_LATER_AMPLITUDE

        
        #region CALCULATE_LATER_COUNTER

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByClose_IfThereAreHigherValuesLater()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet25.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 8;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByClose_IfThereIsHigherValueLaterButOutOfMaxSerieRange()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 5;
            Extremum extremum = new Extremum(dataSet25.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 5;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByClose_IfThereIsNoHigherValue()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            DataSet dataSet39 = getDataSetWithQuotationAndPrice(39, 1.09153, 1.09182, 1.09144, 1.09178, 690);
            DataSet dataSet40 = getDataSetWithQuotationAndPrice(40, 1.09175, 1.09201, 1.09175, 1.09192, 546);
            DataSet dataSet41 = getDataSetWithQuotationAndPrice(41, 1.09194, 1.092, 1.09178, 1.09179, 604);
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            DataSet dataSet44 = getDataSetWithQuotationAndPrice(44, 1.09167, 1.09186, 1.0915, 1.09179, 1327);
            DataSet dataSet45 = getDataSetWithQuotationAndPrice(45, 1.0918, 1.09181, 1.09145, 1.0917, 1421);
            DataSet dataSet46 = getDataSetWithQuotationAndPrice(46, 1.09169, 1.09189, 1.09162, 1.09184, 1097);
            DataSet dataSet47 = getDataSetWithQuotationAndPrice(47, 1.09183, 1.09216, 1.09181, 1.0921, 816);
            DataSet dataSet48 = getDataSetWithQuotationAndPrice(48, 1.0921, 1.09215, 1.09192, 1.09202, 684);
            DataSet dataSet49 = getDataSetWithQuotationAndPrice(49, 1.09201, 1.09226, 1.09201, 1.09214, 691);
            DataSet dataSet50 = getDataSetWithQuotationAndPrice(50, 1.09215, 1.09232, 1.09183, 1.09185, 996);
            DataSet dataSet51 = getDataSetWithQuotationAndPrice(51, 1.09185, 1.09212, 1.0918, 1.092, 678);
            DataSet dataSet52 = getDataSetWithQuotationAndPrice(52, 1.09201, 1.09222, 1.09158, 1.0917, 855);
            DataSet dataSet53 = getDataSetWithQuotationAndPrice(53, 1.09174, 1.09178, 1.09143, 1.09163, 768);
            DataSet dataSet54 = getDataSetWithQuotationAndPrice(54, 1.09162, 1.09178, 1.09148, 1.09153, 981);
            DataSet dataSet55 = getDataSetWithQuotationAndPrice(55, 1.09152, 1.09152, 1.09094, 1.09114, 1151);
            DataSet dataSet56 = getDataSetWithQuotationAndPrice(56, 1.09113, 1.09121, 1.09069, 1.09086, 1219);
            DataSet dataSet57 = getDataSetWithQuotationAndPrice(57, 1.09092, 1.09092, 1.09031, 1.09032, 1155);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);
            mockedManager.Setup(m => m.GetDataSet(39)).Returns(dataSet39);
            mockedManager.Setup(m => m.GetDataSet(40)).Returns(dataSet40);
            mockedManager.Setup(m => m.GetDataSet(41)).Returns(dataSet41);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);
            mockedManager.Setup(m => m.GetDataSet(44)).Returns(dataSet44);
            mockedManager.Setup(m => m.GetDataSet(45)).Returns(dataSet45);
            mockedManager.Setup(m => m.GetDataSet(46)).Returns(dataSet46);
            mockedManager.Setup(m => m.GetDataSet(47)).Returns(dataSet47);
            mockedManager.Setup(m => m.GetDataSet(48)).Returns(dataSet48);
            mockedManager.Setup(m => m.GetDataSet(49)).Returns(dataSet49);
            mockedManager.Setup(m => m.GetDataSet(50)).Returns(dataSet50);
            mockedManager.Setup(m => m.GetDataSet(51)).Returns(dataSet51);
            mockedManager.Setup(m => m.GetDataSet(52)).Returns(dataSet52);
            mockedManager.Setup(m => m.GetDataSet(53)).Returns(dataSet53);
            mockedManager.Setup(m => m.GetDataSet(54)).Returns(dataSet54);
            mockedManager.Setup(m => m.GetDataSet(55)).Returns(dataSet55);
            mockedManager.Setup(m => m.GetDataSet(56)).Returns(dataSet56);
            mockedManager.Setup(m => m.GetDataSet(57)).Returns(dataSet57);

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 20;
            Extremum extremum = new Extremum(dataSet35.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 20;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByClose_IfThereIsHigherValueOnTheLastQuotationAllowedByMaxSerie()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 8;
            Extremum extremum = new Extremum(dataSet25.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 8;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByClose_IfThereIsHigherValueOnTheFirstPositionAfterMaxSerie()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 7;
            Extremum extremum = new Extremum(dataSet25.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 7;
            Assert.AreEqual(expectedResult, result);

        }





        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByHigh_IfThereAreHigherValuesLater()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet27.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 7;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByHigh_IfThereIsHigherValueLaterButOutOfMaxSerieRange()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 5;
            Extremum extremum = new Extremum(dataSet27.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 5;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByHigh_IfThereIsNoHigherValue()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0918, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            DataSet dataSet39 = getDataSetWithQuotationAndPrice(39, 1.09153, 1.09182, 1.09144, 1.09178, 690);
            DataSet dataSet40 = getDataSetWithQuotationAndPrice(40, 1.09175, 1.09201, 1.09175, 1.09192, 546);
            DataSet dataSet41 = getDataSetWithQuotationAndPrice(41, 1.09194, 1.092, 1.09178, 1.09179, 604);
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            DataSet dataSet44 = getDataSetWithQuotationAndPrice(44, 1.09167, 1.09186, 1.0915, 1.09179, 1327);
            DataSet dataSet45 = getDataSetWithQuotationAndPrice(45, 1.0918, 1.09181, 1.09145, 1.0917, 1421);
            DataSet dataSet46 = getDataSetWithQuotationAndPrice(46, 1.09169, 1.09189, 1.09162, 1.09184, 1097);
            DataSet dataSet47 = getDataSetWithQuotationAndPrice(47, 1.09183, 1.09216, 1.09181, 1.0921, 816);
            DataSet dataSet48 = getDataSetWithQuotationAndPrice(48, 1.0921, 1.09215, 1.09192, 1.09202, 684);
            DataSet dataSet49 = getDataSetWithQuotationAndPrice(49, 1.09201, 1.09226, 1.09201, 1.09214, 691);
            DataSet dataSet50 = getDataSetWithQuotationAndPrice(50, 1.09215, 1.09232, 1.09183, 1.09185, 996);
            DataSet dataSet51 = getDataSetWithQuotationAndPrice(51, 1.09185, 1.09212, 1.0918, 1.092, 678);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);
            mockedManager.Setup(m => m.GetDataSet(39)).Returns(dataSet39);
            mockedManager.Setup(m => m.GetDataSet(40)).Returns(dataSet40);
            mockedManager.Setup(m => m.GetDataSet(41)).Returns(dataSet41);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);
            mockedManager.Setup(m => m.GetDataSet(44)).Returns(dataSet44);
            mockedManager.Setup(m => m.GetDataSet(45)).Returns(dataSet45);
            mockedManager.Setup(m => m.GetDataSet(46)).Returns(dataSet46);
            mockedManager.Setup(m => m.GetDataSet(47)).Returns(dataSet47);
            mockedManager.Setup(m => m.GetDataSet(48)).Returns(dataSet48);
            mockedManager.Setup(m => m.GetDataSet(49)).Returns(dataSet49);
            mockedManager.Setup(m => m.GetDataSet(50)).Returns(dataSet50);
            mockedManager.Setup(m => m.GetDataSet(51)).Returns(dataSet51);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 10;
            Extremum extremum = new Extremum(dataSet35.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 10;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByHigh_IfThereIsHigherValueOnTheLastQuotationAllowedByMaxSerie()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 7;
            Extremum extremum = new Extremum(dataSet27.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 7;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForPeakByHigh_IfThereIsHigherValueOnTheFirstPositionAfterMaxSerie()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            DataSet dataSet31 = getDataSetWithQuotationAndPrice(31, 1.09104, 1.09131, 1.09064, 1.09101, 761);
            DataSet dataSet32 = getDataSetWithQuotationAndPrice(32, 1.09091, 1.09181, 1.09091, 1.09166, 1697);
            DataSet dataSet33 = getDataSetWithQuotationAndPrice(33, 1.09165, 1.09175, 1.0916, 1.09165, 754);
            DataSet dataSet34 = getDataSetWithQuotationAndPrice(34, 1.09169, 1.09208, 1.09156, 1.09198, 703);
            DataSet dataSet35 = getDataSetWithQuotationAndPrice(35, 1.09202, 1.09261, 1.09198, 1.0923, 964);
            DataSet dataSet36 = getDataSetWithQuotationAndPrice(36, 1.09232, 1.09232, 1.09175, 1.09189, 559);
            DataSet dataSet37 = getDataSetWithQuotationAndPrice(37, 1.0919, 1.09211, 1.09177, 1.09185, 673);
            DataSet dataSet38 = getDataSetWithQuotationAndPrice(38, 1.09182, 1.09189, 1.0915, 1.09155, 640);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);
            mockedManager.Setup(m => m.GetDataSet(31)).Returns(dataSet31);
            mockedManager.Setup(m => m.GetDataSet(32)).Returns(dataSet32);
            mockedManager.Setup(m => m.GetDataSet(33)).Returns(dataSet33);
            mockedManager.Setup(m => m.GetDataSet(34)).Returns(dataSet34);
            mockedManager.Setup(m => m.GetDataSet(35)).Returns(dataSet35);
            mockedManager.Setup(m => m.GetDataSet(36)).Returns(dataSet36);
            mockedManager.Setup(m => m.GetDataSet(37)).Returns(dataSet37);
            mockedManager.Setup(m => m.GetDataSet(38)).Returns(dataSet38);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 6;
            Extremum extremum = new Extremum(dataSet27.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 6;
            Assert.AreEqual(expectedResult, result);

        }





        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByClose_IfThereAreLowerValuesLater()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            DataSet dataSet44 = getDataSetWithQuotationAndPrice(44, 1.09167, 1.09186, 1.0915, 1.09179, 1327);
            DataSet dataSet45 = getDataSetWithQuotationAndPrice(45, 1.0918, 1.09181, 1.09145, 1.0917, 1421);
            DataSet dataSet46 = getDataSetWithQuotationAndPrice(46, 1.09169, 1.09189, 1.09162, 1.09184, 1097);
            DataSet dataSet47 = getDataSetWithQuotationAndPrice(47, 1.09183, 1.09216, 1.09181, 1.0921, 816);
            DataSet dataSet48 = getDataSetWithQuotationAndPrice(48, 1.0921, 1.09215, 1.09192, 1.09202, 684);
            DataSet dataSet49 = getDataSetWithQuotationAndPrice(49, 1.09201, 1.09226, 1.09201, 1.09214, 691);
            DataSet dataSet50 = getDataSetWithQuotationAndPrice(50, 1.09215, 1.09232, 1.09183, 1.09185, 996);
            DataSet dataSet51 = getDataSetWithQuotationAndPrice(51, 1.09185, 1.09212, 1.0918, 1.092, 678);
            DataSet dataSet52 = getDataSetWithQuotationAndPrice(52, 1.09201, 1.09222, 1.09158, 1.0917, 855);
            DataSet dataSet53 = getDataSetWithQuotationAndPrice(53, 1.09174, 1.09178, 1.09143, 1.09163, 768);
            DataSet dataSet54 = getDataSetWithQuotationAndPrice(54, 1.09162, 1.09178, 1.09148, 1.09153, 981);
            DataSet dataSet55 = getDataSetWithQuotationAndPrice(55, 1.09152, 1.09152, 1.09094, 1.09114, 1151);
            DataSet dataSet56 = getDataSetWithQuotationAndPrice(56, 1.09113, 1.09121, 1.09069, 1.09086, 1219);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);
            mockedManager.Setup(m => m.GetDataSet(44)).Returns(dataSet44);
            mockedManager.Setup(m => m.GetDataSet(45)).Returns(dataSet45);
            mockedManager.Setup(m => m.GetDataSet(46)).Returns(dataSet46);
            mockedManager.Setup(m => m.GetDataSet(47)).Returns(dataSet47);
            mockedManager.Setup(m => m.GetDataSet(48)).Returns(dataSet48);
            mockedManager.Setup(m => m.GetDataSet(49)).Returns(dataSet49);
            mockedManager.Setup(m => m.GetDataSet(50)).Returns(dataSet50);
            mockedManager.Setup(m => m.GetDataSet(51)).Returns(dataSet51);
            mockedManager.Setup(m => m.GetDataSet(52)).Returns(dataSet52);
            mockedManager.Setup(m => m.GetDataSet(53)).Returns(dataSet53);
            mockedManager.Setup(m => m.GetDataSet(54)).Returns(dataSet54);
            mockedManager.Setup(m => m.GetDataSet(55)).Returns(dataSet55);
            mockedManager.Setup(m => m.GetDataSet(56)).Returns(dataSet56);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet43.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 9;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByClose_IfThereIsLowerValueLaterButOutOfMaxSerieRange()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            DataSet dataSet44 = getDataSetWithQuotationAndPrice(44, 1.09167, 1.09186, 1.0915, 1.09179, 1327);
            DataSet dataSet45 = getDataSetWithQuotationAndPrice(45, 1.0918, 1.09181, 1.09145, 1.0917, 1421);
            DataSet dataSet46 = getDataSetWithQuotationAndPrice(46, 1.09169, 1.09189, 1.09162, 1.09184, 1097);
            DataSet dataSet47 = getDataSetWithQuotationAndPrice(47, 1.09183, 1.09216, 1.09181, 1.0921, 816);
            DataSet dataSet48 = getDataSetWithQuotationAndPrice(48, 1.0921, 1.09215, 1.09192, 1.09202, 684);
            DataSet dataSet49 = getDataSetWithQuotationAndPrice(49, 1.09201, 1.09226, 1.09201, 1.09214, 691);
            DataSet dataSet50 = getDataSetWithQuotationAndPrice(50, 1.09215, 1.09232, 1.09183, 1.09185, 996);
            DataSet dataSet51 = getDataSetWithQuotationAndPrice(51, 1.09185, 1.09212, 1.0918, 1.092, 678);
            DataSet dataSet52 = getDataSetWithQuotationAndPrice(52, 1.09201, 1.09222, 1.09158, 1.0917, 855);
            DataSet dataSet53 = getDataSetWithQuotationAndPrice(53, 1.09174, 1.09178, 1.09143, 1.09163, 768);
            DataSet dataSet54 = getDataSetWithQuotationAndPrice(54, 1.09162, 1.09178, 1.09148, 1.09153, 981);
            DataSet dataSet55 = getDataSetWithQuotationAndPrice(55, 1.09152, 1.09152, 1.09094, 1.09114, 1151);
            DataSet dataSet56 = getDataSetWithQuotationAndPrice(56, 1.09113, 1.09121, 1.09069, 1.09086, 1219);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);
            mockedManager.Setup(m => m.GetDataSet(44)).Returns(dataSet44);
            mockedManager.Setup(m => m.GetDataSet(45)).Returns(dataSet45);
            mockedManager.Setup(m => m.GetDataSet(46)).Returns(dataSet46);
            mockedManager.Setup(m => m.GetDataSet(47)).Returns(dataSet47);
            mockedManager.Setup(m => m.GetDataSet(48)).Returns(dataSet48);
            mockedManager.Setup(m => m.GetDataSet(49)).Returns(dataSet49);
            mockedManager.Setup(m => m.GetDataSet(50)).Returns(dataSet50);
            mockedManager.Setup(m => m.GetDataSet(51)).Returns(dataSet51);
            mockedManager.Setup(m => m.GetDataSet(52)).Returns(dataSet52);
            mockedManager.Setup(m => m.GetDataSet(53)).Returns(dataSet53);
            mockedManager.Setup(m => m.GetDataSet(54)).Returns(dataSet54);
            mockedManager.Setup(m => m.GetDataSet(55)).Returns(dataSet55);
            mockedManager.Setup(m => m.GetDataSet(56)).Returns(dataSet56);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 5;
            Extremum extremum = new Extremum(dataSet43.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 5;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByClose_IfThereIsNoLowerValue()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 10;
            Extremum extremum = new Extremum(dataSet16.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 10;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByClose_IfThereIsLowerValueOnTheLastQuotationAllowedByMaxSerie()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            DataSet dataSet44 = getDataSetWithQuotationAndPrice(44, 1.09167, 1.09186, 1.0915, 1.09179, 1327);
            DataSet dataSet45 = getDataSetWithQuotationAndPrice(45, 1.0918, 1.09181, 1.09145, 1.0917, 1421);
            DataSet dataSet46 = getDataSetWithQuotationAndPrice(46, 1.09169, 1.09189, 1.09162, 1.09184, 1097);
            DataSet dataSet47 = getDataSetWithQuotationAndPrice(47, 1.09183, 1.09216, 1.09181, 1.0921, 816);
            DataSet dataSet48 = getDataSetWithQuotationAndPrice(48, 1.0921, 1.09215, 1.09192, 1.09202, 684);
            DataSet dataSet49 = getDataSetWithQuotationAndPrice(49, 1.09201, 1.09226, 1.09201, 1.09214, 691);
            DataSet dataSet50 = getDataSetWithQuotationAndPrice(50, 1.09215, 1.09232, 1.09183, 1.09185, 996);
            DataSet dataSet51 = getDataSetWithQuotationAndPrice(51, 1.09185, 1.09212, 1.0918, 1.092, 678);
            DataSet dataSet52 = getDataSetWithQuotationAndPrice(52, 1.09201, 1.09222, 1.09158, 1.0917, 855);
            DataSet dataSet53 = getDataSetWithQuotationAndPrice(53, 1.09174, 1.09178, 1.09143, 1.09163, 768);
            DataSet dataSet54 = getDataSetWithQuotationAndPrice(54, 1.09162, 1.09178, 1.09148, 1.09153, 981);
            DataSet dataSet55 = getDataSetWithQuotationAndPrice(55, 1.09152, 1.09152, 1.09094, 1.09114, 1151);
            DataSet dataSet56 = getDataSetWithQuotationAndPrice(56, 1.09113, 1.09121, 1.09069, 1.09086, 1219);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);
            mockedManager.Setup(m => m.GetDataSet(44)).Returns(dataSet44);
            mockedManager.Setup(m => m.GetDataSet(45)).Returns(dataSet45);
            mockedManager.Setup(m => m.GetDataSet(46)).Returns(dataSet46);
            mockedManager.Setup(m => m.GetDataSet(47)).Returns(dataSet47);
            mockedManager.Setup(m => m.GetDataSet(48)).Returns(dataSet48);
            mockedManager.Setup(m => m.GetDataSet(49)).Returns(dataSet49);
            mockedManager.Setup(m => m.GetDataSet(50)).Returns(dataSet50);
            mockedManager.Setup(m => m.GetDataSet(51)).Returns(dataSet51);
            mockedManager.Setup(m => m.GetDataSet(52)).Returns(dataSet52);
            mockedManager.Setup(m => m.GetDataSet(53)).Returns(dataSet53);
            mockedManager.Setup(m => m.GetDataSet(54)).Returns(dataSet54);
            mockedManager.Setup(m => m.GetDataSet(55)).Returns(dataSet55);
            mockedManager.Setup(m => m.GetDataSet(56)).Returns(dataSet56);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 9;
            Extremum extremum = new Extremum(dataSet43.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 9;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByClose_IfThereIsLowerValueOnTheFirstPositionAfterMaxSerie()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet42 = getDataSetWithQuotationAndPrice(42, 1.0918, 1.09192, 1.09168, 1.09189, 485);
            DataSet dataSet43 = getDataSetWithQuotationAndPrice(43, 1.09188, 1.09189, 1.09158, 1.09169, 371);
            DataSet dataSet44 = getDataSetWithQuotationAndPrice(44, 1.09167, 1.09186, 1.0915, 1.09179, 1327);
            DataSet dataSet45 = getDataSetWithQuotationAndPrice(45, 1.0918, 1.09181, 1.09145, 1.0917, 1421);
            DataSet dataSet46 = getDataSetWithQuotationAndPrice(46, 1.09169, 1.09189, 1.09162, 1.09184, 1097);
            DataSet dataSet47 = getDataSetWithQuotationAndPrice(47, 1.09183, 1.09216, 1.09181, 1.0921, 816);
            DataSet dataSet48 = getDataSetWithQuotationAndPrice(48, 1.0921, 1.09215, 1.09192, 1.09202, 684);
            DataSet dataSet49 = getDataSetWithQuotationAndPrice(49, 1.09201, 1.09226, 1.09201, 1.09214, 691);
            DataSet dataSet50 = getDataSetWithQuotationAndPrice(50, 1.09215, 1.09232, 1.09183, 1.09185, 996);
            DataSet dataSet51 = getDataSetWithQuotationAndPrice(51, 1.09185, 1.09212, 1.0918, 1.092, 678);
            DataSet dataSet52 = getDataSetWithQuotationAndPrice(52, 1.09201, 1.09222, 1.09158, 1.0917, 855);
            DataSet dataSet53 = getDataSetWithQuotationAndPrice(53, 1.09174, 1.09178, 1.09143, 1.09163, 768);
            DataSet dataSet54 = getDataSetWithQuotationAndPrice(54, 1.09162, 1.09178, 1.09148, 1.09153, 981);
            DataSet dataSet55 = getDataSetWithQuotationAndPrice(55, 1.09152, 1.09152, 1.09094, 1.09114, 1151);
            DataSet dataSet56 = getDataSetWithQuotationAndPrice(56, 1.09113, 1.09121, 1.09069, 1.09086, 1219);
            mockedManager.Setup(m => m.GetDataSet(42)).Returns(dataSet42);
            mockedManager.Setup(m => m.GetDataSet(43)).Returns(dataSet43);
            mockedManager.Setup(m => m.GetDataSet(44)).Returns(dataSet44);
            mockedManager.Setup(m => m.GetDataSet(45)).Returns(dataSet45);
            mockedManager.Setup(m => m.GetDataSet(46)).Returns(dataSet46);
            mockedManager.Setup(m => m.GetDataSet(47)).Returns(dataSet47);
            mockedManager.Setup(m => m.GetDataSet(48)).Returns(dataSet48);
            mockedManager.Setup(m => m.GetDataSet(49)).Returns(dataSet49);
            mockedManager.Setup(m => m.GetDataSet(50)).Returns(dataSet50);
            mockedManager.Setup(m => m.GetDataSet(51)).Returns(dataSet51);
            mockedManager.Setup(m => m.GetDataSet(52)).Returns(dataSet52);
            mockedManager.Setup(m => m.GetDataSet(53)).Returns(dataSet53);
            mockedManager.Setup(m => m.GetDataSet(54)).Returns(dataSet54);
            mockedManager.Setup(m => m.GetDataSet(55)).Returns(dataSet55);
            mockedManager.Setup(m => m.GetDataSet(56)).Returns(dataSet56);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 8;
            Extremum extremum = new Extremum(dataSet43.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 8;
            Assert.AreEqual(expectedResult, result);

        }





        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByLow_IfThereAreLowerValuesLater()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            DataSet dataSet89 = getDataSetWithQuotationAndPrice(89, 1.08893, 1.08899, 1.08863, 1.08892, 1133);
            DataSet dataSet90 = getDataSetWithQuotationAndPrice(90, 1.08896, 1.08933, 1.08893, 1.08926, 685);
            DataSet dataSet91 = getDataSetWithQuotationAndPrice(91, 1.08928, 1.08945, 1.08916, 1.08932, 774);
            DataSet dataSet92 = getDataSetWithQuotationAndPrice(92, 1.0893, 1.08939, 1.08923, 1.08932, 441);
            DataSet dataSet93 = getDataSetWithQuotationAndPrice(93, 1.08935, 1.08944, 1.08924, 1.08932, 764);
            DataSet dataSet94 = getDataSetWithQuotationAndPrice(94, 1.08932, 1.08942, 1.08908, 1.08913, 827);
            DataSet dataSet95 = getDataSetWithQuotationAndPrice(95, 1.08912, 1.08918, 1.08878, 1.0888, 805);
            DataSet dataSet96 = getDataSetWithQuotationAndPrice(96, 1.0888, 1.08966, 1.08859, 1.08904, 905);
            DataSet dataSet97 = getDataSetWithQuotationAndPrice(97, 1.08904, 1.08923, 1.08895, 1.08916, 767);
            DataSet dataSet98 = getDataSetWithQuotationAndPrice(98, 1.08915, 1.08928, 1.08902, 1.08921, 691);
            DataSet dataSet99 = getDataSetWithQuotationAndPrice(99, 1.08922, 1.08926, 1.08911, 1.08925, 675);
            DataSet dataSet100 = getDataSetWithQuotationAndPrice(100, 1.08924, 1.08959, 1.08916, 1.08956, 809);
            DataSet dataSet101 = getDataSetWithQuotationAndPrice(101, 1.08955, 1.08955, 1.08901, 1.0895, 1153);
            DataSet dataSet102 = getDataSetWithQuotationAndPrice(102, 1.08947, 1.08953, 1.08907, 1.0891, 807);
            DataSet dataSet103 = getDataSetWithQuotationAndPrice(103, 1.08911, 1.08955, 1.08906, 1.08955, 822);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);
            mockedManager.Setup(m => m.GetDataSet(89)).Returns(dataSet89);
            mockedManager.Setup(m => m.GetDataSet(90)).Returns(dataSet90);
            mockedManager.Setup(m => m.GetDataSet(91)).Returns(dataSet91);
            mockedManager.Setup(m => m.GetDataSet(92)).Returns(dataSet92);
            mockedManager.Setup(m => m.GetDataSet(93)).Returns(dataSet93);
            mockedManager.Setup(m => m.GetDataSet(94)).Returns(dataSet94);
            mockedManager.Setup(m => m.GetDataSet(95)).Returns(dataSet95);
            mockedManager.Setup(m => m.GetDataSet(96)).Returns(dataSet96);
            mockedManager.Setup(m => m.GetDataSet(97)).Returns(dataSet97);
            mockedManager.Setup(m => m.GetDataSet(98)).Returns(dataSet98);
            mockedManager.Setup(m => m.GetDataSet(99)).Returns(dataSet99);
            mockedManager.Setup(m => m.GetDataSet(100)).Returns(dataSet100);
            mockedManager.Setup(m => m.GetDataSet(101)).Returns(dataSet101);
            mockedManager.Setup(m => m.GetDataSet(102)).Returns(dataSet102);
            mockedManager.Setup(m => m.GetDataSet(103)).Returns(dataSet103);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet89.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 6;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByLow_IfThereIsLowerValueLaterButOutOfMaxSerieRange()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            DataSet dataSet89 = getDataSetWithQuotationAndPrice(89, 1.08893, 1.08899, 1.08863, 1.08892, 1133);
            DataSet dataSet90 = getDataSetWithQuotationAndPrice(90, 1.08896, 1.08933, 1.08893, 1.08926, 685);
            DataSet dataSet91 = getDataSetWithQuotationAndPrice(91, 1.08928, 1.08945, 1.08916, 1.08932, 774);
            DataSet dataSet92 = getDataSetWithQuotationAndPrice(92, 1.0893, 1.08939, 1.08923, 1.08932, 441);
            DataSet dataSet93 = getDataSetWithQuotationAndPrice(93, 1.08935, 1.08944, 1.08924, 1.08932, 764);
            DataSet dataSet94 = getDataSetWithQuotationAndPrice(94, 1.08932, 1.08942, 1.08908, 1.08913, 827);
            DataSet dataSet95 = getDataSetWithQuotationAndPrice(95, 1.08912, 1.08918, 1.08878, 1.0888, 805);
            DataSet dataSet96 = getDataSetWithQuotationAndPrice(96, 1.0888, 1.08966, 1.08859, 1.08904, 905);
            DataSet dataSet97 = getDataSetWithQuotationAndPrice(97, 1.08904, 1.08923, 1.08895, 1.08916, 767);
            DataSet dataSet98 = getDataSetWithQuotationAndPrice(98, 1.08915, 1.08928, 1.08902, 1.08921, 691);
            DataSet dataSet99 = getDataSetWithQuotationAndPrice(99, 1.08922, 1.08926, 1.08911, 1.08925, 675);
            DataSet dataSet100 = getDataSetWithQuotationAndPrice(100, 1.08924, 1.08959, 1.08916, 1.08956, 809);
            DataSet dataSet101 = getDataSetWithQuotationAndPrice(101, 1.08955, 1.08955, 1.08901, 1.0895, 1153);
            DataSet dataSet102 = getDataSetWithQuotationAndPrice(102, 1.08947, 1.08953, 1.08907, 1.0891, 807);
            DataSet dataSet103 = getDataSetWithQuotationAndPrice(103, 1.08911, 1.08955, 1.08906, 1.08955, 822);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);
            mockedManager.Setup(m => m.GetDataSet(89)).Returns(dataSet89);
            mockedManager.Setup(m => m.GetDataSet(90)).Returns(dataSet90);
            mockedManager.Setup(m => m.GetDataSet(91)).Returns(dataSet91);
            mockedManager.Setup(m => m.GetDataSet(92)).Returns(dataSet92);
            mockedManager.Setup(m => m.GetDataSet(93)).Returns(dataSet93);
            mockedManager.Setup(m => m.GetDataSet(94)).Returns(dataSet94);
            mockedManager.Setup(m => m.GetDataSet(95)).Returns(dataSet95);
            mockedManager.Setup(m => m.GetDataSet(96)).Returns(dataSet96);
            mockedManager.Setup(m => m.GetDataSet(97)).Returns(dataSet97);
            mockedManager.Setup(m => m.GetDataSet(98)).Returns(dataSet98);
            mockedManager.Setup(m => m.GetDataSet(99)).Returns(dataSet99);
            mockedManager.Setup(m => m.GetDataSet(100)).Returns(dataSet100);
            mockedManager.Setup(m => m.GetDataSet(101)).Returns(dataSet101);
            mockedManager.Setup(m => m.GetDataSet(102)).Returns(dataSet102);
            mockedManager.Setup(m => m.GetDataSet(103)).Returns(dataSet103);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 4;
            Extremum extremum = new Extremum(dataSet89.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 4;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByLow_IfThereIsNoLowerValue()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            DataSet dataSet15 = getDataSetWithQuotationAndPrice(15, 1.09086, 1.0909, 1.09076, 1.09082, 303);
            DataSet dataSet16 = getDataSetWithQuotationAndPrice(16, 1.09081, 1.09089, 1.09059, 1.0906, 450);
            DataSet dataSet17 = getDataSetWithQuotationAndPrice(17, 1.09061, 1.09099, 1.09041, 1.09097, 660);
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            DataSet dataSet28 = getDataSetWithQuotationAndPrice(28, 1.09182, 1.09182, 1.09057, 1.09103, 1090);
            DataSet dataSet29 = getDataSetWithQuotationAndPrice(29, 1.09084, 1.09124, 1.09055, 1.09107, 1845);
            DataSet dataSet30 = getDataSetWithQuotationAndPrice(30, 1.09101, 1.09147, 1.0909, 1.09117, 1318);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);
            mockedManager.Setup(m => m.GetDataSet(15)).Returns(dataSet15);
            mockedManager.Setup(m => m.GetDataSet(16)).Returns(dataSet16);
            mockedManager.Setup(m => m.GetDataSet(17)).Returns(dataSet17);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);
            mockedManager.Setup(m => m.GetDataSet(28)).Returns(dataSet28);
            mockedManager.Setup(m => m.GetDataSet(29)).Returns(dataSet29);
            mockedManager.Setup(m => m.GetDataSet(30)).Returns(dataSet30);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 10;
            Extremum extremum = new Extremum(dataSet17.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 10;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByLow_IfThereIsLowerValueOnTheLastQuotationAllowedByMaxSerie()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            DataSet dataSet89 = getDataSetWithQuotationAndPrice(89, 1.08893, 1.08899, 1.08863, 1.08892, 1133);
            DataSet dataSet90 = getDataSetWithQuotationAndPrice(90, 1.08896, 1.08933, 1.08893, 1.08926, 685);
            DataSet dataSet91 = getDataSetWithQuotationAndPrice(91, 1.08928, 1.08945, 1.08916, 1.08932, 774);
            DataSet dataSet92 = getDataSetWithQuotationAndPrice(92, 1.0893, 1.08939, 1.08923, 1.08932, 441);
            DataSet dataSet93 = getDataSetWithQuotationAndPrice(93, 1.08935, 1.08944, 1.08924, 1.08932, 764);
            DataSet dataSet94 = getDataSetWithQuotationAndPrice(94, 1.08932, 1.08942, 1.08908, 1.08913, 827);
            DataSet dataSet95 = getDataSetWithQuotationAndPrice(95, 1.08912, 1.08918, 1.08878, 1.0888, 805);
            DataSet dataSet96 = getDataSetWithQuotationAndPrice(96, 1.0888, 1.08966, 1.08859, 1.08904, 905);
            DataSet dataSet97 = getDataSetWithQuotationAndPrice(97, 1.08904, 1.08923, 1.08895, 1.08916, 767);
            DataSet dataSet98 = getDataSetWithQuotationAndPrice(98, 1.08915, 1.08928, 1.08902, 1.08921, 691);
            DataSet dataSet99 = getDataSetWithQuotationAndPrice(99, 1.08922, 1.08926, 1.08911, 1.08925, 675);
            DataSet dataSet100 = getDataSetWithQuotationAndPrice(100, 1.08924, 1.08959, 1.08916, 1.08956, 809);
            DataSet dataSet101 = getDataSetWithQuotationAndPrice(101, 1.08955, 1.08955, 1.08901, 1.0895, 1153);
            DataSet dataSet102 = getDataSetWithQuotationAndPrice(102, 1.08947, 1.08953, 1.08907, 1.0891, 807);
            DataSet dataSet103 = getDataSetWithQuotationAndPrice(103, 1.08911, 1.08955, 1.08906, 1.08955, 822);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);
            mockedManager.Setup(m => m.GetDataSet(89)).Returns(dataSet89);
            mockedManager.Setup(m => m.GetDataSet(90)).Returns(dataSet90);
            mockedManager.Setup(m => m.GetDataSet(91)).Returns(dataSet91);
            mockedManager.Setup(m => m.GetDataSet(92)).Returns(dataSet92);
            mockedManager.Setup(m => m.GetDataSet(93)).Returns(dataSet93);
            mockedManager.Setup(m => m.GetDataSet(94)).Returns(dataSet94);
            mockedManager.Setup(m => m.GetDataSet(95)).Returns(dataSet95);
            mockedManager.Setup(m => m.GetDataSet(96)).Returns(dataSet96);
            mockedManager.Setup(m => m.GetDataSet(97)).Returns(dataSet97);
            mockedManager.Setup(m => m.GetDataSet(98)).Returns(dataSet98);
            mockedManager.Setup(m => m.GetDataSet(99)).Returns(dataSet99);
            mockedManager.Setup(m => m.GetDataSet(100)).Returns(dataSet100);
            mockedManager.Setup(m => m.GetDataSet(101)).Returns(dataSet101);
            mockedManager.Setup(m => m.GetDataSet(102)).Returns(dataSet102);
            mockedManager.Setup(m => m.GetDataSet(103)).Returns(dataSet103);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 6;
            Extremum extremum = new Extremum(dataSet89.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 6;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterCounter_ReturnsProperValueForTroughByLow_IfThereIsLowerValueOnTheFirstPositionAfterMaxSerie()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            DataSet dataSet89 = getDataSetWithQuotationAndPrice(89, 1.08893, 1.08899, 1.08863, 1.08892, 1133);
            DataSet dataSet90 = getDataSetWithQuotationAndPrice(90, 1.08896, 1.08933, 1.08893, 1.08926, 685);
            DataSet dataSet91 = getDataSetWithQuotationAndPrice(91, 1.08928, 1.08945, 1.08916, 1.08932, 774);
            DataSet dataSet92 = getDataSetWithQuotationAndPrice(92, 1.0893, 1.08939, 1.08923, 1.08932, 441);
            DataSet dataSet93 = getDataSetWithQuotationAndPrice(93, 1.08935, 1.08944, 1.08924, 1.08932, 764);
            DataSet dataSet94 = getDataSetWithQuotationAndPrice(94, 1.08932, 1.08942, 1.08908, 1.08913, 827);
            DataSet dataSet95 = getDataSetWithQuotationAndPrice(95, 1.08912, 1.08918, 1.08878, 1.0888, 805);
            DataSet dataSet96 = getDataSetWithQuotationAndPrice(96, 1.0888, 1.08966, 1.08859, 1.08904, 905);
            DataSet dataSet97 = getDataSetWithQuotationAndPrice(97, 1.08904, 1.08923, 1.08895, 1.08916, 767);
            DataSet dataSet98 = getDataSetWithQuotationAndPrice(98, 1.08915, 1.08928, 1.08902, 1.08921, 691);
            DataSet dataSet99 = getDataSetWithQuotationAndPrice(99, 1.08922, 1.08926, 1.08911, 1.08925, 675);
            DataSet dataSet100 = getDataSetWithQuotationAndPrice(100, 1.08924, 1.08959, 1.08916, 1.08956, 809);
            DataSet dataSet101 = getDataSetWithQuotationAndPrice(101, 1.08955, 1.08955, 1.08901, 1.0895, 1153);
            DataSet dataSet102 = getDataSetWithQuotationAndPrice(102, 1.08947, 1.08953, 1.08907, 1.0891, 807);
            DataSet dataSet103 = getDataSetWithQuotationAndPrice(103, 1.08911, 1.08955, 1.08906, 1.08955, 822);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);
            mockedManager.Setup(m => m.GetDataSet(89)).Returns(dataSet89);
            mockedManager.Setup(m => m.GetDataSet(90)).Returns(dataSet90);
            mockedManager.Setup(m => m.GetDataSet(91)).Returns(dataSet91);
            mockedManager.Setup(m => m.GetDataSet(92)).Returns(dataSet92);
            mockedManager.Setup(m => m.GetDataSet(93)).Returns(dataSet93);
            mockedManager.Setup(m => m.GetDataSet(94)).Returns(dataSet94);
            mockedManager.Setup(m => m.GetDataSet(95)).Returns(dataSet95);
            mockedManager.Setup(m => m.GetDataSet(96)).Returns(dataSet96);
            mockedManager.Setup(m => m.GetDataSet(97)).Returns(dataSet97);
            mockedManager.Setup(m => m.GetDataSet(98)).Returns(dataSet98);
            mockedManager.Setup(m => m.GetDataSet(99)).Returns(dataSet99);
            mockedManager.Setup(m => m.GetDataSet(100)).Returns(dataSet100);
            mockedManager.Setup(m => m.GetDataSet(101)).Returns(dataSet101);
            mockedManager.Setup(m => m.GetDataSet(102)).Returns(dataSet102);
            mockedManager.Setup(m => m.GetDataSet(103)).Returns(dataSet103);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            processor.MaxSerieCount = 5;
            Extremum extremum = new Extremum(dataSet89.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterCounter(extremum);
            int expectedResult = 5;
            Assert.AreEqual(expectedResult, result);

        }





        #endregion CALCULATE_LATER_COUNTER


        #region CALCULATE_LATER_CHANGE

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForPeakByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);

            //Assert
            var result = processor.CalculateLaterChange(extremum, 2);
            double expectedResult = (1.09177d - 1.09148d) / 1.09177d;
            double difference = (Math.Abs(expectedResult - result));
            Assert.AreEqual(result, expectedResult);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForPeakByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet20.price, ExtremumType.PeakByClose);
            
            //Assert
            var result = processor.CalculateLaterChange(extremum, 5);
            double expectedResult = (1.09171d - 1.09189d) / 1.09171d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForPeakByClose_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByClose);
            
            //Assert
            var result = processor.CalculateLaterChange(extremum, 5);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForPeakByHigh_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            DataSet dataSet9 = getDataSetWithQuotationAndPrice(9, 1.09178, 1.09219, 1.09143, 1.09149, 657);
            DataSet dataSet10 = getDataSetWithQuotationAndPrice(10, 1.0915, 1.09164, 1.09144, 1.09148, 414);
            DataSet dataSet11 = getDataSetWithQuotationAndPrice(11, 1.09149, 1.09156, 1.09095, 1.091, 419);
            DataSet dataSet12 = getDataSetWithQuotationAndPrice(12, 1.09098, 1.09118, 1.09091, 1.09108, 341);
            DataSet dataSet13 = getDataSetWithQuotationAndPrice(13, 1.09109, 1.09112, 1.09066, 1.09068, 326);
            DataSet dataSet14 = getDataSetWithQuotationAndPrice(14, 1.09066, 1.09088, 1.09052, 1.09085, 476);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);
            mockedManager.Setup(m => m.GetDataSet(9)).Returns(dataSet9);
            mockedManager.Setup(m => m.GetDataSet(10)).Returns(dataSet10);
            mockedManager.Setup(m => m.GetDataSet(11)).Returns(dataSet11);
            mockedManager.Setup(m => m.GetDataSet(12)).Returns(dataSet12);
            mockedManager.Setup(m => m.GetDataSet(13)).Returns(dataSet13);
            mockedManager.Setup(m => m.GetDataSet(14)).Returns(dataSet14);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterChange(extremum, 2);
            double expectedResult = (1.09177d - 1.09148d) / 1.09177d;
            Assert.AreEqual(result, expectedResult);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForPeakByHigh_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet18 = getDataSetWithQuotationAndPrice(18, 1.09099, 1.09129, 1.09092, 1.0911, 745);
            DataSet dataSet19 = getDataSetWithQuotationAndPrice(19, 1.09111, 1.09197, 1.09088, 1.09142, 1140);
            DataSet dataSet20 = getDataSetWithQuotationAndPrice(20, 1.09151, 1.09257, 1.09138, 1.09171, 417);
            DataSet dataSet21 = getDataSetWithQuotationAndPrice(21, 1.09165, 1.09188, 1.0913, 1.09154, 398);
            DataSet dataSet22 = getDataSetWithQuotationAndPrice(22, 1.09152, 1.09181, 1.09129, 1.09155, 518);
            DataSet dataSet23 = getDataSetWithQuotationAndPrice(23, 1.09153, 1.09171, 1.091, 1.09142, 438);
            DataSet dataSet24 = getDataSetWithQuotationAndPrice(24, 1.0912, 1.09192, 1.0911, 1.09162, 532);
            DataSet dataSet25 = getDataSetWithQuotationAndPrice(25, 1.0916, 1.09199, 1.0915, 1.09189, 681);
            DataSet dataSet26 = getDataSetWithQuotationAndPrice(26, 1.0919, 1.09209, 1.09171, 1.09179, 387);
            DataSet dataSet27 = getDataSetWithQuotationAndPrice(27, 1.09173, 1.09211, 1.09148, 1.09181, 792);
            mockedManager.Setup(m => m.GetDataSet(18)).Returns(dataSet18);
            mockedManager.Setup(m => m.GetDataSet(19)).Returns(dataSet19);
            mockedManager.Setup(m => m.GetDataSet(20)).Returns(dataSet20);
            mockedManager.Setup(m => m.GetDataSet(21)).Returns(dataSet21);
            mockedManager.Setup(m => m.GetDataSet(22)).Returns(dataSet22);
            mockedManager.Setup(m => m.GetDataSet(23)).Returns(dataSet23);
            mockedManager.Setup(m => m.GetDataSet(24)).Returns(dataSet24);
            mockedManager.Setup(m => m.GetDataSet(25)).Returns(dataSet25);
            mockedManager.Setup(m => m.GetDataSet(26)).Returns(dataSet26);
            mockedManager.Setup(m => m.GetDataSet(27)).Returns(dataSet27);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet20.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateLaterChange(extremum, 5);
            double expectedResult = (1.09171d - 1.09189d) / 1.09171d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForPeakByHigh_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.PeakByHigh);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 10);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForTroughByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet78 = getDataSetWithQuotationAndPrice(78, 1.09003, 1.09013, 1.08976, 1.08985, 905);
            DataSet dataSet79 = getDataSetWithQuotationAndPrice(79, 1.08986, 1.08995, 1.08936, 1.0894, 1168);
            DataSet dataSet80 = getDataSetWithQuotationAndPrice(80, 1.08941, 1.08954, 1.08915, 1.08926, 1583);
            DataSet dataSet81 = getDataSetWithQuotationAndPrice(81, 1.08928, 1.08949, 1.08912, 1.08934, 1369);
            DataSet dataSet82 = getDataSetWithQuotationAndPrice(82, 1.08932, 1.08966, 1.08926, 1.08954, 1208);
            DataSet dataSet83 = getDataSetWithQuotationAndPrice(83, 1.08953, 1.08983, 1.0895, 1.08977, 827);
            DataSet dataSet84 = getDataSetWithQuotationAndPrice(84, 1.08977, 1.08982, 1.08947, 1.08948, 689);
            DataSet dataSet85 = getDataSetWithQuotationAndPrice(85, 1.08945, 1.08948, 1.08925, 1.08937, 1129);
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            mockedManager.Setup(m => m.GetDataSet(78)).Returns(dataSet78);
            mockedManager.Setup(m => m.GetDataSet(79)).Returns(dataSet79);
            mockedManager.Setup(m => m.GetDataSet(80)).Returns(dataSet80);
            mockedManager.Setup(m => m.GetDataSet(81)).Returns(dataSet81);
            mockedManager.Setup(m => m.GetDataSet(82)).Returns(dataSet82);
            mockedManager.Setup(m => m.GetDataSet(83)).Returns(dataSet83);
            mockedManager.Setup(m => m.GetDataSet(84)).Returns(dataSet84);
            mockedManager.Setup(m => m.GetDataSet(85)).Returns(dataSet85);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet80.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterChange(extremum, 6);
            double expectedResult = (1.08913d - 1.08926d) / 1.08926d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForTroughByClose_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet78 = getDataSetWithQuotationAndPrice(78, 1.09003, 1.09013, 1.08976, 1.08985, 905);
            DataSet dataSet79 = getDataSetWithQuotationAndPrice(79, 1.08986, 1.08995, 1.08936, 1.0894, 1168);
            DataSet dataSet80 = getDataSetWithQuotationAndPrice(80, 1.08941, 1.08954, 1.08915, 1.08926, 1583);
            DataSet dataSet81 = getDataSetWithQuotationAndPrice(81, 1.08928, 1.08949, 1.08912, 1.08934, 1369);
            DataSet dataSet82 = getDataSetWithQuotationAndPrice(82, 1.08932, 1.08966, 1.08926, 1.08954, 1208);
            DataSet dataSet83 = getDataSetWithQuotationAndPrice(83, 1.08953, 1.08983, 1.0895, 1.08977, 827);
            DataSet dataSet84 = getDataSetWithQuotationAndPrice(84, 1.08977, 1.08982, 1.08947, 1.08948, 689);
            DataSet dataSet85 = getDataSetWithQuotationAndPrice(85, 1.08945, 1.08948, 1.08925, 1.08937, 1129);
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            mockedManager.Setup(m => m.GetDataSet(78)).Returns(dataSet78);
            mockedManager.Setup(m => m.GetDataSet(79)).Returns(dataSet79);
            mockedManager.Setup(m => m.GetDataSet(80)).Returns(dataSet80);
            mockedManager.Setup(m => m.GetDataSet(81)).Returns(dataSet81);
            mockedManager.Setup(m => m.GetDataSet(82)).Returns(dataSet82);
            mockedManager.Setup(m => m.GetDataSet(83)).Returns(dataSet83);
            mockedManager.Setup(m => m.GetDataSet(84)).Returns(dataSet84);
            mockedManager.Setup(m => m.GetDataSet(85)).Returns(dataSet85);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet80.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateLaterChange(extremum, 2);
            double expectedResult = (1.08954d - 1.08926d) / 1.08926d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForTroughByClose_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.TroughByClose);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 10);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }


        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForTroughByLow_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsLower()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet78 = getDataSetWithQuotationAndPrice(78, 1.09003, 1.09013, 1.08976, 1.08985, 905);
            DataSet dataSet79 = getDataSetWithQuotationAndPrice(79, 1.08986, 1.08995, 1.08936, 1.0894, 1168);
            DataSet dataSet80 = getDataSetWithQuotationAndPrice(80, 1.08941, 1.08954, 1.08915, 1.08926, 1583);
            DataSet dataSet81 = getDataSetWithQuotationAndPrice(81, 1.08928, 1.08949, 1.08912, 1.08934, 1369);
            DataSet dataSet82 = getDataSetWithQuotationAndPrice(82, 1.08932, 1.08966, 1.08926, 1.08954, 1208);
            DataSet dataSet83 = getDataSetWithQuotationAndPrice(83, 1.08953, 1.08983, 1.0895, 1.08977, 827);
            DataSet dataSet84 = getDataSetWithQuotationAndPrice(84, 1.08977, 1.08982, 1.08947, 1.08948, 689);
            DataSet dataSet85 = getDataSetWithQuotationAndPrice(85, 1.08945, 1.08948, 1.08925, 1.08937, 1129);
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            mockedManager.Setup(m => m.GetDataSet(78)).Returns(dataSet78);
            mockedManager.Setup(m => m.GetDataSet(79)).Returns(dataSet79);
            mockedManager.Setup(m => m.GetDataSet(80)).Returns(dataSet80);
            mockedManager.Setup(m => m.GetDataSet(81)).Returns(dataSet81);
            mockedManager.Setup(m => m.GetDataSet(82)).Returns(dataSet82);
            mockedManager.Setup(m => m.GetDataSet(83)).Returns(dataSet83);
            mockedManager.Setup(m => m.GetDataSet(84)).Returns(dataSet84);
            mockedManager.Setup(m => m.GetDataSet(85)).Returns(dataSet85);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet80.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterChange(extremum, 6);
            double expectedResult = (1.08913d - 1.08926d) / 1.08926d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForTroughByLow_IfQuotationsForComparedIndexExistsAndComparedClosePriceIsHigher()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet78 = getDataSetWithQuotationAndPrice(78, 1.09003, 1.09013, 1.08976, 1.08985, 905);
            DataSet dataSet79 = getDataSetWithQuotationAndPrice(79, 1.08986, 1.08995, 1.08936, 1.0894, 1168);
            DataSet dataSet80 = getDataSetWithQuotationAndPrice(80, 1.08941, 1.08954, 1.08915, 1.08926, 1583);
            DataSet dataSet81 = getDataSetWithQuotationAndPrice(81, 1.08928, 1.08949, 1.08912, 1.08934, 1369);
            DataSet dataSet82 = getDataSetWithQuotationAndPrice(82, 1.08932, 1.08966, 1.08926, 1.08954, 1208);
            DataSet dataSet83 = getDataSetWithQuotationAndPrice(83, 1.08953, 1.08983, 1.0895, 1.08977, 827);
            DataSet dataSet84 = getDataSetWithQuotationAndPrice(84, 1.08977, 1.08982, 1.08947, 1.08948, 689);
            DataSet dataSet85 = getDataSetWithQuotationAndPrice(85, 1.08945, 1.08948, 1.08925, 1.08937, 1129);
            DataSet dataSet86 = getDataSetWithQuotationAndPrice(86, 1.08936, 1.08938, 1.08908, 1.08913, 848);
            DataSet dataSet87 = getDataSetWithQuotationAndPrice(87, 1.08914, 1.08919, 1.08882, 1.0889, 748);
            DataSet dataSet88 = getDataSetWithQuotationAndPrice(88, 1.08893, 1.08916, 1.08884, 1.08894, 1299);
            mockedManager.Setup(m => m.GetDataSet(78)).Returns(dataSet78);
            mockedManager.Setup(m => m.GetDataSet(79)).Returns(dataSet79);
            mockedManager.Setup(m => m.GetDataSet(80)).Returns(dataSet80);
            mockedManager.Setup(m => m.GetDataSet(81)).Returns(dataSet81);
            mockedManager.Setup(m => m.GetDataSet(82)).Returns(dataSet82);
            mockedManager.Setup(m => m.GetDataSet(83)).Returns(dataSet83);
            mockedManager.Setup(m => m.GetDataSet(84)).Returns(dataSet84);
            mockedManager.Setup(m => m.GetDataSet(85)).Returns(dataSet85);
            mockedManager.Setup(m => m.GetDataSet(86)).Returns(dataSet86);
            mockedManager.Setup(m => m.GetDataSet(87)).Returns(dataSet87);
            mockedManager.Setup(m => m.GetDataSet(88)).Returns(dataSet88);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet80.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateLaterChange(extremum, 2);
            double expectedResult = (1.08954d - 1.08926d) / 1.08926d;
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void CalculateLaterChange_ReturnsProperValueForTroughByLow_IfQuotationsForComparedIndexDoesntExist()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.09111, 1.09124, 1.09091, 1.091, 1154);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.09101, 1.09132, 1.09097, 1.09131, 933);
            DataSet dataSet7 = getDataSetWithQuotationAndPrice(7, 1.09131, 1.09167, 1.09114, 1.09165, 1079);
            DataSet dataSet8 = getDataSetWithQuotationAndPrice(8, 1.09164, 1.09183, 1.0915, 1.09177, 1009);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);
            mockedManager.Setup(m => m.GetDataSet(7)).Returns(dataSet7);
            mockedManager.Setup(m => m.GetDataSet(8)).Returns(dataSet8);


            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);
            Extremum extremum = new Extremum(dataSet8.price, ExtremumType.TroughByLow);

            //Assert
            var result = processor.CalculateEarlierChange(extremum, 10);
            double expectedResult = 0d;
            Assert.AreEqual(expectedResult, result);

        }

        #endregion CALCULATE_LATER_CHANGE



        #region EXTRACT_EXTREMUM_GROUPS

        [TestMethod]
        public void ExtractExtremumGroups_IfThereIsNoExtremumByCloseInSourceCollection_ReturnsNothing()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void ExtractExtremumGroups_ReturnsProperExtremumGroupForPeakByCloseAndHighAtTheSameQuotation()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);

            Extremum peakByClose = new Extremum(dataSet3.price, ExtremumType.PeakByClose);
            Extremum peakByHigh = new Extremum(dataSet3.price, ExtremumType.PeakByHigh);
            
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup group = result.ElementAt(0);
            
            Assert.IsTrue(result.Count() == 1);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.IsPeak);
            Assert.IsTrue(group.MasterExtremum == peakByClose);
            Assert.IsNotNull(group.SecondExtremum);
            Assert.IsTrue(group.SecondExtremum == peakByClose);

        }

        [TestMethod]
        public void ExtractExtremumGroups_ReturnsProperExtremumGroupForPeakByCloseAndHighAtPreviousQuotation()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09222, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);

            Extremum peakByHigh = new Extremum(dataSet2.price, ExtremumType.PeakByHigh);
            Extremum peakByClose = new Extremum(dataSet3.price, ExtremumType.PeakByClose);

            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup group = result.ElementAt(0);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.IsPeak);
            Assert.IsTrue(group.MasterExtremum == peakByClose);
            Assert.IsNotNull(group.SecondExtremum);
            Assert.IsTrue(group.SecondExtremum == peakByHigh);

        }

        [TestMethod]
        public void ExtractExtremumGroups_ReturnsProperExtremumGroupForPeakByCloseAndHighAtNextQuotation()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09192, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0922, 1.09111, 1.09112, 1392);

            Extremum peakByClose = new Extremum(dataSet3.price, ExtremumType.PeakByClose);
            Extremum peakByHigh = new Extremum(dataSet4.price, ExtremumType.PeakByHigh);

            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup group = result.ElementAt(0);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.IsPeak);
            Assert.IsTrue(group.MasterExtremum == peakByClose);
            Assert.IsNotNull(group.SecondExtremum);
            Assert.IsTrue(group.SecondExtremum == peakByHigh);

        }

        [TestMethod]
        public void ExtractExtremumGroups_ReturnsProperExtremumGroupForTroughByCloseAndLowAtTheSameQuotation()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09192, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09188, 1.09126, 1.09134, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0916, 1.0917, 1.09151, 1.09162, 1392);

            Extremum troughByClose = new Extremum(dataSet3.price, ExtremumType.TroughByClose);
            Extremum troughByLow = new Extremum(dataSet3.price, ExtremumType.TroughByLow);

            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup group = result.ElementAt(0);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsNotNull(group);
            Assert.IsFalse(group.IsPeak);
            Assert.IsTrue(group.MasterExtremum == troughByClose);
            Assert.IsNotNull(group.SecondExtremum);
            Assert.IsTrue(group.SecondExtremum == troughByClose);

        }

        [TestMethod]
        public void ExtractExtremumGroups_ReturnsProperExtremumGroupForTroughByCloseAndLowAtPreviousQuotation()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09192, 1.09115, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09188, 1.09126, 1.09134, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0916, 1.0917, 1.09151, 1.09162, 1392);

            Extremum troughByLow = new Extremum(dataSet2.price, ExtremumType.TroughByLow);
            Extremum troughByClose = new Extremum(dataSet3.price, ExtremumType.TroughByClose);

            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup group = result.ElementAt(0);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsNotNull(group);
            Assert.IsFalse(group.IsPeak);
            Assert.IsTrue(group.MasterExtremum == troughByClose);
            Assert.IsNotNull(group.SecondExtremum);
            Assert.IsTrue(group.SecondExtremum == troughByLow);

        }

        [TestMethod]
        public void ExtractExtremumGroups_ReturnsProperExtremumGroupForTroughByCloseAndLowAtNextQuotation()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09192, 1.09135, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09188, 1.09126, 1.09134, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0916, 1.0917, 1.09111, 1.09162, 1392);

            Extremum troughByLow = new Extremum(dataSet4.price, ExtremumType.TroughByLow);
            Extremum troughByClose = new Extremum(dataSet3.price, ExtremumType.TroughByClose);

            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup group = result.ElementAt(0);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsNotNull(group);
            Assert.IsFalse(group.IsPeak);
            Assert.IsTrue(group.MasterExtremum == troughByClose);
            Assert.IsNotNull(group.SecondExtremum);
            Assert.IsTrue(group.SecondExtremum == troughByLow);

        }

        [TestMethod]
        public void ExtractExtremumGroups_IfThereIsOrphanedByHighExtremum_ItIsReturnedAsExtremumGroup()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09192, 1.09115, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09208, 1.09126, 1.09134, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0916, 1.0917, 1.09151, 1.09132, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.0916, 1.0917, 1.09151, 1.09182, 1392);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.0916, 1.0917, 1.09151, 1.09172, 1392);

            Extremum peakByHigh = new Extremum(dataSet3.price, ExtremumType.PeakByHigh);
            Extremum peakByClose = new Extremum(dataSet5.price, ExtremumType.PeakByClose);

            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4, dataSet5, dataSet6 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup groupHigh = result.ElementAt(0);
            ExtremumGroup groupClose = result.ElementAt(1);

            Assert.IsTrue(result.Count() == 2);

            Assert.IsNotNull(groupHigh);
            Assert.IsTrue(groupHigh.IsPeak);
            Assert.IsNotNull(groupHigh.MasterExtremum);
            Assert.IsNotNull(groupHigh.SecondExtremum);
            Assert.IsTrue(groupHigh.MasterExtremum == peakByHigh);
            Assert.IsTrue(groupHigh.SecondExtremum == peakByHigh);

            Assert.IsNotNull(groupClose);
            Assert.IsTrue(groupClose.IsPeak);
            Assert.IsNotNull(groupClose.MasterExtremum);
            Assert.IsNotNull(groupClose.SecondExtremum);
            Assert.IsTrue(groupClose.MasterExtremum == peakByClose);
            Assert.IsTrue(groupClose.SecondExtremum == peakByClose);

        }

        [TestMethod]
        public void ExtractExtremumGroups_IfThereIsOrphanedByLowExtremum_ItIsReturnedAsExtremumGroup()
        {

            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09192, 1.09115, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.0918, 1.09096, 1.09134, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0916, 1.0917, 1.09101, 1.09132, 1392);
            DataSet dataSet5 = getDataSetWithQuotationAndPrice(5, 1.0916, 1.0917, 1.09101, 1.09112, 1392);
            DataSet dataSet6 = getDataSetWithQuotationAndPrice(6, 1.0916, 1.0917, 1.09101, 1.09172, 1392);

            Extremum troughByLow = new Extremum(dataSet3.price, ExtremumType.TroughByLow);
            Extremum troughByClose = new Extremum(dataSet5.price, ExtremumType.TroughByClose);

            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);
            mockedManager.Setup(m => m.GetDataSet(5)).Returns(dataSet5);
            mockedManager.Setup(m => m.GetDataSet(6)).Returns(dataSet6);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4, dataSet5, dataSet6 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            ExtremumGroup groupLow = result.ElementAt(0);
            ExtremumGroup groupClose = result.ElementAt(1);

            Assert.IsTrue(result.Count() == 2);

            Assert.IsNotNull(groupLow);
            Assert.IsFalse(groupLow.IsPeak);
            Assert.IsNotNull(groupLow.MasterExtremum);
            Assert.IsNotNull(groupLow.SecondExtremum);
            Assert.IsTrue(groupLow.SecondExtremum == troughByLow);
            Assert.IsTrue(groupLow.MasterExtremum == troughByLow);

            Assert.IsNotNull(groupClose);
            Assert.IsFalse(groupClose.IsPeak);
            Assert.IsNotNull(groupClose.MasterExtremum);
            Assert.IsNotNull(groupClose.SecondExtremum);
            Assert.IsTrue(groupClose.MasterExtremum == troughByClose);
            Assert.IsTrue(groupClose.SecondExtremum == troughByClose);

        }

        [TestMethod]
        public void ExtractExtremumGroups_IfThereIsPeakByCloseExtremumAtFirstQuotation_ItIsSkipped()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);

            Extremum peakByClose = new Extremum(dataSet1.price, ExtremumType.PeakByClose);
            Extremum peakByHigh = new Extremum(dataSet2.price, ExtremumType.PeakByHigh);
            
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            Assert.IsTrue(result.Count() == 0);

        }

        [TestMethod]
        public void ExtractExtremumGroups_IfThereIsTroughByCloseExtremumAtFirstQuotation_ItIsSkipped()
        {
            //Arrange
            Mock<IProcessManager> mockedManager = new Mock<IProcessManager>();
            DataSet dataSet1 = getDataSetWithQuotationAndPrice(1, 1.09191, 1.09187, 1.09162, 1.09177, 1411);
            DataSet dataSet2 = getDataSetWithQuotationAndPrice(2, 1.09177, 1.09182, 1.09165, 1.09174, 1819);
            DataSet dataSet3 = getDataSetWithQuotationAndPrice(3, 1.09191, 1.09218, 1.09186, 1.09194, 1359);
            DataSet dataSet4 = getDataSetWithQuotationAndPrice(4, 1.0915, 1.0916, 1.09111, 1.09112, 1392);

            Extremum troughByClose = new Extremum(dataSet1.price, ExtremumType.TroughByClose);
            Extremum troughByLow = new Extremum(dataSet2.price, ExtremumType.TroughByLow);

            
            mockedManager.Setup(m => m.GetDataSet(1)).Returns(dataSet1);
            mockedManager.Setup(m => m.GetDataSet(2)).Returns(dataSet2);
            mockedManager.Setup(m => m.GetDataSet(3)).Returns(dataSet3);
            mockedManager.Setup(m => m.GetDataSet(4)).Returns(dataSet4);

            DataSet[] dataSets = new DataSet[] { dataSet1, dataSet2, dataSet3, dataSet4 };

            //Act
            ExtremumProcessor processor = new ExtremumProcessor(mockedManager.Object);

            //Assert
            var result = processor.ExtractExtremumGroups(dataSets);
            Assert.IsTrue(result.Count() == 0);

        }

        #endregion EXTRACT_EXTREMUM_GROUPS

    }

}
