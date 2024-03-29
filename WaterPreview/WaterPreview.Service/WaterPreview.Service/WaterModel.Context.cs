﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class dpnetwork_data_20160419_NewEntities : DbContext
    {
        public dpnetwork_data_20160419_NewEntities()
            : base("name=dpnetwork_data_20160419_NewEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Alarm_t> Alarm_t { get; set; }
        public virtual DbSet<AlarmType_t> AlarmType_t { get; set; }
        public virtual DbSet<Area_t> Area_t { get; set; }
        public virtual DbSet<AreaDevice_t> AreaDevice_t { get; set; }
        public virtual DbSet<AreaUser_t> AreaUser_t { get; set; }
        public virtual DbSet<Authority_t> Authority_t { get; set; }
        public virtual DbSet<DeviceOperationCondition_t> DeviceOperationCondition_t { get; set; }
        public virtual DbSet<Flow_t> Flow_t { get; set; }
        public virtual DbSet<FlowCount_t> FlowCount_t { get; set; }
        public virtual DbSet<FlowDay_t> FlowDay_t { get; set; }
        public virtual DbSet<FlowHour_t> FlowHour_t { get; set; }
        public virtual DbSet<FlowMeter_t> FlowMeter_t { get; set; }
        public virtual DbSet<FlowMeterStatus_t> FlowMeterStatus_t { get; set; }
        public virtual DbSet<FlowMonth_t> FlowMonth_t { get; set; }
        public virtual DbSet<FlowReport_t> FlowReport_t { get; set; }
        public virtual DbSet<FlowYear_t> FlowYear_t { get; set; }
        public virtual DbSet<InnerRole_t> InnerRole_t { get; set; }
        public virtual DbSet<InnerRoleAuthority_t> InnerRoleAuthority_t { get; set; }
        public virtual DbSet<OldQuality_t> OldQuality_t { get; set; }
        public virtual DbSet<Pressure_t> Pressure_t { get; set; }
        public virtual DbSet<PressureDay_t> PressureDay_t { get; set; }
        public virtual DbSet<PressureHour_t> PressureHour_t { get; set; }
        public virtual DbSet<PressureMeter_t> PressureMeter_t { get; set; }
        public virtual DbSet<PressureMeterStatus_t> PressureMeterStatus_t { get; set; }
        public virtual DbSet<PressureMonth_t> PressureMonth_t { get; set; }
        public virtual DbSet<PressureYear_t> PressureYear_t { get; set; }
        public virtual DbSet<Quality_t> Quality_t { get; set; }
        public virtual DbSet<QualityDay_t> QualityDay_t { get; set; }
        public virtual DbSet<QualityHour_t> QualityHour_t { get; set; }
        public virtual DbSet<QualityMeter_t> QualityMeter_t { get; set; }
        public virtual DbSet<QualityMeterStatus_t> QualityMeterStatus_t { get; set; }
        public virtual DbSet<QualityMonth_t> QualityMonth_t { get; set; }
        public virtual DbSet<QualityYear_t> QualityYear_t { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SysInfo_t> SysInfo_t { get; set; }
        public virtual DbSet<User_t> User_t { get; set; }
        public virtual DbSet<UserInnerRole_t> UserInnerRole_t { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}
