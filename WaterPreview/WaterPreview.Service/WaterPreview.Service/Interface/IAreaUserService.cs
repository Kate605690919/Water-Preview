using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;

namespace WaterPreview.Service.Interface
{
    public interface IAreaUserService
    {
        List<AreaUser_t> GetAllAreaUser();
    }
}
