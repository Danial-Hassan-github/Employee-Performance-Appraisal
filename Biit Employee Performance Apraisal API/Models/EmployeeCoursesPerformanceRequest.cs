using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class EmployeeCoursesPerformanceRequest
    {
        public int employeeID { get; set; }
        public int sessionID {  get; set; }
        public List<int> coursesIds {  get; set; }
    }
}