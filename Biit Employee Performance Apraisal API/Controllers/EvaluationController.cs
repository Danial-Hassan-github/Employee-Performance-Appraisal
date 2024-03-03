﻿using Biit_Employee_Performance_Apraisal_API.Models;
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
                db.SaveChanges();
                int sessionID=pEER_EVALUATIONs.Select(p => p.session_id).First();
                int employeeID= pEER_EVALUATIONs.Select(p => p.evaluatee_id).First();
                int sub_kpi_id = kpiService.getSubKpiID("peer evaluation");
                int sum=db.PeerEvaluations.Where(p => p.evaluatee_id==employeeID && p.session_id==sessionID).Count()*12;
                int obtained = db.PeerEvaluations.Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID).Sum(x => x.score);
                //double totalScore = pEER_EVALUATIONs.Count() * 12;
                //double obtainedScore = 0;
                /*foreach (var item in pEER_EVALUATIONs)
                {
                    obtainedScore += item.score;
                }*/
                double average=((double)obtained/sum)*kpiService.getSubKpiWeightage(sub_kpi_id,sessionID);
                
                bool check=empScoreService.AddEvaluationScores(sessionID,sub_kpi_id,employeeID,Convert.ToInt32(average));
                if (check)
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, db.PeerEvaluations.Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID));
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
                db.SaveChanges();
                int sessionID = sTUDENT_EVALUATIONs.Select(p => p.session_id).FirstOrDefault();
                int employeeID = sTUDENT_EVALUATIONs.Select(p => p.teacher_id).FirstOrDefault();
                int questionID = sTUDENT_EVALUATIONs.Select(p => p.question_id).FirstOrDefault();
                bool isConfidential = db.Questionaires.Find(questionID).type_id==db.QuestionaireTypes.Where(q => q.name=="confidential").Select(x => x.id).FirstOrDefault();
                int sub_kpi_id = kpiService.getSubKpiID("student evaluation");

                if (isConfidential)
                {
                    sub_kpi_id = kpiService.getSubKpiID("confidential evaluation");
                }
                int sum = db.StudentEvaluations.Where(p => p.teacher_id == employeeID && p.session_id == sessionID).Count() * 12;
                int obtained = db.StudentEvaluations.Where(p => p.teacher_id == employeeID && p.session_id == sessionID).Sum(x => x.score);
                /*double totalScore = sTUDENT_EVALUATIONs.Count() * 12;
                double obtainedScore = 0;
                foreach (var item in sTUDENT_EVALUATIONs)
                {
                    obtainedScore += item.score;
                }*/
                double average = ((double)obtained / sum) * kpiService.getSubKpiWeightage(sub_kpi_id, sessionID);

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
                /*double totalScore = supervisorEvaluations.Count() * 12;
                double obtainedScore = 0;
                foreach (var item in supervisorEvaluations)
                {
                    obtainedScore += item.score;
                }*/
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
        }



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
                /*int sub_kpi_id = kpiService.getSubKpiID("peer evaluation");
                if (isSenior)
                {
                    sub_kpi_id = kpiService.getSubKpiID("senior evaluation");
                }*/
                int sum = db.SupervisorEvaluations.Where(p => p.subordinate_id == employeeID && p.session_id == sessionID).Count() * 12;
                int obtained = db.SupervisorEvaluations.Where(p => p.subordinate_id == employeeID && p.session_id == sessionID).Sum(x => x.score);
                /*double totalScore = supervisorEvaluations.Count() * 12;
                double obtainedScore = 0;
                foreach (var item in supervisorEvaluations)
                {
                    obtainedScore += item.score;
                }*/
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
        }
    }
}
