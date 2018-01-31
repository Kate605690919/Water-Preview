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
    public class PressureMeterController:BaseController
    {
        static IPressureService pressure_service;
        static IPressureMeterService pressuremeter_service;
        static IPressureHourService pressurehour_service;
        static IPressureDayService pressureday_service;
        static IPressureMonthService pressuremonth_service;
        static IAccountService account_service;
        static IAreaDeviceService areadevice_service;

        public PressureMeterController()
        {
            pressure_service = new PressureService();
            pressuremeter_service = new PressureMeterService();
            pressurehour_service = new PressureHourService();
            pressureday_service = new PressureDayService();
            pressuremonth_service = new PressureMonthService();
            account_service = new AccountService();
            areadevice_service = new AreaDeviceService();
        }
        //public PressureMeterController(IPressureService pservice,IPressureMeterService pmservice,
        //    IPressureHourService phourservice, IPressureDayService pdayservice, IPressureMonthService pmonthservice, IAccountService accservice)
        //{
        //    this.AddDisposableObject(pservice);
        //    pressure_service = pservice;

        //    this.AddDisposableObject(pmservice);
        //    pressuremeter_service = pmservice;

        //    this.AddDisposableObject(phourservice);
        //    pressurehour_service = phourservice;

        //    this.AddDisposableObject(pdayservice);
        //    pressureday_service = pdayservice;

        //    this.AddDisposableObject(pmonthservice);
        //    pressuremonth_service = pmonthservice;

        //    this.AddDisposableObject(accservice);
        //    account_service = accservice;
        //}

        /// <summary>
        /// 压力计列表数据
        /// </summary>
        /// <param name="pmuid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult Detail(Guid pmuid,Guid? useruid)
        {
            
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            useruid =useruid == null? UserContext.account.Usr_UId:useruid;

            //获取账号访问设备uid的访问次数，并不设置过期时间
            Func<List<VisitCount>> initvisit = () => { return new List<VisitCount>(); };
            List<VisitCount> vclist = DBHelper.getWithNoExpire<List<VisitCount>>(initvisit, useruid + ConfigurationManager.AppSettings["VisitPressureMeterCount"]);
            //增加账号访问设备uid的访问次数
            Func<List<VisitCount>> visitcount = () => account_service.AddDeviceVisits(vclist, pmuid);
            DBHelper.getAndFresh<VisitCount>(visitcount, useruid + ConfigurationManager.AppSettings["VisitPressureMeterCount"]);

            //获取并返回设备uid的区域状态数据
            AreaDevice_t ad = areadevice_service.GetAreaDeviceByDeviceUid(pmuid);
            Func<List<PressureMeterStatusAndArea>> pmsFunc = () => pressuremeter_service.GetPressureMeterStatusByArea(ad.AD_AreaUid);
            result.Data = DBHelper.get<PressureMeterStatusAndArea>(pmsFunc,
                ConfigurationManager.AppSettings["PressureMeterStatusByAreaUid"] + ad.AD_AreaUid).ToList();

            //Func<List<PressureMeterStatusAndArea>> pmAndStatusArea = () => (pressuremeter_service.GetPressureMeterStatusAndArea());
            //result.Data = DBHelper.get<PressureMeterStatusAndArea>(pmAndStatusArea, ConfigurationManager.AppSettings["allPressureMeterStatusAndArea"]).Where(p => p.pressuremeter.PM_UId == pmuid).ToList();
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
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            result.Data = pressurehour_service.GetPressureHourByUid(pmUid);
            return result;
        }

        /// <summary>
        /// pmuid和两个时间点之间的水压计对应水压示数
        /// </summary>
        /// <param name="pmUid"></param>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <returns></returns>
        public JsonResult GetPressureDetailWithTime(Guid pmUid, DateTime startDt, DateTime endDt)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            TimeSpan timespan = endDt-startDt;
            int start = int.Parse(startDt.ToString("yyyyMMddHH"));
            int end = int.Parse(endDt.ToString("yyyyMMddHH"));

            if(timespan.TotalDays<1){
                result.Data = pressure_service.GetPressureByUidAndTime(pmUid, startDt, endDt).Select(p => new
                {
                    value = p.Pre_Value,
                    time = p.Pre_CreateDt.ToString("yyyyMMddHHmm")
                });
            }else if(timespan.TotalDays<5){
                result.Data = pressurehour_service.GetPressureHourByUidWithTime(pmUid, start, end).Select(p => new
                {
                    value = p.PH_AverageValue,
                    time = p.PH_Time,
                });
            }else{
                result.Data = pressureday_service.GetPressureDayByUidWithTime(pmUid, start, end).Select(p =>
                    new
                    {
                        value = p.PD_AverageValue,
                        time =p.PD_Time
                    });
            }
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
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

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
            User_t account = UserContext.account;
            Guid useruid = account == null||account.Usr_UId==new Guid()?Guid.Parse(Session["wp_username"].ToString()):account.Usr_UId;

            Func<List<PressureMeterData>> pmdataFunc = ()=>pressuremeter_service.GetPressureMetersDataByUser(account);
            var pmdataanalysis = DBHelper.get<PressureMeterData>(pmdataFunc, ConfigurationManager.AppSettings["allPressureAnalysisByUserUid"] + account.Usr_UId);
            result.Data = pmdataanalysis.Where(p=>p.pressuremeter.PM_UId==pmuid).FirstOrDefault();
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
            User_t account = UserContext.account;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<PressureMeterData> pmdataanalysis = new List<PressureMeterData>();
            List<PressureMeter_t> pmlist = new List<PressureMeter_t>();
            if (account.Usr_Type == 3)
            {
                //获取客户的水压计，当前水压计未绑定客户
            }
            else
            {
                pmlist = pressuremeter_service.GetAllPressureMeter();
                foreach (var item in pmlist)
                {
                    Func<PressureMeterData> pmdataFunc = () => pressuremeter_service.GetAnalysisByPressureMeter(item, (DateTime)item.PM_CountLast);
                    var pmdata = DBHelper.getT<PressureMeterData>(pmdataFunc, ConfigurationManager.AppSettings["PressureMeterAnalysisByPMUid"] + item.PM_UId);
                    pmdataanalysis.Add(pmdata);
                }
            }

            //Func<List<PressureMeterData>> pmdataFunc = () => pressuremeter_service.GetPressureMetersDataByUser(account);
            //var pmdataanalysis = DBHelper.get<PressureMeterData>(pmdataFunc, ConfigurationManager.AppSettings["allPressureAnalysisByUserUid"] + account.Usr_UId);
            List<PressureMeterData> pmdatalist = new List<PressureMeterData>();

            Func<List<VisitCount>> initvisit = () => { return new List<VisitCount>(); };
            List<VisitCount> vclist = DBHelper.get<List<VisitCount>>(initvisit, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitPressureMeterCount"]);

            if (vclist.Count > 0)
            {
                vclist.OrderByDescending(p => p.count).ToList();

                for (var i = 0; i < vclist.Count; i++)
                {
                    var pmdata = pmdataanalysis.FirstOrDefault(p => p.pressuremeter.PM_UId == Guid.Parse(vclist[i].uid));
                    pmdatalist.Add(pmdata);
                }
                for (var i = 0; i < pmdataanalysis.Count; i++)
                {
                    if (vclist.Where(p => p.uid == pmdataanalysis[i].pressuremeter.PM_UId.ToString()).Count() == 0)
                    {
                        pmdatalist.Add(pmdataanalysis[i]);
                    }
                }
            }
            else
            {
                pmdatalist = pmdataanalysis;
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
            List<PressureMeterData> pmdatalist = new List<PressureMeterData>();
            User_t account = UserContext.account;
            //Func<List<PressureMeterData>> pmdataFunc = () => pressuremeter_service.GetPressureMetersDataByUser(account);
            //var pmdataanalysis = DBHelper.get<PressureMeterData>(pmdataFunc, ConfigurationManager.AppSettings["allPressureAnalysisByUserUid"] + account.Usr_UId);
            List<PressureMeterData> pmdataanalysis = new List<PressureMeterData>();
            List<PressureMeter_t> pmlist = new List<PressureMeter_t>();
            if (account.Usr_Type == 3)
            {
                //获取客户的水压计，当前水压计未绑定客户
            }
            else
            {
                pmlist = pressuremeter_service.GetAllPressureMeter();
                foreach (var item in pmlist)
                {
                    Func<PressureMeterData> pmdataFunc = () => pressuremeter_service.GetAnalysisByPressureMeter(item, (DateTime)item.PM_CountLast);
                    var pmdata = DBHelper.getT<PressureMeterData>(pmdataFunc, ConfigurationManager.AppSettings["PressureMeterAnalysisByPMUid"] + item.PM_UId);
                    pmdataanalysis.Add(pmdata);
                }
            }

            pmdatalist = pmdataanalysis.Where(p => p.lastday_pressure_proportion != "无法计算").OrderByDescending(p => p.lastday_pressure_proportion).Take(3).ToList();

            string dataresult = ToJson<List<PressureMeterData>>.Obj2Json<List<PressureMeterData>>(pmdatalist).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }


    }
}