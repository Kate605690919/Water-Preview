using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Interface
{
    public interface IPressureDayService
    {
        List<PressureDay_t> GetPressureDayByUidWithTime(Guid pmUid, int startTime, int endTime);
    }
}
