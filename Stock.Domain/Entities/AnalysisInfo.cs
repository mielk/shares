using Stock.DAL.TransferObjects;
using System;

namespace Stock.Domain.Entities
{
    public class AnalysisInfo
    {
        public int AssetId { get; set; }
        public int TimeframeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public double MinLevel { get; set; }
        public double MaxLevel { get; set; }
        public int Counter { get; set; }

        public static AnalysisInfo FromDto(AnalysisInfoDto dto)
        {
            var analysisInfo = new AnalysisInfo
            {
                AssetId = dto.AssetId,
                TimeframeId = dto.TimeframeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                StartIndex = dto.StartIndex,
                EndIndex = dto.EndIndex,
                MinLevel = (double) dto.MinLevel,
                MaxLevel = (double) dto.MaxLevel,
                Counter = dto.Counter
            };
            return analysisInfo;
        }

    }
}
