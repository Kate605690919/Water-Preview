using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;

namespace WaterPreview.Service.Service
{
    public class QualityMeterService:BaseService<QualityMeter_t>,IQualityMeterService
    {
        public List<QualityMeterStatusAndArea> GetQualityMeterStatusByArea(Guid areaUid)
        {
            IQualityMeterStatusService qms_service = new QualityMeterStatusService();
            IAreaService area_service = new AreaService();
            IAreaDeviceService areadevice_service = new AreaDeviceService();

            List<QualityMeterStatusAndArea> qmsalist = new List<QualityMeterStatusAndArea>();
            List<QualityMeter_t> qmlist = FindAll().Where(p=>!p.QM_Description.Contains("可不用")).ToList();

            List<AreaDevice_t> adlist = areadevice_service.GetAreaDeviceByAreaUid(areaUid);

            foreach (var aditem in adlist)
            {
                if (qmlist.Where(p => p.QM_UId == aditem.AD_DeviceUid).Count() > 0)
                {
                    QualityMeterStatusAndArea item = new QualityMeterStatusAndArea()
                    {
                        qualitymeter = FindAll().FirstOrDefault(p => p.QM_UId == aditem.AD_DeviceUid),
                        status = qms_service.GetQualityMeterStatusByUid(aditem.AD_DeviceUid).FirstOrDefault(),
                        area = area_service.GetAreaByDeviceUid(aditem.AD_DeviceUid)
                    };
                    qmsalist.Add(item);
                }
            }
            return qmsalist;
        }
    }
}
