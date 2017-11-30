using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.RedisContract
{
    public class VisitCount
    {
        public string uid { get; set; }
        public int count { get; set; }
    }
}
