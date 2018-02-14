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
        public int MasterExtremumDateIndex { get; set; }
        public double Value { get; set; }
    }
}