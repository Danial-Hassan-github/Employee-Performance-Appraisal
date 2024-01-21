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
        KpiService kpiService = new KpiService();
        EmployeeScoreService empScoreService = new EmployeeScoreService();

        [HttpPost]
        [Route("api/Evaluation/PostPeerEvaluation")]
        public HttpResponseMessage PostPeerEvaluation(List<PeerEvaluation> pEER_EVALUATIONs)
        {
            try
            {
                db.PeerEvaluations.AddRange(pEER_EVALUATIONs);
                int sessionID=pEER_EVALUATIONs.Select(p => p.session_id).First();
                int employeeID= pEER_EVALUATIONs.Select(p => p.evaluatee_id).First();
                int sub_kpi_id = kpiService.getSubKpiID("peer evaluation");
                double totalScore = pEER_EVALUATIONs.Count() * 12;
                double obtainedScore = 0;
                foreach (var item in pEER_EVALUATIONs)
                {
                    obtainedScore += item.score;
                }
                double average=(obtainedScore/totalScore)*kpiService.getSubKpiWeightage(sub_kpi_id,sessionID);
                
                bool check=empScoreService.AddEvaluationScores(sessionID,sub_kpi_id,employeeID,Convert.ToInt32(average));
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
        }



        [HttpPost]
        [Route("api/Evaluation/PostStudentEvaluation")]
        public HttpResponseMessage PostStudentEvaluation(List<StudentEvaluation> sTUDENT_EVALUATIONs)
        {
            try
            {
                db.StudentEvaluations.AddRange(sTUDENT_EVALUATIONs);
                int sessionID = sTUDENT_EVALUATIONs.Select(p => p.session_id).First();
                int employeeID = sTUDENT_EVALUATIONs.Select(p => p.teacher_id).First();
                int questionID = sTUDENT_EVALUATIONs.Select(p => p.question_id).First();
                bool isConfidential = db.Questionaires.Find(questionID).type_id==db.QuestionaireTypes.Where(q => q.name=="confidential").Select(x => x.id).First();
                int sub_kpi_id = kpiService.getSubKpiID("student evaluation");

                if (isConfidential)
                {
                    sub_kpi_id = kpiService.getSubKpiID("confidential evaluation");
                }
                double totalScore = sTUDENT_EVALUATIONs.Count() * 12;
                double obtainedScore = 0;
                foreach (var item in sTUDENT_EVALUATIONs)
                {
                    obtainedScore += item.score;
                }
                double average = (obtainedScore / totalScore) * kpiService.getSubKpiWeightage(sub_kpi_id, sessionID);

                bool check=empScoreService.AddEvaluationScores(sessionID, sub_kpi_id, employeeID, Convert.ToInt32(average));
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
        }



        [HttpPost]
        [Route("api/Evaluation/PostSupervisorEvaluation")]
        public HttpResponseMessage PostSupervisorEvaluation(List<PeerEvaluation> pEER_EVALUATIONs)
        {
            try
            {
                db.PeerEvaluations.AddRange(pEER_EVALUATIONs);
                int sessionID = pEER_EVALUATIONs.Select(p => p.session_id).First();
                int employeeID = pEER_EVALUATIONs.Select(p => p.evaluatee_id).First();
                int sub_kpi_id = kpiService.getSubKpiID("peer evaluation");
                double totalScore = pEER_EVALUATIONs.Count() * 12;
                double obtainedScore = 0;
                foreach (var item in pEER_EVALUATIONs)
                {
                    obtainedScore += item.score;
                }
                double average = (obtainedScore / totalScore) * kpiService.getSubKpiWeightage(sub_kpi_id, sessionID);

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
        }



        [HttpPost]
        [Route("api/Evaluation/PostSeniorTeacherEvaluation")]
        public HttpResponseMessage PostSeniorTeacherEvaluation(List<PeerEvaluation> pEER_EVALUATIONs)
        {
            try
            {
                db.PeerEvaluations.AddRange(pEER_EVALUATIONs);
                int sessionID = pEER_EVALUATIONs.Select(p => p.session_id).First();
                int employeeID = pEER_EVALUATIONs.Select(p => p.evaluatee_id).First();
                int sub_kpi_id = kpiService.getSubKpiID("peer evaluation");
                double totalScore = pEER_EVALUATIONs.Count() * 12;
                double obtainedScore = 0;
                foreach (var item in pEER_EVALUATIONs)
                {
                    obtainedScore += item.score;
                }
                double average = (obtainedScore / totalScore) * kpiService.getSubKpiWeightage(sub_kpi_id, sessionID);

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
        }
    }
}
