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
        public List<QualityMeterStatusAndArea> GetQualityMeterStatusAndArea()
        {
            IQualityMeterStatusService pms_service = new QualityMeterStatusService();
            IAreaService area_service = new AreaService();
            List<QualityMeterStatusAndArea> qmsalist = new List<QualityMeterStatusAndArea>();
            List<QualityMeter_t> qmlist = FindAll();
            foreach (var qmsa_item in qmlist)
            {
                QualityMeterStatusAndArea item = new QualityMeterStatusAndArea()
                {
                    qualitymeter = FindAll().Where(p => p.QM_UId == qmsa_item.QM_UId).FirstOrDefault(),
                    status = pms_service.GetPressureMeterStatusByUid(qmsa_item.QM_UId).FirstOrDefault(),
                    area = area_service.GetAreaByDeviceUid(qmsa_item.QM_UId)
                };
                qmsalist.Add(item);
            }
            return qmsalist;
        }
    }
}
