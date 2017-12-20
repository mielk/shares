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
        public int Id { get; set; }
        public int ShareId { get; set; }
        public int DateIndex { get; set; }
        public int ExtremumType { get; set; }
        public double Value { get; set; }

        public static Extremum FromDto(ExtremumDto dto)
        {
            var extremum = new Extremum
            {
                Id = dto.Id,
                DateIndex = dto.DateIndex,
                ShareId = dto.ShareId,
                ExtremumType = dto.ExtremumType,
                Value = dto.Value
            };
            return extremum;
        }

    }

}