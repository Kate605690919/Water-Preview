using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.other
{
    public partial class GetSubAreaList
    {

        public GetSubAreaList(Guid areaUid){
            RecursionAreaNode(areaUid);
        }

        public List<Area_t> Area_SubNode = new List<Area_t>();

        private void RecursionAreaNode(Guid areaUid)
        {
            IAreaService area_service = new AreaService();

            if (areaUid == null || areaUid == new Guid()) return;

            List<Area_t> childarealist = area_service.GetAllArea().Where(p => p.Ara_Up == areaUid).OrderBy(p => p.Ara_Code).ToList();
            if (childarealist.Count > 0)
            {
                foreach (var item in childarealist)
                {
                    this.Area_SubNode.Add(item);
                    RecursionAreaNode(item.Ara_UId);
                }
            }
            else
            {
                //return subarealist;
            }
        }
    }
}
