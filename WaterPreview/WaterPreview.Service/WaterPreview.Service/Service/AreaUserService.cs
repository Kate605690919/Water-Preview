using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class AreaUserService:BaseService<AreaUser_t>,IAreaUserService
    {
        public List<AreaUser_t> GetAllAreaUser()
        {
            return FindAll();
        }

       
    }
}
