using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.DAL.TransferObjects;
using Stock.DAL.Infrastructure;
using Stock.Utils;

namespace Stock.DAL.Repositories
{
    public class EFQuotationRepository : IQuotationRepository
    {

        public AnalysisInfoDto GetAnalysisInfo(int shareId)
        {
            using (var context = new EFDbContext())
            {
                return context.AnalysisInfos.SingleOrDefault(ai => ai.ShareId == shareId);
            }
        }

        public IEnumerable<QuotationDto> GetQuotations(int shareId)
        {
            IEnumerable<QuotationDto> results;
            using (var context = new EFDbContext())
            {

                results = context.Quotations.Where(q => q.ShareId == shareId).ToList();
            }
            return results;
        }

        public IEnumerable<ExtremumDto> GetExtrema(int shareId)
        {
            IEnumerable<ExtremumDto> results;
            using (var context = new EFDbContext())
            {

                results = context.Extrema.Where(e => e.ShareId == shareId).ToList();
            }
            return results;
        }

    }
}