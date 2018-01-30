using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.other;
using WaterPreview.Service.Service;

namespace WaterPreview.Service.Interface
{
    public class AreaService:BaseService<Area_t>,IAreaService
    {


        public Area_t GetAreaByUserUid(Guid useruid)
        {
            IAreaUserService areauser_service = new AreaUserService();
            AreaUser_t areauser = areauser_service.GetAllAreaUser().Where(p => p.AU_UserUId == useruid).SingleOrDefault();
            return FindAll().Where(p => p.Ara_UId == areauser.AU_AreaUId).SingleOrDefault();
        }

        public Area_t GetAreaByDeviceUid(Guid deviceUid)
        {
            IAreaDeviceService ad_service = new AreaDeviceService();
            AreaDevice_t ad = ad_service.GetAreaDeviceByDeviceUid(deviceUid);
            return ad.AD_AreaUid==new Guid()?new Area_t():FindAll().Where(p => p.Ara_UId == ad.AD_AreaUid).FirstOrDefault();
        }

        public List<Area_t> GetAllArea()
        {
            return FindAll();
        }

        /// <summary>
        /// 获取本区域及所有子区域
        /// </summary>
        /// <param name="areaUid"></param>
        /// <returns></returns>
        public List<Area_t> GetSubArea(Guid areaUid)
        {
            List<Area_t> arealist = new List<Area_t>();
            Area_t area = FindAll().Where(p=>p.Ara_UId==areaUid).FirstOrDefault();
            arealist.Add(area);
            GetSubAreaList subarea = new GetSubAreaList(areaUid);
            arealist.AddRange(subarea.Area_SubNode);

            return arealist;
        }

        
    }
}
