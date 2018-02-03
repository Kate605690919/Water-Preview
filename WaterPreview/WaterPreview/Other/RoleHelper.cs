using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WaterPreview.Other
{
    public class RoleHelper
    {
        public class Role
        {
            //流量计权限
            [DefaultValue(false)]
            public bool FlowMeterView { get; set; }
            [DefaultValue(false)]
            public bool FlowMeterManage { get; set; }

            //压力计权限
            [DefaultValue(false)]
            public bool PressureMeterView { get; set; }
            [DefaultValue(false)]
            public bool PressureMeterManage { get; set; }

            //水质计权限
            [DefaultValue(false)]
            public bool QualityMeterView { get; set; }
            [DefaultValue(false)]
            public bool QualityMeterManage { get; set; }

            //区域管理权限
            [DefaultValue(false)]
            public bool AreaView { get; set; }
            [DefaultValue(false)]
            public bool AreaManage { get; set; }


            //客户管理权限
            [DefaultValue(false)]
            public bool ClientView { get; set; }
            [DefaultValue(false)]
            public bool ClientManage { get; set; }

            //职员管理权限
            [DefaultValue(false)]
            public bool StaffView { get; set; }
            [DefaultValue(false)]
            public bool StaffManage { get; set; }


            //职位管理权限
            [DefaultValue(false)]
            public bool RoleView { get; set; }
            [DefaultValue(false)]
            public bool RoleManage { get; set; }

        }

        public static Role GetAllPermission(Role role)
        {
            //var role = new Role();
            role.FlowMeterView = true;
            role.FlowMeterManage = true;

            role.PressureMeterView = true;
            role.PressureMeterManage = true;

            role.QualityMeterView = true;
            role.QualityMeterManage = true;

            role.AreaView = true;
            role.AreaManage = true;

            role.ClientView = true;
            role.ClientManage = true;

            role.StaffView = true;
            role.StaffManage = true;

            role.RoleView = true;
            role.RoleManage = true;

            return role;
        }

        public static Role GetFlowMeterViewPermission(Role role)
        {
            //var role = new Role();
            role.FlowMeterView = true;
            return role;
        }

        public static Role GetFlowMeterManagePermission(Role role)
        {
            //var role = new Role();
            role.FlowMeterView = true;
            role.FlowMeterManage = true;
            return role;
        }

        public static Role GetPressureMeterViewPermission(Role role)
        {
            //var role = new Role();
            role.PressureMeterView = true;
            return role;
        }

        public static Role GetPressureMeterManagePermission(Role role)
        {
            //var role = new Role();
            role.PressureMeterView = true;
            role.PressureMeterManage = true;
            return role;
        }

        public static Role GetQualityMeterViewPermission(Role role)
        {
            //var role = new Role();
            role.QualityMeterView = true;
            return role;
        }

        public static Role GetQualityMeterManagePermission(Role role)
        {
            //var role = new Role();
            role.QualityMeterView = true;
            role.QualityMeterManage = true;
            return role;
        }

        public static Role GetAreaViewPermission(Role role)
        {
            //var role = new Role();
            role.AreaView = true;
            return role;
        }

        public static Role GetAreaManagePermission(Role role)
        {
            //var role = new Role();
            role.AreaView = true;
            role.AreaManage = true;
            return role;
        }

        public static Role GetClientViewPermission(Role role)
        {
            //var role = new Role();
            role.ClientView = true;
            return role;
        }

        public static Role GetClientManagePermission(Role role)
        {
            //var role = new Role();
            role.ClientView = true;
            role.ClientManage = true;
            return role;
        }

        public static Role GetStaffViewPermission(Role role)
        {
            //var role = new Role();
            role.StaffView = true;
            return role;
        }

        public static Role GetStaffManagePermission(Role role)
        {
            //var role = new Role();
            role.StaffView = true;
            role.StaffManage = true;
            return role;
        }

        public static Role GetRolesViewPermission(Role role)
        {
            //var role = new Role();
            role.RoleView = true;
            return role;
        }

        public static Role GetRolesManagePermission(Role role)
        {
            //var role = new Role();
            role.RoleView = true;
            role.RoleManage = true;
            return role;
        }
    }
}