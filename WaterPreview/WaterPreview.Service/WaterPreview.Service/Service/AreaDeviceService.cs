using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class AreaDeviceService:BaseService<AreaDevice_t>,IAreaDeviceService
    {
        public AreaDevice_t GetAreaDeviceByDeviceUid(Guid deviceUid)
        {
            var ad = FindAll().Where(p=>p.AD_DeviceUid==deviceUid);
            return ad.Count() == 0 ? new AreaDevice_t() : ad.FirstOrDefault();
        }
    }
}
