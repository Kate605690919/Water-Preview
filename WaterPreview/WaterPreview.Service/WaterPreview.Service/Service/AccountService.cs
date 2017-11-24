using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;

namespace WaterPreview.Service.Service
{
    public class AccountService : BaseService<User_t>,IAccountService
    {
        public List<User_t> GetAllAccount()
        {
            return FindAll().ToList();
        }

        public User_t GetAccountByUid(Guid uid)
        {
            return FindAll().Where(p => p.Usr_UId == uid).SingleOrDefault();
        }

        public User_t GetAccountByName(string name)
        {
            return FindAll().Where(p => p.Usr_Name == name).SingleOrDefault();
        }
    }
}
