using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.RedisContract
{
    public class QualityMeterStatusAndArea
    {

        public  QualityMeter_t qualitymeter { get; set; }
        public  QualityMeterStatus_t status { get; set; }
        public  Area_t area { get; set; }
    }
}
