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
                return Request.CreateResponse(HttpStatusCode.OK, db.Sessions);
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
                return Request.CreateResponse(HttpStatusCode.OK, db.Sessions);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
