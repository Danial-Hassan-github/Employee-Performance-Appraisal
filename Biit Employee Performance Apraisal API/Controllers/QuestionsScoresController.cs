using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class QuestionsScoresController : ApiController
    {
        public class QuestionsScoresRequest
        {
            public List<int> employeeIDs { get; set; }
            public int sessionID { get; set; }
            public int evaluationTypeID { get; set; }
        }

        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        [HttpGet]
        public HttpResponseMessage GetQuestionsScores(int employeeID, int sessionID, int evaluationTypeID)
        {
            int maxWeightage = (int)db.OptionsWeightages.OrderByDescending(x => x.weightage).First().weightage;

            try
            {
                IQueryable<object> evaluations = null;

                switch (evaluationTypeID)
                {
                    case 1: // Student
                        evaluations = db.StudentEvaluations.Where(e => e.teacher_id == employeeID && e.session_id == sessionID).Select(e => new { e.question_id, e.score });
                        break;
                    case 2: // Peer
                        evaluations = db.PeerEvaluations.Where(e => e.evaluatee_id == employeeID && e.session_id == sessionID).Select(e => new { e.question_id, e.score });
                        break;
                    case 3: // Senior
                        evaluations = db.SeniorTeacherEvaluations.Where(e => e.junior_teacher_id == employeeID && e.session_id == sessionID).Select(e => new { e.question_id, e.score });
                        break;
                    case 4: // Supervisor
                        evaluations = db.SupervisorEvaluations.Where(e => e.subordinate_id == employeeID && e.session_id == sessionID).Select(e => new { e.question_id, e.score });
                        break;
                    case 5: // Director
                        evaluations = db.DirectorEvaluations.Where(e => e.evaluatee_id == employeeID && e.session_id == sessionID).Select(e => new { e.question_id, e.score });
                        break;
                    case 6: // Confidential
                        evaluations = db.ConfidentialEvaluations.Where(e => e.teacher_id == employeeID && e.session_id == sessionID).Select(e => new { e.question_id, e.score });
                        break;
                    case 7: // Degree Exit
                        evaluations = db.DegreeExitEvaluations.Where(e => e.supervisor_id == employeeID && e.session_id == sessionID).Select(e => new { e.question_id, e.score });
                        break;
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid evaluation type ID");
                }

                if (evaluations != null)
                {
                    var evaluationsWithQuestions = evaluations
                        .AsEnumerable()
                        .GroupBy(e => ((dynamic)e).question_id)
                        .Select(g => new
                        {
                            QuestionId = g.Key,
                            ObtainedScore = g.Sum(e => ((dynamic)e).score),
                            totalScore = g.Count() * maxWeightage
                        })
                        .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                            eval => eval.QuestionId,
                            question => question.id,
                            (eval, question) => new
                            {
                                question = question,
                                average = ((double)eval.ObtainedScore / eval.totalScore) * 100
                            })
                        .ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No evaluations found for the given criteria");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetMultiEmployeeQuestionsScores([FromBody] QuestionsScoresRequest request)
        {
            int maxWeightage = (int)db.OptionsWeightages.OrderByDescending(x => x.weightage).First().weightage;

            try
            {
                var evaluations = new List<object>();

                foreach (var employeeID in request.employeeIDs)
                {
                    IQueryable<object> employeeEvaluations = null;

                    switch (request.evaluationTypeID)
                    {
                        case 1: // Student
                            employeeEvaluations = db.StudentEvaluations.Where(e => e.teacher_id == employeeID && e.session_id == request.sessionID).Select(e => new { e.question_id, e.score });
                            break;
                        case 2: // Peer
                            employeeEvaluations = db.PeerEvaluations.Where(e => e.evaluatee_id == employeeID && e.session_id == request.sessionID).Select(e => new { e.question_id, e.score });
                            break;
                        case 3: // Senior
                            employeeEvaluations = db.SeniorTeacherEvaluations.Where(e => e.junior_teacher_id == employeeID && e.session_id == request.sessionID).Select(e => new { e.question_id, e.score });
                            break;
                        case 4: // Supervisor
                            employeeEvaluations = db.SupervisorEvaluations.Where(e => e.subordinate_id == employeeID && e.session_id == request.sessionID).Select(e => new { e.question_id, e.score });
                            break;
                        case 5: // Director
                            employeeEvaluations = db.DirectorEvaluations.Where(e => e.evaluatee_id == employeeID && e.session_id == request.sessionID).Select(e => new { e.question_id, e.score });
                            break;
                        case 6: // Confidential
                            employeeEvaluations = db.ConfidentialEvaluations.Where(e => e.teacher_id == employeeID && e.session_id == request.sessionID).Select(e => new { e.question_id, e.score });
                            break;
                        case 7: // Degree Exit
                            employeeEvaluations = db.DegreeExitEvaluations.Where(e => e.supervisor_id == employeeID && e.session_id == request.sessionID).Select(e => new { e.question_id, e.score });
                            break;
                        default:
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid evaluation type ID");
                    }

                    if (employeeEvaluations != null)
                    {
                        var evaluationsWithQuestions = employeeEvaluations
                            .AsEnumerable()
                            .GroupBy(e => ((dynamic)e).question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => ((dynamic)e).score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == request.evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
                                })
                            .ToList();

                        var employee = db.Employees.Find(employeeID);

                        evaluations.Add(new
                        {
                            employee = employee,
                            questionScores = evaluationsWithQuestions
                        });
                    }
                }

                if (evaluations.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, evaluations);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No evaluations found for the given criteria");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
