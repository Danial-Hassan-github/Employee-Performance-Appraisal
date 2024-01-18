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
        
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage GetCourses()
        {
            Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
            var courses = db.Courses;
            return Request.CreateResponse(HttpStatusCode.OK, courses);
        }

        // GET api/<controller>/5
        /*public string Get(int id)
        {
            return "value";
        }*/

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}