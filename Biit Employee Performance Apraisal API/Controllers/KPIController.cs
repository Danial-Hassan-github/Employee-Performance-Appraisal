using Biit_Employee_Performance_Apraisal_API.Models;
using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class KPIController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        KpiService kpiService = new KpiService();


        /*
         GetKPIs used to fetch all availble kpi,s wheater they're included in session or not
        */
        [HttpGet]
        [Route("api/KPI/GetKPIs")]
        public HttpResponseMessage GetKPIs()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Kpis);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /*
         GetSessionKPIs is being used to fetch kpi's availbe in specific session
        */
        [HttpGet]
        [Route("api/KPI/GetSessionKPIs")]
        public HttpResponseMessage GetSessionKPIs(int sessionID)
        {
            try
            {
                var result = db.KpiWeightages
                    .Join(db.Kpis,
                          weightage => weightage.kpi_id,
                          kpi => kpi.id,
                          (weightage, kpi) => new { Weightage = weightage, Kpi = kpi })
                    .Where(combined => combined.Weightage.session_id == sessionID)
                    .GroupBy(combined => combined.Weightage.group_kpi_id)
                    .Select(group => new
                    {
                        GroupKpiId = group.Key,
                        Records = group.Select(g => new
                        {
                                g.Kpi.id,
                                g.Kpi.name,
                                kpiWeightage = g.Weightage
                        }).ToList()
                    })
                    .ToList();

                var enrichedResult = result
                    .GroupJoin(db.GroupKpis,
                               grouped => grouped.GroupKpiId,
                               groupKpi => groupKpi.id,
                               (grouped, groupKpi) => new
                               {
                                   groupKpi = groupKpi.FirstOrDefault(), // Default to null if no match
                                   records = grouped.Records
                               })
                    .Select(grp => new
                    {
                        groupKpi = grp.groupKpi == null ? null : new
                        {
                            grp.groupKpi.id,
                            grp.groupKpi.kpi_id,
                            department = db.Departments.FirstOrDefault(d => d.id == grp.groupKpi.department_id),
                            designation = db.Designations.FirstOrDefault(d => d.id == grp.groupKpi.designation_id),
                            dmployeeType = db.EmployeeTypes.FirstOrDefault(e => e.id == grp.groupKpi.employee_type_id),
                            employee = db.Employees.FirstOrDefault(e => e.id == grp.groupKpi.employee_id)
                        },
                        kpiList = grp.records
                    })
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, enrichedResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/KPI/GetUniqueKpis")]
        public HttpResponseMessage GetUniqueKpis()
        {
            try
            {
                var uniqueKpis = db.Kpis
        /*.GroupJoin(db.KpiDesignations, k => k.id, kd => kd.kpi_id, (k, kds) => new { k, kds })
        .SelectMany(x => x.kds.DefaultIfEmpty(), (x, kd) => new { x.k, kd })
        .GroupJoin(db.KpiDepartments, x => x.k.id, kdep => kdep.kpi_id, (x, kdeps) => new { x.k, x.kd, kdeps })
        .SelectMany(x => x.kdeps.DefaultIfEmpty(), (x, kdep) => new { x.k, x.kd, kdep, x.kdeps })
        .GroupJoin(db.KpiEmployeeTypes, x => x.k.id, ket => ket.kpi_id, (x, kets) => new { x.k, x.kd, x.kdep, kets })
        .SelectMany(x => x.kets.DefaultIfEmpty(), (x, ket) => new { x.k, x.kd, x.kdep, ket, x.kets })
        .GroupJoin(db.KpiEmployees, x => x.k.id, ke => ke.kpi_id, (x, kes) => new { x.k, x.kd, x.kdep, x.ket, kes })
        .SelectMany(x => x.kes.DefaultIfEmpty(), (x, ke) => new { Kpi = x.k, Designation = x.kd, Department = x.kdep, EmployeeType = x.ket, Employee = ke })*/
        .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, uniqueKpis);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /*
         GetEEmployeeTypeKPIs is being used to fetch kpi's availbe in specific session for specific type of employees
        */
        /*[HttpGet]
        public HttpResponseMessage GetEmployeeTypeKPIs(int employeeTypeID,int sessionID)
        {
            try
            {
                var result = db.KpiWeightages.Join(db.Kpis, x => x.kpi_id, y => y.id, (x, y) => new { x, y }).Where(combined => combined.x.session_id == sessionID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }*/

        [HttpPost]
        [Route("api/KPI/PostGroupKpi")]
        public HttpResponseMessage PostGroupKpi(GroupKpiWithWeightage groupKpiWithWeightage)
        {
            try
            {
                var kpi = db.Kpis.Add(groupKpiWithWeightage.kpi);
                var kpiGroup = db.GroupKpis.Where(x => x.department_id == groupKpiWithWeightage.department_id &&
                                                    x.designation_id == groupKpiWithWeightage.designation_id &&
                                                    x.employee_type_id == groupKpiWithWeightage.employee_type_id).FirstOrDefault();

                KpiWeightage kpiWeightage = new KpiWeightage();
                kpiWeightage.session_id = groupKpiWithWeightage.session_id;
                kpiWeightage.kpi_id = kpi.id;
                kpiWeightage.weightage = groupKpiWithWeightage.weightage;

                // if kpis for that specific group already exists
                if (kpiGroup != null)
                {
                    kpiWeightage.group_kpi_id = kpiGroup.id;
                    db.KpiWeightages.Add(kpiWeightage);
                }
                else
                {
                    GroupKpi groupKpi = new GroupKpi();
                    groupKpi.kpi_id = kpi.id;
                    groupKpi.department_id = groupKpiWithWeightage.department_id;
                    groupKpi.designation_id = groupKpiWithWeightage.designation_id;
                    groupKpi.employee_type_id = groupKpiWithWeightage.employee_type_id;

                    var newKpiGroup = db.GroupKpis.Add(groupKpi);
                    var commonKpis = db.KpiWeightages.Where(x => x.group_kpi_id == null);

                    foreach (KpiWeightage weightage in commonKpis)
                    {
                        weightage.group_kpi_id = groupKpi.id;
                        db.KpiWeightages.Add(weightage);
                    }
                    kpiWeightage.group_kpi_id = newKpiGroup.id;
                    db.KpiWeightages.Add(kpiWeightage);
                }

                if (groupKpiWithWeightage.subKpiWeightages.Any())
                {
                    List<SubKpiWeightage> subKpiWeightages = groupKpiWithWeightage.subKpiWeightages;
                    for (int i = 0; i < subKpiWeightages.Count; i++)
                    {
                        subKpiWeightages[i].kpi_id = kpi.id;
                        subKpiWeightages[i].deleted = false;
                    }
                    db.SubKpiWeightages.AddRange(subKpiWeightages);
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Group kpi Added successfully");
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/KPI/PostEmployeeKpi")]
        public HttpResponseMessage PostEmployeeKpi(EmployeeKpi employeeKpi)
        {
            try
            {
                var employee = employeeKpi.employee;
                var kpi = employeeKpi.kpi;
                var empKpiGroup = db.GroupKpis.Where(x => x.department_id == employee.department_id &&
                                                    x.designation_id == employee.designation_id &&
                                                    x.employee_type_id == employee.employee_type_id &&
                                                    x.employee_id == employee.id).FirstOrDefault();

                KpiWeightage kpiWeightage = new KpiWeightage();
                kpiWeightage.session_id = employeeKpi.session_id;
                kpiWeightage.kpi_id = kpi.id;
                kpiWeightage.weightage = employeeKpi.weightage;

                // if kpis for that specific employee already exists
                if (empKpiGroup != null)
                {
                    kpiWeightage.group_kpi_id = empKpiGroup.id;
                    // check whether the kpi is already available or not
                    var check = db.KpiWeightages.Where(x => x.kpi_id == kpi.id && 
                                                        x.group_kpi_id == kpiWeightage.group_kpi_id &&
                                                        x.session_id == kpiWeightage.session_id).FirstOrDefault();
                    if (check == null)
                    {
                        db.KpiWeightages.Add(kpiWeightage);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee kpi already exists for this employee");
                    }

                }
                // if we are add kpi first time for that employee
                else
                {
                    GroupKpi groupKpi = new GroupKpi();
                    groupKpi.kpi_id = kpi.id;
                    groupKpi.employee_id = employee.id;
                    groupKpi.department_id = employee.department_id;
                    groupKpi.designation_id = employee.designation_id;
                    groupKpi.employee_type_id = employee.employee_type_id;

                    var newKpiGroup = db.GroupKpis.Add(groupKpi);
                    var commonKpis = db.KpiWeightages.Where(x => x.group_kpi_id == null);

                    foreach(KpiWeightage weightage in commonKpis)
                    {
                        weightage.group_kpi_id = groupKpi.id;
                        db.KpiWeightages.Add(weightage);
                    }
                    kpiWeightage.group_kpi_id = newKpiGroup.id;
                    db.KpiWeightages.Add(kpiWeightage);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Kpi Added Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /*
         for adding new kpi
        */
        [HttpPost]
        [Route("api/KPI/PostGeneralKpi")]
        public HttpResponseMessage PostGeneralKpi([FromBody] KpiWithSubKpis kpiWithSubKpis)
        {
            try
            {
                if (kpiWithSubKpis.weightage.weightage > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Weightage caannot be more than 100");
                }
                var k = db.Kpis.Add(kpiWithSubKpis.kpi);
                KpiWeightage kpiWeightage = kpiWithSubKpis.weightage;
                kpiWeightage.kpi_id = k.id;
                db.KpiWeightages.Add(kpiWithSubKpis.weightage);
                if (kpiWithSubKpis.subKpiWeightages.Any())
                {
                    List<SubKpiWeightage> subKpiWeightages = kpiWithSubKpis.subKpiWeightages;
                    for (int i = 0; i < subKpiWeightages.Count; i++)
                    {
                        subKpiWeightages[i].kpi_id = k.id;
                        subKpiWeightages[i].deleted = false;
                    }
                    db.SubKpiWeightages.AddRange(subKpiWeightages);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, k);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /*
         To update kpi settings
         */
        [HttpPut]
        [Route("api/KPI/PutKPI")]
        public HttpResponseMessage PutKPI([FromBody] Kpi kPI, int weightage, int sessionID, int employeeTypeID)
        {
            try
            {
                if (weightage > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Weightage caannot be more than 100%");
                }
                var kpi = db.Kpis.Find(kPI.id);
                kpi.name = kPI.name;
                var kpi_weightage = db.KpiWeightages.Find(kPI.id, sessionID);
                kpiService.adjustKpiWeightages(weightage, sessionID, kPI.id, employeeTypeID);
                int leftOverWeightage = db.KpiWeightages.Where(x => x.session_id == sessionID).Sum(y => y.weightage);
                leftOverWeightage -= weightage;
                kpi_weightage.weightage = weightage;

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, kpi);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/KPI/PostGroupKpi")]
        public HttpResponseMessage PostGroupKpi(GroupKpiDetails groupKpiDetails)
        {
            try
            {
                db.GroupKpis.Add(groupKpiDetails.groupKpi);
                db.Kpis.Add(groupKpiDetails.kpi);
                db.SubKpiWeightages.AddRange(groupKpiDetails.subKpiWeightages);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Group Kpi Added Successfully");
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/KPI/GetKpiGroup")]
        public HttpResponseMessage GetKpiGroup(int groupID, int sessionID)
        {
            try
            {
                int? id = groupID;
                if(groupID == 0)
                {
                    id = null;
                }
                var kpiWeightages = from kw in db.KpiWeightages
                                    join k in db.Kpis on kw.kpi_id equals k.id
                                    where kw.group_kpi_id == id && kw.session_id == sessionID // Use id here
                                    select new
                                    {
                                        k.id,
                                        k.name,
                                        kpiWeightage = kw
                                    };

                return Request.CreateResponse(HttpStatusCode.OK, kpiWeightages.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/KPI/GetKpiWeightages")]
        public HttpResponseMessage GetKpiWeightages([FromBody] GroupKpi groupKpiEntity, int sessionId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, kpiService.GetKpiWeightages(groupKpiEntity, sessionId));
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /*[HttpDelete]
        public HttpResponseMessage DeleteKPI(int id)
        {
            try
            {
                var kpi = db.Kpis.Find(id);
                kpi.status = 1;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,"Deleted Successfully");
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }*/
    }
}