using Stock.DAL.Infrastructure;
using Stock.DAL.Repositories;
using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DAL.Repositories
{

    public class EFTrendlineRepository : ITrendlineRepository
    {

        public IEnumerable<TrendlineDto> GetTrendlines(int assetId, int timeframeId)
        {
            IEnumerable<TrendlineDto> trendlines;
            using (var context = new EFDbContext())
            {
                trendlines = context.Trendlines.Where(t => t.AssetId == assetId && t.TimeframeId == timeframeId).ToList();
            }
            return trendlines;
        }

        public TrendlineDto GetTrendlineById(int id)
        {
            using (var context = new EFDbContext())
            {
                return context.Trendlines.SingleOrDefault(t => t.Id == id);
            }
        }

    }

}