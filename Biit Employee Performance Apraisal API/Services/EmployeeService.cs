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
        public bool AddEmployee(EMPLOYEE employee)
        {
            if (ValidateEmployeeData(employee))
            {
                try
                {
                    db.EMPLOYEEs.Add(employee);
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

        public bool UpdateEmployee(EMPLOYEE employee)
        {
            if (ValidateEmployeeData(employee))
            {
                try
                {
                    var emp = db.EMPLOYEEs.Find(employee.EmployeeID);
                    if (emp!=null)
                    {
                        emp.Name = employee.Name;
                        emp.Email = employee.Email;
                        emp.Password = employee.Password;
                        emp.Designation = employee.Designation;
                        emp.Salary = employee.Salary;
                        emp.EmployeeTypeID = employee.EmployeeTypeID;
                        emp.DOJ = employee.DOJ;
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

        public bool ValidateEmployeeData(EMPLOYEE employee)
        {
            /*if (employee.Name == string.Empty)
            {
                message = "Please Enter Name";
            }
            else if (Regex.IsMatch(employee.Email, @""))
            {
                if (string.IsNullOrEmpty(employee.Name))
                    message = "Please Enter Email";
                else
                    message = "Email is not valid";
            }
            else if (Regex.IsMatch(employee.Password, @""))
            {
                if (string.IsNullOrEmpty(employee.Name))
                    message = "Please Enter Password";
                else
                    message = "Password should be of 6 to 12 characters";
            }
            else if (employee.Designation == string.Empty)
            {
                message = "Please Select Designation";
            }
            else if (employee.Department == string.Empty)
            {
                message = "Please Select Department";
            }
            else if (employee.Salary == null)
            {
                message = "Please Enter Salary";
            }
            else if (employee.DOJ == null)
            {
                message = "Please Enter Date of Joining";
            }*/

            if (message==string.Empty)
                return true;

            return false;
        }
    }
}