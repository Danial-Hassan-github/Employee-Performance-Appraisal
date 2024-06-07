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
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        [HttpGet]
        public HttpResponseMessage GetQuestionsScoresByEvaluationId(int employeeID, int sessionID, int evaluationTypeID)
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

    }
}
