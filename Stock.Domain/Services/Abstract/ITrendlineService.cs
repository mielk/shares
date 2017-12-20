using System;
using System.Collections.Generic;
using Stock.Domain.Entities;

namespace Stock.Domain.Services
{
    public interface ITrendlineService
    {
        IEnumerable<Trendline> GetTrendlines(int shareId);
        Trendline GetTrendlineById(int id);
    }
}
