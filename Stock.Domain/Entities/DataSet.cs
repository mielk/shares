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
        public int ShareId { get; set; }
        public DateTime Date { get; set; }
        public int DateIndex { get; set; }
        //Subitems.
        public Quotation quotation { get; set; }
        public Price price { get; set; }

        public DataSet(int shareId, DateTime date, int dateIndex)
        {
            this.ShareId = shareId;
            this.Date = date;
            this.DateIndex = dateIndex;
        }

    }
}
