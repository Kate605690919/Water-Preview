using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class PressureService:BaseService<Pressure_t>,IPressureService
    {
        public List<Pressure_t> GetPressureByUidAndTime(Guid pmUid, DateTime startTime, DateTime endTime)
        {
            var plist = FindAll().Where(p => p.Pre_PressureMeterUId == pmUid).ToList();
            return plist.Count == 0 ? new List<Pressure_t>() : plist.Where(p => p.Pre_CreateDt > startTime && p.Pre_CreateDt < endTime).OrderBy(p => p.Pre_CreateDt).ToList();
        }
    }
}
