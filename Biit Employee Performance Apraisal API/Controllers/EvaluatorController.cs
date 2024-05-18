using Biit_Employee_Performance_Apraisal_API.Models;
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

        [Route("api/Evaluator/GetEvaluators")]
        public HttpResponseMessage GetEvaluators(int sessionID)
        {
            try
            {
                var result= db.Evaluators.Where(evaluator => evaluator.session_id == sessionID).ToList();
                /*var result = db.EMPLOYEEs
    .Join(db.EVALUATORs, employee => employee.EmployeeID, evaluator => evaluator.EvaluatorID, (employee, evaluator) => new { employee, evaluator }).Where(evaluator => evaluator.evaluator.Deleted==false)
    .Join(db.SESSIONs, combined => combined.evaluator.SessionID, session => session.SessionID, (combined, session) => new { combined.employee, combined.evaluator, session })
    .Where(combined => combined.session.SessionID == sessionID)
    .Select(combined => combined.employee);*/
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        [Route("api/Evaluator/GetEvaluatees")]
        public IHttpActionResult GetEvaluatees(int evaluatorID,int sessionID)
        {
            var evaluateesId = db.Evaluators
                .Where(evaluator => evaluator.id == evaluatorID && evaluator.session_id == sessionID)
                .Select(e => e.evaluatee_id)
                .ToList();
            var evaluatees = db.Employees
                .Where(e => evaluateesId.Contains(e.id))
                .ToList();
            if (evaluatees == null)
            {
                return NotFound();
            }

            return Ok(evaluatees);
        }

        [HttpPost]
        public HttpResponseMessage PostEvaluator([FromBody] EvaluatorEvaluatees evaluatorEvaluatees)
        {
            try
            {
                List<Evaluator> evaluators = new List<Evaluator>();
                for (int i = 0; i < evaluatorEvaluatees.evaluatee_ids.Count; i++)
                {
                    evaluators.Add(new Evaluator { 
                        id = evaluatorEvaluatees.evaluator_id, 
                        session_id=evaluatorEvaluatees.session_id,
                        evaluatee_id = evaluatorEvaluatees.evaluatee_ids[i] 
                    });
                }
                var result=db.Evaluators.AddRange(evaluators).ToList();
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        /*[HttpDelete]
        public IHttpActionResult DeleteEvaluator(int evaluatorID,int sessionID)
        {
            var evaluator=db.Evaluators.Where(e => e.id==evaluatorID && e.session_id==sessionID).ToList();
            db.Evaluators.RemoveRange(evaluator);
            db.SaveChanges();

            return Ok(evaluator);
        }*/
    }
}