using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class FlowDayService:BaseService<FlowDay_t>,IFlowDayService
    {

        public List<FlowDay_t> GetAllFlowDayByFMUid(Guid fmuid)
        {
            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
            return db.FlowDay_t.Where(p=>p.Fld_FlowMeterUid==fmuid).ToList();
            //return fdlist.Count() == 0 ? new List<FlowDay_t>() : fdlist.ToList();
        }

        
    }
}
