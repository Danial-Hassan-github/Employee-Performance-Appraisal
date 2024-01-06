using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EvaluatorController : ApiController
    {
        private Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        // GET: api/Evaluator
        [ActionName("GetEvaluators")]
        [Route("api/Evaluator/GetEvaluators/{sessionID:int}")]
        public HttpResponseMessage GetEVALUATORs(int sessionID)
        {
            try
            {
                var result = db.EMPLOYEEs
    .Join(db.EVALUATORs, employee => employee.EmployeeID, evaluator => evaluator.EvaluatorID, (employee, evaluator) => new { employee, evaluator })
    .Join(db.SESSIONs, combined => combined.evaluator.SessionID, session => session.SessionID, (combined, session) => new { combined.employee, combined.evaluator, session })
    .Where(combined => combined.session.SessionID == sessionID)
    .Select(combined => combined.employee);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }
        // GET: api/Evaluator/5
        [ActionName("GetEvaluator")]
        [Route("api/Evaluator/GetEvaluator/{id:int}")]
        [ResponseType(typeof(EVALUATOR))]
        public IHttpActionResult GetEVALUATOR(int id)
        {
            EVALUATOR eVALUATOR = db.EVALUATORs.Find(id);
            if (eVALUATOR == null)
            {
                return NotFound();
            }

            return Ok(eVALUATOR);
        }

        // POST: api/Evaluator
        [HttpPost]
        [ResponseType(typeof(EVALUATOR))]
        public HttpResponseMessage PostEVALUATOR(List<EVALUATOR> eVALUATORs,int sessionID)
        {
            EVALUATOR eVALUATOR = new EVALUATOR();
            try
            {
                db.EVALUATORs.AddRange(eVALUATORs);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, eVALUATOR);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        // DELETE: api/Evaluator/5
        [ResponseType(typeof(EVALUATOR))]
        [HttpDelete]
        public IHttpActionResult DeleteEVALUATOR(List<int> id)
        {
            var eVALUATOR = db.EVALUATORs.Where(x=>id.Contains(x.EvaluatorID)).ToList();
            if (eVALUATOR == null)
            {
                return NotFound();
            }
            var idsFromEvaluator=eVALUATOR.Select(x=>x.EvaluatorID).ToList();
            var PeerEvaluationToDelete = db.PEER_EVALUATION.Where(x=>idsFromEvaluator.Contains(x.EvaluatorID)).ToList();
            if (PeerEvaluationToDelete.Count>0)
            {
                db.PEER_EVALUATION.RemoveRange(PeerEvaluationToDelete);
            }
            db.EVALUATORs.RemoveRange(eVALUATOR);
            db.SaveChanges();

            return Ok(eVALUATOR);
        }
    }
}