//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WaterPreview.Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class QualityMeterStatus_t
    {
        public long QMS_Id { get; set; }
        public System.Guid QMS_DeviceUid { get; set; }
        public decimal QMS_T1 { get; set; }
        public decimal QMS_T2 { get; set; }
        public decimal QMS_T3 { get; set; }
        public decimal QMS_T4 { get; set; }
        public decimal QMS_Q1 { get; set; }
        public decimal QMS_Q2 { get; set; }
        public decimal QMS_Q3 { get; set; }
        public decimal QMS_Q4 { get; set; }
        public System.DateTime QMS_UpdateDt { get; set; }
        public Nullable<decimal> QMS_BatteryStatus { get; set; }
        public Nullable<decimal> QMS_AntennaSignal { get; set; }
        public Nullable<int> QMS_IsTimeOverException { get; set; }
        public Nullable<int> QMS_IsMainBatteryException { get; set; }
        public Nullable<int> QMS_IsModemBatteryException { get; set; }
        public Nullable<int> QMS_ValueExceptionNum { get; set; }
    }
}
