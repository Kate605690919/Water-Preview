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
using WaterPreview.Redis;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;

namespace WaterPreview.Controllers
{
    public class AreaController:BaseController
    {
        private static IAreaService areaService;
        private static IFlowMeterService flowmeterService;
        private static IPressureMeterService pressuremeterService;
        private static IQualityMeterService qualitymeterService;


        public AreaController(IAreaService arService, IFlowMeterService fmService,IPressureMeterService pmService,IQualityMeterService qmService)
        {
            this.AddDisposableObject(arService);
            areaService = arService;
            this.AddDisposableObject(fmService);
            flowmeterService = fmService;

            this.AddDisposableObject(pmService);
            pressuremeterService = pmService;
            this.AddDisposableObject(qmService);
            qualitymeterService = qmService;
        }



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
                areauid = UserContext.areaSourceUid;
                area = all.First(p => p.Ara_UId == areauid);
                areaChild = GetChild(area.Ara_UId, all);

            }
            var list = new
            {
                text = area.Ara_Name,
                description = area.Ara_Description,
                id = area.Ara_UId,
                children = areaChild
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


            Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => flowmeterService.GetFlowMeterStatusAndArea();
            List<FlowMeterStatusAndArea> fmstatusAndAreaList = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea, 
                ConfigurationManager.AppSettings["allFlowMeterStatusAndArea"]);

            User_t account = UserContext.account;
            if (account.Usr_Type == 3)
            {
                fms_areas = fmstatusAndAreaList.Where(p => p.flowmeter.FM_WaterConsumerUId == account.Usr_UId).ToList();
            }
            else
            {
                //筛选出子区域范围内的所有流量计
                List<Area_t> subarealist = areaService.GetSubArea(areaUid);
                foreach (var item in fmstatusAndAreaList)
                {
                    if (subarealist.Where(p => p.Ara_UId == item.area.Ara_UId).Count() > 0)
                    {
                        fms_areas.Add(item);
                    }
                }

               
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
            Func<List<PressureMeterStatusAndArea>> pmAndStatusArea = () => pressuremeterService.GetPressureMeterStatusAndArea();
            List<PressureMeterStatusAndArea> pmstatusAndAreaList = DBHelper.get<PressureMeterStatusAndArea>(pmAndStatusArea,
                ConfigurationManager.AppSettings["allPressureMeterStatusAndArea"]).ToList();

            User_t account = UserContext.account;
            List<PressureMeterStatusAndArea> pms_areas = new List<PressureMeterStatusAndArea>();
            List<PressureMeterStatusAndArea> pms_areas_order = new List<PressureMeterStatusAndArea>();

            if (account.Usr_Type != 3)
            {
                List<Area_t> subarealist = areaService.GetSubArea(areaUid);
                foreach (var item in pmstatusAndAreaList)
                {
                    if (subarealist.Where(p => p.Ara_UId == item.area.Ara_UId).Count() > 0)
                    {
                        pms_areas.Add(item);
                    }
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
            Func<List<QualityMeterStatusAndArea>> qmAndStatusArea = () => qualitymeterService.GetQualityMeterStatusAndArea();
            List<QualityMeterStatusAndArea> qmstatusAndAreaList = DBHelper.get<QualityMeterStatusAndArea>(qmAndStatusArea,ConfigurationManager.AppSettings["allQualityMeterStatusAndArea"]);


            //result.Data = qmstatusAndAreaList;
            string dataresult = ToJson<List<QualityMeterStatusAndArea>>.Obj2Json<List<QualityMeterStatusAndArea>>(qmstatusAndAreaList).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }

        public JsonResult GetMapData()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            Guid uid = Guid.Parse(Session["wp_username"].ToString());
            User_t account = UserContext.account;
            List<FlowMeterStatusAndArea> fmlist = new List<FlowMeterStatusAndArea>();
            List<PressureMeterStatusAndArea> pmlist = new List<PressureMeterStatusAndArea>();
            List<QualityMeterStatusAndArea> qmlist = new List<QualityMeterStatusAndArea>();

            Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => flowmeterService.GetFlowMeterStatusAndArea();
            List<FlowMeterStatusAndArea> fmstatusAndAreaList = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea, 
                ConfigurationManager.AppSettings["allFlowMeterStatusAndArea"]);

            Func<List<PressureMeterStatusAndArea>> pmAndStatusArea = () => pressuremeterService.GetPressureMeterStatusAndArea();
            List<PressureMeterStatusAndArea> pmstatusAndAreaList = DBHelper.get<PressureMeterStatusAndArea>(pmAndStatusArea,
                ConfigurationManager.AppSettings["allPressureMeterStatusAndArea"]).ToList();

            Func<List<QualityMeterStatusAndArea>> qmAndStatusArea = () => qualitymeterService.GetQualityMeterStatusAndArea();
            List<QualityMeterStatusAndArea> qmstatusAndAreaList = DBHelper.get<QualityMeterStatusAndArea>(qmAndStatusArea,
                ConfigurationManager.AppSettings["allQualityMeterStatusAndArea"]);

            if(account.Usr_Type==3){
                fmlist = fmstatusAndAreaList.Where(p => p.flowmeter.FM_WaterConsumerUId==account.Usr_UId).ToList();
                //qmlist = qmstatusAndAreaList.Where(p=>p.qualitymeter);
                //pmlist = pmstatusAndAreaList.Where(p=>p.pressuremeter)
            }
            else
            {
                fmlist = fmstatusAndAreaList;
                pmlist = pmstatusAndAreaList;
                qmlist = qmstatusAndAreaList;
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