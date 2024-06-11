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
    public class EmployeeSubKpiPerformanceController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        EmployeeScoreService service = new EmployeeScoreService();
        [HttpGet]
        [Route("api/EmployeeSubKpiPerformance/GetSubKpiEmployeePerformance")]
        public HttpResponseMessage GetSubKpiEmployeePerformance(int employeeID, int sessionID)
        {
            try
            {
                // Retrieve the sub-KPI scores with related sub-KPI data
                var result = db.SubkpiEmployeeScores
                               .Where(emp => emp.employee_id == employeeID && emp.session_id == sessionID)
                               .Select(empScore => new
                               {
                                   empScore.employee_id,
                                   empScore.subkpi_id,
                                   name = empScore.SubKpi.name,
                                   empScore.score,
                                   // weightage = empScore. weightage != null ? weightage.weightage : (double?)null
                               })
                               .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
