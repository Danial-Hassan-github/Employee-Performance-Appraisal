using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class StudentController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        public HttpResponseMessage GetStudentCourses(int studentID,int sessionID)
        {
            try
            {
                var result= db.Courses.Join(db.Enrollments, course=> course.id,enrollment=> enrollment.course_id,(course, enrollment) => new { course, enrollment }).Where(combined => combined.enrollment.student_id==studentID && combined.enrollment.session_id==sessionID).Select(c => c.course).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStudentCourseTeacher(int studentID, int courseID, int sessionID)
        {
            try
            {
                var result = db.Employees.Join(db.Enrollments, teacher => teacher.id, enrollment => enrollment.teacher_id, (teacher, enrollment) => new { teacher, enrollment }).Where(combined => combined.enrollment.student_id == studentID && combined.enrollment.session_id == sessionID && combined.enrollment.course_id == courseID).Select(e => e.teacher).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
