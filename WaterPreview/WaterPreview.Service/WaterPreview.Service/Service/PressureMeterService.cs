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
    public class PressureMeterService:BaseService<PressureMeter_t>,IPressureMeterService
    {
        public List<PressureMeter_t> GetAllPressureMeter()
        {
            return FindAll();
        }

        public List<PressureMeterStatusAndArea> GetPressureMeterStatusAndArea()
        {
            IPressureMeterStatusService pms_service = new PressureMeterStatusService();
            IAreaService area_service = new AreaService();
            List<PressureMeterStatusAndArea> pmsalist = new List<PressureMeterStatusAndArea>();
            List<PressureMeter_t> pmlist = FindAll();
            foreach (var pmsa_item in pmlist)
            {
                PressureMeterStatusAndArea item = new PressureMeterStatusAndArea()
                {
                    pressuremeter = FindAll().Where(p => p.PM_UId == pmsa_item.PM_UId).FirstOrDefault(),
                    status = pms_service.GetPressureMeterStatusByUid(pmsa_item.PM_UId).FirstOrDefault(),
                    area = area_service.GetAreaByDeviceUid(pmsa_item.PM_UId)
                };
                pmsalist.Add(item);
            }
            return pmsalist;
        }
    }
}
