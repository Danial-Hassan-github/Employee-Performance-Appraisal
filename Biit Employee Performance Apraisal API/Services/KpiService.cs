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
            int id=db.Kpis.Where(kpi => kpi.name.Equals(kpi_title)).FirstOrDefault().id;
            return id;
        }

        public int getSubKpiID(string sub_kpi_title)
        {
            int id = db.SubKpis.Where(kpi => kpi.name.Equals(sub_kpi_title)).FirstOrDefault().id;
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
            double weightage = db.KpiWeightages.Where(x => x.kpi_id == kpi_id && x.session_id == sessionID).FirstOrDefault().weightage;
            return weightage;
        }

        public bool isWeightageExceeded(int newKpiWeightage, int sessionID)
        {
            var totalWeightage=db.KpiWeightages.Where(x => x.session_id == sessionID).Sum(y=>y.weightage);
            if ((totalWeightage+newKpiWeightage)>100)
            {
                return true;
            }
            return false;
        }

        public int getPreviousKpisTotalWeightage(int newKpiWeightage, int sessionID)
        {
            var previousKpisTotalWeightage = db.KpiWeightages.Where(x => x.session_id == sessionID).Sum(y => y.weightage);
            int leftOverWeightageForPreviousKpis = 100 - newKpiWeightage;
            //previousKpisTotalWeightage = previousKpisTotalWeightage - newKpiWeightage;
            return leftOverWeightageForPreviousKpis;//adjusted weigtage for previous kpi's
        }

        /*public bool adjustKpiWeigtage(int weightage, int sessionID)
        {
            try
            {
                if (isWeightageExceeded(weightage, sessionID))
                {
                    List<KpiWeightage> kpi_weightage_list = db.Kpis.Join(db.KpiWeightages, kpi => kpi.id, kpi_weightage => kpi_weightage.kpi_id, (kpi, kpi_weightage) => new { kpi, kpi_weightage }).
                        Select(combined => combined.kpi_weightage).Where(kpi_weightage => kpi_weightage.session_id == sessionID).ToList();
                    int previousKpisTotalWeightage = getPreviousKpisTotalWeightage(weightage, sessionID);
                    double percentageToSet = previousKpisTotalWeightage * 0.01;
                    foreach (var item in kpi_weightage_list)
                    {
                        double itemWeightage = percentageToSet * (item.weightage);
                        item.weightage = Convert.ToInt32(itemWeightage);
                    }
                    db.SaveChanges();
                }
                return true;
            }catch(Exception e) {
                return false;
            }
        }*/

        public bool adjustKpiWeightages(int weightage,int sessionID,int kpi_id,int employeeTypeID)
        {
            try
            {
                if (isWeightageExceeded(weightage,sessionID))
                {
                    List<KpiWeightage> kpi_weightage_list = db.Kpis
                        .Join(db.KpiWeightages, kpi => kpi.id, kpi_weightage => kpi_weightage.kpi_id, (kpi, kpi_weightage) => new { kpi, kpi_weightage })
                        .Join(db.KpiEmployeeTypes, combined => combined.kpi_weightage.kpi_id, kpiEmployeeType => kpiEmployeeType.kpi_id, (combined, kpiEmployeeType) => new { combined, kpiEmployeeType })
                        .Where(combined => combined.combined.kpi_weightage.session_id == sessionID && combined.combined.kpi_weightage.kpi_id != kpi_id && combined.kpiEmployeeType.id == employeeTypeID)
                        .Select(combined => combined.combined.kpi_weightage)
                        .ToList();

                    /*List<KpiWeightage> kpi_weightage_list = db.Kpis.Join(db.KpiWeightages, kpi => kpi.id, kpi_weightage => kpi_weightage.kpi_id, (kpi, kpi_weightage) => new { kpi, kpi_weightage }).Join(db.KpiEmployeeTypes,combined => combined.kpi.id,type => type.kpi_id, (combined, type) => new {combined, type })
                        Select(combined => combined.kpi_weightage).Where(kpi_weightage => kpi_weightage.session_id == sessionID && kpi_weightage.kpi_id != kpi_id).ToList();*/
                    int sumOfWeightage = db.KpiWeightages.Where(x => x.session_id == sessionID && x.kpi_id != kpi_id).Sum(y => y.weightage);
                    int leftOverWeightage = getPreviousKpisTotalWeightage(weightage, sessionID);
                    int difference = sumOfWeightage - leftOverWeightage;
                    foreach (var item in kpi_weightage_list)
                    {
                        double newWeightage = item.weightage - (((double)item.weightage / sumOfWeightage) * difference); ;
                        item.weightage = Convert.ToInt32(newWeightage);
                    }
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public double getSubKpiWeightage(int sub_kpi_id, int sessionID)
        {
            double weightage = db.SubKpiWeightages.Where(x => x.sub_kpi_id == sub_kpi_id && x.session_id == sessionID).FirstOrDefault().weightage;
            return weightage;
        }
    }
}