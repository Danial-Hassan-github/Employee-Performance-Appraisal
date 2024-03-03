using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Web.Http;
using Biit_Employee_Performance_Apraisal_API.Services;
using Biit_Employee_Performance_Apraisal_API.Models;
using Newtonsoft.Json;
using System.Text;

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
                var employeesDetails = db.Employees
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
    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, employeesDetails);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Employee/GetEmployeesWithKpiScores")]
        public HttpResponseMessage GetEmployeesWithKpiScores()
        {
            try
            {
                var employeesWithScores = db.Employees
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
                            emp = empDesigDept.Employee,
                            desig = empDesigDept.Designation,
                            dep = empDesigDept.Department,
                            empType1 = empType
                        })
                    .GroupJoin(db.KpiEmployeeScores,
                        emp => emp.emp.id,
                        score => score.employee_id,
                        (emp, scores) => new
                        {
                            employee = emp.emp,
                            designation = emp.desig,
                            department = emp.dep,
                            employeeType = emp.empType1,
                            totalScore = scores.Sum(s => s.score) ?? 0
                        })
                    .OrderByDescending(e => e.totalScore)
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, employeesWithScores);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
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