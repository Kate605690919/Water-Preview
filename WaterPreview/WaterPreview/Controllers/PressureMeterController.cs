using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
using WaterPreview.Redis;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;

namespace WaterPreview.Controllers
{
    public class PressureMeterController:BaseController
    {

        static IPressureMeterService pressuremeter_service;
        static IPressureHourService pressurehour_service;
        static IPressureMonthService pressuremonth_service;


        public PressureMeterController(IPressureMeterService pmservice, IPressureHourService phourservice,IPressureMonthService pmonthservice)
        {
            this.AddDisposableObject(pmservice);
            pressuremeter_service = pmservice;

            this.AddDisposableObject(phourservice);
            pressurehour_service = phourservice;

            this.AddDisposableObject(pmonthservice);
            pressuremonth_service = pmonthservice;
        }

        /// <summary>
        /// 压力计列表数据
        /// </summary>
        /// <param name="pmuid"></param>
        /// <returns></returns>
        public JsonResult Detail(Guid pmuid)
        {
            JsonResult result = new JsonResult();
            Func<List<PressureMeterStatusAndArea>> pmAndStatusArea = () => (pressuremeter_service.GetPressureMeterStatusAndArea());
            result.Data = DBHelper.get<PressureMeterStatusAndArea>(pmAndStatusArea, ConfigurationManager.AppSettings["allPressureMeterStatusAndArea"]).Where(p => p.pressuremeter.PM_UId == pmuid).ToList();
            return result;
        }

        /// <summary>
        /// 获取压力计数据详情列表
        /// </summary>
        /// <param name="pmUid"></param>
        /// <returns></returns>
        public JsonResult GetPressureDetail(Guid pmUid)
        {
            JsonResult result = new JsonResult();
            result.Data = pressurehour_service.GetPressureHourByUid(pmUid);
            return result;
        }

        /// <summary>
        /// 热力图数据
        /// </summary>
        /// <param name="pmuid"></param>
        /// <returns></returns>
        public JsonResult RecentPressureData(Guid pmuid)
        {
            JsonResult result = new JsonResult();
            var phlist = pressurehour_service.GetPressureHourByUid(pmuid);
            result.Data = new
            {
                value = phlist.Select(p => p.PH_AverageValue),
                time = phlist.Select(p => p.PH_Time)
            };
            return result;
        }

        /// <summary>
        /// 水压分析数据
        /// </summary>
        /// <param name="pmuid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public JsonResult PressureAnalysis(Guid pmuid, DateTime time)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            PressureMeter_t pm = pressuremeter_service.GetAllPressureMeter().Where(p=>p.PM_UId==pmuid).FirstOrDefault();
            result.Data = pressuremeter_service.GetAnalysisByPressureMeter(pm,time);
            return result;
        }

 
        /// <summary>
        /// 输出经常访问的水压计分析数据
        /// </summary>
        /// <param name="pmUids"></param>
        /// <returns></returns>
        public JsonResult GetMostVisitsPressureMeter(string[] pmUids)
        {
            JsonResult result = new JsonResult();
            User_t account = UserContext.GetCurrentAccount();
            List<PressureMeterData> pmdatalist = new List<PressureMeterData>();
            if (pmUids.Length == 0&&account.Usr_Type!=3)
            {
                List<PressureMeter_t> pmlist = pressuremeter_service.GetAllPressureMeter().Take(2).ToList();
                for (int i = 0; i < 2; i++)
                {
                    PressureMeter_t pm = pressuremeter_service.GetAllPressureMeter().Where(p => p.PM_UId == pmlist[i].PM_UId).FirstOrDefault();
                    var pmdata = pressuremeter_service.GetAnalysisByPressureMeter(pm, (DateTime)pm.PM_CountLast);
                    pmdatalist.Add(pmdata);
                }
            }
            else if (pmUids.Length!=0)
            {
                for (int i = 0; i < pmUids.Length; i++)
                {
                    PressureMeter_t pm = pressuremeter_service.GetAllPressureMeter().Where(p => p.PM_UId == Guid.Parse(pmUids[i])).FirstOrDefault();
                    var pmdata = pressuremeter_service.GetAnalysisByPressureMeter(pm, (DateTime)pm.PM_CountLast);
                    pmdatalist.Add(pmdata);
                }
            }
            string dataresult = ToJson<List<PressureMeterData>>.Obj2Json<List<PressureMeterData>>(pmdatalist).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");
            result.Data = dataresult;
            return result;
        }

        /// <summary>
        /// 输出昨日水压变化幅度从大到小排行的水压计列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLastDayPressureList()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            List<PressureMeterData> fmdatalist = new List<PressureMeterData>();
            User_t account = new User_t();
            if (account.Usr_Type == 3)
            {
                //List<FlowMeter_t> fmlist_customer = pressuremeter_service.GetAllPressureMeter();
                //if (fmlist_customer.Count > 0)
                //{
                //    fmdatalist = GetAnalysisData(fmlist_customer).OrderByDescending(p => p.lastday_flow_proportion).Take(3).ToList();
                //    //昨日流量趋势占比降序排列，前三
                //}

            }
            else
            {
                List<PressureMeter_t> pmlist = pressuremeter_service.GetAllPressureMeter();
                List<PressureMeterData> pmdata = GetAnalysisData(pmlist);
                fmdatalist = pmdata.OrderByDescending(p => p.lastday_pressure_proportion).Take(3).ToList();
            }

            string dataresult = ToJson<List<PressureMeterData>>.Obj2Json<List<PressureMeterData>>(fmdatalist).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }

        public List<PressureMeterData> GetAnalysisData(List<PressureMeter_t> pmlist)
        {
            List<PressureMeterData> pmdatalist = new List<PressureMeterData>();

            foreach (var item in pmlist)
            {
                PressureMeterData pmdata = pressuremeter_service.GetAnalysisByPressureMeter(item, (DateTime)item.PM_CountLast);
                //暂时先用数据库中设备最新的时间来获取对应的分析数据,后续将时间调整为实时的日期
                pmdatalist.Add(pmdata);
            }
            return pmdatalist;
        }
    }
}