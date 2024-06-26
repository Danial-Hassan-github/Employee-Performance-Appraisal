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
        [Route("api/EvaluationScores/GetQuestionsScoresByEvaluationId")]
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
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
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
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
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
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
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
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
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
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
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
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
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
                                    average = ((double)eval.ObtainedScore / eval.totalScore) * 100
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

        [HttpGet]
        [Route("api/EvaluationScores/GetQuestionsScores")]
        public HttpResponseMessage GetQuestionsScores(int employeeID1, int employeeID2, int sessionID, int evaluationTypeID, int courseID)
        {
            int maxWeightage = (int)db.OptionsWeightages.OrderByDescending(x => x.weightage).First().weightage;
            try
            {
                var questionnaireType = db.QuestionaireTypes.Where(x => x.id == evaluationTypeID).FirstOrDefault();

                if (questionnaireType != null)
                {
                    if (questionnaireType.name.ToLower().Equals("student"))
                    {
                        var evaluationsWithQuestions = db.StudentEvaluations
                            .Where(e => (e.student_id == employeeID1 && e.teacher_id == employeeID2) && e.session_id == sessionID && e.course_id == courseID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                TotalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("peer"))
                    {
                        var evaluationsWithQuestions = db.PeerEvaluations
                            .Where(e => (e.evaluator_id == employeeID1 && e.evaluatee_id == employeeID2) && e.session_id == sessionID && e.course_id == courseID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                TotalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("senior"))
                    {
                        var evaluationsWithQuestions = db.SeniorTeacherEvaluations
                            .Where(e => (e.senior_teacher_id == employeeID1 && e.junior_teacher_id == employeeID2) && e.session_id == sessionID && e.course_id == courseID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                TotalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("supervisor"))
                    {
                        var evaluationsWithQuestions = db.SupervisorEvaluations
                            .Where(e => (e.supervisor_id == employeeID1 && e.subordinate_id == employeeID2) && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                TotalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("director"))
                    {
                        var evaluationsWithQuestions = db.DirectorEvaluations
                            .Where(e => (e.evaluatee_id == employeeID1 && e.evaluatee_id == employeeID2) && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                TotalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("confidential"))
                    {
                        var evaluationsWithQuestions = db.ConfidentialEvaluations
                            .Where(e => (e.student_id == employeeID1 && e.teacher_id == employeeID2) && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                TotalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                    else if (questionnaireType.name.ToLower().Equals("degree exit"))
                    {
                        var evaluationsWithQuestions = db.DegreeExitEvaluations
                            .Where(e => (e.student_id == employeeID1 && e.supervisor_id == employeeID2) && e.session_id == sessionID)
                            .GroupBy(e => e.question_id)
                            .Select(g => new
                            {
                                QuestionId = g.Key,
                                ObtainedScore = g.Sum(e => e.score),
                                TotalScore = g.Count() * maxWeightage
                            })
                            .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                                eval => eval.QuestionId,
                                question => question.id,
                                (eval, question) => new
                                {
                                    question = question,
                                    obtainedScore = eval.ObtainedScore,
                                    totalScore = eval.TotalScore
                                })
                            .ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, evaluationsWithQuestions);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Evaluation type not found");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /*[HttpGet]
        public HttpResponseMessage GetEvaluationsScores(int employeeID, int sessionID)
        {
            try
            {
                var result = db.
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }*/

    }
}
