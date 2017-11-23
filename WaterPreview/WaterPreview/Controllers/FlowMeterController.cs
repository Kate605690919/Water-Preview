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
    public class FlowMeterController:BaseController
    {
        private static IFlowMeterService flowmeter_Service;
        private static IFlowMonthService flowmonth_Service;
        private static IFlowHourService flowhour_Service;



        public FlowMeterController(IFlowMeterService fmservice,IFlowMonthService fmonthservice,IFlowHourService fhourservice)
        {
            this.AddDisposableObject(fmservice);
            flowmeter_Service = fmservice;

            this.AddDisposableObject(fmonthservice);
            flowmonth_Service = fmonthservice;

            this.AddDisposableObject(fhourservice);
            flowhour_Service = fhourservice;
        }

        /// <summary>
        /// 输出流量计Uid和对应time的各个流量分析数值
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public JsonResult Analysis(Guid uid, DateTime time)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            User_t account = UserContext.GetCurrentAccount();
            if (account.Usr_Type == 3)
            {
                Func<List<FlowMeterData>> fmdataFunc = () => flowmeter_Service.GetFlowMetersDataByUserUid(account);
                var fmdataanalysis = DBHelper.get<FlowMeterData>(fmdataFunc, UserContext.allFlowAnalysisByUserUid + account.Usr_UId);
                result.Data = fmdataanalysis.Where(p => p.flowmeter.FM_UId == uid).FirstOrDefault();

            }
            else
            {
                FlowMeter_t fm = flowmeter_Service.GetAllFlowMeter().Where(p => p.FM_UId == uid).FirstOrDefault();
                var fmanalysis = flowmeter_Service.GetAnalysisByFlowMeter(fm);
                result.Data = fmanalysis;
            }
            

            return result;
        }


        //public IEnumerable<FlowHour_t> GetdayFlowByUidAndDate(Guid uid, DateTime date)
        //{
        //    List<FlowHour_t> OneDay = new List<FlowHour_t>();
        //    int year = date.Year;
        //    int month = date.Month;
        //    //int day = new DateTime(year, month, 1).AddDays(-1).Day;

        //    List<FlowHour_t> result = new List<FlowHour_t>();
        //    var time = int.Parse(date.ToString("yyyyMMdd"));
        //    var lastmonth = date.AddMonths(-1).AddDays(1);
        //    var days = date.Subtract(lastmonth).Days;

        //    //var starttime = int.Parse(lastmonth.ToString("yyyyMMdd"));

        //    dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();

        //    List<FlowHour_t> fhlist = db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == uid).ToList();
        //    for (var i = 0; i <= days - 1; i++)
        //    {
        //        //resultt.AddRange(db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == uid && p.Flh_Time >= (time * 100 + 9) && p.Flh_Time <= ((time + 1) * 100 + 9)).OrderBy(p => p.Flh_Time).ToList().Where(p => p.Flh_Time % 100 >= 2 && p.Flh_Time % 100 <= 4));
        //        var day = int.Parse(lastmonth.AddDays(i).ToString("yyyyMMdd"));
        //        var secday = int.Parse(lastmonth.AddDays(i + 1).ToString("yyyyMMdd"));
        //        List<FlowHour_t> fhPerHour = fhlist.Where(p => p.Flh_Time >= (day * 100 + 9) && p.Flh_Time <= (secday * 100 + 9)).ToList();
        //        result.AddRange(fhPerHour);

        //    }
        //    return result;

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public JsonResult Detail(Guid uid)
        {
            JsonResult result = new JsonResult();
            //Func<List<FlowMeter_t>> t = ()=>flowmeter_Service.GetAllFlowMeter().Where(p=>p.FM_UId==uid).ToList();

            Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => (flowmeter_Service.GetFlowMeterStatusAndArea());
            result.Data = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea, ConfigurationManager.AppSettings["allFlowMeterStatusAndArea"]).Where(p => p.flowmeter.FM_UId == uid).ToList();
            //result.Data = DBHelper.get<FlowMeter_t>(t,UserContext.allFlowMeter);
            return result;
        }


        /// <summary>
        /// 热力图数据
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public JsonResult RecentFlowData(Guid uid)
        {

            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
            var data = db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == uid).OrderByDescending(p => p.Flh_Time).Take(500).ToList();
            result.Data = new { value = data.Select(p => p.Flh_TotalValue), time = data.Select(p => p.Flh_Time) };
            return result;
        }

        public JsonResult currentData(Guid uid, DateTime startDt, DateTime endDt)
        {
            TimeSpan ts = endDt - startDt;
            JsonResult rs = new JsonResult();
            rs.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();

            if (ts.TotalDays < 1)
            {
                rs.Data = db.Flow_t.Where(p => p.Flw_FlowMeterUId == uid && p.Flw_CreateDt > startDt && p.Flw_CreateDt < endDt).OrderBy(p => p.Flw_CreateDt).ToList().Select(p => new
                {
                    value = p.Flw_TotalValue,
                    time = p.Flw_CreateDt.ToString("yyyyMMddHHmm")
                });
            }
            else if (ts.TotalDays < 5)
            {
                int start = int.Parse(startDt.ToString("yyyyMMddHH"));
                int end = int.Parse(endDt.ToString("yyyyMMddHH"));
                rs.Data = db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == uid && p.Flh_Time > start && p.Flh_Time < end).OrderBy(p => p.Flh_Time).ToList().Select(p => new
                {
                    value = p.Flh_TotalValue,
                    time = p.Flh_Time
                });

            }
            else
            {
                int start = int.Parse(startDt.ToString("yyyyMMdd"));
                int end = int.Parse(endDt.ToString("yyyyMMdd"));
                rs.Data = db.FlowDay_t.Where(p => p.Fld_FlowMeterUid == uid && p.Fld_Time > start && p.Fld_Time < end).OrderBy(p => p.Fld_Time).Select(p => new
                {
                    value = p.Fld_TotalValue,
                    time = p.Fld_Time
                });
            }
            return rs;
        }

        /// <summary>
        /// 获取经常访问的流量计分析数据
        /// </summary>
        /// <param name="fmUids"></param>
        /// <returns></returns>
        public JsonResult GetMostVisitsFlowMeter(string[] fmUids)
        {
            JsonResult result = new JsonResult();

            List<FlowMeterData> fmdatalist = new List<FlowMeterData>();
            for (var i = 0; i < fmUids.Length; i++)
            {
                FlowMeter_t fm = flowmeter_Service.GetAllFlowMeter().Where(p=>p.FM_UId==Guid.Parse(fmUids[i])).FirstOrDefault();
                var fmdata = flowmeter_Service.GetAnalysisByFlowMeter(fm);
                fmdatalist.Add(fmdata);
            }
            string dataresult = ToJson<List<FlowMeterData>>.Obj2Json<List<FlowMeterData>>(fmdatalist).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
                //flowmeter_Service
            return result;
        }
    }
}