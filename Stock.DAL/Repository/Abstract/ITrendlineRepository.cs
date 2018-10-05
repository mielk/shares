using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.DAL.TransferObjects;

namespace Stock.DAL.Repositories
{
    public interface ITrendlineRepository
    {
        IEnumerable<TrendlineDto> GetTrendlines(int assetId, int timeframeId);
        IEnumerable<TrendlineDto> GetVisibleTrendlines(int assetId, int timeframeId);
        TrendlineDto GetTrendlineById(int id);

        IEnumerable<TrendHitDto> GetTrendHits();
        IEnumerable<TrendBreakDto> GetTrendBreaks();
        IEnumerable<TrendRangeDto> GetTrendRanges();
        IEnumerable<ExtremumGroupDto> GetExtremumGroups(int assetId, int timeframeId);
    }
}
