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
        private static IAccountService account_Service;
        private static IFlowService flow_Service;



        public FlowMeterController(IFlowMeterService fmservice,IFlowMonthService fmonthservice,IFlowHourService fhourservice,IFlowDayService fdayservice,IAccountService accservice,IFlowService flowservice)
        {
            this.AddDisposableObject(fmservice);
            flowmeter_Service = fmservice;

            this.AddDisposableObject(fmonthservice);
            flowmonth_Service = fmonthservice;

            this.AddDisposableObject(fhourservice);
            flowhour_Service = fhourservice;

            this.AddDisposableObject(fdayservice);
            flowday_Service = fdayservice;

            this.AddDisposableObject(accservice);
            account_Service = accservice;

            this.AddDisposableObject(flowservice);
            flow_Service = flowservice;
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
            User_t account = UserContext.account;

                Func<List<FlowMeterData>> fmdataFunc = () => flowmeter_Service.GetFlowMetersDataByUserUid(account);

                var fmdataanalysis = DBHelper.get<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["allFlowAnalysisByUserUid"] + account.Usr_UId);
                result.Data = fmdataanalysis.Where(p => p.flowmeter.FM_UId == uid).FirstOrDefault();

            
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
            //获取账号访问设备uid的访问次数，不设置过期时间
            Func<List<VisitCount>> initvisit = ()=>{return new List<VisitCount>();};
            List<VisitCount> vclist = DBHelper.getWithNoExpire<List<VisitCount>>(initvisit, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitFlowMeterCount"]);
            //增加账号访问设备uid的访问次数
            Func<List<VisitCount>> visitcount = () => account_Service.AddDeviceVisits(vclist,uid);
            DBHelper.getAndFresh<VisitCount>(visitcount, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitFlowMeterCount"]);
            //获取并返回设备uid的区域状态数据
            Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => (flowmeter_Service.GetFlowMeterStatusAndArea());
            result.Data = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea, ConfigurationManager.AppSettings["allFlowMeterStatusAndArea"]).Where(p => p.flowmeter.FM_UId == uid).ToList();
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

            if (ts.TotalDays < 1)
            {
                rs.Data = flow_Service.GetFlowByMeterUidAndTime(uid,startDt,endDt).Select(p => new
                {
                    value = p.Flw_TotalValue,
                    time = p.Flw_CreateDt.ToString("yyyyMMddHHmm")
                });
            }
            else if (ts.TotalDays < 5)
            {
                int start = int.Parse(startDt.ToString("yyyyMMddHH"));
                int end = int.Parse(endDt.ToString("yyyyMMddHH"));
                rs.Data = flowhour_Service.GetTimeFlowHourByUid(uid,start+1,end-1).OrderBy(p => p.Flh_Time).ToList().Select(p => new
                {
                    value = p.Flh_TotalValue,
                    time = p.Flh_Time
                });

            }
            else
            {
                int start = int.Parse(startDt.ToString("yyyyMMdd"));
                int end = int.Parse(endDt.ToString("yyyyMMdd"));
                rs.Data = flowday_Service.GetAllFlowDayByFMUid(uid).Where(p => p.Fld_Time > start && p.Fld_Time < end).OrderBy(p => p.Fld_Time).Select(p => new
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
        public JsonResult GetMostVisitsFlowMeter()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<FlowMeterData> fmdatalist = new List<FlowMeterData>();
            User_t account = UserContext.account;
            //获取账号访问设备次数的list
            Func<List<VisitCount>> initvisit = () => { return new List<VisitCount>(); };
            List<VisitCount> vclist = DBHelper.get<List<VisitCount>>(initvisit, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitFlowMeterCount"]);

            Func<List<FlowMeterData>> fmdataFunc = () => flowmeter_Service.GetFlowMetersDataByUserUid(account);
            List<FlowMeterData> fmdataanalysis = DBHelper.get<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["allFlowAnalysisByUserUid"] + account.Usr_UId);

            if (vclist.Count > 0)
            {
                vclist = vclist.OrderByDescending(p => p.count).ToList();
                for (var i = 0; i < vclist.Count; i++)
                {
                    var fmdata = fmdataanalysis.Where(p => p.flowmeter.FM_UId == Guid.Parse(vclist[i].uid)).FirstOrDefault();
                    fmdatalist.Add(fmdata);
                }
                for (var i = 0; i < fmdataanalysis.Count; i++)
                {
                    if (vclist.Where(p => p.uid == fmdataanalysis[i].flowmeter.FM_UId.ToString()).Count() == 0)
                    {
                        fmdatalist.Add(fmdataanalysis[i]);
                    }
                }
            }
            else
            {
                fmdatalist = fmdataanalysis;
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
            User_t account = UserContext.account;

            Func<List<FlowMeterData>> fmdataFunc = () => flowmeter_Service.GetFlowMetersDataByUserUid(account);
            List<FlowMeterData> fmdataanalysis = DBHelper.get<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["allFlowAnalysisByUserUid"] + account.Usr_UId);
            //先剔除“无法计算”的昨日流量比例，按大小排序，再补上“无法计算”
            var fmdatatemp1 = fmdataanalysis.Where(p => p.lastday_flow_proportion != "无法计算").ToList();
            if (fmdatatemp1.Count >=0&&fmdatatemp1.Count<3)
            {
                fmdatalist.AddRange(fmdatatemp1);
                var fmdatatemp2 = fmdataanalysis.Where(p => p.lastday_flow_proportion == "无法计算").ToList();
                if (fmdatatemp2.Count > 0)
                {
                    fmdatalist.AddRange(fmdatatemp2);
                    fmdatalist = fmdatalist.Take(3).ToList();
                }
            }
            else if (fmdatatemp1.Count >= 3)
            {
                fmdatalist = fmdatatemp1.OrderByDescending(p => p.lastday_flow_proportion).Take(3).ToList();

            }

            
            string dataresult = ToJson<List<FlowMeterData>>.Obj2Json<List<FlowMeterData>>(fmdatalist).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;  
            return result;
        }

        /// <summary>
        /// 获取区域流量
        /// </summary>
        /// <returns></returns>
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

    }
}