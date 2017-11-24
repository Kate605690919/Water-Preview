using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.RedisContract
{
    public class PressureMeterStatusAndArea
    {
        public  PressureMeter_t pressuremeter { get; set; }
        public  PressureMeterStatus_t status { get; set; }
        public  Area_t area { get; set; }
    }
}
