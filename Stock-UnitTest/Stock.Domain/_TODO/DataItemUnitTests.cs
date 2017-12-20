//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Stock.Domain.Entities;
//using Stock.Domain.Enums;

//namespace Stock_UnitTest.Stock.Domain
//{
//    [TestClass]
//    public class DataItemUnitTests
//    {


//        #region GetOpenOrClosePrice
//        [TestMethod]
//        [TestCategory("GetOpenOrClosePrice")]
//        public void GetOpenOrClosePrice_return_proper_value_for_resistance_line()
//        {
//            DataItem item = new DataItem { Quotation = new Quotation { Open = 0.7601, Close = 0.7605 } };
//            Assert.AreEqual(0.7605, item.GetOpenOrClosePrice(TrendlineType.Resistance));
//        }


//        [TestMethod]
//        [TestCategory("GetOpenOrClosePrice")]
//        public void GetOpenOrClosePrice_return_proper_value_for_support_line()
//        {
//            DataItem item = new DataItem{ Quotation = new Quotation { Open = 0.7601, Close = 0.7605 } };
//            Assert.AreEqual(0.7601, item.GetOpenOrClosePrice(TrendlineType.Support));
//        }

//        #endregion



//        #region GetHighOrLowPrice
//        [TestMethod]
//        [TestCategory("GetHighOrLowPrice")]
//        public void GetHighOrLowPrice_return_proper_value_for_resistance_line()
//        {
//            DataItem item = new DataItem { Quotation = new Quotation { Low = 0.7601, High = 0.7605 } };
//            Assert.AreEqual(0.7605, item.GetHighOrLowPrice(TrendlineType.Resistance));
//        }


//        [TestMethod]
//        [TestCategory("GetHighOrLowPrice")]
//        public void GetHighOrLowPrice_return_proper_value_for_support_line()
//        {
//            DataItem item = new DataItem { Quotation = new Quotation { Low = 0.7601, High = 0.7605 } };
//            Assert.AreEqual(0.7601, item.GetHighOrLowPrice(TrendlineType.Support));
//        }

//        #endregion



//        #region GetClosestDistance
//        [TestMethod]
//        [TestCategory("GetClosestDistance")]
//        public void GetClosestDistance_returns_high_price_for_ascending_trend_if_H_is_closest_to_the_trend()
//        {

//            var item = new DataItem
//            {
//                Quotation = new Quotation { Open = 100, Low = 99, High = 101, Close = 99.5 }
//            };
//            var level = 100.8d;

//            Assert.AreEqual(0.2d, Math.Round(item.GetClosestDistance(TrendlineType.Resistance, level), 2));

//        }

//        [TestMethod]
//        [TestCategory("GetClosestDistance")]
//        public void GetClosestDistance_returns_open_price_for_ascending_trend_if_OpenPrice_is_closest_to_the_trend()
//        {

//            var item = new DataItem
//            {
//                Quotation = new Quotation { Open = 100, Low = 99, High = 101, Close = 99.5 }
//            };
//            var level = 100.2d;

//            Assert.AreEqual(0.2d, Math.Round(item.GetClosestDistance(TrendlineType.Resistance, level), 2));

//        }

//        [TestMethod]
//        [TestCategory("GetClosestDistance")]
//        public void GetClosestDistance_returns_close_price_for_ascending_trend_if_ClosePrice_is_closest_to_the_trend()
//        {

//            var item = new DataItem
//            {
//                Quotation = new Quotation { Open = 99.4, Low = 99, High = 101, Close = 100.5 }
//            };
//            var level = 100d;

//            Assert.AreEqual(0.5d,  Math.Round(item.GetClosestDistance(TrendlineType.Resistance, level), 2));

//        }

//        [TestMethod]
//        [TestCategory("GetClosestDistance")]
//        public void GetClosestDistance_returns_low_price_for_descending_trend_if_L_is_closest_to_the_trend()
//        {

//            var item = new DataItem
//            {
//                Quotation = new Quotation { Open = 93, Low = 90.5, High = 95, Close = 94 }
//            };
//            var level = 90;

//            Assert.AreEqual(0.5d,  Math.Round(item.GetClosestDistance(TrendlineType.Support, level), 2));

//        }

//        [TestMethod]
//        [TestCategory("GetClosestDistance")]
//        public void GetClosestDistance_returns_open_price_for_descending_trend_if_OpenPrice_is_closest_to_the_trend()
//        {

//            var item = new DataItem
//            {
//                Quotation = new Quotation { Open = 93, Low = 90.5, High = 95, Close = 94 }
//            };
//            var level = 93.2d;

//            Assert.AreEqual(0.2d,  Math.Round(item.GetClosestDistance(TrendlineType.Support, level), 2));

//        }

//        [TestMethod]
//        [TestCategory("GetClosestDistance")]
//        public void GetClosestDistance_returns_close_price_for_descending_trend_if_ClosePrice_is_closest_to_the_trend()
//        {

//            var item = new DataItem
//            {
//                Quotation = new Quotation { Open = 97, Low = 90.5, High = 97.2, Close = 94 }
//            };
//            var level = 93.2d;

//            Assert.AreEqual(0.8d,  Math.Round(item.GetClosestDistance(TrendlineType.Support, level), 2));
//        }

//        #endregion



//        #region GetValue


//        private DataItem generateDataItemForGetValueUnitTests(double open, double low, double high, double close)
//        {
//            return new DataItem
//            {
//                AssetId = 1,
//                Date = DateTime.Now,
//                Index = 1,
//                Timeframe = Timeframe.GetTimeframe(TimeframeSymbol.D1),
//                Quotation = new Quotation { Open = open, Low = low, High = high, Close = close}
//            };
//        }


//        [TestMethod]
//        [TestCategory("GetValue")]
//        public void GetValue_returns_proper_value_for_peak_by_close()
//        {

//            //given
//            var item = generateDataItemForGetValueUnitTests(100, 98, 104, 102);

//            //when
//            var value = item.GetValue(true, true);

//            //then
//            Assert.AreEqual(102, value);

//        }


//        [TestMethod]
//        [TestCategory("GetValue")]
//        public void GetValue_returns_proper_value_for_peak_by_high()
//        {

//            //given
//            var item = generateDataItemForGetValueUnitTests(100, 98, 104, 102);

//            //when
//            var value = item.GetValue(true, false);

//            //then
//            Assert.AreEqual(104, value);

//        }


//        [TestMethod]
//        [TestCategory("GetValue")]
//        public void GetValue_returns_proper_value_for_trough_by_close()
//        {

//            //given
//            var item = generateDataItemForGetValueUnitTests(100, 98, 104, 102);

//            //when
//            var value = item.GetValue(false, true);

//            //then
//            Assert.AreEqual(102, value);

//        }


//        [TestMethod]
//        [TestCategory("GetValue")]
//        public void GetValue_returns_proper_value_for_trough_by_low()
//        {

//            //given
//            var item = generateDataItemForGetValueUnitTests(100, 98, 104, 102);

//            //when
//            var value = item.GetValue(false, false);

//            //then
//            Assert.AreEqual(98, value);

//        }



//        #endregion



//        #region TrendlineAnalysisStep
//        #endregion


//        #region Distance
//        #endregion


//        #region IsInRange
//        #endregion


//        #region GetMinDifference
//        #endregion


//        #region GetMaxDifference
//        #endregion


//        #region GetProperOpenOrClose
//        #endregion


//        #region GetProperHighOrLow
//        #endregion


//        #region IsTrendlineBroken
//        #endregion


//    }
//}
