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
        EMPLOYEE employee;
        STUDENT student;
        [HttpGet]
        public HttpResponseMessage Login(string username, string password)
        {
            Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
            try
            {
                if (username.Contains('@'))
                {
                    employee=db.EMPLOYEEs.Where(emp => emp.Email.Equals(username) && emp.Password.Equals(password)).FirstOrDefault();
                }
                else
                {
                    student=db.STUDENTs.Where(std => std.AridNO.Equals(username) && std.Password.Equals(password)).FirstOrDefault();
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
