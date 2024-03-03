using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class EmployeeService
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        public string message = string.Empty;
        public bool AddEmployee(Employee employee)
        {
            if (ValidateEmployeeData(employee))
            {
                try
                {
                    db.Employees.Add(employee);
                    int i = db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool UpdateEmployee(Employee employee)
        {
            if (ValidateEmployeeData(employee))
            {
                try
                {
                    var emp = db.Employees.Find(employee.id);
                    if (emp!=null)
                    {
                        emp.name = employee.name;
                        emp.email = employee.email;
                        emp.password = employee.password;
                        emp.designation_id = employee.designation_id;
                        emp.salary = employee.salary;
                        emp.department_id = employee.department_id;
                        emp.employee_type_id = employee.employee_type_id;
                        emp.doj = employee.doj;
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool ValidateEmployeeData(Employee employee)
        {
            if (employee.name == string.Empty)
            {
                message = "Please Enter Name";
            }
            else if (!Regex.IsMatch(employee.email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                if (string.IsNullOrEmpty(employee.email))
                    message = "Please Enter Email";
                else
                    message = "Email is not valid";
            }
            else if (!Regex.IsMatch(employee.password, @".{6,10}$"))
            {
                if (string.IsNullOrEmpty(employee.name))
                    message = "Please Enter Password";
                else
                    message = "Password should be of 6 to 12 characters";
            }
            else if (employee.designation_id != null)
            {
                message = "Please Select Designation";
            }
            else if (employee.department_id != null)
            {
                message = "Please Select Department";
            }
            else if (employee.salary == null)
            {
                message = "Please Enter Salary";
            }
            else if (employee.doj == null)
            {
                message = "Please Enter Date of Joining";
            }

            if (message==string.Empty)
                return true;

            return false;
        }
    }
}