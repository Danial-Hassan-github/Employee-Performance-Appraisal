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
                return Request.CreateResponse(HttpStatusCode.OK,db.KPIs.Where(kpi => kpi.status==1));
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostKPI([FromBody] KPI kPI,KPI_WEIGHTAGE kPI_WEIGHTAGE)
        {
            try
            {
                db.KPIs.Add(kPI);
                db.KPI_WEIGHTAGE.Add(kPI_WEIGHTAGE);
                int i = db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, kPI);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutKPI(int id, [FromBody] KPI kPI,int weightage)
        {
            try
            {
                var kpi = db.KPIs.Find(id);
                kpi.Name = kPI.Name;
                var kpi_weightage = db.KPI_WEIGHTAGE.Find(id);
                kpi_weightage.Weightage = weightage;
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
                var kpi = db.KPIs.Find(id);
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