using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
using WaterPreview.Redis;
using WaterPreview.Service;
using WaterPreview.Service.Interface;


namespace WaterPreview.Controllers
{
    public class FlowMeterController:BaseController
    {
        private static IFlowMeterService flowmeter_Service;
        private static IFlowMonthService flowmonth_Service;


        public FlowMeterController(IFlowMeterService fmservice,IFlowMonthService fmonthservice)
        {
            this.AddDisposableObject(fmservice);
            flowmeter_Service = fmservice;

            this.AddDisposableObject(fmonthservice);
            flowmonth_Service = fmonthservice;
        }

        public JsonResult Analysis(Guid uid, DateTime time)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var timeint = int.Parse(time.ToString("yyyyMM"));
            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
            //获取当前设备前一月9点至当天9点的flowhour
            var origindata = GetdayFlowByUidAndDate(uid, time);
            //当前日期的前一天
            var lastdaytime = int.Parse(time.AddDays(-1).ToString("yyyyMMdd"));
            var beforelastdaytime = int.Parse(time.AddDays(-2).ToString("yyyyMMdd"));

            //前一天凌晨2-4点流量均值
            var yerstodaydata = origindata.Where(p => p.Flh_Time >= (lastdaytime * 100 + 9) &&
                p.Flh_Time <= ((lastdaytime + 1) * 100 + 9)).Where(p => p.Flh_Time % 100 >= 2 && p.Flh_Time % 100 <= 4)
                .GroupBy(p => p.Flh_FlowMeterUid).Select(p => p.Average(p1 => p1.Flh_TotalValue)).FirstOrDefault();
   
            var lastdaytotal = db.FlowDay_t
                .FirstOrDefault(p => p.Fld_FlowMeterUid == uid && p.Fld_Time == lastdaytime).Fld_TotalValue;
            var beforelastdaytotal = db.FlowDay_t
                .FirstOrDefault(p => p.Fld_FlowMeterUid == uid && p.Fld_Time == beforelastdaytime).Fld_TotalValue;
            var orderflow = origindata.OrderBy(p => p.Flh_TotalValue).ToList();

            var data = origindata.Select(p => (double)p.Flh_TotalValue).ToArray();//上月的每小时流量总值数组

            var totalMonth = data.Sum();//月总流量值

            var mean = data.Average();//月总流量平均数

            //平均数
            //var mean = ArrayStatistics.Mean(data);
            ////无偏总体方差
            //var variance = ArrayStatistics.Variance(data);
            ////总体方差
            //var populationVariance = ArrayStatistics.PopulationVariance(data);
            ////无偏总体标准差

            //var standardDeviation = ArrayStatistics.StandardDeviation(data);
            ////中位数
            //var medianInplace = ArrayStatistics.MedianInplace(data);
            ////百分位数 p=1,p=7
            //var percentileInplace1 = ArrayStatistics.PercentileInplace(data, 1);
            //var percentileInplace5 = ArrayStatistics.PercentileInplace(data, 5);


            //上月用水量
            var monthflow = flowmonth_Service.GetAllFlowMonth().FirstOrDefault(p => p.Flm_Time == timeint && p.Flm_FlowMeterUid == uid).Flm_TotalValue;
            //var lastmonthflow = flowmeterService.GetAllFlowMonth().FirstOrDefault(p => p.Flm_Time == (timeint+1)).Flm_TotalValue;

            var final = Math.Round(mean * 24 * 30 / (Double)monthflow, 4);
            //percentileInplace1 = percentileInplace1, percentileInplace5 = percentileInplace5 ;

            result.Data = new
            {
                //mean = Math.Round((Double)mean, 4).ToString(),//昨日流量平均值
                monthflow = Math.Round((Double)monthflow, 4),//上月总流量
                result = monthflow.HasValue ? final + "" : "无法计算",//上月总流量趋势
                lastday = yerstodaydata.HasValue ? Math.Round((decimal)yerstodaydata, 4) + "" : "无法计算",//昨夜凌晨2-4点流量均值
                nightproportion = monthflow.HasValue ? Math.Round((decimal)((yerstodaydata * 24 * 30) / monthflow), 4) + "" : "无法计算",//夜间用水量*24*30/总用水量
                lastdaytotal = Math.Round((decimal)lastdaytotal, 4),//昨日总流量
                lastdayproportion = beforelastdaytotal == 0 ? "无法计算" : Math.Round((decimal)((lastdaytotal - beforelastdaytotal) / beforelastdaytotal), 4) + "" //昨日总流量变化趋势
            };

            //result.Data = null;
            return result;
        }


        public IEnumerable<FlowHour_t> GetdayFlowByUidAndDate(Guid uid, DateTime date)
        {
            List<FlowHour_t> OneDay = new List<FlowHour_t>();
            int year = date.Year;
            int month = date.Month;
            //int day = new DateTime(year, month, 1).AddDays(-1).Day;

            List<FlowHour_t> result = new List<FlowHour_t>();
            var time = int.Parse(date.ToString("yyyyMMdd"));
            var lastmonth = date.AddMonths(-1).AddDays(1);
            var days = date.Subtract(lastmonth).Days;

            //var starttime = int.Parse(lastmonth.ToString("yyyyMMdd"));

            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();

            List<FlowHour_t> fhlist = db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == uid).ToList();
            for (var i = 0; i <= days - 1; i++)
            {
                //resultt.AddRange(db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == uid && p.Flh_Time >= (time * 100 + 9) && p.Flh_Time <= ((time + 1) * 100 + 9)).OrderBy(p => p.Flh_Time).ToList().Where(p => p.Flh_Time % 100 >= 2 && p.Flh_Time % 100 <= 4));
                var day = int.Parse(lastmonth.AddDays(i).ToString("yyyyMMdd"));
                var secday = int.Parse(lastmonth.AddDays(i + 1).ToString("yyyyMMdd"));
                List<FlowHour_t> fhPerHour = fhlist.Where(p => p.Flh_Time >= (day * 100 + 9) && p.Flh_Time <= (secday * 100 + 9)).ToList();
                result.AddRange(fhPerHour);

            }
            return result;

        }

        public JsonResult Detail(Guid uid)
        {
            JsonResult result = new JsonResult();
            Func<List<FlowMeter_t>> t = ()=>flowmeter_Service.GetAllFlowMeter().Where(p=>p.FM_UId==uid).ToList();
            result.Data = DBHelper.get<FlowMeter_t>(t,UserContext.allFlowMeter);
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
    }
}