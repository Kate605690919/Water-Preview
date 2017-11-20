using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Service;
using WaterPreview.Service.Interface;

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
            var lastMonth = int.Parse(time.AddMonths(-1).ToString("yyyyMM"));//当前时间
            var beforelastMonth = int.Parse(time.AddMonths(-2).ToString("yyyyMM"));

            var lastdayPressure = GetDayPressureByUid(pmuid, time);
            var lastday_AvgData = lastdayPressure.Select(p=>p.PH_AverageValue).Average();

            var beforelastday = GetDayPressureByUid(pmuid, time.AddDays(-1));
            var beforelastday_AvgData = beforelastday.Select(p => p.PH_AverageValue).Average();
            
            var lastmonth_AvgData = pressuremonth_service.GetAllPressureMonth().Where(p=>p.PM_PressureMeterUid==pmuid&&p.PM_Time==lastMonth).Select(p=>p.PM_AverageValue).First();
            var beforelastmonth_AvgData = pressuremonth_service.GetAllPressureMonth().Where(p=>p.PM_PressureMeterUid==pmuid&&p.PM_Time==beforelastMonth).Select(p=>p.PM_AverageValue).First();

            var lastnight_AvgData = lastdayPressure.Where(p => p.PH_Time % 100 >= 2 && p.PH_Time % 100 <= 4).Select(p => p.PH_AverageValue).Average();

            result.Data = new 
            {
                //昨日水压平均值
                lastdayAvg = Math.Round(lastday_AvgData,4),
                //昨日水压平均值变化趋势
                lastdayAvg_proportion = beforelastday_AvgData==0?"无法计算":Math.Round((lastday_AvgData-beforelastday_AvgData)/beforelastday_AvgData,4).ToString(),
                //上月水压平均值
                lastmonthAvg = lastmonth_AvgData,
                //上月水压平均值变化趋势
                lastmonthAvg_proportion = beforelastmonth_AvgData==0?"无法计算":Math.Round((lastmonth_AvgData-beforelastmonth_AvgData)/beforelastmonth_AvgData,4)+"",
                //昨夜凌晨2-4点水压均值
                lastnightAvg = lastnight_AvgData,
                //昨夜凌晨2-4点水压均值/当天水压均值
                lastnightAvg_proportion = lastday_AvgData==0?"无法计算":Math.Round((lastnight_AvgData-lastday_AvgData)/lastday_AvgData,4)+""

            };
            return result;
        }

        /// <summary>
        /// 获取time的前天9点到昨天8点的各小时的水压数据
        /// </summary>
        /// <param name="pmuid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<PressureHour_t> GetDayPressureByUid(Guid pmuid,DateTime time){
            List<PressureHour_t> phlist = new List<PressureHour_t>();

            var endday = int.Parse(time.AddDays(-1).ToString("yyyyMMdd"));
            var startday = int.Parse(time.AddDays(-2).ToString("yyyyMMdd"));
            phlist = pressurehour_service.GetPressureHourByUid(pmuid).Where(p => p.PH_Time >= (startday * 100 + 9)&&p.PH_Time<(endday*100+9)).ToList();
            return phlist;
        }
    }
}