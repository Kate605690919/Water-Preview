using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
   public  class RoleService: BaseService<InnerRole_t>,IRoleService
    {
       public InnerRole_t GetRoles(Guid uid)
       {
           return FindAll().FirstOrDefault(p=>p.Ir_UId==uid);
       } 
    }
}
