using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class SubKpiService
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        public double getSubKpiWeightage(int sub_kpi_id, int sessionID)
        {
            double weightage = db.SubKpiWeightages
                .Where(x => x.sub_kpi_id == sub_kpi_id && x.session_id == sessionID)
                .FirstOrDefault().weightage;
            return weightage;
        }

        public int getSubKpiID(string sub_kpi_title)
        {
            int id = db.SubKpis
                .Where(kpi => kpi.name.StartsWith(sub_kpi_title))
                .FirstOrDefault().id;
            return id;
        }
    }
}