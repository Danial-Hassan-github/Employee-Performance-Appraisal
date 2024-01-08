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
        [HttpGet]
        public HttpResponseMessage GetEmployeePerformance(int employeeID, int sessionID)
        {
            Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
            try
            {
                var result = db.KPI_EMPLOYEE_SCORE.Where(emp => emp.EmployeeID==employeeID && emp.SessionID==sessionID).ToList();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }
    }
}
