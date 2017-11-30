using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class PressureDayService:BaseService<PressureDay_t>,IPressureDayService
    {
        public List<PressureDay_t> GetPressureDayByUidWithTime(Guid pmUid, int startTime, int endTime)
        {
            var pdlist = FindAll().Where(p => p.PD_PressureMeterUid == pmUid).ToList();
            return pdlist.Count == 0 ? new List<PressureDay_t>() : pdlist.Where(p => p.PD_Time > startTime && p.PD_Time < endTime).OrderBy(p => p.PD_Time).ToList();
        }
    }
}
