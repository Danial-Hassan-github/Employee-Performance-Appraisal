using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class EmployeeKpiScore
    {
        public KpiEmployeeScore employeeScore {  get; set; }
        public String KpiTitle { get; set; }
        public int Weightage {  get; set; }
    }
}