using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;

namespace WaterPreview.Other
{
    public class UserContext
    {

        static IAccountService account_service = new AccountService();
        static IAreaUserService areauser_service = new AreaUserService();


        public static User_t account = new User_t();

        public static Guid areaSourceUid = Guid.Parse("6F6B8DB5-1202-4644-B1B2-A52284D73E07");
        public static string allArea = "AllArea";
        public static string allFlowMeter = "AllFlowMeter";
        public static string allFlowMeterStatusAndArea = "allFlowMeterStatusAndArea";
        public static string allPressureMeterStatusAndArea = "allPressureMeterStatusAndArea";
        public static string allQualityMeterStatusAndArea = "allQualityMeterStatusAndArea";


        public static User_t GetCurrentAccount()
        {
            if (HttpContext.Current.Request.Cookies.Count != 0 && HttpContext.Current.Request.Cookies["username"].Value != null)
            {
                Guid uid = Guid.Parse(HttpContext.Current.Request.Cookies["username"].Value);
                account = account_service.GetAccountByUid(uid);
            }
            else
            {
                account = new User_t();
            }
            return account;
        }

        public static Guid GetAreaByUserUid(Guid useruid)
        {
            AreaUser_t areauser = areauser_service.GetAllAreaUser().Where(p=>p.AU_UserUId==useruid).FirstOrDefault();
            if (areauser.AU_UId == new Guid())
            {
                return areaSourceUid;
            }
            return areauser.AU_AreaUId;
        }
    }
}