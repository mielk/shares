using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DAL.Repositories
{
    public interface IQuotationRepository
    {
        AnalysisInfoDto GetAnalysisInfo(int assetId, int timeframeId);
        IEnumerable<QuotationDto> GetQuotations(int assetId, int timeframeId);
        IEnumerable<ExtremumDto> GetExtrema(int assetId, int timeframeId);
    }
}
