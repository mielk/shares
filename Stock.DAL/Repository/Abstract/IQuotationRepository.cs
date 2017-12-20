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
        AnalysisInfoDto GetAnalysisInfo(int shareId);
        IEnumerable<QuotationDto> GetQuotations(int shareId);
        IEnumerable<ExtremumDto> GetExtrema(int shareId);
    }
}
