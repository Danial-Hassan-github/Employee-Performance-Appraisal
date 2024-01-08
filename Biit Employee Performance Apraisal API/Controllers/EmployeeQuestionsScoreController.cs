using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EmployeeQuestionsScoreController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        public HttpResponseMessage GetEmployeeQuestionsScore(int employeeID,int sessionID)
        {
            try
            {
                var result = db.PEER_EVALUATION.Join(db.STUDENT_EVALUATION, peer => peer.EvaluateeID, std => std.TeacherID, (peer, std) => new { peer, std });
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
