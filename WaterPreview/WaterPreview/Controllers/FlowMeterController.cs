using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
using WaterPreview.Other.Attribute;
using WaterPreview.Redis;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;
using WaterPreview.Service.Service;


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
        private static IAreaDeviceService areadevice_Service;


        public FlowMeterController()
        {
            flowmeter_Service = new FlowMeterService();
            flowmonth_Service = new FlowMonthService();
            flowhour_Service = new FlowHourService();
            flowday_Service = new FlowDayService();
            account_Service = new AccountService();
            flow_Service = new FlowService();
            areadevice_Service = new AreaDeviceService();
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
            //User_t account = UserContext.account;
            FlowMeter_t flowmeter = flowmeter_Service.GetFlowMeterByFMUid(uid);
            Func<FlowMeterData> fmdataFunc = () => flowmeter_Service.GetAnalysisByFlowMeter(flowmeter, (DateTime)time);

            result.Data = DBHelper.getT<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["FlowMeterAnalysisByFMUid"]+uid);

                //Func<List<FlowMeterData>> fmdataFunc = () => flowmeter_Service.GetFlowMetersDataByUserUid(account);

                //var fmdataanalysis = DBHelper.get<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["allFlowAnalysisByUserUid"] + account.Usr_UId);
                //result.Data = fmdataanalysis.Where(p => p.flowmeter.FM_UId == uid).FirstOrDefault();

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

            AreaDevice_t ad = areadevice_Service.GetAreaDeviceByDeviceUid(uid);

            Func<List<FlowMeterStatusAndArea>> fmsFunc = () => flowmeter_Service.GetFlowMeterStatusByArea(ad.AD_AreaUid);
            result.Data = DBHelper.get<FlowMeterStatusAndArea>(fmsFunc,
                ConfigurationManager.AppSettings["FlowMeterStatusByAreaUid"] + ad.AD_AreaUid).Where(p=>p.flowmeter.FM_UId==uid).ToList();

            //Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => (flowmeter_Service.GetFlowMeterStatusAndArea());
            //result.Data = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea, ConfigurationManager.AppSettings["allFlowMeterStatusAndArea"]).Where(p => p.flowmeter.FM_UId == uid).ToList();
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
            var data = flowhour_Service.GetFlowHourByFMUid(uid).OrderByDescending(p => p.Flh_Time).Take(500).ToList();
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
                var fds = flowday_Service.GetAllFlowDayByFMUid(uid).Where(p => p.Fld_Time > start && p.Fld_Time < end).OrderBy(p => p.Fld_Time).ToList();
                List<object> fdsobj = new List<object>();
                foreach (var item in fds)
                {
                    object obj = new
                    {
                        value = item.Fld_TotalValue,
                        time = item.Fld_Time
                    };
                    fdsobj.Add(obj);
                }
                rs.Data = fdsobj;
                //rs.Data =fds.Select(p => new
                //{
                //    value = p.Fld_TotalValue,
                //    time = p.Fld_Time
                //});
                //rs.Data = flowday_Service.GetAllFlowDayByFMUid(uid).Where(p => p.Fld_Time > start && p.Fld_Time < end).OrderBy(p => p.Fld_Time).Select(p => new
                //{
                //    value = p.Fld_TotalValue,
                //    time = p.Fld_Time
                //});
            }
            return rs;
        }

        /// <summary>
        /// 输出经常访问的流量计分析数据
        /// </summary>
        /// <param name="fmUids"></param>
        /// <returns></returns>
        /// 
        public JsonResult GetMostVisitsFlowMeter()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<FlowMeterData> fmdata_account = new List<FlowMeterData>();
            User_t account = UserContext.account;
            //获取账号访问设备次数的list
            Func<List<VisitCount>> initvisit = () => { return new List<VisitCount>(); };
            List<VisitCount> vclist = DBHelper.get<List<VisitCount>>(initvisit, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitFlowMeterCount"]);

            //Func<List<FlowMeterData>> fmdataFunc = () => flowmeter_Service.GetFlowMetersDataByUserUid(account);
            //List<FlowMeterData> fmdataanalysis = DBHelper.get<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["allFlowAnalysisByUserUid"] + account.Usr_UId);
            List<FlowMeter_t> fmlist = new List<FlowMeter_t>();

            if (account.Usr_Type == 3)
            {
                fmlist = flowmeter_Service.GetFlowMetersByUserUid(account.Usr_UId).ToList();
            }
            else
            {
                fmlist = flowmeter_Service.GetAllFlowMeter();
            }
            if (vclist.Count > 0)
            {
                vclist = vclist.OrderByDescending(p => p.count).ToList();
                foreach(var vcitem in vclist){
                    FlowMeter_t fm = fmlist.First(p => p.FM_UId == Guid.Parse(vcitem.uid));
                    Func<FlowMeterData> fmdataFunc = () => flowmeter_Service.GetAnalysisByFlowMeter(fm, (DateTime)fm.FM_FlowCountLast);
                    var fmdata = DBHelper.getT<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["FlowMeterAnalysisByFMUid"]+fm.FM_UId);
                    fmdata_account.Add(fmdata);

                }
                foreach (FlowMeter_t item in fmlist)
                {
                    if (vclist.All(p => p.uid != item.FM_UId.ToString()))
                    {
                        Func<FlowMeterData> fmdataFunc = () => flowmeter_Service.GetAnalysisByFlowMeter(item, (DateTime)item.FM_FlowCountLast);
                        var fmdata = DBHelper.getT<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["FlowMeterAnalysisByFMUid"] + item.FM_UId);
                        fmdata_account.Add(fmdata);
                    }
                }
            }
            else
            {
                foreach (var item in fmlist)
                {
                    Func<FlowMeterData> fmdataFunc = () => flowmeter_Service.GetAnalysisByFlowMeter(item, (DateTime)item.FM_FlowCountLast);
                    var fmdata = DBHelper.getT<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["FlowMeterAnalysisByFMUid"]+item.FM_UId);
                    fmdata_account.Add(fmdata);

                }
            }

            string dataresult = ToJson<List<FlowMeterData>>.Obj2Json<List<FlowMeterData>>(fmdata_account).Replace("\\\\", "");
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
            List<FlowMeterData> fmdata_account = new List<FlowMeterData>();
            User_t account = UserContext.account;
            List<FlowMeter_t> fmlist = new List<FlowMeter_t>();
            if (account.Usr_Type == 3)
            {
                fmlist = flowmeter_Service.GetFlowMetersByUserUid(account.Usr_UId);
            }
            else
            {
                fmlist = flowmeter_Service.GetAllFlowMeter();
            }
            List<FlowMeterData> fmdatalist = new List<FlowMeterData>();
            foreach (var item in fmlist)
            {
                Func<FlowMeterData> fmdataFunc = () => flowmeter_Service.GetAnalysisByFlowMeter(item, (DateTime)item.FM_FlowCountLast);
                var fmdata = DBHelper.getT<FlowMeterData>(fmdataFunc, ConfigurationManager.AppSettings["FlowMeterAnalysisByFMUid"]+item.FM_UId);

                //var fmdata = GetAnalysisByFlowMeter(item,(DateTime)item.FM_FlowCountLast);
                fmdatalist.Add(fmdata);

            }
            
            //先剔除“无法计算”的昨日流量比例，按大小排序，再补上“无法计算”
            var fmdatatemp1 = fmdatalist.Where(p => p.lastday_flow_proportion != "无法计算").ToList();
            if (fmdatatemp1.Count >=0&&fmdatatemp1.Count<3)
            {
                fmdatalist.AddRange(fmdatatemp1);
                var fmdatatemp2 = fmdatalist.Where(p => p.lastday_flow_proportion == "无法计算").ToList();
                if (fmdatatemp2.Count > 0)
                {
                    fmdata_account.AddRange(fmdatatemp2);
                    fmdata_account = fmdata_account.Take(3).ToList();
                }
            }
            else if (fmdatatemp1.Count >= 3)
            {
                fmdata_account = fmdatatemp1.OrderByDescending(p => p.lastday_flow_proportion).Take(3).ToList();

            }


            string dataresult = ToJson<List<FlowMeterData>>.Obj2Json<List<FlowMeterData>>(fmdata_account).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;  
            return result;
        }

        /// <summary>
        /// 获取区域流量,暂时没用到
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