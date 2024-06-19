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

        [HttpGet]
        [Route("api/Evaluator/GetEmployeeEvaluatorsByQuestion")]
        public HttpResponseMessage GetEmployeeEvaluatorsByQuestion(int employeeID, int questionID, int courseID, int sessionID)
        {
            try
            {
                var evaluationTypeID = db.Questionaires.FirstOrDefault(q => q.id == questionID).type_id;
                // Validate evaluation type
                var evaluationTypeRecord = db.QuestionaireTypes.FirstOrDefault(t => t.id == evaluationTypeID);

                if (evaluationTypeRecord == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid evaluation type");
                }

                // Get evaluation type ID
                var evaluationTypeId = evaluationTypeRecord.id;

                // Initialize the evaluation check flag
                List<Employee> employees = null;
                List<Student> students = null;

                // Check based on the evaluation type
                switch (evaluationTypeRecord.name.ToLower())
                {
                    case "student":
                        students = db.StudentEvaluations
                                        .Join(db.Students,
                                            se => se.student_id,
                                            s => s.id,
                                            (se, e) => new { se, e })
                                        .Where(joined => joined.se.question_id == questionID &&
                                                    joined.se.session_id == sessionID &&
                                                    joined.se.teacher_id == employeeID &&
                                                    joined.se.course_id == courseID)
                                        .Select(x => x.e)
                                        .ToList();
                        break;

                    case "peer":
                        // Assuming there is a PeerEvaluations table
                        employees = db.PeerEvaluations
                                        .Join(db.Employees,
                                            pe => pe.evaluator_id,
                                            e => e.id,
                                            (pe, e) => new { pe, e })
                                        .Where(joined => joined.pe.question_id == questionID &&
                                                    joined.pe.session_id == sessionID &&
                                                    joined.pe.evaluatee_id == employeeID &&
                                                    joined.pe.course_id == courseID)
                                        .Select(x => x.e)
                                        .ToList();
                        break;

                    case "confidential":
                        // Assuming there is a ConfidentialEvaluations table
                        students = db.ConfidentialEvaluations
                                        .Join(db.Students,
                                            ce => ce.student_id,
                                            s => s.id,
                                            (ce, s) => new { ce, s })
                                        .Where(joined => joined.ce.question_id == questionID &&
                                                    joined.ce.session_id == sessionID &&
                                                    joined.ce.teacher_id == employeeID)
                                        .Select(x => x.s)
                                        .ToList();
                        break;

                    case "supervisor":
                        // Assuming there is a SupervisorEvaluations table
                        employees = db.SupervisorEvaluations
                                        .Join(db.Employees,
                                            se => se.supervisor_id,
                                            e => e.id,
                                            (se, e) => new { se, e })
                                        .Where(joined => joined.se.question_id == questionID &&
                                                    joined.se.session_id == sessionID &&
                                                    joined.se.subordinate_id == employeeID)
                                        .Select(x => x.e)
                                        .ToList();
                        break;

                    case "director":
                        // Assuming there is a DirectorEvaluations table
                        employees = db.DirectorEvaluations
                                        .Join(db.Employees,
                                            de => de.evaluator_id,
                                            e => e.id,
                                            (de, e) => new { de, e })
                                        .Where(joined => joined.de.question_id == questionID &&
                                                    joined.de.session_id == sessionID &&
                                                    joined.de.evaluatee_id == employeeID)
                                        .Select(x => x.e)
                                        .ToList();
                        break;

                    case "senior":
                        // Assuming there is a SeniorEvaluations table
                        employees = db.SeniorTeacherEvaluations
                                        .Join(db.Employees,
                                            se => se.senior_teacher_id,
                                            e => e.id,
                                            (se, e) => new { se, e })
                                        .Where(joined => joined.se.question_id == questionID &&
                                                    joined.se.session_id == sessionID &&
                                                    joined.se.junior_teacher_id == employeeID &&
                                                    joined.se.course_id == courseID)
                                        .Select(x => x.e)
                                        .ToList();
                        break;

                    case "degree exit":
                        // Assuming there is a DegreeExitEvaluations table
                        employees = db.DegreeExitEvaluations
                                        .Join(db.Employees,
                                            de => de.student_id,
                                            e => e.id,
                                            (de, e) => new { de, e })
                                        .Where(joined => joined.de.question_id == questionID &&
                                                    joined.de.session_id == sessionID &&
                                                    joined.de.supervisor_id == employeeID)
                                        .Select(x => x.e)
                                        .ToList();
                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Unknown evaluation type");

                }

                if (students != null)
                    return Request.CreateResponse(HttpStatusCode.OK, students);
                else
                    return Request.CreateResponse(HttpStatusCode.OK, employees);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
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