using Biit_Employee_Performance_Apraisal_API.Models;
using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
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
                                   weightage = 0
                               })
                               .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeSubKpiPerformance/GetSubKpiMultiEmployeePerformance")]
        public HttpResponseMessage GetSubKpiMultiEmployeePerformance([FromUri] List<int> employeeIDs, int sessionID)
        {
            try
            {
                // Retrieve the sub-KPI scores with related sub-KPI data for multiple employees
                var result = db.SubkpiEmployeeScores
                               .Where(emp => employeeIDs.Contains(emp.employee_id) && emp.session_id == sessionID)
                               .GroupBy(emp => emp.employee_id)
                               .Select(g => new
                               {
                                   employee = db.Employees.Where(x => x.id == g.Key).FirstOrDefault(),
                                   subKpiPerformances = g.Select(empScore => new
                                   {
                                       empScore.employee_id,
                                       empScore.subkpi_id,
                                       empScore.SubKpi.name,
                                       empScore.score,
                                       weightage = 0
                                   }).ToList()
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
