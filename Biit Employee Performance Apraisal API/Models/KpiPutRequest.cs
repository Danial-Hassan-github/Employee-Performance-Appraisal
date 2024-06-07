using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class KpiPutRequest
    {
        public int group_kpi_id {  get; set; }
        public int id {  get; set; }
        public string name { get; set; }
        public KpiWeightage kpiWeightage { get; set; }
        public List<SubKpiWeightage> subKpiWeightages { get; set; }
    }
}