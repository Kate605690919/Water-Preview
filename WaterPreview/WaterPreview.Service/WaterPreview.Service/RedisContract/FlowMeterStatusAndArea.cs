using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WaterPreview.Service;

namespace WaterPreview.Service.RedisContract
{
    public class FlowMeterStatusAndArea
    {
        public  FlowMeter_t flowmeter {get;set;}
        public  FlowMeterStatus_t status { get; set; }
        public  Area_t area { get; set; }
    }
}