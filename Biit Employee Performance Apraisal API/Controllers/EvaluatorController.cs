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
            var eVALUATOR = db.Evaluators.Where(evaluator => evaluator.id == evaluatorID && evaluator.session_id == sessionID).ToList();
            if (eVALUATOR == null)
            {
                return NotFound();
            }

            return Ok(eVALUATOR);
        }

        [HttpPost]
        public HttpResponseMessage PostEvaluator([FromBody] EvaluatorEvaluatees evaluatorEvaluatees)
        {
            try
            {
                List<Evaluator> evaluators = new List<Evaluator>();
                for (int i = 0; i < evaluatorEvaluatees.evaluatee_id.Count; i++)
                {
                    evaluators.Add(new Evaluator { id = evaluatorEvaluatees.evaluator.id, session_id=evaluatorEvaluatees.evaluator.session_id,evaluatee_id = evaluatorEvaluatees.evaluatee_id[i] });
                    //eVALUATOR.evaluatee_id = EvaluateesID[i];
                    /*var evaluator = db.Evaluators.Find(eVALUATOR);
                    if (evaluator!=null)
                    {
                        evaluator.deleted = false;
                    }
                    else
                    {
                        db.Evaluators.Add(eVALUATOR);
                    }*/
                    /*db.Evaluators.Add(eVALUATOR);
                    db.SaveChanges();*/
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