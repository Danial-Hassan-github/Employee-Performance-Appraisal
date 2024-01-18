using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EmployeeScoreController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        EmployeeScoreService service = new EmployeeScoreService();
        [HttpPost]
        public HttpResponseMessage AddEmployeeScore(KpiEmployeeScore employeeScore)
        {
            if (service.isEmployeeScoreExists(employeeScore))
            {
                service.UpdateEmployeeKpiScore(employeeScore);
            }
            else
            {
                service.AddEmployeeKpiScore(employeeScore);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
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
        }
    }
}
