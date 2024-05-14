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
    public class EmployeeKpiPerformanceController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        EmployeeScoreService service = new EmployeeScoreService();

        [HttpGet]
        public HttpResponseMessage GetKpiEmployeePerformance(int employeeID, int sessionID)
        {
            try
            {
                var result = from empScore in db.KpiEmployeeScores
                             where empScore.employee_id == employeeID && empScore.session_id == sessionID
                             from kpi in db.Kpis.Where(k => k.id == empScore.kpi_id).DefaultIfEmpty()
                             where kpi != null
                             from weightage in db.KpiWeightages
                                 .Where(w => w.kpi_id == empScore.kpi_id && w.session_id == empScore.session_id)
                                 .DefaultIfEmpty()
                             select new
                             {
                                 empScore.employee_id,
                                 empScore.kpi_id,
                                 kpi_title = kpi.name,
                                 empScore.score,
                                 weightage.weightage
                             };
                return Request.CreateResponse(HttpStatusCode.OK, result.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetKpiEmployeePerformanceMultiSession(int employeeID, int startingSessionID, int endingSessionID)
        {
            try
            {
                var result = from empScore in db.KpiEmployeeScores
                             join session in db.Sessions
                                 on empScore.session_id equals session.id
                             where empScore.employee_id == employeeID &&
                                   empScore.session_id >= startingSessionID &&
                                   empScore.session_id <= endingSessionID
                             join kpi in db.Kpis
                                 on empScore.kpi_id equals kpi.id into kpiGroup
                             from kpi in kpiGroup.DefaultIfEmpty()
                             join weightage in db.KpiWeightages
                                 on new { empScore.kpi_id, empScore.session_id } equals new { weightage.kpi_id, weightage.session_id } into weightageGroup
                             from weightage in weightageGroup.DefaultIfEmpty()
                             group new { empScore, session, kpi, weightage } by session into sessionGroup
                             select new
                             {
                                 session = sessionGroup.Key,
                                 scores = sessionGroup.Select(item => new
                                 {
                                     item.empScore.employee_id,
                                     item.empScore.kpi_id,
                                     kpi_title = item.kpi != null ? item.kpi.name : null,
                                     item.empScore.score,
                                     weightage_weightage = item.weightage != null ? item.weightage.weightage : (decimal?)null
                                 })
                             };

                return Request.CreateResponse(HttpStatusCode.OK, result.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        public HttpResponseMessage GetSubKpiEmployeePerformance(int kpiID,int employeeID, int sessionID)
        {
            try
            {
                int sub_kpiId=db.SubKpis.Where(x => x.kpi_id==kpiID).FirstOrDefault().id;
                var result = db.SubkpiEmployeeScores.Where(emp => emp.employee_id == employeeID && emp.session_id == sessionID && emp.subkpi_id == sub_kpiId).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddFreeKpiEmployeeScore(KpiEmployeeScore employeeScore)
        {
            if (service.isEmployeeScoreExists(employeeScore))
            {
                service.UpdateEmployeeKpiScore(employeeScore);
            }
            else
            {
                service.AddEmployeeKpiScore(employeeScore);
            }
            var result = db.KpiEmployeeScores.Find(employeeScore.kpi_id, employeeScore.employee_id, employeeScore.session_id);
            return Request.CreateResponse(HttpStatusCode.OK,result);
        }

        /*[HttpPost]
        public HttpResponseMessage FreeKpiEmployeeScore(KpiEmployeeScore kpiEmployeeScore)
        {
            try
            {
                db.KpiEmployeeScores.Add(kpiEmployeeScore);
                return Request.CreateResponse(HttpStatusCode.OK, kpiEmployeeScore);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }*/
    }
}
