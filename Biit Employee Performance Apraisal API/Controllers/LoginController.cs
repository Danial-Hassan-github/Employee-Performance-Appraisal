using Biit_Employee_Performance_Apraisal_API.Models;
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
        public HttpResponseMessage Login(string emailOrAridNo, string password)
        {
            Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
            try
            {
                if (emailOrAridNo.Contains('@'))
                {
                    var employeeDetails = db.Employees
    .Where(emp => emp.email == emailOrAridNo && emp.password == password)
    .Join(db.Designations,
          emp => emp.designation_id,
          desig => desig.id,
          (emp, desig) => new { Employee = emp, Designation = desig })
    .Join(db.Departments,
          empDesig => empDesig.Employee.department_id,
          dept => dept.id,
          (empDesig, dept) => new { empDesig.Employee, empDesig.Designation, Department = dept })
    .Join(db.EmployeeTypes,
          empDesigDept => empDesigDept.Employee.employee_type_id,
          empType => empType.id,
          (empDesigDept, empType) => new
          {
              employee = empDesigDept.Employee,
              designation = empDesigDept.Designation,
              department = empDesigDept.Department,
              employeeType = empType
          })
    .FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, employeeDetails);
                }
                else
                {
                    student=db.Students.Where(std => std.arid_no.Equals(emailOrAridNo) && std.password.Equals(password)).FirstOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, student);
                }
            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }
    }
}
