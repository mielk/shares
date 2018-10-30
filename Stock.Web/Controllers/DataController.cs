using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stock.Domain.Services;
using Stock.Domain.Entities;

namespace Stock.Web.Controllers
{
    public class DataController : Controller
    {

        private const int SHARE_ID = 1;

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetDataSetsInfo(int assetId, int timeframeId)
        {
            IDataSetService dataSetService = new DataSetService();
            AnalysisInfo info = dataSetService.GetAnalysisInfo(assetId, timeframeId);
            var json = new { info = info };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetDataSets(int assetId, int timeframeId)
        {
            IDataSetService dataSetService = new DataSetService();
            ITrendlineService trendlineService = new TrendlineService();

            IEnumerable<DataSet> dataSets = dataSetService.GetDataSets(assetId, timeframeId);
            IEnumerable<ExtremumGroup> extremumGroups = trendlineService.GetExtremumGroups(assetId, timeframeId);
            IEnumerable<Trendline> trendlines = trendlineService.GetVisibleTrendlines(assetId, timeframeId);
            var json = new { quotations = dataSets, extremumGroups = extremumGroups, trendlines = trendlines };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

    }

}
