using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class EmployeeIdsWithSession
    {
        public List<int> employeeIds { get; set;}
        public int session_id {  get; set;}
    }
}