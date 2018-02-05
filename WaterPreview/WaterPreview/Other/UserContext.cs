using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;
using WaterPreview.Service.Service;

namespace WaterPreview.Other
{
    public class UserContext
    {

        static IAccountService account_service = new AccountService();
        static IAreaUserService areauser_service = new AreaUserService();
        static IAreaDeviceService areadevice_service = new AreaDeviceService();

        public static User_t account;

        public static string access_token;

        public static Guid AreaSourceUid = Guid.Parse("6F6B8DB5-1202-4644-B1B2-A52284D73E07");
        

        public static User_t GetCurrentAccount()
        {
            //if (HttpContext.Current.Request.Cookies.Count != 0 && HttpContext.Current.Request.Cookies[ConfigurationManager.AppSettings["CookieName"]].Value != null)
            //{
            //    Guid uid = Guid.Parse(HttpContext.Current.Request.Cookies[ConfigurationManager.AppSettings["CookieName"]].Value);
            //    account = uid == new Guid() ? new User_t() : account_service.GetAccountByUid(uid);
            //}
            //else
            //{
            //    account = new User_t();
            //}
            return account;
        }

        public static Guid GetAreaByUserUid(Guid useruid)
        {
            AreaUser_t areauser = areauser_service.GetAllAreaUser().FirstOrDefault(p => p.AU_UserUId==useruid);
            if (areauser == null || areauser.AU_UId == new Guid())
            {
                return AreaSourceUid;
            }
            return areauser.AU_AreaUId;
        }

        
    }
}