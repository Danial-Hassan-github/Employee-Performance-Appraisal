using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class MultiEmployeeCoursePerformanceRequest
    {
        public List<int> employeeIds {  get; set; }
        public int courseId { get; set; }
        public int sessionId {  get; set; }
    }
}