using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.RedisContract
{
    public class DevicesDataAndUser
    {
        public User_t account{get;set;}
        public List<FlowMeterData> flowmeterdata{get;set;}
        //public List<PressureMeter_t> pressuremeters;
        //public List<QualityMeter_t> qualitymeters;
    }

    public class FlowMeterData
    {
        public FlowMeter_t flowmeter { get; set; }
        public double lastday_flow { get; set; }
        public double lastday_flow_proportion { get; set; }
        public double night_flow { get; set; }
        public double night_flow_proportion { get; set; }
        public double month_flow { get; set; }
        public double month_flow_proportion { get; set; }

    }


}
