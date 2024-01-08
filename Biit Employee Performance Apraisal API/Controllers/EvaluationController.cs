using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EvaluationController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        [HttpPost]
        [Route("api/Evaluation/PostPeerEvaluation")]
        public HttpResponseMessage PostPeerEvaluation(List<PEER_EVALUATION> pEER_EVALUATIONs)
        {
            try
            {
                db.PEER_EVALUATION.AddRange(pEER_EVALUATIONs);
                return Request.CreateResponse(HttpStatusCode.OK, "Submitted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Evaluation/PostStudentEvaluation")]
        public HttpResponseMessage PostStudentEvaluation(List<STUDENT_EVALUATION> sTUDENT_EVALUATIONs)
        {
            try
            {
                db.STUDENT_EVALUATION.AddRange(sTUDENT_EVALUATIONs);
                return Request.CreateResponse(HttpStatusCode.OK, "Submitted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
