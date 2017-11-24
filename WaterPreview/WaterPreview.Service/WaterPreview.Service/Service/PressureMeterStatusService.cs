using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class PressureMeterStatusService:BaseService<PressureMeterStatus_t>,IPressureMeterStatusService
    {
        public List<PressureMeterStatus_t> GetPressureMeterStatusByUid(Guid pmuid)
        {
            var pmlist = FindAll().Where(p => p.PMS_DeviceUid == pmuid);
            return pmlist.Count() == 0 ? new List<PressureMeterStatus_t>() : pmlist.ToList();
        }
    }
}
