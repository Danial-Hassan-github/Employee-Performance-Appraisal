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


        /*
         for adding new kpi
        */
        [HttpPost]
        [Route("api/KPI/PostKPI")]
        public HttpResponseMessage PostKPI([FromBody] Kpi kPI, int weightage, int sessionID, int employeeTypeID)
        {
            try
            {
                if (weightage > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Weightage caannot be more than 100");
                }
                var k = db.Kpis.Add(kPI);
                // db.SaveChanges();
                kpiService.adjustKpiWeightages(weightage, sessionID, k.id, employeeTypeID);
                //kpiService.adjustKpiWeigtage(weightage, sessionID);
                KpiWeightage kpiWeightage = new KpiWeightage();
                kpiWeightage.kpi_id = k.id;
                kpiWeightage.weightage = weightage;
                kpiWeightage.session_id = sessionID;
                db.KpiWeightages.Add(kpiWeightage);
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

        [HttpGet]
        [Route("api/KPI/GetKpiGroup")]
        public HttpResponseMessage GetKpiGroup(int groupID)
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
                                    where kw.group_kpi_id == id // Use id here
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
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
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