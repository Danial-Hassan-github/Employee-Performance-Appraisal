using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EvaluationScoresController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        public HttpResponseMessage GetQuestionsScoresByEvaluationId(int employeeID, int sessionID, int evaluationTypeID)
        {
            try
            {
                var questionnaireType = db.QuestionaireTypes.Where(x => x.id == evaluationTypeID).FirstOrDefault();
                var result = db.Questionaires.Join(db.StudentEvaluations, x => x.id, y => y.question_id, (x,y) => new { x,y }).Where(combined => combined.x.type_id == evaluationTypeID);
                if (questionnaireType != null)
                {
                    if (questionnaireType.name.ToLower().Equals("student") || questionnaireType.name.ToLower().Equals("confidential"))
                    {
                        // This block is executing for "Student" evaluation table data
                        var evaluationsWithQuestions = db.StudentEvaluations
                            .Where(e => e.teacher_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                TotalScore = g.Sum(e => e.score)
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.TotalScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("senior") || questionnaireType.name.ToLower().Equals("peer") || questionnaireType.name.ToLower().Equals("supervisor") || questionnaireType.name.ToLower().Equals("director"))
                    {
                        // This block is executing for "Peer" evaluation table data
                        var evaluationsWithQuestions = db.PeerEvaluations
                            .Where(e => e.evaluatee_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                TotalScore = g.Sum(e => e.score)
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.TotalScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
