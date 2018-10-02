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

        public IEnumerable<TrendlineDto> GetVisibleTrendlines(int assetId, int timeframeId)
        {
            IEnumerable<TrendlineDto> trendlines;
            using (var context = new EFDbContext())
            {
                trendlines = context.Trendlines.Where(t => t.AssetId == assetId && t.TimeframeId == timeframeId && t.ShowOnChart == true).ToList();
            }
            return trendlines;
        }

        public TrendlineDto GetTrendlineById(int id)
        {
            using (var context = new EFDbContext())
            {
                return context.Trendlines.SingleOrDefault(t => t.TrendlineId == id);
            }
        }



        public IEnumerable<TrendHitDto> GetTrendHits()
        {
            IEnumerable<TrendHitDto> trendHits;
            using (var context = new EFDbContext())
            {
                trendHits = context.TrendHits.ToList();
            }
            return trendHits;
        }

        public IEnumerable<TrendBreakDto> GetTrendBreaks()
        {
            IEnumerable<TrendBreakDto> trendBreaks;
            using (var context = new EFDbContext())
            {
                trendBreaks = context.TrendBreaks.ToList();
            }
            return trendBreaks;
        }

        public IEnumerable<TrendRangeDto> GetTrendRanges()
        {
            IEnumerable<TrendRangeDto> trendRanges;
            using (var context = new EFDbContext())
            {
                trendRanges = context.TrendRanges.ToList();
            }
            return trendRanges;
        }

        public IEnumerable<ExtremumGroupDto> GetExtremumGroups()
        {
            IEnumerable<ExtremumGroupDto> extremumGroups;
            using (var context = new EFDbContext())
            {
                extremumGroups = context.ExtremumGroups.ToList();
            }
            return extremumGroups;
        }

    }

}