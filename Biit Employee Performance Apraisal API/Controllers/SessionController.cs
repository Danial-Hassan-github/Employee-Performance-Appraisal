using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class SessionController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        [Route("api/Session/GetSessions")]
        public HttpResponseMessage GetSessions()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,db.Sessions.OrderByDescending(s => s.id).ToList());
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Session/GetCurrentSession")]
        public HttpResponseMessage GetCurrentSession()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Sessions.OrderByDescending(s => s.id).First());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostSession(Session session) {
            try
            {
                int previousSessionId = db.Sessions.OrderByDescending(x => x.id).First().id;
                var sessionEntity = db.Sessions.Add(session);
                int newSessionId = sessionEntity.id;
                
                var oldKpiWeightages = db.KpiWeightages.Where(x => x.session_id == previousSessionId);
                var oldSubKpiWeightages = db.SubKpiWeightages.Where(x => x.session_id == previousSessionId);

                foreach (var newRecord in oldKpiWeightages)
                {
                    // newRecord.session_id = newSessionId;
                    // newRecord.id = 0;
                    KpiWeightage newKpiWeightage = new KpiWeightage()
                    {
                        // group_kpi_id = newRecord.group_kpi_id,
                        kpi_id = newRecord.kpi_id,
                        session_id = newSessionId,
                        weightage = newRecord.weightage,
                    };
                    db.KpiWeightages.Add(newKpiWeightage);
                }

                foreach (var newRecord in oldSubKpiWeightages)
                {
                    // newRecord.session_id = newSessionId;
                    SubKpiWeightage subKpiWeightage = new SubKpiWeightage()
                    {
                        kpi_id = newRecord.kpi_id,
                        sub_kpi_id = newRecord.sub_kpi_id,
                        session_id = newSessionId,
                        weightage = newRecord.weightage,
                    };
                    db.SubKpiWeightages.Add(subKpiWeightage);
                }

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, sessionEntity);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
