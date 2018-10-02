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

        public IEnumerable<Trendline> GetVisibleTrendlines(int assetId, int timeframeId)
        {
            _repository = new EFTrendlineRepository();
            var dtos = _repository.GetVisibleTrendlines(assetId, timeframeId);
            return GetTrendlines(dtos);
        }

        private IEnumerable<Trendline> GetTrendlines(IEnumerable<TrendlineDto> dtos)
        {
            IEnumerable<Trendline> trendlines = dtos.Select(dto => Trendline.FromDto(dto));
            IEnumerable<TrendRange> trendRanges = GetTrendRanges();
            Dictionary<int, ExtremumGroup> extremumGroupsMap = GetExtremumGroupsMap();
            Dictionary<int, Trendline> trendlinesMap = new Dictionary<int, Trendline>();

            foreach (var trendline in trendlines)
            {
                ExtremumGroup baseEG;
                ExtremumGroup counterEG;
                extremumGroupsMap.TryGetValue(trendline.BaseExtremumGroupId, out baseEG);
                extremumGroupsMap.TryGetValue(trendline.CounterExtremumGroupId, out counterEG);
                trendline.BaseExtremumGroup = baseEG;
                trendline.CounterExtremumGroup = counterEG;

                trendlinesMap.Add(trendline.Id, trendline);

            }

            foreach (var trendRange in trendRanges)
            {
                Trendline trendline;
                trendlinesMap.TryGetValue(trendRange.TrendlineId, out trendline);
                if (trendline != null)
                {
                    trendline.AddTrendRange(trendRange);
                }
            }

            return trendlinesMap.Values;

        }


        private IEnumerable<TrendRange> GetTrendRanges()
        {
            _repository = new EFTrendlineRepository();
            IEnumerable<TrendRangeDto> dtos = _repository.GetTrendRanges();
            IEnumerable<TrendRange> trendRanges = dtos.Select(tr => TrendRange.FromDto(tr));
            Dictionary<int, TrendHit> trendHits = GetTrendHitsMap();
            Dictionary<int, TrendBreak> trendBreaks = GetTrendBreaksMap();
            List<TrendRange> result = new List<TrendRange>();

            foreach (var trendRange in trendRanges)
            {
                TrendHit th;
                TrendBreak tb;

                if (trendRange.BaseIsHit == 1)
                {
                    trendHits.TryGetValue(trendRange.BaseId, out th);
                    trendRange.StartDelimiter = th;
                }
                else
                {
                    trendBreaks.TryGetValue(trendRange.BaseId, out tb);
                    trendRange.StartDelimiter = tb;
                }


                if (trendRange.CounterIsHit == 1)
                {
                    trendHits.TryGetValue(trendRange.CounterId, out th);
                    trendRange.EndDelimiter = th;
                }
                else
                {
                    trendBreaks.TryGetValue(trendRange.CounterId, out tb);
                    trendRange.EndDelimiter = tb;
                }

                result.Add(trendRange);

            }

            return result;

        }

        private Dictionary<int, TrendHit> GetTrendHitsMap()
        {
            _repository = new EFTrendlineRepository();
            IEnumerable<TrendHitDto> dtos = _repository.GetTrendHits();
            IEnumerable<TrendHit> trendHits = dtos.Select(th => TrendHit.FromDto(th));
            Dictionary<int, TrendHit> trendHitsMap = new Dictionary<int, TrendHit>();
            
            Dictionary<int, ExtremumGroup> extremumGroupsMap = GetExtremumGroupsMap();
            foreach (var trendHit in trendHits)
            {
                ExtremumGroup eg = null;
                extremumGroupsMap.TryGetValue(trendHit.ExtremumGroupId, out eg);
                if (eg != null)
                {
                    trendHit.ExtremumGroup = eg;
                }
                trendHitsMap.Add(trendHit.TrendHitId, trendHit);
            }

            return trendHitsMap;

        }

        private Dictionary<int, TrendBreak> GetTrendBreaksMap()
        {
            _repository = new EFTrendlineRepository();
            IEnumerable<TrendBreakDto> dtos = _repository.GetTrendBreaks();

            Dictionary<int, TrendBreak> trendBreaksMap = new Dictionary<int, TrendBreak>();
            foreach (var dto in dtos)
            {
                var trendBreak = TrendBreak.FromDto(dto);
                trendBreaksMap.Add(trendBreak.TrendBreakId, trendBreak);
            }
            return trendBreaksMap;

        }

        private Dictionary<int, ExtremumGroup> GetExtremumGroupsMap()
        {
            _repository = new EFTrendlineRepository();
            Dictionary<int, ExtremumGroup> map = new Dictionary<int, ExtremumGroup>();
            foreach (var dto in _repository.GetExtremumGroups())
            {
                var eg = ExtremumGroup.FromDto(dto);
                map.Add(eg.ExtremumGroupId, eg);
            }
            return map;
        }

        public Trendline GetTrendlineById(int id)
        {
            _repository = new EFTrendlineRepository();
            var dto = _repository.GetTrendlineById(id);
            return Trendline.FromDto(dto);
        }

    }

}
