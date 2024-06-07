using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class GroupKpiDetailsPutRequest
    {
        public GroupKpi groupKpi { get; set; }
        public List<KpiWithSubKpis> kpiList { get; set; }
    }
}