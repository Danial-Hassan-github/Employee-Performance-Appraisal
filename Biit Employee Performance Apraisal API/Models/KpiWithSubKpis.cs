using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class KpiWithSubKpis
    {
        public Kpi kpi {  get; set; }
        public KpiWeightage weightage { get; set; }
        public List<SubKpiWeightage> subKpiWeightages { get; set; }
    }
}