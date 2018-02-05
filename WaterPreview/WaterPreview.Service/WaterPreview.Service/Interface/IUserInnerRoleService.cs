using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Interface
{
    public interface IUserInnerRoleService
    {
        List<UserInnerRole_t> GetByUid(Guid uid);
    }
}
