using Biit_Employee_Performance_Apraisal_API.Models;
using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EvaluationController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        EvaluationService evaluationService = new EvaluationService();
        SubKpiService subKpiService = new SubKpiService();
        KpiService kpiService = new KpiService();
        EmployeeScoreService empScoreService = new EmployeeScoreService();

        [HttpPost]
        [Route("api/Evaluation/PostPeerEvaluation")]
        public HttpResponseMessage PostPeerEvaluation(List<PeerEvaluation> peerEvaluations)
        {
            try
            {
                db.PeerEvaluations.AddRange(peerEvaluations);
                db.SaveChanges();

                var sessionID = peerEvaluations.First().session_id;
                var employeeID = peerEvaluations.First().evaluatee_id;
                var evaluationType = evaluationService.GetEvaluationType(peerEvaluations.First().question_id);
                var subKpiID = subKpiService.getSubKpiID(evaluationType);

                var sum = evaluationService.GetSumOfPeerEvaluations(employeeID, sessionID);
                var obtained = evaluationService.GetObtainedPeerEvaluationScore(employeeID, sessionID);
                var subKpiWeightage = subKpiService.getSubKpiWeightage(subKpiID, sessionID);
                var average = ((double)obtained / sum) * subKpiWeightage;

                // Ensure the average is within the Int32 range before conversion
                if (average > int.MaxValue)
                {
                    average = int.MaxValue;
                }
                else if (average < int.MinValue)
                {
                    average = int.MinValue;
                }

                if (empScoreService.AddEvaluationScores(sessionID, subKpiID, employeeID, Convert.ToInt32(average)))
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, db.PeerEvaluations.Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Route("api/Evaluation/PostStudentEvaluation")]
        public HttpResponseMessage PostStudentEvaluation(List<StudentEvaluation> studentEvaluations)
        {
            try
            {
                db.StudentEvaluations.AddRange(studentEvaluations);
                db.SaveChanges();
                int sessionID = studentEvaluations.First().session_id;
                int employeeID = studentEvaluations.First().teacher_id;
                int questionID = studentEvaluations.First().question_id;
                bool isConfidential = db.Questionaires.Find(questionID).type_id==db.QuestionaireTypes.Where(q => q.name=="confidential")
                    .Select(x => x.id)
                    .FirstOrDefault();
                int sub_kpi_id = subKpiService.getSubKpiID("student evaluation");

                if (isConfidential)
                {
                    sub_kpi_id = subKpiService.getSubKpiID("confidential evaluation");
                }
                int sum = evaluationService.GetSumOfStudentEvaluations(employeeID, sessionID);
                int obtained = db.StudentEvaluations.Where(p => p.teacher_id == employeeID && p.session_id == sessionID).Sum(x => x.score);
                double average = ((double)obtained / sum) * subKpiService.getSubKpiWeightage(sub_kpi_id, sessionID);

                // Ensure the average is within the Int32 range before conversion
                if (average > int.MaxValue)
                {
                    average = int.MaxValue;
                }
                else if (average < int.MinValue)
                {
                    average = int.MinValue;
                }

                bool check=empScoreService.AddEvaluationScores(sessionID, sub_kpi_id, employeeID, Convert.ToInt32(average));
                if (check)
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Submitted");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "something went wrong");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("api/Evaluation/PostDegreeExitEvaluation")]
        public HttpResponseMessage PostDegreeExitEvaluation(List<DegreeExitEvaluation> degreeExitEvaluations)
        {
            try
            {
                db.DegreeExitEvaluations.AddRange(degreeExitEvaluations);
                db.SaveChanges();

                var sessionID = degreeExitEvaluations.First().session_id;
                var employeeID = degreeExitEvaluations.First().teacher_id;
                var evaluationType = evaluationService.GetEvaluationType(degreeExitEvaluations.First().question_id);
                var subKpiID = subKpiService.getSubKpiID(evaluationType);

                var sum = evaluationService.GetSumOfDegreeExitEvaluations(employeeID, sessionID);
                var obtained = evaluationService.GetObtainedDegreeExitEvaluationScore(employeeID, sessionID);
                var subKpiWeightage = subKpiService.getSubKpiWeightage(subKpiID, sessionID);
                var average = ((double)obtained / sum) * subKpiWeightage;

                // Ensure the average is within the Int32 range before conversion
                if (average > int.MaxValue)
                {
                    average = int.MaxValue;
                }
                else if (average < int.MinValue)
                {
                    average = int.MinValue;
                }

                if (empScoreService.AddEvaluationScores(sessionID, subKpiID, employeeID, Convert.ToInt32(average)))
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, db.PeerEvaluations.Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("api/Evaluation/PostSeniorTeacherEvaluation")]
        public HttpResponseMessage PostSeniorTeacherEvaluation(List<SeniorTeacherEvaluation> seniorTeacherEvaluations)
        {
            try
            {
                db.SeniorTeacherEvaluations.AddRange(seniorTeacherEvaluations);
                db.SaveChanges();

                var sessionID = seniorTeacherEvaluations.First().session_id;
                var employeeID = seniorTeacherEvaluations.First().junior_teacher_id;
                var evaluationType = evaluationService.GetEvaluationType(seniorTeacherEvaluations.First().question_id);
                var subKpiID = subKpiService.getSubKpiID(evaluationType);

                var sum = evaluationService.GetSumOfSeniorTeacherEvaluations(employeeID, sessionID);
                var obtained = evaluationService.GetObtainedSeniorTeacherEvaluationScore(employeeID, sessionID);
                var subKpiWeightage = subKpiService.getSubKpiWeightage(subKpiID, sessionID);
                var average = ((double) obtained / sum) * subKpiWeightage;

                // Ensure the average is within the Int32 range before conversion
                if (average > int.MaxValue)
                {
                    average = int.MaxValue;
                }
                else if (average < int.MinValue)
                {
                    average = int.MinValue;
                }

                if (empScoreService.AddEvaluationScores(sessionID, subKpiID, employeeID, Convert.ToInt32(average)))
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, db.SeniorTeacherEvaluations.Where(p => p.junior_teacher_id == employeeID && p.session_id == sessionID));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /* [HttpPost]
         [Route("api/Evaluation/PostSupervisorEvaluation")]
         public HttpResponseMessage PostSupervisorEvaluation(List<SupervisorEvaluation> supervisorEvaluations)
         {
             try
             {
                 db.SupervisorEvaluations.AddRange(supervisorEvaluations);
                 db.SaveChanges();
                 int sessionID = supervisorEvaluations.Select(p => p.session_id).FirstOrDefault();
                 int employeeID = supervisorEvaluations.Select(p => p.subordinate_id).FirstOrDefault();

                 int sub_kpi_id = kpiService.getSubKpiID("supervisor evaluation");
                 int sum = db.SupervisorEvaluations.Where(p => p.subordinate_id == employeeID && p.session_id == sessionID).Count() * 12;
                 int obtained = db.SupervisorEvaluations.Where(p => p.subordinate_id == employeeID && p.session_id == sessionID).Sum(x => x.score);
                 *//*double totalScore = supervisorEvaluations.Count() * 12;
                 double obtainedScore = 0;
                 foreach (var item in supervisorEvaluations)
                 {
                     obtainedScore += item.score;
                 }*//*
                 double average = ((double)obtained / sum) * kpiService.getSubKpiWeightage(sub_kpi_id, sessionID);

                 bool check = empScoreService.AddEvaluationScores(sessionID, sub_kpi_id, employeeID, Convert.ToInt32(average));
                 if (check)
                 {
                     db.SaveChanges();
                     return Request.CreateResponse(HttpStatusCode.OK, "Submitted");
                 }
                 else
                 {
                     return Request.CreateResponse(HttpStatusCode.OK, "something went wrong");
                 }
             }
             catch (Exception ex)
             {
                 return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
             }
         }*/


        /*
                [HttpPost]
                [Route("api/Evaluation/PostSeniorTeacherEvaluation")]
                public HttpResponseMessage PostSeniorTeacherEvaluation(List<SupervisorEvaluation> supervisorEvaluations)
                {
                    try
                    {
                        db.SupervisorEvaluations.AddRange(supervisorEvaluations);
                        db.SaveChanges();
                        int sessionID = supervisorEvaluations.Select(p => p.session_id).FirstOrDefault();
                        int employeeID = supervisorEvaluations.Select(p => p.subordinate_id).FirstOrDefault();
                        int questionID = supervisorEvaluations.Select(p => p.question_id).FirstOrDefault();
                        bool isSenior = db.Questionaires.Find(questionID).type_id == db.QuestionaireTypes.Where(q => q.name == "senior").Select(x => x.id).FirstOrDefault();
                        int sub_kpi_id = kpiService.getSubKpiID("senior evaluation");
                        *//*int sub_kpi_id = kpiService.getSubKpiID("peer evaluation");
                        if (isSenior)
                        {
                            sub_kpi_id = kpiService.getSubKpiID("senior evaluation");
                        }*//*
                        int sum = db.SupervisorEvaluations.Where(p => p.subordinate_id == employeeID && p.session_id == sessionID).Count() * 12;
                        int obtained = db.SupervisorEvaluations.Where(p => p.subordinate_id == employeeID && p.session_id == sessionID).Sum(x => x.score);
                        *//*double totalScore = supervisorEvaluations.Count() * 12;
                        double obtainedScore = 0;
                        foreach (var item in supervisorEvaluations)
                        {
                            obtainedScore += item.score;
                        }*//*
                        double average = ((double)obtained / sum) * kpiService.getSubKpiWeightage(sub_kpi_id, sessionID);

                        bool check = empScoreService.AddEvaluationScores(sessionID, sub_kpi_id, employeeID, Convert.ToInt32(average));
                        if (check)
                        {
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "Submitted");
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "something went wrong");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }*/

        [HttpGet]
        [Route("api/Evaluation/isEvaluated")]
        public HttpResponseMessage IsEvaluated(int studentId, int teacherId, int courseId, int sessionId, string evaluationType)
        {
            try
            {
                var typeExists = db.QuestionaireTypes.Any(t => t.name == evaluationType);

                if (!typeExists)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid evaluation type");
                }

                var a = db.QuestionaireTypes.FirstOrDefault(t => t.name == evaluationType).id;
                var isEvaluated = db.StudentEvaluations
                                    .Join(db.Questionaires,
                                        se => se.question_id,
                                        q => q.id,
                                        (se, q) => new { se, q })
                                    .Where(joined => joined.se.student_id == studentId &&
                                joined.se.teacher_id == teacherId &&
                                joined.se.course_id == courseId &&
                                joined.se.session_id == sessionId &&
                                joined.q.type_id == a)
                                    .ToList();

                if (isEvaluated.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Already Evaluated");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not Evaluated Yet");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }
}
