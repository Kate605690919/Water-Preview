using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterPreview.Redis
{
    public class DBHelper
    {
        /// <summary>
        /// redis的主机
        /// </summary>
        public static string host = "192.168.2.70";
        /// <summary>
        /// redis的数据库引索，有点像sql server的数据库名
        /// </summary>
        public static int DB_INDEX = 1;
        /// <summary>
        /// 待使用的键名，较长而且会使用多次，所以声明为变量
        /// </summary>
        public static string ALLFLOWMETER = "AllFlowMeter";
        public static string ALLAREA = "ALLAREA";
        public static string ALLPRESSUREMETER = "AllPressureMeter";
        public static string ALLQUALITYMETER = "AllQualityMeter";


        public static List<T> get<T>(Func<List<T>> init, string key, int dbindex = 1)
        {
            using (RedisClient rc = new RedisClient(host))
            {
                rc.Select(dbindex);
                var name = key;
                if (!rc.Exists(name))
                {
                    var list = init();
                    rc.Set(name, JsonConvert.SerializeObject(list));
                    //设置值的过期时间为24小时
                    rc.Expire(name, new TimeSpan(24, 0, 0));
                    return list;
                }
                return JsonConvert.DeserializeObject<List<T>>(rc.Get(name));

            }
        }

        public static void ClearCache()
        {
            try
            {
                //if (!IsOpenCache) return;
                using (IRedisClient Redis = new RedisClient(host))
                {
                    Redis.FlushAll();
                }
            }
            catch { return; }
        } 

        //#region 获取流量计、水压计、水质计和区域列表
        ///// <summary>
        ///// 获取所有流量计
        ///// </summary>
        ///// <returns>流量计、流量计状态、对应区域列表</returns>
        //public static List<FlowMeterJoinAreaStatus> getallflowmeter()
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLFLOWMETER;
        //        //如果不存在ALLFLOWMETER这个名称的值的话
        //        if (!rc.Exists(name))
        //        {
        //            //去数据库读取所有流量计的值
        //            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
        //            List<FlowMeterJoinAreaStatus> djasList = new List<FlowMeterJoinAreaStatus>();

        //            List<FlowMeter_t> devicelist = db.FlowMeter_t.ToList();
        //            foreach (var item in devicelist)
        //            {
        //                FlowMeterJoinAreaStatus djas = new FlowMeterJoinAreaStatus();
        //                var areadevice = db.AreaDevice_t.Where(p => p.AD_DeviceUid == item.FM_UId).FirstOrDefault();

        //                djas.flowmeter = item;
        //                if (areadevice != null)
        //                {
        //                    djas.status = db.FlowMeterStatus_t.Where(p => p.FMS_DeviceUid == areadevice.AD_DeviceUid).FirstOrDefault();
        //                    djas.area = db.Area_t.Where(p => p.Ara_UId == areadevice.AD_AreaUid).FirstOrDefault();
        //                }
        //                else
        //                {
        //                    djas.status = new FlowMeterStatus_t();
        //                    djas.area = new Area_t();
        //                }
        //                djasList.Add(djas);

        //            }

        //            //将值转换为json格式后，设置值
        //            rc.Set(name, JsonConvert.SerializeObject(djasList));
        //            //设置值的过期时间为24小时          
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //            return djasList;
        //        }
        //        //如果存在的话，则从redis中读取出ALLFLOWMETER这个名称的值
        //        return JsonConvert.DeserializeObject<List<FlowMeterJoinAreaStatus>>(rc.Get(name));
        //    }
        //}

        ///// <summary>
        ///// 获取所有水压计
        ///// </summary>
        ///// <returns>水压计、水压计状态、对应区域列表</returns>
        //public static List<PressureMeterJoinAreaStatus> getallpressuremeter()
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLPRESSUREMETER;
        //        if (!rc.Exists(name))
        //        {

        //            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
        //            List<PressureMeterJoinAreaStatus> djasList = new List<PressureMeterJoinAreaStatus>();

        //            List<PressureMeter_t> devicelist = db.PressureMeter_t.ToList();
        //            foreach (var item in devicelist)
        //            {
        //                PressureMeterJoinAreaStatus djas = new PressureMeterJoinAreaStatus();
        //                var areadevice = db.AreaDevice_t.Where(p => p.AD_DeviceUid == item.PM_UId).FirstOrDefault();
        //                djas.pressuremeter = item;
        //                if (areadevice != null)
        //                {
        //                    djas.status = db.PressureMeterStatus_t.Where(p => p.PMS_DeviceUid == areadevice.AD_DeviceUid).FirstOrDefault();
        //                    djas.area = db.Area_t.Where(p => p.Ara_UId == areadevice.AD_AreaUid).FirstOrDefault();
        //                }
        //                else
        //                {
        //                    djas.status = new PressureMeterStatus_t();
        //                    djas.area = new Area_t();
        //                }
        //                djasList.Add(djas);

        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(djasList));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //            return djasList;

        //        }
        //        return JsonConvert.DeserializeObject<List<PressureMeterJoinAreaStatus>>(rc.Get(name));
        //    }
        //}

        ///// <summary>
        ///// 获取所有水质计
        ///// </summary>
        ///// <returns>水质计、水质计状态、对应区域列表</returns>
        //public static List<QualityMeterJoinAreaStatus> getallqualitymeter()
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLQUALITYMETER;
        //        if (!rc.Exists(name))
        //        {

        //            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
        //            List<QualityMeterJoinAreaStatus> djasList = new List<QualityMeterJoinAreaStatus>();

        //            List<QualityMeter_t> devicelist = db.QualityMeter_t.ToList();
        //            foreach (var item in devicelist)
        //            {
        //                QualityMeterJoinAreaStatus djas = new QualityMeterJoinAreaStatus();
        //                var areadevice = db.AreaDevice_t.Where(p => p.AD_DeviceUid == item.QM_UId).FirstOrDefault();
        //                djas.qualitymeter = item;
        //                if (areadevice != null)
        //                {
        //                    djas.status = db.QualityMeterStatus_t.Where(p => p.QMS_DeviceUid == areadevice.AD_DeviceUid).FirstOrDefault();
        //                    djas.area = db.Area_t.Where(p => p.Ara_UId == areadevice.AD_AreaUid).FirstOrDefault();
        //                }
        //                else
        //                {
        //                    djas.status = new QualityMeterStatus_t();
        //                    djas.area = new Area_t();
        //                }
        //                djasList.Add(djas);

        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(djasList));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //            return djasList;

        //        }
        //        return JsonConvert.DeserializeObject<List<QualityMeterJoinAreaStatus>>(rc.Get(name));
        //    }
        //}

        ///// <summary>
        ///// 获取所有区域列表
        ///// </summary>
        ///// <returns></returns>
        //public static List<Area_t> getallarea()
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLAREA;
        //        if (!rc.Exists(name))
        //        {
        //            dpnetwork_data_20160419_NewEntities db = new dpnetwork_data_20160419_NewEntities();
        //            var list = db.Area_t.ToList();
        //            rc.Set(name, JsonConvert.SerializeObject(list));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //            return list;
        //        }
        //        return JsonConvert.DeserializeObject<List<Area_t>>(rc.Get(name));
        //    }
        //}

        //#endregion

        //#region 修改流量计、水压计、水质计或区域
        ///// <summary>
        ///// 修改allarea、allflowmeter、allpressuremeter、allqualitymeter中的area
        ///// </summary>
        ///// <param name="area"></param>
        ///// <returns></returns>
        //public static bool changeArea(Area_t area)
        //{
        //    using (var rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var areaname = ALLAREA;
        //        var flowmetername = ALLFLOWMETER;
        //        var pressuremetername = ALLPRESSUREMETER;
        //        var qualitymetername = ALLQUALITYMETER;

        //        if (!rc.Exists(areaname) || !rc.Exists(flowmetername) || !rc.Exists(pressuremetername) || !rc.Exists(qualitymetername))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<Area_t> arealist = JsonConvert.DeserializeObject<List<Area_t>>(rc.Get(areaname));
        //            List<FlowMeterJoinAreaStatus> fmlist = JsonConvert.DeserializeObject<List<FlowMeterJoinAreaStatus>>(rc.Get(flowmetername));
        //            List<PressureMeterJoinAreaStatus> pmlist = JsonConvert.DeserializeObject<List<PressureMeterJoinAreaStatus>>(rc.Get(pressuremetername));
        //            List<QualityMeterJoinAreaStatus> qmlist = JsonConvert.DeserializeObject<List<QualityMeterJoinAreaStatus>>(rc.Get(qualitymetername));

        //            foreach (var item in arealist)
        //            {
        //                if (item.Ara_UId == area.Ara_UId)
        //                {
        //                    item.Ara_Name = area.Ara_Name;
        //                    item.Ara_Lat = area.Ara_Lat;
        //                    item.Ara_Lng = area.Ara_Lng;
        //                    item.Ara_Level = area.Ara_Level;
        //                    item.Ara_Up = area.Ara_Up;
        //                    item.Ara_Enable = area.Ara_Enable;
        //                    item.Ara_Description = area.Ara_Description;
        //                    item.Ara_Code = area.Ara_Code;
        //                }
        //            }
        //            rc.Set(areaname, JsonConvert.SerializeObject(arealist));
        //            rc.Expire(areaname, new TimeSpan(24, 0, 0));

        //            foreach (var item in fmlist)
        //            {
        //                if (item.area.Ara_UId == area.Ara_UId)
        //                {
        //                    item.area.Ara_Name = area.Ara_Name;
        //                    item.area.Ara_Lat = area.Ara_Lat;
        //                    item.area.Ara_Lng = area.Ara_Lng;
        //                    item.area.Ara_Level = area.Ara_Level;
        //                    item.area.Ara_Up = area.Ara_Up;
        //                    item.area.Ara_Enable = area.Ara_Enable;
        //                    item.area.Ara_Description = area.Ara_Description;
        //                    item.area.Ara_Code = area.Ara_Code;
        //                }
        //            }
        //            rc.Set(flowmetername, JsonConvert.SerializeObject(fmlist));
        //            rc.Expire(flowmetername, new TimeSpan(24, 0, 0));

        //            foreach (var item in pmlist)
        //            {
        //                if (item.area.Ara_UId == area.Ara_UId)
        //                {
        //                    item.area.Ara_Name = area.Ara_Name;
        //                    item.area.Ara_Lat = area.Ara_Lat;
        //                    item.area.Ara_Lng = area.Ara_Lng;
        //                    item.area.Ara_Level = area.Ara_Level;
        //                    item.area.Ara_Up = area.Ara_Up;
        //                    item.area.Ara_Enable = area.Ara_Enable;
        //                    item.area.Ara_Description = area.Ara_Description;
        //                    item.area.Ara_Code = area.Ara_Code;
        //                }
        //            }
        //            rc.Set(pressuremetername, JsonConvert.SerializeObject(pmlist));
        //            rc.Expire(pressuremetername, new TimeSpan(24, 0, 0));

        //            foreach (var item in qmlist)
        //            {
        //                if (item.area.Ara_UId == area.Ara_UId)
        //                {
        //                    item.area.Ara_Name = area.Ara_Name;
        //                    item.area.Ara_Lat = area.Ara_Lat;
        //                    item.area.Ara_Lng = area.Ara_Lng;
        //                    item.area.Ara_Level = area.Ara_Level;
        //                    item.area.Ara_Up = area.Ara_Up;
        //                    item.area.Ara_Enable = area.Ara_Enable;
        //                    item.area.Ara_Description = area.Ara_Description;
        //                    item.area.Ara_Code = area.Ara_Code;
        //                }
        //            }
        //            rc.Set(qualitymetername, JsonConvert.SerializeObject(qmlist));
        //            rc.Expire(qualitymetername, new TimeSpan(24, 0, 0));
        //        }


        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 修改allqualitymeter中的qualitymeter
        ///// </summary>
        ///// <param name="qualitymeter"></param>
        ///// <returns></returns>
        //public static bool ChangeQualityMeter(QualityMeter_t qualitymeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLQUALITYMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<QualityMeterJoinAreaStatus> qmas = JsonConvert.DeserializeObject<List<QualityMeterJoinAreaStatus>>(rc.Get(name));
        //            foreach (var item in qmas)
        //            {
        //                if (item.qualitymeter.QM_UId == qualitymeter.QM_UId)
        //                {
        //                    item.qualitymeter.QM_Code = qualitymeter.QM_Code;
        //                    item.qualitymeter.QM_Lat = qualitymeter.QM_Lat;
        //                    item.qualitymeter.QM_Lng = qualitymeter.QM_Lng;
        //                    item.qualitymeter.QM_Description = qualitymeter.QM_Description;
        //                    item.qualitymeter.QM_CountLast = qualitymeter.QM_CountLast;
        //                    item.qualitymeter.QM_MapId = qualitymeter.QM_MapId;
        //                    item.qualitymeter.QM_RecevieLast = qualitymeter.QM_RecevieLast;
        //                }
        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(qmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 修改allflowmeter中的flowmeter
        ///// </summary>
        ///// <param name="qualitymeter"></param>
        ///// <returns></returns>
        //public static bool ChangeFlowMeter(FlowMeter_t flowmeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLFLOWMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<FlowMeterJoinAreaStatus> fmas = JsonConvert.DeserializeObject<List<FlowMeterJoinAreaStatus>>(rc.Get(name));
        //            foreach (var item in fmas)
        //            {
        //                if (item.flowmeter.FM_UId == flowmeter.FM_UId)
        //                {
        //                    item.flowmeter.FM_AlarmMode = flowmeter.FM_AlarmMode;
        //                    item.flowmeter.FM_AlarmNumber = flowmeter.FM_AlarmNumber;
        //                    item.flowmeter.FM_AlarmStatus = flowmeter.FM_AlarmStatus;
        //                    item.flowmeter.FM_AlarmThreshold = flowmeter.FM_AlarmThreshold;
        //                    item.flowmeter.FM_AlarmTimeout = flowmeter.FM_AlarmTimeout;
        //                    item.flowmeter.FM_BatteryAlarmThreshold = flowmeter.FM_BatteryAlarmThreshold;
        //                    item.flowmeter.FM_Class = flowmeter.FM_Class;
        //                    item.flowmeter.FM_Code = flowmeter.FM_Code;
        //                    item.flowmeter.FM_Description = flowmeter.FM_Description;
        //                    item.flowmeter.FM_DeviceAlarmNumber = flowmeter.FM_DeviceAlarmNumber;
        //                    item.flowmeter.FM_Enable = flowmeter.FM_Enable;
        //                    item.flowmeter.FM_FlowAlarmEnable = flowmeter.FM_FlowAlarmEnable;
        //                    item.flowmeter.FM_FlowAlarmLasttime = flowmeter.FM_FlowAlarmLasttime;
        //                    item.flowmeter.FM_FlowAlarmMode = flowmeter.FM_FlowAlarmMode;
        //                    item.flowmeter.FM_FlowAlarmThreshold = flowmeter.FM_FlowAlarmThreshold;
        //                    item.flowmeter.FM_FlowCountLast = flowmeter.FM_FlowCountLast;
        //                    item.flowmeter.FM_FlowHourAlarmThreshold = flowmeter.FM_FlowHourAlarmThreshold;
        //                    item.flowmeter.FM_IsLeaf = flowmeter.FM_IsLeaf;
        //                    item.flowmeter.FM_Lat = flowmeter.FM_Lat;
        //                    item.flowmeter.FM_Level = flowmeter.FM_Level;
        //                    item.flowmeter.FM_Lng = flowmeter.FM_Lng;
        //                    item.flowmeter.FM_mLevel = flowmeter.FM_mLevel;
        //                    item.flowmeter.FM_ModemAlarmThreshold = flowmeter.FM_ModemAlarmThreshold;
        //                    item.flowmeter.FM_ParentFlowMeterUId = flowmeter.FM_ParentFlowMeterUId;
        //                    item.flowmeter.FM_WaterConsumerUId = flowmeter.FM_WaterConsumerUId;

        //                }
        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(fmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 修改allpressuremeter中的pressuremeter
        ///// </summary>
        ///// <param name="qualitymeter"></param>
        ///// <returns></returns>
        //public static bool ChangePressureMeter(PressureMeter_t pressuremeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLPRESSUREMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<PressureMeterJoinAreaStatus> pmas = JsonConvert.DeserializeObject<List<PressureMeterJoinAreaStatus>>(rc.Get(name));
        //            foreach (var item in pmas)
        //            {
        //                if (item.pressuremeter.PM_UId == pressuremeter.PM_UId)
        //                {
        //                    item.pressuremeter.PM_Code = pressuremeter.PM_Code;
        //                    item.pressuremeter.PM_CountLast = pressuremeter.PM_CountLast;
        //                    item.pressuremeter.PM_Description = pressuremeter.PM_Description;
        //                    item.pressuremeter.PM_Lat = pressuremeter.PM_Lat;
        //                    item.pressuremeter.PM_Lng = pressuremeter.PM_Lng;
        //                }
        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(pmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        //#endregion

        //#region 添加流量计、水压计、水质计或区域
        ///// <summary>
        ///// 添加流量计，区域和流量计详情为空
        ///// </summary>
        ///// <param name="flowmeter"></param>
        ///// <returns></returns>
        //public static bool AddFlowMeter(FlowMeter_t flowmeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLFLOWMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<FlowMeterJoinAreaStatus> fmas = JsonConvert.DeserializeObject<List<FlowMeterJoinAreaStatus>>(rc.Get(name));
        //            FlowMeterJoinAreaStatus fm = new FlowMeterJoinAreaStatus()
        //            {
        //                flowmeter = flowmeter,//此类无Id值
        //                area = new Area_t(),
        //                status = new FlowMeterStatus_t()
        //            };

        //            fmas.Add(fm);
        //            rc.Set(name, JsonConvert.SerializeObject(fmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 添加水压计，区域和压力计详情为空
        ///// </summary>
        ///// <param name="pressuremeter"></param>
        ///// <returns></returns>
        //public static bool AddPressureMeter(PressureMeter_t pressuremeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLPRESSUREMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<PressureMeterJoinAreaStatus> qmas = JsonConvert.DeserializeObject<List<PressureMeterJoinAreaStatus>>(rc.Get(name));
        //            PressureMeterJoinAreaStatus pm = new PressureMeterJoinAreaStatus()
        //            {
        //                pressuremeter = pressuremeter,//此类无Id值
        //                area = new Area_t(),
        //                status = new PressureMeterStatus_t()
        //            };

        //            qmas.Add(pm);
        //            rc.Set(name, JsonConvert.SerializeObject(qmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 添加水质计，区域和水质计详情为空
        ///// </summary>
        ///// <param name="qualitymeter"></param>
        ///// <returns></returns>
        //public static bool AddQualityMeter(QualityMeter_t qualitymeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLQUALITYMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<QualityMeterJoinAreaStatus> qmas = JsonConvert.DeserializeObject<List<QualityMeterJoinAreaStatus>>(rc.Get(name));
        //            QualityMeterJoinAreaStatus qm = new QualityMeterJoinAreaStatus()
        //            {
        //                qualitymeter = qualitymeter,//此类无Id值
        //                area = new Area_t(),
        //                status = new QualityMeterStatus_t()
        //            };

        //            qmas.Add(qm);
        //            rc.Set(name, JsonConvert.SerializeObject(qmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 给区域列表添加区域
        ///// </summary>
        ///// <param name="area"></param>
        ///// <returns></returns>
        //public static bool AddArea(Area_t area)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        var areaname = ALLAREA;
        //        if (!rc.Exists(areaname))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<Area_t> arealist = JsonConvert.DeserializeObject<List<Area_t>>(rc.Get(areaname));
        //            arealist.Add(area);
        //            rc.Set(areaname, JsonConvert.SerializeObject(arealist));
        //            rc.Expire(areaname, new TimeSpan(24, 0, 0));

        //            //无需给ALLFLOWMETER、ALLPRESSUREMETER、ALLQUALITYMETER单独增加area属性
        //        }
        //    }
        //    return true;
        //}
        //#endregion

        //#region 删除流量计、水压计、水质计或区域

        //public static bool DeleteFlowMeter(FlowMeter_t flowmeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLFLOWMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<FlowMeterJoinAreaStatus> fmas = JsonConvert.DeserializeObject<List<FlowMeterJoinAreaStatus>>(rc.Get(name));
        //            foreach (var item in fmas)
        //            {
        //                if (item.flowmeter.FM_UId == flowmeter.FM_UId)
        //                {
        //                    fmas.Remove(item);
        //                }
        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(fmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        //public static bool DeletePressureMeter(PressureMeter_t pressuremeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLPRESSUREMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<PressureMeterJoinAreaStatus> pmas = JsonConvert.DeserializeObject<List<PressureMeterJoinAreaStatus>>(rc.Get(name));
        //            foreach (var item in pmas)
        //            {
        //                if (item.pressuremeter.PM_UId == pressuremeter.PM_UId)
        //                {
        //                    pmas.Remove(item);

        //                }
        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(pmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        //public static bool DeleteQualityMeter(QualityMeter_t qualitymeter)
        //{
        //    using (RedisClient rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var name = ALLQUALITYMETER;
        //        if (!rc.Exists(name))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<QualityMeterJoinAreaStatus> qmas = JsonConvert.DeserializeObject<List<QualityMeterJoinAreaStatus>>(rc.Get(name));
        //            foreach (var item in qmas)
        //            {
        //                if (item.qualitymeter.QM_UId == qualitymeter.QM_UId)
        //                {
        //                    qmas.Remove(item);

        //                }
        //            }
        //            rc.Set(name, JsonConvert.SerializeObject(qmas));
        //            rc.Expire(name, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}
        //public static bool DeleteArea(Area_t area)
        //{
        //    using (var rc = new RedisClient(host))
        //    {
        //        rc.Select(DB_INDEX);
        //        var areaname = ALLAREA;
        //        var flowmetername = ALLFLOWMETER;
        //        var pressuremetername = ALLPRESSUREMETER;
        //        var qualitymetername = ALLAREA;

        //        if (!rc.Exists(areaname) || !rc.Exists(flowmetername) || !rc.Exists(pressuremetername) || !rc.Exists(qualitymetername))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            List<Area_t> arealist = JsonConvert.DeserializeObject<List<Area_t>>(rc.Get(areaname));
        //            List<FlowMeterJoinAreaStatus> fmlist = JsonConvert.DeserializeObject<List<FlowMeterJoinAreaStatus>>(rc.Get(flowmetername));
        //            List<PressureMeterJoinAreaStatus> pmlist = JsonConvert.DeserializeObject<List<PressureMeterJoinAreaStatus>>(rc.Get(pressuremetername));
        //            List<QualityMeterJoinAreaStatus> qmlist = JsonConvert.DeserializeObject<List<QualityMeterJoinAreaStatus>>(rc.Get(qualitymetername));

        //            foreach (var item in arealist)
        //            {
        //                if (item.Ara_UId == area.Ara_UId)
        //                {
        //                    arealist.Remove(item);
        //                }
        //            }
        //            rc.Set(areaname, JsonConvert.SerializeObject(arealist));
        //            rc.Expire(areaname, new TimeSpan(24, 0, 0));

        //            foreach (var item in fmlist)
        //            {
        //                if (item.area.Ara_UId == area.Ara_UId)
        //                {
        //                    fmlist.Remove(item);
        //                }
        //            }
        //            rc.Set(flowmetername, JsonConvert.SerializeObject(fmlist));
        //            rc.Expire(flowmetername, new TimeSpan(24, 0, 0));

        //            foreach (var item in pmlist)
        //            {
        //                if (item.area.Ara_UId == area.Ara_UId)
        //                {
        //                    pmlist.Remove(item);
        //                }
        //            }
        //            rc.Set(pressuremetername, JsonConvert.SerializeObject(pmlist));
        //            rc.Expire(pressuremetername, new TimeSpan(24, 0, 0));

        //            foreach (var item in qmlist)
        //            {
        //                if (item.area.Ara_UId == area.Ara_UId)
        //                {
        //                    qmlist.Remove(item);
        //                }
        //            }
        //            rc.Set(qualitymetername, JsonConvert.SerializeObject(qmlist));
        //            rc.Expire(qualitymetername, new TimeSpan(24, 0, 0));
        //        }
        //    }
        //    return true;
        //}

        //#endregion


    }
}