using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class CourseController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        [Route("api/Course/GetCourses")]
        public HttpResponseMessage GetCourses()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Courses);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Course/GetTeacherCourses")]
        public HttpResponseMessage GetTeacherCourses(int teacherID)
        {
            try
            {
                var result = db.Courses.Join(db.Enrollments, course => course.id, enrollment => enrollment.course_id, (course, enrollment) => new { course, enrollment }).Where(combined => combined.enrollment.teacher_id == teacherID).Select(c => c.course).Distinct().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Course/GetCourseTeachers")]
        public HttpResponseMessage GetCourseTeachers(int studentID, int courseID, int sessionID)
        {
            try
            {
                var result = db.Employees.Join(db.Enrollments, teacher => teacher.id, enrollment => enrollment.teacher_id, (teacher, enrollment) => new { teacher, enrollment })
                    .Where(combined => combined.enrollment.student_id == studentID && combined.enrollment.session_id == sessionID && combined.enrollment.course_id == courseID)
                    .Select(e => e.teacher)
                    .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Course/GetStudentCourses")]
        public HttpResponseMessage GetStudentCourses(int studentID, int sessionID)
        {
            try
            {
                var result = db.Courses.Join(db.Enrollments, course => course.id, enrollment => enrollment.course_id, (course, enrollment) => new { course, enrollment }).Where(combined => combined.enrollment.student_id == studentID && combined.enrollment.session_id == sessionID).Select(c => c.course).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
