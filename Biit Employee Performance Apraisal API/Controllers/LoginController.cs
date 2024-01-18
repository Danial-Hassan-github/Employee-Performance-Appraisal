using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class LoginController : ApiController
    {
        Employee employee;
        Student student;
        [HttpGet]
        public HttpResponseMessage Login(string username, string password)
        {
            Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
            try
            {
                if (username.Contains('@'))
                {
                    employee=db.Employees.Where(emp => emp.email.Equals(username) && emp.password.Equals(password)).FirstOrDefault();
                }
                else
                {
                    student=db.Students.Where(std => std.arid_no.Equals(username) && std.password.Equals(password)).FirstOrDefault();
                }
                if (student!=null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, student);
                }
                return Request.CreateResponse(HttpStatusCode.OK, employee);
            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }
    }
}
