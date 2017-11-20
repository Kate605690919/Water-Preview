using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.RedisContract;

namespace WaterPreview.Service.Interface
{
    public interface IFlowMeterService
    {
        List<FlowMeter_t> GetAllFlowMeter();

        List<FlowMeterStatusAndArea> GetFlowMeterStatusAndArea();
    }
}
