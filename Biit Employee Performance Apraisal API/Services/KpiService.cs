using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class KpiService
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        
        public int getKpiID(string kpi_title)
        {
            int id=db.Kpis.Where(kpi => kpi.name.Equals(kpi_title)).First().id;
            return id;
        }

        public int getSubKpiID(string sub_kpi_title)
        {
            int id = db.SubKpis.Where(kpi => kpi.name.Equals(sub_kpi_title)).First().id;
            return id;
        }
        public bool isFreeKpi(int kpi_id)
        {
            int count=db.SubKpis.Where(sk => sk.kpi_id == kpi_id).Count();
            if (count==0)
            {
                return true;
            }
            return false;
        }

        public double getKpiWeightage(int kpi_id,int sessionID)
        {
            double weightage = db.KpiWeightages.Where(x => x.kpi_id == kpi_id && x.session_id == sessionID).First().weightage;
            return weightage;
        }

        public double getSubKpiWeightage(int sub_kpi_id, int sessionID)
        {
            double weightage = db.SubKpiWeightages.Where(x => x.sub_kpi_id == sub_kpi_id && x.session_id == sessionID).First().weightage;
            return weightage;
        }
    }
}