using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class GroupKpiWithWeightage
    {
        public Kpi kpi { get; set; }
        public List<SubKpiWeightage> subKpiWeightages { get; set; }
        public int department_id {  get; set; }
        public int designation_id { get; set; }
        public int employee_type_id {  get; set; }
        public int employee_id {  get; set; }
        public int weightage { get; set; }
        public int session_id { get; set; }
    }
}