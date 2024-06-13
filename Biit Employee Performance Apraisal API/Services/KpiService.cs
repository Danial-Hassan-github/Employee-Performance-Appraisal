using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{      
    public class KpiService
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        
        public int getKpiID(string kpi_title)
        {
            int id=db.Kpis
                .Where(kpi => kpi.name.Equals(kpi_title)).
                FirstOrDefault().id;
            return id;
        }

        public bool isFreeKpi(int kpi_id, int session_id)
        {
            int count=db.SubKpiWeightages
                .Where(sk => sk.kpi_id == kpi_id && sk.session_id == session_id)
                .Count();
            if (count==0)
            {
                return true;
            }
            return false;
        }

        public double getKpiWeightage(int kpi_id,int sessionID)
        {
            double weightage = db.KpiWeightages
                .Where(x => x.kpi_id == kpi_id && x.session_id == sessionID)
                .FirstOrDefault().weightage;
            return weightage;
        }

        public bool isWeightageExceeded(int newKpiWeightage, int sessionID)
        {
            var totalWeightage=db.KpiWeightages
                .Where(x => x.session_id == sessionID)
                .Sum(y=>y.weightage);
            if ((totalWeightage+newKpiWeightage)>100)
            {
                return true;
            }
            return false;
        }

        public int getPreviousKpisTotalWeightage(int newKpiWeightage, int sessionID)
        {
            var previousKpisTotalWeightage = db.KpiWeightages
                .Where(x => x.session_id == sessionID)
                .Sum(y => y.weightage);
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
                    /*List<KpiWeightage> kpi_weightage_list = db.Kpis
                        .Join(db.KpiWeightages, kpi => kpi.id, kpi_weightage => kpi_weightage.kpi_id, (kpi, kpi_weightage) => new { kpi, kpi_weightage })
                        .Join(db.KpiEmployeeTypes, combined => combined.kpi_weightage.kpi_id, kpiEmployeeType => kpiEmployeeType.kpi_id, (combined, kpiEmployeeType) => new { combined, kpiEmployeeType })
                        .Where(combined => combined.combined.kpi_weightage.session_id == sessionID && combined.combined.kpi_weightage.kpi_id != kpi_id && combined.kpiEmployeeType.employee_type_id == employeeTypeID)
                        .Select(combined => combined.combined.kpi_weightage)
                        .ToList();

                    *//*List<KpiWeightage> kpi_weightage_list = db.Kpis.Join(db.KpiWeightages, kpi => kpi.id, kpi_weightage => kpi_weightage.kpi_id, (kpi, kpi_weightage) => new { kpi, kpi_weightage }).Join(db.KpiEmployeeTypes,combined => combined.kpi.id,type => type.kpi_id, (combined, type) => new {combined, type })
                        Select(combined => combined.kpi_weightage).Where(kpi_weightage => kpi_weightage.session_id == sessionID && kpi_weightage.kpi_id != kpi_id).ToList();*//*
                    int sumOfWeightage = db.KpiWeightages.Where(x => x.session_id == sessionID && x.kpi_id != kpi_id).Sum(y => y.weightage);
                    int leftOverWeightage = getPreviousKpisTotalWeightage(weightage, sessionID);
                    int difference = sumOfWeightage - leftOverWeightage;
                    foreach (var item in kpi_weightage_list)
                    {
                        double newWeightage = item.weightage - (((double)item.weightage / sumOfWeightage) * difference); ;
                        item.weightage = Convert.ToInt32(newWeightage);
                    }
                    db.SaveChanges();*/
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void UpdateWeightage(List<KpiWeightage> kpiWeightages)
        {
            var recordsToUpdate = db.KpiWeightages.Where(x => kpiWeightages.Select(k => k.id).Contains(x.id)).ToList();

            foreach (var record in recordsToUpdate)
            {
                var kpiWeightage = kpiWeightages.Find(k => k.id == record.id);

                record.weightage = kpiWeightage.weightage;

                db.Entry(record).State = EntityState.Modified;
            }

            db.SaveChanges();
        }


        public void AddKpi(Kpi kpi, KpiWeightage kpiWeightage, GroupKpi groupKpi)
        {
            try
            {
                Kpi kpiEntity = db.Kpis.Add(kpi);
                int sessionID = kpiWeightage.session_id;

                if (groupKpi != null)
                {
                    groupKpi.kpi_id = kpiEntity.id;
                    if (groupKpi.employee_id != null)
                    {
                        Employee employee = db.Employees.FirstOrDefault(x => x.id == groupKpi.employee_id);
                        if (employee != null)
                        {
                            groupKpi.designation_id = employee.designation_id;
                            groupKpi.department_id = employee.department_id;
                            groupKpi.employee_type_id = employee.employee_type_id;
                        }
                    }

                    GroupKpi groupKpiEntity = db.GroupKpis.Add(groupKpi);
                    kpiWeightage.kpi_id = kpiEntity.id;
                    // kpiWeightage.group_kpi_id = groupKpiEntity.id;
                    db.KpiWeightages.Add(kpiWeightage);

                    List<KpiWeightage> kpiWeightages = GetKpiWeightages(groupKpiEntity, sessionID);
                }

                // List<KpiWeightage> unGroupKpiWeightages = db.KpiWeightages.Where(x => x.group_kpi_id == null && x.session_id == sessionID).ToList();

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, rethrow it, etc.)
                // Logging the exception could be done using a logging framework
                // For simplicity, just rethrowing it here
                throw;
            }
        }

        public List<KpiWeightage> GetKpiWeightages(GroupKpi groupKpiEntity, int sessionId)
        {
            IQueryable<GroupKpi> query = db.GroupKpis;

            if (groupKpiEntity.designation_id != null && groupKpiEntity.department_id == null && groupKpiEntity.employee_type_id == null)
            {
                query = query.Where(x => x.designation_id == groupKpiEntity.designation_id);
            }
            else if (groupKpiEntity.designation_id == null && groupKpiEntity.department_id != null && groupKpiEntity.employee_type_id == null)
            {
                query = query.Where(x => x.department_id == groupKpiEntity.department_id);
            }
            else if (groupKpiEntity.designation_id == null && groupKpiEntity.department_id == null && groupKpiEntity.employee_type_id != null)
            {
                query = query.Where(x => x.employee_type_id == groupKpiEntity.employee_type_id);
            }
            else if (groupKpiEntity.designation_id != null && groupKpiEntity.department_id != null && groupKpiEntity.employee_type_id == null)
            {
                query = query.Where(x => x.designation_id == groupKpiEntity.designation_id && x.department_id == groupKpiEntity.department_id);
            }
            else if (groupKpiEntity.designation_id != null && groupKpiEntity.department_id == null && groupKpiEntity.employee_type_id != null)
            {
                query = query.Where(x => x.designation_id == groupKpiEntity.designation_id && x.employee_type_id == groupKpiEntity.employee_type_id);
            }
            else if (groupKpiEntity.designation_id == null && groupKpiEntity.department_id != null && groupKpiEntity.employee_type_id != null)
            {
                query = query.Where(x => x.department_id == groupKpiEntity.department_id && x.employee_type_id == groupKpiEntity.employee_type_id);
            }
            else if (groupKpiEntity.designation_id != null && groupKpiEntity.department_id != null && groupKpiEntity.employee_type_id != null)
            {
                query = query.Where(x => x.designation_id == groupKpiEntity.designation_id &&
                                         x.department_id == groupKpiEntity.department_id &&
                                         x.employee_type_id == groupKpiEntity.employee_type_id);
            }
            else
            {
                return db.KpiWeightages.Where(x => x.session_id == sessionId).ToList();
            }

            List<int> groupKpiIds = query.Select(y => y.id).Distinct().ToList();
            return db.KpiWeightages
                     .Where(k => k.session_id == sessionId)
                     .ToList();
        }
    }
}