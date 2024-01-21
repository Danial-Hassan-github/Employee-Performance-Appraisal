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
                db.SaveChanges();
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
                var employeeScore=db.KpiEmployeeScores.Find(kpiEmployeeScore.kpi_id, kpiEmployeeScore.employee_id, kpiEmployeeScore.session_id);
                employeeScore.score += kpiEmployeeScore.score;
                if (employeeScore.total_score!=null)
                {
                    employeeScore.total_score += kpiEmployeeScore.total_score;
                }
                else
                {
                    employeeScore.total_score = kpiEmployeeScore.total_score;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateEmployeeSubKpiScore(SubkpiEmployeeScore subKpiEmployeeScore)
        {
            try
            {
                var employeeScore = db.SubkpiEmployeeScores.Find(subKpiEmployeeScore.subkpi_id, subKpiEmployeeScore.employee_id, subKpiEmployeeScore.session_id);
                employeeScore.score += subKpiEmployeeScore.score;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool isSubKpiEmployeeScoreExists(SubkpiEmployeeScore subkpiEmployeeScore)
        {
            var employeeScore = db.SubkpiEmployeeScores.Find(subkpiEmployeeScore.subkpi_id, subkpiEmployeeScore.employee_id, subkpiEmployeeScore.session_id);
            if (employeeScore != null)
            {
                return true;
            }
            return false;
        }

        public bool isEmployeeScoreExists(KpiEmployeeScore kpiEmployeeScore)
        {
            var employeeScore=db.KpiEmployeeScores.Find(kpiEmployeeScore.kpi_id,kpiEmployeeScore.employee_id,kpiEmployeeScore.session_id);
            if (employeeScore!=null)
            {
                return true;
            }
            return false;
        }

        public bool AddEvaluationScores(int sessionID,int sub_kpi_id,int employeeID,int score)
        {
            try
            {
                KpiEmployeeScore kpiEmployeeScores = new KpiEmployeeScore();
                SubkpiEmployeeScore subkpiEmployeeScores = new SubkpiEmployeeScore();
                subkpiEmployeeScores.subkpi_id = sub_kpi_id;
                subkpiEmployeeScores.employee_id = employeeID;
                subkpiEmployeeScores.session_id = sessionID;
                subkpiEmployeeScores.score = score;
                if (isSubKpiEmployeeScoreExists(subkpiEmployeeScores))
                {
                    UpdateEmployeeSubKpiScore(subkpiEmployeeScores);
                }
                else
                {
                    db.SubkpiEmployeeScores.Add(subkpiEmployeeScores);
                }
                kpiEmployeeScores.kpi_id = db.SubKpis.Where(x=>x.id==sub_kpi_id).FirstOrDefault().kpi_id;
                kpiEmployeeScores.employee_id = employeeID;
                kpiEmployeeScores.session_id = sessionID;
                kpiEmployeeScores.score = score;
                if (isEmployeeScoreExists(kpiEmployeeScores))
                {
                    UpdateEmployeeKpiScore(kpiEmployeeScores);
                }
                else
                {
                    AddEmployeeKpiScore(kpiEmployeeScores);
                }
                db.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}