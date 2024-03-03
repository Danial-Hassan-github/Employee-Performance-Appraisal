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
        public HttpResponseMessage GetSessions()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,db.Sessions);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostSessions(Session session) {
            try
            {
                var sessionEntity=db.Sessions.Add(session);
                return Request.CreateResponse(HttpStatusCode.OK, sessionEntity);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
