using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Entities
{
    public class TrendRange
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
        public double? ExtremumPriceCrossPenaltyPoints { get; set; }
        public int? ExtremumPriceCrossCounter { get; set; }
        public double? OCPriceCrossPenaltyPoints { get; set; }
        public int? OCPriceCrossCounter { get; set; }
        public int? TotalCandles { get; set; }
        public double? AverageVariation { get; set; }
        public double? ExtremumVariation { get; set; }
        public double? OpenCloseVariation { get; set; }
        public double? BaseHitValue { get; set; }
        public double? CounterHitValue { get; set; }
        public double? Value { get; set; }
        public ITrendRangeDelimiter StartDelimiter { get; set; }
        public ITrendRangeDelimiter EndDelimiter { get; set; }


        public static TrendRange FromDto(TrendRangeDto dto)
        {
            var trendRange = new TrendRange
            {
                TrendRangeId = dto.TrendRangeId,
                TrendlineId = dto.TrendlineId,
                BaseId = dto.BaseId,
                BaseIsHit = dto.BaseIsHit,
                BaseDateIndex = dto.BaseDateIndex,
                CounterId = dto.CounterId,
                CounterIsHit = dto.CounterIsHit,
                CounterDateIndex = dto.CounterDateIndex,
                IsPeak = dto.IsPeak,
                ExtremumPriceCrossPenaltyPoints = dto.ExtremumPriceCrossPenaltyPoints,
                ExtremumPriceCrossCounter = dto.ExtremumPriceCrossCounter,
                OCPriceCrossPenaltyPoints = dto.OCPriceCrossPenaltyPoints,
                OCPriceCrossCounter = dto.OCPriceCrossCounter,
                TotalCandles = dto.TotalCandles,
                AverageVariation = dto.AverageVariation,
                ExtremumVariation = dto.ExtremumVariation,
                OpenCloseVariation = dto.OpenCloseVariation,
                BaseHitValue = dto.BaseHitValue,
                CounterHitValue = dto.CounterHitValue,
                Value = dto.Value
            };
            return trendRange;
        }

    }
}
