using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Interface
{
    public interface IAreaDeviceService
    {
        AreaDevice_t GetAreaDeviceByDeviceUid(Guid deviceUid);
    }
}
