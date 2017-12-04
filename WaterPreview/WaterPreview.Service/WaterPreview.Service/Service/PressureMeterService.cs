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
    public class PressureMeterService:BaseService<PressureMeter_t>,IPressureMeterService
    {
        public List<PressureMeter_t> GetAllPressureMeter()
        {
            return FindAll();
        }

        public List<PressureMeterStatusAndArea> GetPressureMeterStatusAndArea()
        {
            IPressureMeterStatusService pms_service = new PressureMeterStatusService();
            IAreaService area_service = new AreaService();
            List<PressureMeterStatusAndArea> pmsalist = new List<PressureMeterStatusAndArea>();
            List<PressureMeter_t> pmlist = FindAll();
            foreach (var pmsa_item in pmlist)
            {
                PressureMeterStatusAndArea item = new PressureMeterStatusAndArea()
                {
                    pressuremeter = FindAll().Where(p => p.PM_UId == pmsa_item.PM_UId).FirstOrDefault(),
                    status = pms_service.GetPressureMeterStatusByUid(pmsa_item.PM_UId).FirstOrDefault(),
                    area = area_service.GetAreaByDeviceUid(pmsa_item.PM_UId),
                };
                pmsalist.Add(item);
            }
            return pmsalist;
        }

        //等水压计和客户有绑定信息了 查询客户对应的水压数据
        //public List<PressureMeterData> GetPressureMetersDataByUser(User_t account)
        //{
        //    List<PressureMeterData> pmdatalist = new List<PressureMeterData>();

        //    foreach
        //}

        public PressureMeterData GetAnalysisByPressureMeter(PressureMeter_t pm,DateTime datetime)
        {

            IPressureHourService ph_service = new PressureHourService();
            IPressureMonthService pm_service = new PressureMonthService();

            var lastMonth = int.Parse(datetime.AddMonths(-1).ToString("yyyyMM"));//当前时间
            var beforelastMonth = int.Parse(datetime.AddMonths(-2).ToString("yyyyMM"));

            var lastdayPressure = ph_service.GetDayPressureByUid(pm.PM_UId, datetime);
            var lastday_AvgData = lastdayPressure.Count == 0 ? 0 : lastdayPressure.Select(p => p.PH_AverageValue).Average();

            var beforelastday = ph_service.GetDayPressureByUid(pm.PM_UId, datetime.AddDays(-1));
            var beforelastday_AvgData = beforelastday.Count == 0 ? 0 : beforelastday.Select(p => p.PH_AverageValue).Average();

            var lastmonth_Avg = pm_service.GetPressureMonthByPMUid(pm.PM_UId).Where(p => p.PM_Time == lastMonth).ToList();
            var lastmonth_AvgData = lastmonth_Avg.Count == 0 ? 0 : lastmonth_Avg.Select(p => p.PM_AverageValue).First();

            var beforelastmonth_Avg = pm_service.GetPressureMonthByPMUid(pm.PM_UId).Where(p =>p.PM_Time == beforelastMonth).ToList();
            var beforelastmonth_AvgData = beforelastmonth_Avg.Count == 0 ? 0 : beforelastmonth_Avg.Select(p => p.PM_AverageValue).First();

            var lastnight_Avg = lastdayPressure.Where(p => p.PH_Time % 100 >= 2 && p.PH_Time % 100 <= 4).ToList();
            var lastnight_AvgData = lastnight_Avg.Count == 0 ? 0 : lastnight_Avg.Select(p => p.PH_AverageValue).Average();

            PressureMeterData pmanalysis = new PressureMeterData()
            {
                pressuremeter = pm,
                lastday_pressure = Math.Round(lastday_AvgData, 4)+"",
                lastday_pressure_proportion = beforelastday_AvgData == 0 ? "无法计算" : Math.Round((lastday_AvgData - beforelastday_AvgData) / beforelastday_AvgData, 4).ToString("0.00%"),

                night_pressure = lastnight_AvgData+"",
                night_pressure_proportion = lastday_AvgData == 0 ? "无法计算" : Math.Round((lastnight_AvgData - lastday_AvgData) / lastday_AvgData, 4).ToString("0.00%"),
                month_pressure = lastmonth_AvgData+"",
                month_pressure_proportion = beforelastmonth_AvgData == 0 ? "无法计算" : Math.Round((lastmonth_AvgData - beforelastmonth_AvgData) / beforelastmonth_AvgData, 4).ToString("0.00%"),
                
            };

            return pmanalysis;
        }

        public List<PressureMeterData> GetPressureMetersDataByUser(User_t account)
        {
            List<PressureMeter_t> pmlist = new List<PressureMeter_t>();
            List<PressureMeterData> pmdatalist = new List<PressureMeterData>();

            if (account.Usr_Type == 3)
            {
                //获取客户的水压计，当前水压计未绑定客户
            }
            else
            {
                pmlist = FindAll();
                foreach (var item in pmlist)
                {
                    var pmdata = GetAnalysisByPressureMeter(item, (DateTime)item.PM_CountLast);
                    pmdatalist.Add(pmdata);
                }
            }
            return pmdatalist;
        }

    }
}
