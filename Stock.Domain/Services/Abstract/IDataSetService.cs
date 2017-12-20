using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using Stock.DAL.Repositories;

namespace Stock.Domain.Services
{
    public interface IDataSetService
    {
        IEnumerable<DataSet> GetDataSets(int shareId);
        AnalysisInfo GetAnalysisInfo(int shareId);
    }
}