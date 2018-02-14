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
    public class TrendlineService : ITrendlineService
    {

        private ITrendlineRepository _repository;
        


        public IEnumerable<Trendline> GetTrendlines(int assetId, int timeframeId)
        {
            _repository = new EFTrendlineRepository();
            var dtos = _repository.GetTrendlines(assetId, timeframeId);
            return GetTrendlines(dtos);
        }

        private IEnumerable<Trendline> GetTrendlines(IEnumerable<TrendlineDto> dtos)
        {
            List<Trendline> result = new List<Trendline>();
            foreach (var dto in dtos)
            {
                var trendline = Trendline.FromDto(dto);
                result.Add(trendline);
            }
            return result.Where(t => t.ShowOnChart).Take(100);
        }

        public Trendline GetTrendlineById(int id)
        {
            _repository = new EFTrendlineRepository();
            var dto = _repository.GetTrendlineById(id);
            return Trendline.FromDto(dto);
        }

    }
}
