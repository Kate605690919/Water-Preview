using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Interface
{
    public interface IPressureService
    {
        List<Pressure_t> GetPressureByUidAndTime(Guid pmUid, DateTime startTime, DateTime endTime);
    }
}
