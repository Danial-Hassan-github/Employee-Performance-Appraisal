using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class EmployeeKpi
    {
        public Employee employee { get; set; }
        public Kpi kpi { get; set; }
        public int group_kpi_id {  get; set; }
        public int weightage { get; set; }
        public int session_id { get; set; }
    }
}