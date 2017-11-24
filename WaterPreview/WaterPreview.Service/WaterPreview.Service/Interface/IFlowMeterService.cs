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

        List<FlowMeter_t> GetFlowMetersByUserUid(Guid userUid);

        List<FlowMeterData> GetFlowMetersDataByUserUid(User_t account);

        FlowMeterData GetAnalysisByFlowMeter(FlowMeter_t fm,DateTime datetime);

    }
}
