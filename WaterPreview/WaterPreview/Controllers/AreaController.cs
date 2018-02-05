using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
using WaterPreview.Other.Attribute;
using WaterPreview.Redis;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;
using WaterPreview.Service.Service;

namespace WaterPreview.Controllers
{
    public class AreaController:BaseController
    {
        private static IAreaService areaService;
        private static IFlowMeterService flowmeterService;
        private static IPressureMeterService pressuremeterService;
        private static IQualityMeterService qualitymeterService;
        private static IAreaUserService areauserService;


        public AreaController()
        {
            areaService = new AreaService();
            flowmeterService = new FlowMeterService();
            pressuremeterService = new PressureMeterService();
            qualitymeterService = new QualityMeterService();
        }
        //public AreaController(IAreaService arService, IFlowMeterService fmService,IPressureMeterService pmService,IQualityMeterService qmService)
        //{
        //    this.AddDisposableObject(arService);
        //    areaService = arService;
        //    this.AddDisposableObject(fmService);
        //    flowmeterService = fmService;

        //    this.AddDisposableObject(pmService);
        //    pressuremeterService = pmService;
        //    this.AddDisposableObject(qmService);
        //    qualitymeterService = qmService;
        //}



        public JsonResult AreaTree()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            IAreaService area_service = new AreaService();
            Func<List<Area_t>> func = () => area_service.GetAllArea().ToList();
            List<Area_t> all = DBHelper.get<Area_t>(func, ConfigurationManager.AppSettings["AllArea"]);
            var areaChild = new object();
            Area_t area = new Area_t();
            Guid areauid = new Guid();

            User_t account = UserContext.account;

            if (account.Usr_Type == 3)
            {
                areauid = UserContext.GetAreaByUserUid(UserContext.account.Usr_UId);
                area = all.First(p => p.Ara_UId == areauid);
            }
            else
            {
                areauid = UserContext.AreaSourceUid;
                area = all.First(p => p.Ara_UId == areauid);
                areaChild = GetChild(area.Ara_UId, all);

            }
            var list = new
            {
                text = area.Ara_Name,
                description = area.Ara_Description,
                id = area.Ara_UId,
                Lat = area.Ara_Lat,//纬度
                Lng = area.Ara_Lng,//经度
                children = areaChild,
            };
            result.Data = list;
            return result;
        }

        private dynamic GetChild(Guid uid, List<Area_t> all)
        {
            return all.Where(p => p.Ara_Up == uid)
                .OrderBy(p => p.Ara_Code).ToList().Select(p =>
                new
                {
                    text = p.Ara_Name,
                    description = p.Ara_Description,
                    //code=p.Ara_Short,
                    //isleaf = p.Ara_IsLeaf,
                    Lat = p.Ara_Lat,//纬度
                    Lng = p.Ara_Lng,//经度
                    id = p.Ara_UId,
                    children = GetChild(p.Ara_UId, all)
                });
        }


        public JsonResult GetFlowMeterByAreaUid(Guid areaUid)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            List<FlowMeterStatusAndArea> fms_areas_order = new List<FlowMeterStatusAndArea>();
            List<FlowMeterStatusAndArea> fms_areas = new List<FlowMeterStatusAndArea>();

            //获取areauid对应的设备信息，并以areauid区分存储在redis
            Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => flowmeterService.GetFlowMeterStatusByArea(areaUid);
            List<FlowMeterStatusAndArea> fmstatusAndAreaList = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea,
                ConfigurationManager.AppSettings["FlowMeterStatusByAreaUid"]+areaUid);

            User_t account = UserContext.account;
            if (account.Usr_Type == 3)
            {
                fms_areas = fmstatusAndAreaList.Where(p => p.flowmeter.FM_WaterConsumerUId == account.Usr_UId).ToList();
            }
            else
            {
                //筛选出子区域范围内的所有流量计
                List<Area_t> subarealist = areaService.GetSubArea(areaUid);

                foreach (var areaitem in subarealist)
                {
                    Func<List<FlowMeterStatusAndArea>> fmsFunc = () => flowmeterService.GetFlowMeterStatusByArea(areaUid);
                    var  fmslist = DBHelper.get<FlowMeterStatusAndArea>(fmsFunc,
                        ConfigurationManager.AppSettings["FlowMeterStatusByAreaUid"] + areaitem.Ara_UId);
                    fms_areas.AddRange(fmslist);
                }
                //foreach (var item in fmstatusAndAreaList)
                //{
                //    if (subarealist.Where(p => p.Ara_UId == item.area.Ara_UId).Count() > 0)
                //    {
                //        fms_areas.Add(item);
                //    }
                //}

               
            }

            //获取设备访问次数,根据访问次数排序,再将剩余的设备整合
            Func<List<VisitCount>> initvisit = () => { return new List<VisitCount>(); };
            List<VisitCount> vclist = DBHelper.getWithNoExpire<List<VisitCount>>(initvisit, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitFlowMeterCount"]);
            vclist = vclist.OrderByDescending(p => p.count).ToList();
            foreach (var item in vclist)
            {
                var data = fms_areas.Where(p => p.flowmeter.FM_UId == Guid.Parse(item.uid)).FirstOrDefault();
                if (data != null)
                {
                    fms_areas_order.Add(data);
                }
            }
            foreach (var item in fms_areas)
            {
                if (vclist.Where(p => p.uid == item.flowmeter.FM_UId.ToString()).Count() == 0)
                {
                    fms_areas_order.Add(item);
                }
            }

            string dataresult = ToJson<List<FlowMeterStatusAndArea>>.Obj2Json<List<FlowMeterStatusAndArea>>(fms_areas_order);
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }

        public JsonResult GetPressureMeterByAreaUid(Guid areaUid)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => flowmeterService.GetFlowMeterStatusByArea(areaUid);
            //List<FlowMeterStatusAndArea> fmstatusAndAreaList = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea,
            //    ConfigurationManager.AppSettings["FlowMeterStatusByAreaUid"] + areaUid);

            Func<List<PressureMeterStatusAndArea>> pmAndStatusArea = () => pressuremeterService.GetPressureMeterStatusByArea(areaUid);
            List<PressureMeterStatusAndArea> pmstatusAndAreaList = DBHelper.get<PressureMeterStatusAndArea>(pmAndStatusArea,
                ConfigurationManager.AppSettings["PressureMeterStatusByAreaUid"]+areaUid).ToList();

            User_t account = UserContext.account;
            List<PressureMeterStatusAndArea> pms_areas = new List<PressureMeterStatusAndArea>();
            List<PressureMeterStatusAndArea> pms_areas_order = new List<PressureMeterStatusAndArea>();

            if (account.Usr_Type != 3)
            {
                List<Area_t> subarealist = areaService.GetSubArea(areaUid);
                foreach (var item in subarealist)
                {
                    Func<List<PressureMeterStatusAndArea>> pmsaFunc = () => pressuremeterService.GetPressureMeterStatusByArea(areaUid);
                    var pmsalist = DBHelper.get<PressureMeterStatusAndArea>(pmsaFunc,
                        ConfigurationManager.AppSettings["PressureMeterStatusByAreaUid"] + areaUid).ToList();
                    pms_areas.AddRange(pmsalist);

                }

                //获取设备访问次数,根据访问次数排序,再将剩余的设备整合
                Func<List<VisitCount>> initvisit = () => { return new List<VisitCount>(); };
                List<VisitCount> vclist = DBHelper.getWithNoExpire<List<VisitCount>>(initvisit, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitPressureMeterCount"]);
                vclist = vclist.OrderByDescending(p => p.count).ToList();
                foreach (var item in vclist)
                {
                    var data = pms_areas.Where(p => p.pressuremeter.PM_UId == Guid.Parse(item.uid)).FirstOrDefault();
                    if (data != null)
                    {
                        pms_areas_order.Add(data);
                    }
                }
                foreach (var item in pms_areas)
                {
                    if (vclist.Where(p => p.uid == item.pressuremeter.PM_UId.ToString()).Count() == 0)
                    {
                        pms_areas_order.Add(item);
                    }
                }
            }
            

            //result.Data = pmstatusAndAreaList;
            string dataresult = ToJson<List<PressureMeterStatusAndArea>>.Obj2Json<List<PressureMeterStatusAndArea>>(pms_areas_order).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }

        public JsonResult GetQualityMeterByAreaUid(Guid areaUid)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<QualityMeterStatusAndArea> qms_areas = new List<QualityMeterStatusAndArea>();
            List<QualityMeterStatusAndArea> qms_areas_order = new List<QualityMeterStatusAndArea>();
            if (UserContext.account.Usr_Type != 3)
            {
                


                List<Area_t> subarealist = areaService.GetSubArea(areaUid);
                foreach (var item in subarealist)
                {
                    Func<List<QualityMeterStatusAndArea>> qmAndStatusArea = () => qualitymeterService.GetQualityMeterStatusByArea(areaUid);
                    var qmslist = DBHelper.get<QualityMeterStatusAndArea>(qmAndStatusArea, ConfigurationManager.AppSettings["QualityMeterStatusByAreaUid"] + areaUid);
                    qms_areas.AddRange(qmslist);

                }

                //获取设备访问次数,根据访问次数排序,再将剩余的设备整合
                //Func<List<VisitCount>> initvisit = () => { return new List<VisitCount>(); };
                //List<VisitCount> vclist = DBHelper.getWithNoExpire<List<VisitCount>>(initvisit, UserContext.account.Usr_UId + ConfigurationManager.AppSettings["VisitQualityMeterCount"]);
                //vclist = vclist.OrderByDescending(p => p.count).ToList();
                //foreach (var item in vclist)
                //{
                //    var data = qms_areas.Where(p => p.qualitymeter.QM_UId == Guid.Parse(item.uid)).FirstOrDefault();
                //    if (data != null)
                //    {
                //        qms_areas_order.Add(data);
                //    }
                //}
                //foreach (var item in qms_areas)
                //{
                //    if (vclist.Where(p => p.uid == item.qualitymeter.QM_UId.ToString()).Count() == 0)
                //    {
                //        qms_areas_order.Add(item);
                //    }
                //}
            }
           

            //result.Data = qmstatusAndAreaList;
            string dataresult = ToJson<List<QualityMeterStatusAndArea>>.Obj2Json<List<QualityMeterStatusAndArea>>(qms_areas).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }

        public JsonResult GetMapData()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //Guid uid = Guid.Parse(Session["wp_username"].ToString());
            User_t account = UserContext.account;
            List<FlowMeterStatusAndArea> fmlist = new List<FlowMeterStatusAndArea>();
            List<PressureMeterStatusAndArea> pmlist = new List<PressureMeterStatusAndArea>();
            List<QualityMeterStatusAndArea> qmlist = new List<QualityMeterStatusAndArea>();

            //Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => flowmeterService.GetFlowMeterStatusAndArea();
            //List<FlowMeterStatusAndArea> fmstatusAndAreaList = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea, 
            //    ConfigurationManager.AppSettings["allFlowMeterStatusAndArea"]);

            //Func<List<PressureMeterStatusAndArea>> pmAndStatusArea = () => pressuremeterService.GetPressureMeterStatusAndArea();
            //List<PressureMeterStatusAndArea> pmstatusAndAreaList = DBHelper.get<PressureMeterStatusAndArea>(pmAndStatusArea,
            //    ConfigurationManager.AppSettings["allPressureMeterStatusAndArea"]).ToList();

            //Func<List<QualityMeterStatusAndArea>> qmAndStatusArea = () => qualitymeterService.GetQualityMeterStatusAndArea();
            //List<QualityMeterStatusAndArea> qmstatusAndAreaList = DBHelper.get<QualityMeterStatusAndArea>(qmAndStatusArea,
            //    ConfigurationManager.AppSettings["allQualityMeterStatusAndArea"]);

            if(account.Usr_Type==3){
                //fmlist = fmstatusAndAreaList.Where(p => p.flowmeter.FM_WaterConsumerUId==account.Usr_UId).ToList();
                //qmlist = qmstatusAndAreaList.Where(p=>p.qualitymeter);
                //pmlist = pmstatusAndAreaList.Where(p=>p.pressuremeter);

                AreaUser_t au = areauserService.GetAreaUserByUser(account.Usr_UId);
                Func<List<FlowMeterStatusAndArea>> fmsFunc = () => flowmeterService.GetFlowMeterStatusByArea(au.AU_AreaUId);
                fmlist = DBHelper.get<FlowMeterStatusAndArea>(fmsFunc,
                    ConfigurationManager.AppSettings["FlowMeterStatusByAreaUid"] + au.AU_AreaUId).Where(p=>p.flowmeter.FM_WaterConsumerUId==account.Usr_UId).ToList();
            }
            else
            {
                List<Area_t> arealist = areaService.GetAllArea();
                foreach (var areaitem in arealist)
                {
                    Func<List<FlowMeterStatusAndArea>> fmsFunc = () => flowmeterService.GetFlowMeterStatusByArea(areaitem.Ara_UId);
                    var fmdata = DBHelper.get<FlowMeterStatusAndArea>(fmsFunc,
                        ConfigurationManager.AppSettings["FlowMeterStatusByAreaUid"] + areaitem.Ara_UId);
                    fmlist.AddRange(fmdata);

                    Func<List<PressureMeterStatusAndArea>> pmsFunc = () => pressuremeterService.GetPressureMeterStatusByArea(areaitem.Ara_UId);
                    var pmsalist = DBHelper.get<PressureMeterStatusAndArea>(pmsFunc,
                        ConfigurationManager.AppSettings["PressureMeterStatusByAreaUid"] + areaitem.Ara_UId).ToList();
                    pmlist.AddRange(pmsalist);

                    Func<List<QualityMeterStatusAndArea>> qmAndStatusArea = () => qualitymeterService.GetQualityMeterStatusByArea(areaitem.Ara_UId);
                    var qmslist = DBHelper.get<QualityMeterStatusAndArea>(qmAndStatusArea, ConfigurationManager.AppSettings["QualityMeterStatusByAreaUid"] + areaitem.Ara_UId);

                    qmlist.AddRange(qmslist);
                }
                //fmlist = fmstatusAndAreaList;
                //pmlist = pmstatusAndAreaList;
                //qmlist = qmstatusAndAreaList;
            }
            result.Data = new
            {
                fmstatusAndAreaList = fmlist,
                pmstatusAndAreaList = pmlist,
                qmstatusAndAreaList = qmlist,

            };
            return result;
        }
    }
}