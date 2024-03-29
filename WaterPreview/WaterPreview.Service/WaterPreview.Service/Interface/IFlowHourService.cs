﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Interface
{
    public interface IFlowHourService
    {

        List<FlowHour_t> GetFlowHourByFMUid(Guid fmUid);

        List<FlowHour_t> GetTimeFlowHourByUid(Guid fmUid, int startTime, int endTime);

        List<FlowHour_t> GetMonthFlowByUidAndDate(Guid uid, DateTime date);
    }
}
