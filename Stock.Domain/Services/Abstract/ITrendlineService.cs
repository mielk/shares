using System;
using System.Collections.Generic;
using Stock.Domain.Entities;

namespace Stock.Domain.Services
{
    public interface ITrendlineService
    {
        IEnumerable<ExtremumGroup> GetExtremumGroups(int assetId, int timeframeId);
        IEnumerable<Trendline> GetTrendlines(int assetId, int timeframeId);
        IEnumerable<Trendline> GetVisibleTrendlines(int assetId, int timeframeId);
        Trendline GetTrendlineById(int id);
    }
}
