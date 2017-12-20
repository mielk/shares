using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Utils;

namespace Stock.Domain.Entities
{
    public class Quotation
    {
        public int Id { get; set; }
        public int ShareId { get; set; }
        public DateTime Date { get; set; }
        public int DateIndex { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }

        public static Quotation FromDto(QuotationDto dto)
        {
            var quotation = new Quotation
            {
                Id = dto.Id,
                ShareId = dto.ShareId,
                Date = dto.Date,
                DateIndex = dto.DateIndex,
                Open = dto.Open,
                High = dto.High,
                Low = dto.Low,
                Close = dto.Close
            };
            return quotation;
        }

    }
}
