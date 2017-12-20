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
        public ActionResult GetDataSetsInfo(int shareId)
        {
            IDataSetService dataSetService = new DataSetService();
            AnalysisInfo info = dataSetService.GetAnalysisInfo(shareId);
            var json = new { info = info };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetDataSets(int shareId)
        {
            IDataSetService dataSetService = new DataSetService();
            ITrendlineService trendlineService = new TrendlineService();

            IEnumerable<DataSet> dataSets = dataSetService.GetDataSets(shareId);
            IEnumerable<Trendline> trendlines = trendlineService.GetTrendlines(shareId);
            var json = new { quotations = dataSets, trendlines = trendlines };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
