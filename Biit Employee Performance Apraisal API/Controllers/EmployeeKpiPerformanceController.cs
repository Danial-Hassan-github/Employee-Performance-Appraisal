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
                var result = db.KpiEmployeeScores.Where(emp => emp.employee_id==employeeID && emp.session_id==sessionID).ToList();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSubKpiEmployeePerformance(int kpiID,int employeeID, int sessionID)
        {
            try
            {
                int sub_kpiId=db.SubKpis.Where(x => x.kpi_id==kpiID).FirstOrDefault().id;
                var result = db.SubkpiEmployeeScores.Where(emp => emp.employee_id == employeeID && emp.session_id == sessionID && emp.subkpi_id == sub_kpiId).ToList();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, result);
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
