using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class KPIController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        KpiService kpiService=new KpiService();


        /*
         GetKPIs used to fetch all availble kpi,s wheater they're included in session or not
        */
        [HttpGet]
        [Route("api/KPI/GetKPIs")]
        public HttpResponseMessage GetKPIs()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,db.Kpis);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        /*
         GetSessionKPIs is being used to fetch kpi's availbe in specific session
        */
        [HttpGet]
        [Route("api/KPI/GetSessionKPIs")]
        public HttpResponseMessage GetSessionKPIs(int sessionID)
        {
            try
            {
                var result = db.KpiWeightages.Join(db.Kpis,x=>x.kpi_id,y=>y.id,(x,y)=>new {x,y}).Where(combined=>combined.x.session_id==sessionID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/KPI/GetSubKPIs")]
        public HttpResponseMessage GetSubKPIs(int kpi_id,int sessionID)
        {
            try
            {
                var result = db.SubKpiWeightages.Join(db.SubKpis, x => x.sub_kpi_id, y => y.id, (x, y) => new { x, y }).Where(combined => combined.x.session_id == sessionID && combined.y.kpi_id==kpi_id).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        /*
         GetEEmployeeTypeKPIs is being used to fetch kpi's availbe in specific session for specific type of employees
        */
        /*[HttpGet]
        public HttpResponseMessage GetEmployeeTypeKPIs(int employeeTypeID,int sessionID)
        {
            try
            {
                var result = db.KpiWeightages.Join(db.Kpis, x => x.kpi_id, y => y.id, (x, y) => new { x, y }).Where(combined => combined.x.session_id == sessionID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }*/


        /*
         for adding new kpi
        */
        [HttpPost]
        [Route("api/KPI/PostKPI")]
        public HttpResponseMessage PostKPI([FromBody] Kpi kPI,int weightage,int sessionID,int employeeTypeID)
        {
            try
            {
                if (weightage>100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Weightage caannot be more than 100");
                }
                var k = db.Kpis.Add(kPI);
                db.SaveChanges();
                kpiService.adjustKpiWeightages(weightage, sessionID, k.id, employeeTypeID);
                //kpiService.adjustKpiWeigtage(weightage, sessionID);
                KpiWeightage kpiWeightage = new KpiWeightage();
                kpiWeightage.kpi_id = k.id;
                kpiWeightage.weightage = weightage;
                kpiWeightage.session_id = sessionID;
                db.KpiWeightages.Add(kpiWeightage);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, k);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        /*
         To update kpi settings
         */
        [HttpPut]
        [Route("api/KPI/PutKPI")]
        public HttpResponseMessage PutKPI([FromBody] Kpi kPI,int weightage,int sessionID)
        {
            try
            {
                if (weightage > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Weightage caannot be more than 100%");
                }
                var kpi = db.Kpis.Find(kPI.id);
                kpi.name = kPI.name;
                var kpi_weightage = db.KpiWeightages.Find(kPI.id,sessionID);
                kpiService.adjustKpiWeightages(weightage, sessionID,kPI.id);
                int leftOverWeightage = db.KpiWeightages.Where(x => x.session_id == sessionID).Sum(y => y.weightage);
                leftOverWeightage -= weightage;
                kpi_weightage.weightage = weightage;

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, kpi);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,ex.Message);
            }
        }

        /*[HttpDelete]
        public HttpResponseMessage DeleteKPI(int id)
        {
            try
            {
                var kpi = db.Kpis.Find(id);
                kpi.status = 1;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,"Deleted Successfully");
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }*/
    }
}