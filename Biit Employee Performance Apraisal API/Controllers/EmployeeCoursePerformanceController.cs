using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Appraisal_API.Controllers
{
    public class EmployeeCoursePerformanceController : ApiController
    {
        private Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        [HttpGet]
        public HttpResponseMessage GetEmployeeCoursePerformance(int employeeID, int sessionID, int courseID)
        {
            try
            {
                double average = CalculateEmployeePerformance(employeeID, sessionID, courseID);
                
                return Request.CreateResponse(HttpStatusCode.OK, average);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeCoursePerformance/CompareEmployeeCoursePerformance")]
        public HttpResponseMessage CompareEmployeeCoursePerformance(int employeeID1, int employeeID2, int sessionID, int courseID)
        {
            try
            {
                double average1 = CalculateEmployeePerformance(employeeID1, sessionID, courseID);
                double average2 = CalculateEmployeePerformance(employeeID2, sessionID, courseID);

                var response = new
                {
                    firstEmpAverage = average1,
                    secondEmpAverage = average2
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [NonAction]
        private double CalculateEmployeePerformance(int employeeID, int sessionID, int courseID)
        {
            var result = db.StudentEvaluations
                           .Where(x => x.teacher_id == employeeID && x.session_id == sessionID && x.course_id == courseID)
                           .ToList();

            int totalRecords = result.Count();
            if (totalRecords == 0)
            {
                return 0;
            }

            int totalScore = (int)(db.OptionsWeightages.OrderByDescending(x => x.id).First().weightage * totalRecords);
            int obtainedScore = result.Sum(x => x.score);

            if (totalScore == 0)
            {
                return 0;
            }

            double average = ((double)obtainedScore / totalScore) * 100;
            return average;
        }
    }
}
