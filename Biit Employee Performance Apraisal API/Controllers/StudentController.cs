using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class StudentController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        [Route("api/Student/GetStudentsBySection")]
        public HttpResponseMessage GetStudentsBySection(int semester, string section)
        {
            try
            {
                var result = db.Students.Where(x => x.semester == semester && x.section.Equals(section)).OrderByDescending(x => x.cgpa).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // CREATED BY SHAISTA CHANNA
        [HttpGet]
        [Route("api/Student/GetStudentSessionTeacher")]
        public HttpResponseMessage GetStudentSessionTeacher(int studentID, int sessionID)
        {
            try
            {
                var result = db.Employees.Join(db.Enrollments, teacher => teacher.id, enrollment => enrollment.teacher_id, (teacher, enrollment) => new { teacher, enrollment }).Where(combined => combined.enrollment.student_id == studentID && combined.enrollment.session_id == sessionID).Select(e => e.teacher).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /*[HttpGet]
        [Route("api/Student/GetStudentCourses")]
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
        }*/

        [HttpPost]
        public HttpResponseMessage PostConfidentialEvaluatorStudents([FromBody] List<ConfidentialEvaluatorStudent> confidentialEvaluatorStudents)
        {
            try
            {
                var result = db.ConfidentialEvaluatorStudents.AddRange(confidentialEvaluatorStudents);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Confidential Evaluator Students Added Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /*[HttpGet]
        public HttpResponseMessage GetCourseTeacher(int studentID, int courseID, int sessionID)
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
        }*/
    }
}
