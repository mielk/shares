using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Entities
{
    public class TrendBreak
    {
        public int TrendBreakId { get; set; }
        public int TrendlineId { get; set; }
        public int DateIndex { get; set; }
        public int BreakFromAbove { get; set; }
        public double Value { get; set; }

        public static TrendBreak FromDto(TrendBreakDto dto)
        {
            var trendBreak = new TrendBreak
            {
                TrendBreakId = dto.TrendBreakId,
                TrendlineId = dto.TrendlineId,
                DateIndex = dto.DateIndex,
                BreakFromAbove = dto.BreakFromAbove,
                Value = dto.Value
            };
            return trendBreak;
        }

    }
}
