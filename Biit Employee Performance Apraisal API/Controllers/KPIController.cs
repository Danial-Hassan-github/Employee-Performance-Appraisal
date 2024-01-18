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
        [HttpGet]
        public HttpResponseMessage GetKPIs(int sessionID)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,db.Kpis.Where(kpi => kpi.status==1));
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostKPI([FromBody] Kpi kPI,KpiWeightage kPI_WEIGHTAGE)
        {
            try
            {
                db.Kpis.Add(kPI);
                db.KpiWeightages.Add(kPI_WEIGHTAGE);
                int i = db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, kPI);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutKPI(int id, [FromBody] Kpi kPI,int weightage)
        {
            try
            {
                var kpi = db.Kpis.Find(id);
                kpi.name = kPI.name;
                var kpi_weightage = db.KpiWeightages.Find(id);
                kpi_weightage.weightage = weightage;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, kpi);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,ex.Message);
            }
        }

        [HttpDelete]
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
        }
    }
}