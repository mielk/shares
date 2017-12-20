//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Stock.Domain.Entities;
//using Stock.Domain.Enums;

//namespace Stock_UnitTest.Stock.Domain
//{
//    [TestClass]
//    public class TrendlineUnitTests
//    {


//        private Asset asset = new Asset(1, "NZDUSD");
//        private Timeframe timeframe = Timeframe.GetTimeframe(TimeframeSymbol.D1);



//        private Trendline ProduceTrendline(TrendlineType type, int initialIndex, double initialLevel, 
//                                                        int boundIndex, double boundLevel)
//        {

//            double initialPeak = type == TrendlineType.Resistance ? 0.5 : 0;
//            double initialTrough = type == TrendlineType.Resistance ? 0 : 0.5;
//            DataItem initialItem = new DataItem
//            {
//                AssetId = asset.Id,
//                Timeframe = timeframe,
//                Index = initialIndex,
//                Price = new Price { 
//                    AssetId = asset.Id,
//                    PeakByClose = initialPeak,
//                    PeakByHigh = initialPeak,
//                    TroughByLow = initialTrough,
//                    TroughByClose = initialTrough
//                }
//            };
//            DataItem boundItem = new DataItem
//            {
//                AssetId = asset.Id,
//                Timeframe = timeframe,
//                Index = boundIndex,
//                Price = new Price
//                {
//                    AssetId = asset.Id,
//                    PeakByClose = initialPeak,
//                    PeakByHigh = initialPeak,
//                    TroughByLow = initialTrough,
//                    TroughByClose = initialTrough
//                }
//            };


//            return new Trendline(asset, timeframe, initialItem, initialLevel, boundItem, boundLevel);

//        }


//        #region IsProperExtremum

//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_false_for_support_line_if_item_is_not_trough(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Support, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { TroughByClose = 0, TroughByLow = 0, PeakByClose = 1, PeakByHigh = 1 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, new DataItem());

//            //then
//            Assert.IsFalse(isExtremum);

//        }




//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_false_for_resistance_line_if_item_is_not_peak(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Resistance, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { PeakByHigh = 0, PeakByClose = 0, TroughByLow = 1, TroughByClose = 1 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, new DataItem());

//            //then
//            Assert.IsFalse(isExtremum);

//        }



//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_false_for_support_line_if_item_is_trough_but_previous_item_was_previous_hit(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Support, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { PeakByHigh = 0, PeakByClose = 0, TroughByLow = 0, TroughByClose = 1 }
//            };

//            //when
//            trendBreak.PreviousHit = new DataItem { Index = 4 };
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, new DataItem());

//            //then
//            Assert.IsFalse(isExtremum);

//        }




//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_false_for_resistance_line_if_item_is_peak_but_previous_item_was_previous_hit(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Resistance, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { PeakByHigh = 0, PeakByClose = 1, TroughByLow = 0, TroughByClose = 0 }
//            };

//            //when
//            trendBreak.PreviousHit = new DataItem { Index = 4 };
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, new DataItem());

//            //then
//            Assert.IsFalse(isExtremum);

//        }




//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_false_for_support_line_if_item_is_trough_by_close_but_next_item_is_trough_by_low(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Support, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { TroughByLow = 0, TroughByClose = 1 }
//            };
//            DataItem nextItem = new DataItem
//            {
//                Index = 6,
//                Price = new Price { TroughByLow = 1, TroughByClose = 0 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, nextItem);

//            //then
//            Assert.IsFalse(isExtremum);

//        }




//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_false_for_resistance_line_if_item_is_peak_by_close_but_next_item_is_peak_by_high(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Resistance, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { PeakByHigh = 0, PeakByClose = 1 }
//            };
//            DataItem nextItem = new DataItem
//            {
//                Index = 6,
//                Price = new Price { PeakByHigh = 1, PeakByClose = 0 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, nextItem);

//            //then
//            Assert.IsFalse(isExtremum);

//        }



//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_true_for_support_line_if_item_is_trough_by_low(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Support, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { TroughByClose = 0, TroughByLow = 0.5 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, new DataItem { Price = new Price() });

//            //then
//            Assert.IsTrue(isExtremum);

//        }



//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_true_for_resistance_line_if_item_is_peak_by_high(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Resistance, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { PeakByClose = 0, PeakByHigh = 0.5 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, new DataItem { Price = new Price() });

//            //then
//            Assert.IsTrue(isExtremum);

//        }



//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_true_for_support_line_if_item_is_trough_by_close_but_next_item_is_not_trough_by_low(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Support, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { TroughByLow = 0, TroughByClose = 0.5 }
//            };
//            DataItem nextItem = new DataItem
//            {
//                Index = 6,
//                Price = new Price { TroughByLow = 0, TroughByClose = 0 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, nextItem);

//            //then
//            Assert.IsTrue(isExtremum);

//        }



//        [TestMethod]
//        [TestCategory("IsProperExtremum")]
//        public void IsProperExtremum_returns_true_for_resistance_line_if_item_is_peak_by_close_but_next_item_is_not_peak_by_high(){

//            //given
//            Trendline trendBreak = ProduceTrendline(TrendlineType.Resistance, 1, 1d, 10, 0.99d);
//            DataItem checkedItem = new DataItem
//            {
//                Index = 5,
//                Price = new Price { PeakByClose = 0, PeakByHigh = 0.5 }
//            };
//            DataItem nextItem = new DataItem
//            {
//                Index = 6,
//                Price = new Price { PeakByHigh = 0, PeakByClose = 0 }
//            };

//            //when
//            bool isExtremum = trendBreak.IsProperExtremum(checkedItem, nextItem);

//            //then
//            Assert.IsTrue(isExtremum);
//        }


//        #endregion




//    }
//}
