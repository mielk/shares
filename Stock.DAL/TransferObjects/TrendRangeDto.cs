using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DAL.TransferObjects
{
    public class TrendRangeDto
    {
        public int TrendRangeId { get; set; }
        public int TrendlineId { get; set; }
        public int BaseId { get; set; }
        public int BaseIsHit { get; set; }
        public int BaseDateIndex { get; set; }
        public int CounterId { get; set; }
        public int CounterIsHit { get; set; }
        public int CounterDateIndex { get; set; }
        public int IsPeak { get; set; }
        public double ExtremumPriceCrossPenaltyPoints { get; set; }
        public int ExtremumPriceCrossCounter { get; set; }
        public double OCPriceCrossPenaltyPoints { get; set; }
        public int OCPriceCrossCounter { get; set; }
        public int TotalCandles { get; set; }
        public double AverageVariation { get; set; }
        public double ExtremumVariation { get; set; }
        public double OpenCloseVariation { get; set; }
        public double BaseHitValue { get; set; }
        public double CounterHitValue { get; set; }
        public double Value { get; set; }
    }
}
