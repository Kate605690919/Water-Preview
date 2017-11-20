using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class FlowDayService:BaseService<FlowDay_t>,IFlowDayService
    {

        public List<FlowDay_t> GetAllFlowDay(Guid fmuid)
        {
            var fdlist = FindAll().Where(p=>p.Fld_FlowMeterUid==fmuid);
            return fdlist.Count() == 0 ? new List<FlowDay_t>() : fdlist.ToList();
        }
    }
}
