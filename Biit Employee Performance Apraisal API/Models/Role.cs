using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class Role
    {
        public Designation Designation { get; set; }
        public Department Department { get; set; }
        public EmployeeType EmployeeType { get; set; }
    }
}