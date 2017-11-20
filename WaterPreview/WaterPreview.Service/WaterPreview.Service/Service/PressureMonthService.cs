using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class PressureMonthService:BaseService<PressureMonth_t>,IPressureMonthService
    {
        public List<PressureMonth_t> GetAllPressureMonth()
        {
            return FindAll();
        }
    }
}
