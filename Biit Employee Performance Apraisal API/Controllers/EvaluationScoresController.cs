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
            int maxWeightage = (int)db.OptionsWeightages.OrderByDescending(x => x.weightage).First().weightage;
            try
            {
                var questionnaireType = db.QuestionaireTypes.Where(x => x.id == evaluationTypeID).FirstOrDefault();
                /*var result = db.Questionaires
                    .Join(db.StudentEvaluations, x => x.id, y => y.question_id, (x,y) => new { x,y })
                    .Where(combined => combined.x.type_id == evaluationTypeID);*/
                if (questionnaireType != null)
                {
                    if (questionnaireType.name.ToLower().Equals("student"))
                    {
                        // This block is executing for "Student" evaluation table data
                        var evaluationsWithQuestions = db.StudentEvaluations
                            .Where(e => e.teacher_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.totalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("peer"))
                    {
                        // This block is executing for "Peer" evaluation table data
                        var evaluationsWithQuestions = db.PeerEvaluations
                            .Where(e => e.evaluatee_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.totalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("senior"))
                    {
                        // This block is executing for "SeniorTeacher" evaluation table data
                        var evaluationsWithQuestions = db.SeniorTeacherEvaluations
                            .Where(e => e.junior_teacher_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.totalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("supervisor"))
                    {
                        // This block is executing for "Supervisor" evaluation table data
                        var evaluationsWithQuestions = db.SupervisorEvaluations
                            .Where(e => e.subordinate_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.totalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("director"))
                    {
                        // This block is executing for "Director" evaluation table data
                        var evaluationsWithQuestions = db.DirectorEvaluations
                            .Where(e => e.evaluatee_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.totalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("confidential"))
                    {
                        // This block is executing for "Confidential" evaluation table data
                        var evaluationsWithQuestions = db.ConfidentialEvaluations
                            .Where(e => e.teacher_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.totalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("degree exit"))
                    {
                        // This block is executing for "Degree Exit" evaluation table data
                        var evaluationsWithQuestions = db.DegreeExitEvaluations
                            .Where(e => e.supervisor_id == employeeID && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                totalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.totalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Evaluation type not found");
            }catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
