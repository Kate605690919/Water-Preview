using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class FlowMonthService:BaseService<FlowMonth_t>,IFlowMonthService
    {
        public List<FlowMonth_t> GetAllFlowMonth()
        {
            return FindAll();
        }
    }
}
