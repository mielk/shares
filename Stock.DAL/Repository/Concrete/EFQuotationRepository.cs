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

        public AnalysisInfoDto GetAnalysisInfo(int assetId, int timeframeId)
        {
            using (var context = new EFDbContext())
            {
                return context.AnalysisInfos.SingleOrDefault(ai => ai.AssetId == assetId && ai.TimeframeId == timeframeId);
            }
        }

        public IEnumerable<QuotationDto> GetQuotations(int assetId, int timeframeId)
        {
            IEnumerable<QuotationDto> results;
            using (var context = new EFDbContext())
            {

                results = context.Quotations.Where(q => q.AssetId == assetId && q.TimeframeId == timeframeId).ToList();
            }
            return results;
        }

        public IEnumerable<ExtremumDto> GetExtrema(int assetId, int timeframeId)
        {
            IEnumerable<ExtremumDto> results;
            using (var context = new EFDbContext())
            {

                results = context.Extrema.Where(e => e.AssetId == assetId && e.TimeframeId == timeframeId).ToList();
            }
            return results;
        }

    }
}