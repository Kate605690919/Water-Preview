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
        //后续添加水压计和水质计的分析数据
    }

    public class FlowMeterData
    {
        public FlowMeter_t flowmeter { get; set; }
        public string lastday_flow { get; set; }
        public string lastday_flow_proportion { get; set; }
        public string night_flow { get; set; }
        public string night_flow_proportion { get; set; }
        public string month_flow { get; set; }
        public string month_flow_proportion { get; set; }

    }


}
