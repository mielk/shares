using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.DAL.TransferObjects
{
    public class ExtremumDto
    {
        [Key]
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
    }
}