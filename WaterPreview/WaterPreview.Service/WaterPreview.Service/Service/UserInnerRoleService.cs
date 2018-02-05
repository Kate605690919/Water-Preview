using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class UserInnerRoleService: BaseService<UserInnerRole_t>, IUserInnerRoleService
    {
        public List<UserInnerRole_t> GetByUid(Guid uid)
        {
            return FindAll().Where(p => p.UIr_UserUId == uid).ToList();
        } 
    }
}
