﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterPreview.Service.RedisContract;

namespace WaterPreview.Service.Interface
{
    public interface IAccountService
    {
        List<User_t> GetAllAccount();

        User_t GetAccountByUid(Guid uid);

        User_t GetAccountByName(string name);

        List<VisitCount> AddDeviceVisits(List<VisitCount> dvlist, Guid deviceUid);

        bool ValidateAccount(string username, string password);
    }
}
