using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class JuniorEmployeeController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        [HttpGet]
        public HttpResponseMessage GetTeacherJuniors(int teacherID, int sessionID)
        {
            try
            {
                var juniorsWithCourses = db.TeacherJuniors
                    .Where(x => x.senior_teacher_id == teacherID && x.session_id == sessionID)
                    .Select(x => new
                    {
                        employee = db.Employees.Where(y => y.id == x.junior_teacher_id).FirstOrDefault(), // This will include the entire TeacherJunior object
                        course = db.Courses.FirstOrDefault(c => c.id == x.course_id) // Fetch the corresponding course
                    })
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, juniorsWithCourses);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSupervisorJuniors(string designation, string department, string employeeType)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
