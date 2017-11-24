using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Interface
{
    public interface IPressureHourService
    {
        List<PressureHour_t> GetPressureHourByUid(Guid pmUid);

        List<PressureHour_t> GetDayPressureByUid(Guid pmuid, DateTime time);
    }
}
