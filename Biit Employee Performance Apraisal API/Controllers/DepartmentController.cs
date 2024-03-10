using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class DepartmentController : ApiController
    {

        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        public HttpResponseMessage GetDepartments()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Departments);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
