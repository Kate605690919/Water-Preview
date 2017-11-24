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
        private static IFlowDayService flowday_Service;



        public FlowMeterController(IFlowMeterService fmservice,IFlowMonthService fmonthservice,IFlowHourService fhourservice,IFlowDayService fdayservice)
        {
            this.AddDisposableObject(fmservice);
            flowmeter_Service = fmservice;

            this.AddDisposableObject(fmonthservice);
            flowmonth_Service = fmonthservice;

            this.AddDisposableObject(fhourservice);
            flowhour_Service = fhourservice;

            this.AddDisposableObject(fdayservice);
            flowday_Service = fdayservice;
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
                var fmanalysis = flowmeter_Service.GetAnalysisByFlowMeter(fm,time);
                result.Data = fmanalysis;
            }
            

            return result;
        }


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
        /// 输出经常访问的流量计分析数据
        /// </summary>
        /// <param name="fmUids"></param>
        /// <returns></returns>
        public JsonResult GetMostVisitsFlowMeter(string[] fmUids)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<FlowMeterData> fmdatalist = new List<FlowMeterData>();
            User_t account = UserContext.GetCurrentAccount();
            if (fmUids.Length >= 3)
            {
                for (var i = 0; i < fmUids.Length; i++)
                {
                    FlowMeter_t fm = flowmeter_Service.GetAllFlowMeter().Where(p => p.FM_UId == Guid.Parse(fmUids[i])).FirstOrDefault();
                    var fmdata = flowmeter_Service.GetAnalysisByFlowMeter(fm, (DateTime)fm.FM_FlowCountLast);
                    fmdatalist.Add(fmdata);
                }
            }
            else
            {
                if(account.Usr_Type==3){
                    List<FlowMeter_t> fmlist = flowmeter_Service.GetFlowMetersByUserUid(account.Usr_UId);
                    if (fmlist.Count>0&&fmlist.Count<3)
                    {
                        for (var i = 0; i < fmlist.Count; i++)
                        {
                            FlowMeter_t fm = flowmeter_Service.GetAllFlowMeter().Where(p => p.FM_UId == fmlist[i].FM_UId).FirstOrDefault();
                            var fmdata = flowmeter_Service.GetAnalysisByFlowMeter(fm, (DateTime)fm.FM_FlowCountLast);
                            fmdatalist.Add(fmdata);
                        }
                    }
                    else if (fmlist.Count > 3)
                    {
                        List<FlowMeter_t> new_fmlist = fmlist.Take(3).ToList();
                        for (var i = 0; i < new_fmlist.Count; i++)
                        {
                            FlowMeter_t fm = flowmeter_Service.GetAllFlowMeter().Where(p => p.FM_UId == new_fmlist[i].FM_UId).FirstOrDefault();
                            var fmdata = flowmeter_Service.GetAnalysisByFlowMeter(fm, (DateTime)fm.FM_FlowCountLast);
                            fmdatalist.Add(fmdata);
                        }
                    }
                }
                else
                {
                    List<FlowMeter_t> fmlist = flowmeter_Service.GetAllFlowMeter().Take(3).ToList();
                    for (var i = 0; i < fmlist.Count; i++)
                    {
                        FlowMeter_t fm = flowmeter_Service.GetAllFlowMeter().Where(p => p.FM_UId == fmlist[i].FM_UId).FirstOrDefault();
                        var fmdata = flowmeter_Service.GetAnalysisByFlowMeter(fm, (DateTime)fm.FM_FlowCountLast);
                        fmdatalist.Add(fmdata);
                    }
                }
            }
            
            string dataresult = ToJson<List<FlowMeterData>>.Obj2Json<List<FlowMeterData>>(fmdatalist).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;               
            return result;
        }

        /// <summary>
        /// 输出昨日流量变化幅度从大到小排行的流量计列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLastDayFlowList()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            List<FlowMeterData> fmdatalist = new List<FlowMeterData>();
            User_t account = new User_t();
            if (account.Usr_Type == 3)
            {
                List<FlowMeter_t> fmlist_customer = flowmeter_Service.GetFlowMetersByUserUid(account.Usr_UId);
                if (fmlist_customer.Count > 0)
                {
                    fmdatalist = GetAnalysisData(fmlist_customer).OrderByDescending(p=>p.lastday_flow_proportion).Take(3).ToList();
                    //昨日流量趋势占比降序排列，前三
                }
            }
            else
            {
                List<FlowMeter_t> fmlist = flowmeter_Service.GetAllFlowMeter();
                fmdatalist = GetAnalysisData(fmlist).OrderByDescending(p => p.lastday_flow_proportion).Take(3).ToList();
            }
            
            string dataresult = ToJson<List<FlowMeterData>>.Obj2Json<List<FlowMeterData>>(fmdatalist).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;  
            return result;
        }

        public JsonResult GetAreaAvgFlow()
        {
            //List<FlowMeterData> fmdatalist = new List<FlowMeterData>();
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<AreaAnalysisData> areadatalist = new List<AreaAnalysisData>();
            User_t user = new User_t();
            if (user.Usr_Type == 3)
            {
                Guid arealist = UserContext.GetAreaByUserUid(user.Usr_UId);
                List<FlowMeter_t> fmlist = new List<FlowMeter_t>();
                foreach (var item in fmlist)
                {
                    int daytime = int.Parse(((DateTime)item.FM_FlowCountLast).ToString("yyyyMMdd"));
                    List<FlowDay_t> flowday = flowday_Service.GetAllFlowDayByFMUid(item.FM_UId).Where(p => p.Fld_Time == daytime).ToList();
                    if(flowday.Count!=0){
                        decimal areaAvg = (decimal)flowday.FirstOrDefault().Fld_TotalValue;
                    }
                    
                }
            }
            result.Data = areadatalist;

            return result;
        }

        public List<FlowMeterData> GetAnalysisData(List<FlowMeter_t> fmlist)
        {
            List<FlowMeterData> fmdatalist = new List<FlowMeterData>();

            foreach (var item in fmlist)
            {
                FlowMeterData fmdata = flowmeter_Service.GetAnalysisByFlowMeter(item, (DateTime)item.FM_FlowCountLast);
                //暂时先用数据库中设备最新的时间来获取对应的分析数据,后续将时间调整为实时的日期
                fmdatalist.Add(fmdata);
            }
            return fmdatalist;
        }

    }
}