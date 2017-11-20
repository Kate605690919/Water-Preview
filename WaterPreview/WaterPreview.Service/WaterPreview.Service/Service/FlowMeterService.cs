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
    public class FlowMeterService:BaseService<FlowMeter_t>,IFlowMeterService
    {
        public List<FlowMeter_t> GetAllFlowMeter()
        {
            return FindAll();
        }

        public List<FlowMeterStatusAndArea> GetFlowMeterStatusAndArea()
        {
            IFlowMeterStatusService fms_service = new FlowMeterStatusService();
            IAreaService area_service = new AreaService();
            List<FlowMeterStatusAndArea> fmsalist = new List<FlowMeterStatusAndArea>();
            List<FlowMeter_t> fmlist = FindAll();
            foreach(var fmsa_item in fmlist){
                FlowMeterStatusAndArea item = new FlowMeterStatusAndArea()
                {
                    flowmeter = FindAll().Where(p=>p.FM_UId==fmsa_item.FM_UId).FirstOrDefault(),
                    status = fms_service.GetFlowMeterStatusByUid(fmsa_item.FM_UId).FirstOrDefault(),
                    area = area_service.GetAreaByDeviceUid(fmsa_item.FM_UId)
                };
                fmsalist.Add(item);
            }
            return fmsalist;
        }
    }
}
