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
    }
}
