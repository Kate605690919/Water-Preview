using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class FlowService:BaseService<Flow_t>,IFlowService
    {

        public List<Flow_t> GetFlowByMeterUidAndTime(Guid fmUid, DateTime starttime, DateTime endtime)
        {
            var flowlist = FindAll().Where(p=>p.Flw_FlowMeterUId==fmUid).ToList();
            return flowlist.Count == 0 ? new List<Flow_t>() : flowlist.Where(p => p.Flw_CreateDt > starttime && p.Flw_CreateDt < endtime).OrderBy(p => p.Flw_CreateDt).ToList();
        }
    }
}
