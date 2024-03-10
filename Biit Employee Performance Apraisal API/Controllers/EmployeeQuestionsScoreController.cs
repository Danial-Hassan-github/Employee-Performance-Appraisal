using Biit_Employee_Performance_Apraisal_API.Models;
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
        public HttpResponseMessage GetEmployeeQuestionsScore(int employeeID, int sessionID)
        {
            try
            {
                var result = db.PeerEvaluations
                    .Join(db.StudentEvaluations,
                        peer => peer.evaluatee_id,
                        student => student.teacher_id,
                        (peer, std) => new { peer, std })
                    .Where(x => x.peer.evaluatee_id == employeeID && x.peer.session_id == sessionID)
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
