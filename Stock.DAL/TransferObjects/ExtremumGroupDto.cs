using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DAL.TransferObjects
{
    public class ExtremumGroupDto
    {
        [Key]
        public int ExtremumGroupId { get; set; }
        public int AssetId { get; set; }
        public int TimeframeId { get; set; }
        public int IsPeak { get; set; }
        public int MasterExtremumId { get; set; }
        public int MasterDateIndex { get; set; }
        public int SlaveExtremumId { get; set; }
        public int SlaveDateIndex { get; set; }
        public int StartDateIndex { get; set; }
        public int EndDateIndex { get; set; }
        public double OCPriceLevel { get; set; }
        public double ExtremumPriceLevel { get; set; }
        public double MiddlePriceLevel { get; set; }
    }
}
