using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class EmployeeScoreService
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        public bool AddEmployeeKpiScore(KpiEmployeeScore kpiEmployeeScore)
        {
            try
            {
                db.KpiEmployeeScores.Add(kpiEmployeeScore);
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateEmployeeKpiScore(KpiEmployeeScore kpiEmployeeScore)
        {
            try
            {
                var employeeScore=db.KpiEmployeeScores.Find(kpiEmployeeScore);
                employeeScore.score += kpiEmployeeScore.score;
                employeeScore.total_score += kpiEmployeeScore.total_score;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool isEmployeeScoreExists(KpiEmployeeScore kpiEmployeeScore)
        {
            var employeeScore=db.KpiEmployeeScores.Find(kpiEmployeeScore);
            if (employeeScore!=null)
            {
                return true;
            }
            return false;
        }
    }
}