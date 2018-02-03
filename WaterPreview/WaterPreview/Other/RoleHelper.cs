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
            public bool FlowMeterAdd { get; set; }
            [DefaultValue(false)]
            public bool FlowMeterDetail { get; set; }
            [DefaultValue(false)]
            public bool FlowMeterModify { get; set; }
            [DefaultValue(false)]
            public bool FlowMeterDelete { get; set; }

            //压力计权限
            [DefaultValue(false)]
            public bool PressureMeterView { get; set; }
            [DefaultValue(false)]
            public bool PressureMeterAdd { get; set; }
            [DefaultValue(false)]
            public bool PressureMeterDetail { get; set; }
            [DefaultValue(false)]
            public bool PressureMeterModify { get; set; }
            [DefaultValue(false)]
            public bool PressureMeterDelete { get; set; }

            //区域管理权限
            [DefaultValue(false)]
            public bool AreaView { get; set; }
            [DefaultValue(false)]
            public bool AreaAdd { get; set; }
            [DefaultValue(false)]
            public bool AreaDetail { get; set; }
            [DefaultValue(false)]
            public bool AreaModify { get; set; }
            [DefaultValue(false)]
            public bool AreaDelete { get; set; }

            //客户管理权限
            [DefaultValue(false)]
            public bool ClientView { get; set; }
            [DefaultValue(false)]
            public bool ClientAdd { get; set; }
            [DefaultValue(false)]
            public bool ClientDetail { get; set; }
            [DefaultValue(false)]
            public bool ClientModify { get; set; }
            [DefaultValue(false)]
            public bool ClientDelete { get; set; }
            //职员管理权限
            [DefaultValue(false)]
            public bool StaffView { get; set; }
            [DefaultValue(false)]
            public bool StaffAdd { get; set; }
            [DefaultValue(false)]
            public bool StaffDetail { get; set; }
            [DefaultValue(false)]
            public bool StaffModify { get; set; }
            [DefaultValue(false)]
            public bool StaffDelete { get; set; }

            //职位管理权限
            [DefaultValue(false)]
            public bool RoleView { get; set; }
            [DefaultValue(false)]
            public bool RoleAdd { get; set; }
            [DefaultValue(false)]
            public bool RoleDetail { get; set; }
            [DefaultValue(false)]
            public bool RoleModify { get; set; }
            [DefaultValue(false)]
            public bool RoleDelete { get; set; }

        }

        public static Role GetAllPermission(Role role)
        {
            //var role = new Role();
            role.FlowMeterView = true;
            role.FlowMeterAdd = true;
            role.FlowMeterDetail = true;
            role.FlowMeterModify = true;
            role.FlowMeterDelete = true;

            role.PressureMeterView = true;
            role.PressureMeterAdd = true;
            role.PressureMeterDetail = true;
            role.PressureMeterModify = true;
            role.PressureMeterDelete = true;

            role.AreaView = true;
            role.AreaAdd = true;
            role.AreaDetail = true;
            role.AreaModify = true;
            role.AreaDelete = true;

            role.ClientView = true;
            role.ClientAdd = true;
            role.ClientDetail = true;
            role.ClientModify = true;
            role.ClientDelete = true;

            role.StaffView = true;
            role.StaffAdd = true;
            role.StaffDetail = true;
            role.StaffModify = true;
            role.StaffDelete = true;


            role.RoleView = true;
            role.RoleAdd = true;
            role.RoleDetail = true;
            role.RoleModify = true;
            role.RoleDelete = true;

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
            role.FlowMeterAdd = true;
            role.FlowMeterDetail = true;
            role.FlowMeterModify = true;
            role.FlowMeterDelete = true;
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
            role.PressureMeterAdd = true;
            role.PressureMeterDetail = true;
            role.PressureMeterModify = true;
            role.PressureMeterDelete = true;
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
            role.AreaAdd = true;
            role.AreaDetail = true;
            role.AreaModify = true;
            role.AreaDelete = true;
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
            role.ClientAdd = true;
            role.ClientDetail = true;
            role.ClientModify = true;
            role.ClientDelete = true;
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
            role.StaffAdd = true;
            role.StaffDetail = true;
            role.StaffModify = true;
            role.StaffDelete = true;
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
            role.RoleAdd = true;
            role.RoleDetail = true;
            role.RoleModify = true;
            role.RoleDelete = true;
            return role;
        }
    }
}