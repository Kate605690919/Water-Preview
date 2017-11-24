using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class QualityMeterStatusService:BaseService<QualityMeterStatus_t>,IQualityMeterStatusService
    {
        public List<QualityMeterStatus_t> GetPressureMeterStatusByUid(Guid qmuid)
        {
            var qmlist = FindAll().Where(p => p.QMS_DeviceUid == qmuid);
            return qmlist.Count() == 0 ? new List<QualityMeterStatus_t>() : qmlist.ToList();
        }
    }
}
