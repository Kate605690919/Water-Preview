using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class FlowMeterStatusService:BaseService<FlowMeterStatus_t>,IFlowMeterStatusService
    {
        public List<FlowMeterStatus_t> GetFlowMeterStatusByUid(Guid fmuid)
        {
            var fmlist = FindAll().Where(p=>p.FMS_DeviceUid==fmuid);
            return fmlist.Count() == 0 ? new List<FlowMeterStatus_t>() : fmlist.ToList();
        }
    }
}
