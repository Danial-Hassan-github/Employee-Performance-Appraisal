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
        [HttpGet]
        public HttpResponseMessage GetEmployeePerformance(int employeeID, int sessionID)
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
    }
}
