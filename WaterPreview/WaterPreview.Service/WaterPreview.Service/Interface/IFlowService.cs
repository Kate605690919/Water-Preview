using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Interface
{
    public interface IFlowService
    {
        List<Flow_t> GetFlowByMeterUidAndTime(Guid fmUid, DateTime starttime, DateTime endtime);
    }
}
