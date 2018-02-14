using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.DAL.Repositories;
using Stock.DAL.TransferObjects;

namespace Stock.Domain.Services
{
    public class DataSetService : IDataSetService
    {

        private IQuotationRepository _repository;

        public IEnumerable<DataSet> GetDataSets(int assetId, int timeframeId)
        {
            _repository = new EFQuotationRepository();
            var quotationDtos = _repository.GetQuotations(assetId, timeframeId).OrderBy(q => q.DateIndex);
            var extremumDtos = _repository.GetExtrema(assetId, timeframeId);
            var result = new List<DataSet>();

            foreach (var dto in quotationDtos)
            {
                var ds = new DataSet(assetId, timeframeId, dto.Date, dto.DateIndex);
                var quotation = Quotation.FromDto(dto);
                ds.quotation = quotation;
                ds.price = new Price();
                result.Add(ds);
            }

            foreach (var dto in extremumDtos)
            {
                var extremum = Extremum.FromDto(dto);
                var ds = result.SingleOrDefault(d => d.DateIndex == extremum.DateIndex && d.TimeframeId == extremum.TimeframeId);
                if (ds != null && ds.price != null)
                {
                    ds.price.SetExtremum(extremum);
                }
            }

            return result;

        }

        public AnalysisInfo GetAnalysisInfo(int assetId, int timeframeId)
        {
            _repository = new EFQuotationRepository();
            AnalysisInfoDto dto = _repository.GetAnalysisInfo(assetId, timeframeId);
            return AnalysisInfo.FromDto(dto);
        }

    }
}
