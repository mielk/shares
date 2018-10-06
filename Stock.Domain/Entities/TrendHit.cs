using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Entities
{
    public class TrendHit : ITrendRangeDelimiter
    {
        public int TrendHitId { get; set; }
        public int TrendlineId { get; set; }
        public int ExtremumGroupId { get; set; }
        public double? Value { get; set; }
        public double? PointsForDistance { get; set; }
        public double? PointsForValue { get; set; }
        public double? Gap { get; set; }
        public double? RelativeGap { get; set; }
        public ExtremumGroup ExtremumGroup { get; set; }

        public static TrendHit FromDto(TrendHitDto dto)
        {
            var trendHit = new TrendHit
            {
                TrendHitId = dto.TrendHitId,
                TrendlineId = dto.TrendlineId,
                ExtremumGroupId = dto.ExtremumGroupId,
                Value = dto.Value,
                PointsForDistance = dto.PointsForDistance,
                PointsForValue = dto.PointsForValue,
                Gap = dto.Gap,
                RelativeGap = dto.RelativeGap
            };
            return trendHit;
        }

    }
}