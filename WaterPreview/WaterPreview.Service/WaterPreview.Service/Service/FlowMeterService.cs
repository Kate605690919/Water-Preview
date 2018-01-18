using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;

namespace WaterPreview.Service.Service
{
    public class FlowMeterService : BaseService<FlowMeter_t>, IFlowMeterService
    {
        public List<FlowMeter_t> GetAllFlowMeter()
        {
            return FindAll();
        }

        public List<FlowMeter_t> GetFlowMetersByUserUid(Guid userUid)
        {
            return FindAll().Where(p => p.FM_WaterConsumerUId == userUid).ToList();
        }

        public List<FlowMeterStatusAndArea> GetFlowMeterStatusAndArea()
        {
            IFlowMeterStatusService fms_service = new FlowMeterStatusService();
            IAreaService area_service = new AreaService();
            List<FlowMeterStatusAndArea> fmsalist = new List<FlowMeterStatusAndArea>();
            List<FlowMeter_t> fmlist = FindAll();
            foreach (var fmsa_item in fmlist)
            {
                FlowMeterStatusAndArea item = new FlowMeterStatusAndArea()
                {
                    flowmeter = FindAll().Where(p => p.FM_UId == fmsa_item.FM_UId).FirstOrDefault(),
                    status = fms_service.GetFlowMeterStatusByUid(fmsa_item.FM_UId).FirstOrDefault(),
                    area = area_service.GetAreaByDeviceUid(fmsa_item.FM_UId)
                };
                fmsalist.Add(item);
            }
            return fmsalist;
        }

        /// <summary>
        /// 客户获取最新的流量分析数据
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public List<FlowMeterData> GetFlowMetersDataByUserUid(User_t account)
        {
            List<FlowMeter_t> fmlist = new List<FlowMeter_t>();
            if (account.Usr_Type == 3)
            {
                fmlist = FindAll().Where(p => p.FM_WaterConsumerUId == account.Usr_UId).ToList();
            }
            else
            {
                fmlist = FindAll();
            }

            List<FlowMeterData> fmdatalist = new List<FlowMeterData>();
            foreach (var item in fmlist)
            {
                var fmdata = GetAnalysisByFlowMeter(item,(DateTime)item.FM_FlowCountLast);
                fmdatalist.Add(fmdata);

            }


            return fmdatalist;
        }

        /// <summary>
        /// 管理员获取流量计分析数据、获取某一流量计分析数据
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        public FlowMeterData GetAnalysisByFlowMeter(FlowMeter_t fm,DateTime datetime)
        {
            IFlowHourService fh_service = new FlowHourService();
            IFlowDayService fd_service = new FlowDayService();
            IFlowMonthService flowmonth_Service = new FlowMonthService();

                List<FlowHour_t> fhlist = fh_service.GetMonthFlowByUidAndDate(fm.FM_UId, (DateTime)fm.FM_FlowCountLast);
                //DateTime time = (DateTime)fm.FM_FlowCountLast;
                int timeint = int.Parse(datetime.ToString("yyyyMM"));
                //当前日期的前一天
                var lastdaytime = int.Parse(datetime.AddDays(-1).ToString("yyyyMMdd"));
                var beforelastdaytime = int.Parse(datetime.AddDays(-2).ToString("yyyyMMdd"));

                //前一天凌晨2-4点流量均值
                var yerstoday = fhlist.Where(p => p.Flh_Time >= (lastdaytime * 100 + 9) &&
                p.Flh_Time <= ((lastdaytime + 1) * 100 + 9)).Where(p => p.Flh_Time % 100 >= 2 && p.Flh_Time % 100 <= 4)
                .ToList();
                var yerstodaydata = yerstoday.Count == 0 ? 0 : yerstoday.Average(p => p.Flh_TotalValue);

                var lastday = fd_service.GetAllFlowDayByFMUid(fm.FM_UId).Where(p => p.Fld_Time == lastdaytime).ToList();
                var lastdaytotal = lastday.Count == 0 ? 0 : lastday.FirstOrDefault().Fld_TotalValue;

                var beforelast = fd_service.GetAllFlowDayByFMUid(fm.FM_UId).Where(p => p.Fld_Time == beforelastdaytime).ToList();
                var beforelastdaytotal = beforelast.Count == 0 ? 0 : beforelast.FirstOrDefault().Fld_TotalValue;
                var orderflow = fhlist.OrderBy(p => p.Flh_TotalValue).ToList();
                var data = fhlist.Select(p => (double)p.Flh_TotalValue).ToArray();//上月的每小时流量总值数组
                var totalMonth = data.Sum();//月总流量值
                var mean = data.Count() == 0 ? 0 : data.Average();//月总流量平均数

                //上月用水量
                var monthflow = flowmonth_Service.GetAllFlowMonth().Where(p => p.Flm_Time == timeint && p.Flm_FlowMeterUid == fm.FM_UId).ToList();
                var monthflowdata = monthflow.Count == 0 ? 0 : monthflow.FirstOrDefault().Flm_TotalValue;
                //var lastmonthflow = flowmeterService.GetAllFlowMonth().FirstOrDefault(p => p.Flm_Time == (timeint+1)).Flm_TotalValue;
                var final = Math.Round(mean * 24 * 30 / (Double)monthflowdata, 4);

                FlowMeterData fmdata = new FlowMeterData()
                {
                    flowmeter = fm,
                    lastday_flow = Math.Round((decimal)lastdaytotal, 4) + "",//昨日总流量
                    lastday_flow_proportion = beforelastdaytotal == 0 ? "无法计算" : Math.Round((decimal)((lastdaytotal - beforelastdaytotal) / beforelastdaytotal), 4).ToString("0.00%"), //昨日总流量变化趋势
                    night_flow = yerstodaydata == 0 ? "无法计算" : Math.Round((decimal)yerstodaydata, 4) + "",//昨夜凌晨2-4点流量均值
                    month_flow = Math.Round((Double)monthflowdata, 4) + "",//上月总流量
                    night_flow_proportion = monthflowdata == 0 ? "无法计算" : Math.Round((decimal)((yerstodaydata * 24 * 30) / monthflowdata), 4).ToString("0.00%"),//夜间用水量*24*30/总用水量
                    month_flow_proportion = monthflowdata == 0 ? "无法计算" : final.ToString("0.00%"),//上月总流量趋势
                };
                return fmdata;
        }


    }
}
