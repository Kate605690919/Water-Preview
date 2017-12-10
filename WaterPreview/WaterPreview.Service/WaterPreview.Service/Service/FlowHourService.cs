using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class FlowHourService:BaseService<FlowHour_t>,IFlowHourService
    {
        public List<FlowHour_t> GetFlowHourByFMUid(Guid fmUid)
        {
            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
            var fhlist = db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == fmUid).ToList();
            return fhlist.Count() == 0 ? new List<FlowHour_t>() : fhlist.ToList();
        }

        public List<FlowHour_t> GetTimeFlowHourByUid(Guid fmUid,int startTime,int endTime)
        {
            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
            var fhlist = db.FlowHour_t.Where(p => p.Flh_FlowMeterUid == fmUid).ToList();
            return  fhlist.Count()==0?new List<FlowHour_t>():fhlist.Where(p=>p.Flh_Time<=endTime&&p.Flh_Time>=startTime).ToList();

        }

        public List<FlowHour_t> GetDayFlowByUidAndDate(Guid uid, DateTime date)
        {
            List<FlowHour_t> OneDay = new List<FlowHour_t>();
            int year = date.Year;
            int month = date.Month;

            List<FlowHour_t> result = new List<FlowHour_t>();
            var time = int.Parse(date.ToString("yyyyMMdd"));
            var lastmonth = date.AddMonths(-1).AddDays(1);
            var days = date.Subtract(lastmonth).Days;

            for (var i = 0; i <= days - 1; i++)
            {
                var day = int.Parse(lastmonth.AddDays(i).ToString("yyyyMMdd"));
                var secday = int.Parse(lastmonth.AddDays(i + 1).ToString("yyyyMMdd"));
                var startTime = day * 100 + 9;
                var endTime = secday * 100 + 9;
                List<FlowHour_t> fhPerHour = GetTimeFlowHourByUid(uid, startTime, endTime);
                result.AddRange(fhPerHour);
            }
            return result;
        }
    }
}
