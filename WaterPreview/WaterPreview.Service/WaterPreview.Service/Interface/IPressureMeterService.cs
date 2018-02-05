using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.RedisContract;

namespace WaterPreview.Service.Interface
{
    public interface IPressureMeterService
    {
        List<PressureMeter_t> GetAllPressureMeter();

        List<PressureMeterStatusAndArea> GetPressureMeterStatusByArea(Guid areaUid);

        PressureMeter_t GetPressureMeterByPMUid(Guid pmUid);

        PressureMeterData GetAnalysisByPressureMeter(PressureMeter_t pm, DateTime datetime);

        List<PressureMeterData> GetPressureMetersDataByUser(User_t account);
    }
}
