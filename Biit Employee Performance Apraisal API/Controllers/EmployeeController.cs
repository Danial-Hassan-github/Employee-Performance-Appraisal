using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Web.Http;
using Biit_Employee_Performance_Apraisal_API.Services;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EmployeeController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        EmployeeService EmployeeService = new EmployeeService();
        [HttpGet]
        public HttpResponseMessage GetEmployees()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Employees);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostEmployee([FromBody] Employee employee)
        {
            if (EmployeeService.AddEmployee(employee))
            {
                return Request.CreateResponse(HttpStatusCode.OK, employee);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError,EmployeeService.message);
        }

        [HttpPut]
        public HttpResponseMessage PutEmployee([FromBody] Employee employee)
        {
            if (EmployeeService.UpdateEmployee(employee))
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Employees);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, EmployeeService.message);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteEmployee(int id)
        {
            try
            {
                var employee = db.Employees.Find(id);
                employee.deleted = true;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully");
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}