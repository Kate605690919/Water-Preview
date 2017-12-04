using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class PressureHourService:BaseService<PressureHour_t>,IPressureHourService
    {
        public List<PressureHour_t> GetPressureHourByUid(Guid pmUid)
        {
            var phlist = FindAll().Where(p => p.PH_PressureMeterUid == pmUid);
            return phlist.Count()==0 ?  new List<PressureHour_t>(): phlist.ToList();
        }

        public List<PressureHour_t> GetDayPressureByUid(Guid pmuid, DateTime time)
        {
            List<PressureHour_t> phlist = new List<PressureHour_t>();

            var endday = int.Parse(time.AddDays(-1).ToString("yyyyMMdd"));
            var startday = int.Parse(time.AddDays(-2).ToString("yyyyMMdd"));
            phlist = GetPressureHourByUid(pmuid).Where(p => p.PH_Time >= (startday * 100 + 9) && p.PH_Time < (endday * 100 + 9)).ToList();
            return phlist;
        }

        public List<PressureHour_t> GetPressureHourByUidWithTime(Guid pmUid, int startTime, int endTime)
        {
            var phlist = FindAll().Where(p => p.PH_PressureMeterUid == pmUid).ToList();
            return phlist.Count == 0 ? new List<PressureHour_t>() : phlist.Where(p => p.PH_Time > startTime && p.PH_Time < endTime).OrderBy(p=>p.PH_Time).ToList();
        }
    }
}
