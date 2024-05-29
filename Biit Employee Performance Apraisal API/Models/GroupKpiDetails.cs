using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class GroupKpiDetails
    {
        public GroupKpi groupKpi {  get; set; }
        public Kpi kpi { get; set; }
        public KpiWeightage kpiWeightage { get; set; }
        public List<SubKpiWeightage> subKpiWeightages { get; set;}
    }
}