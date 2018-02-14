﻿using System;
using System.Collections.Generic;
using Stock.Domain.Entities;

namespace Stock.Domain.Services
{
    public interface ITrendlineService
    {
        IEnumerable<Trendline> GetTrendlines(int assetId, int timeframeId);
        Trendline GetTrendlineById(int id);
    }
}
