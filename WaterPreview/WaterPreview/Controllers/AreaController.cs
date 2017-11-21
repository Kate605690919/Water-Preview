using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            DBHelper.ClearCache();
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            IAreaService area_service = new AreaService();
            Func<List<Area_t>> func = () => area_service.GetAllArea().ToList();
            List<Area_t> all = DBHelper.get<Area_t>(func, UserContext.allArea);
            var areaChild = new object();
            Area_t area = new Area_t();
            Guid areauid = new Guid();

            if (UserContext.account.Usr_Type == 3)
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
            Func<List<FlowMeterStatusAndArea>> fmAndStatusArea = () => flowmeterService.GetFlowMeterStatusAndArea();
            List<FlowMeterStatusAndArea> fmstatusAndAreaList = DBHelper.get<FlowMeterStatusAndArea>(fmAndStatusArea, UserContext.allFlowMeterStatusAndArea);


            string dataresult = ToJson<List<FlowMeterStatusAndArea>>.Obj2Json<List<FlowMeterStatusAndArea>>(fmstatusAndAreaList);
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }

        public JsonResult GetPressureMeterByAreaUid(Guid areaUid)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Func<List<PressureMeterStatusAndArea>> pmAndStatusArea = () => pressuremeterService.GetPressureMeterStatusAndArea();
            List<PressureMeterStatusAndArea> pmstatusAndAreaList = DBHelper.get<PressureMeterStatusAndArea>(pmAndStatusArea, UserContext.allPressureMeterStatusAndArea);
            //result.Data = pmstatusAndAreaList;
            string dataresult = ToJson<List<PressureMeterStatusAndArea>>.Obj2Json<List<PressureMeterStatusAndArea>>(pmstatusAndAreaList).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }

        public JsonResult GetQualityMeterByAreaUid(Guid areaUid)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Func<List<QualityMeterStatusAndArea>> qmAndStatusArea = () => qualitymeterService.GetQualityMeterStatusAndArea();
            List<QualityMeterStatusAndArea> qmstatusAndAreaList = DBHelper.get<QualityMeterStatusAndArea>(qmAndStatusArea, UserContext.allQualityMeterStatusAndArea);
            //result.Data = qmstatusAndAreaList;
            string dataresult = ToJson<List<QualityMeterStatusAndArea>>.Obj2Json<List<QualityMeterStatusAndArea>>(qmstatusAndAreaList).Replace("\\\\", "");
            dataresult = dataresult.Replace("\\\\", "");

            result.Data = dataresult;
            return result;
        }
    }
}