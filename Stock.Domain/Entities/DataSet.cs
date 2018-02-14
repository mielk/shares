using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Entities
{
    public class DataSet
    {
        public int AssetId { get; set; }
        public int TimeframeId { get; set; }
        public DateTime Date { get; set; }
        public int DateIndex { get; set; }
        //Subitems.
        public Quotation quotation { get; set; }
        public Price price { get; set; }

        public DataSet(int assetId, int timeframeId, DateTime date, int dateIndex)
        {
            this.AssetId = assetId;
            this.TimeframeId = timeframeId;
            this.Date = date;
            this.DateIndex = dateIndex;
        }

    }
}
