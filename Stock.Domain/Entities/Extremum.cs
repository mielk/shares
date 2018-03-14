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
        public double Value { get; set; }

        public static Extremum FromDto(ExtremumDto dto)
        {
            var extremum = new Extremum
            {
                ExtremumId = dto.ExtremumId,
                DateIndex = dto.DateIndex,
                AssetId = dto.AssetId,
                TimeframeId = dto.TimeframeId,
                ExtremumTypeId = dto.ExtremumTypeId,
                Value = dto.Value
            };
            return extremum;
        }

    }

}