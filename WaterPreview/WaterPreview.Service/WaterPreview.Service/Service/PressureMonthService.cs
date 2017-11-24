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

        public List<PressureMonth_t> GetPressureMonthByPMUid(Guid pmUid)
        {
            List<PressureMonth_t> pmlist = FindAll().Where(p => p.PM_PressureMeterUid == pmUid).ToList();
            return pmlist.Count==0?new List<PressureMonth_t>():pmlist;
        }
    }
}
