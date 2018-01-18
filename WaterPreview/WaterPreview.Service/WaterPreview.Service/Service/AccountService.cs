using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.Base;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;

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
            var user = FindAll().Where(p => p.Usr_UId == uid).ToList();
            return user.Count==0?new User_t():user.FirstOrDefault();
        }

        public User_t GetAccountByName(string name)
        {
            var user = FindAll().Where(p => p.Usr_Name == name).ToList();
            return user.Count==0?new User_t():user.FirstOrDefault();
        }

        public List<VisitCount> AddDeviceVisits(List<VisitCount> dvlist, Guid deviceUid)
        {
            
            var vc = dvlist.Count==0?new VisitCount():dvlist.Where(p => p.uid == deviceUid.ToString()).FirstOrDefault();
            if (vc==null||vc.count==0)
            {
                VisitCount new_vc = new VisitCount(){
                    uid = deviceUid.ToString(),
                    count = 1,
                };
                dvlist.Add(new_vc);
            }
            else vc.count += 1;
            return dvlist;
        }

        public bool ValidateAccount(string username, string password)
        {
            User_t user = GetAccountByName(username);
            if (user.Usr_UId == new Guid() || user.Usr_Password != password)
            {
                return false;
            }
            return true;
        }

    }
}
