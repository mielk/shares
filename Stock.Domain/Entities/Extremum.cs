using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.DAL.TransferObjects;
using Stock.Domain.Services;
using Stock.Utils;

namespace Stock.Domain.Entities
{
    public class Extremum
    {
        public int ExtremumId { get; set; }
        public int AssetId { get; set; }
        public int TimeframeId { get; set; }
        public int DateIndex { get; set; }
        public int ExtremumTypeId { get; set; }
        public double? Value { get; set; }
        public bool IsEvaluationOpen { get; set; }
        public int? EarlierCounter { get; set; }
        public double? EarlierAmplitude { get; set; }
        public double? EarlierTotalArea { get; set; }
        public double? EarlierAverageArea { get; set; }
        public double? EarlierChange1 { get; set; }
        public double? EarlierChange2 { get; set; }
        public double? EarlierChange3 { get; set; }
        public double? EarlierChange5 { get; set; }
        public double? EarlierChange10 { get; set; }
        public int? LaterCounter { get; set; }
        public double? LaterAmplitude { get; set; }
        public double? LaterTotalArea { get; set; }
        public double? LaterAverageArea { get; set; }
        public double? LaterChange1 { get; set; }
        public double? LaterChange2 { get; set; }
        public double? LaterChange3 { get; set; }
        public double? LaterChange5 { get; set; }
        public double? LaterChange10 { get; set; }




        public static Extremum FromDto(ExtremumDto dto)
        {
            var extremum = new Extremum
            {
                ExtremumId = dto.ExtremumId,
                DateIndex = dto.DateIndex,
                AssetId = dto.AssetId,
                TimeframeId = dto.TimeframeId,
                ExtremumTypeId = dto.ExtremumTypeId,
                Value = dto.Value,
                IsEvaluationOpen = dto.IsEvaluationOpen,
                EarlierCounter = dto.EarlierCounter,
                EarlierAmplitude = dto.EarlierAmplitude,
                EarlierTotalArea = dto.EarlierTotalArea,
                EarlierAverageArea = dto.EarlierAverageArea,
                EarlierChange1 = dto.EarlierChange1,
                EarlierChange2 = dto.EarlierChange2,
                EarlierChange3 = dto.EarlierChange3,
                EarlierChange5 = dto.EarlierChange5,
                EarlierChange10 = dto.EarlierChange10,
                LaterCounter = dto.LaterCounter,
                LaterAmplitude = dto.LaterAmplitude,
                LaterTotalArea = dto.LaterTotalArea,
                LaterAverageArea = dto.LaterAverageArea,
                LaterChange1 = dto.LaterChange1,
                LaterChange2 = dto.LaterChange2,
                LaterChange3 = dto.LaterChange3,
                LaterChange5 = dto.LaterChange5,
                LaterChange10 = dto.LaterChange10
            };
            return extremum;
        }

    }

}