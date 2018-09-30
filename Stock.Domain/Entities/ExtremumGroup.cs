using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Entities
{
    public class ExtremumGroup
    {
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


        public static ExtremumGroup FromDto(ExtremumGroupDto dto)
        {
            var extremumGroup = new ExtremumGroup
            {
                ExtremumGroupId = dto.ExtremumGroupId,
                AssetId = dto.AssetId,
                TimeframeId = dto.TimeframeId,
                IsPeak = dto.IsPeak,
                MasterExtremumId = dto.MasterExtremumId,
                MasterDateIndex = dto.MasterDateIndex,
                SlaveExtremumId = dto.SlaveExtremumId,
                SlaveDateIndex = dto.SlaveDateIndex,
                StartDateIndex = dto.StartDateIndex,
                EndDateIndex = dto.EndDateIndex,
                OCPriceLevel = dto.OCPriceLevel,
                ExtremumPriceLevel = dto.ExtremumPriceLevel,
                MiddlePriceLevel = dto.MiddlePriceLevel
            };
            return extremumGroup;
        }

    }
}
