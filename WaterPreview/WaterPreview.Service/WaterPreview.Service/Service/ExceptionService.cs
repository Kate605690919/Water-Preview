using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class ExceptionService: BaseService<Alarm_t>, IExceptionService
    {
        public List<Alarm_t> GetException(Guid userUid)
        {
            FlowMeterService flowmeterservice = new FlowMeterService();
            List<FlowMeter_t> flowmeter = flowmeterservice.GetFlowMetersByUserUid(userUid);
            //List<FlowMeter_t> flowmeterexception = new List<FlowMeter_t>();
            List<Alarm_t> alarm = new List<Alarm_t>();
            alarm = FindAll();
            List<Alarm_t> alarmexception = new List<Alarm_t>();
            foreach (var item in alarm)
            {
                if(flowmeter.Where(p=>p.FM_UId == item.Alarm_DeviceUid) != null)
                {
                    alarmexception.Add(item);
                }
            }
            return alarmexception;
        }

    }
}
