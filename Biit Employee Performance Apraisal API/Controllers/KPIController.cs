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
                    .GroupBy(combined => combined.Kpi.department_id)
                    .Select(group => new
                    {
                        departmentId = group.Key,
                        kpiList = group.Select(g => new
                        {
                            g.Kpi.id,
                            g.Kpi.name,
                            kpiWeightage = g.Weightage
                        }).ToList()
                    })
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [HttpPost]
        [Route("api/KPI/PostKpi")]
        public HttpResponseMessage PostKpi([FromBody] KpiWithSubKpis kpiWithSubKpis)
        {
            try
            {
                if (kpiWithSubKpis.weightage.weightage > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Weightage cannot be more than 100");
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

        [HttpPut]
        [Route("api/KPI/PutKpi")]
        public HttpResponseMessage PutKpi(List<KpiPutRequest> kpiPutRequests)
        {
            try
            {
                foreach (KpiPutRequest k in kpiPutRequests)
                {
                    var kpi = db.Kpis.Where(x => x.id == k.id).FirstOrDefault();
                    kpi.name = k.name;
                    var kpiWeightage = db.KpiWeightages.Where(x => x.kpi_id == k.id).FirstOrDefault();
                    kpiWeightage.weightage = k.kpiWeightage.weightage;
                    if (k.subKpiWeightages != null)
                    {
                        foreach (var item in k.subKpiWeightages)
                        {
                            var subKpiWeightage = db.SubKpiWeightages.Where(x => x.sub_kpi_id == item.sub_kpi_id && x.session_id == item.session_id && x.deleted == false).FirstOrDefault();
                            if (subKpiWeightage != null)
                            {
                                subKpiWeightage.weightage = item.weightage;
                            }
                            else
                            {
                                item.deleted = false;
                                item.kpi_id = k.id;
                                db.SubKpiWeightages.Add(item);
                            }
                        }

                        foreach (var item in k.deletedSubKpis)
                        {
                            var subKpiWeightage = db.SubKpiWeightages.Where(x => x.sub_kpi_id == item.sub_kpi_id && x.kpi_id == k.id && x.session_id == item.session_id && x.deleted == false).FirstOrDefault();
                            if (subKpiWeightage != null)
                            {
                                subKpiWeightage.deleted = true;
                            }
                        }
                    }
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "kpi Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
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

        /*[HttpPost]
        [Route("api/KPI/PostGroupKpi")]
        public HttpResponseMessage PostGroupKpi(GroupKpiWithWeightage groupKpiWithWeightage)
        {
            try
            {
                var kpi = db.Kpis.Add(groupKpiWithWeightage.kpi);
                var kpiGroup = db.GroupKpis.Where(x => x.department_id == groupKpiWithWeightage.department_id &&
                                                    x.designation_id == groupKpiWithWeightage.designation_id &&
                                                    x.employee_type_id == groupKpiWithWeightage.employee_type_id &&
                                                    x.employee_id == groupKpiWithWeightage.employee_id).FirstOrDefault();

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
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Group kpi Added successfully");
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }*/

        /*[HttpPost]
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
        }*/


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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Weightage cannot be more than 100");
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
        /*[HttpPut]
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
        }*/

        /*[HttpPost]
        [Route("api/KPI/PostGroupKpi")]
        public HttpResponseMessage PostGroupKpi(GroupKpiWithWeightage groupKpiWithWeightage)
        {
            try
            {
                GroupKpi group = new GroupKpi();
                group.department_id = groupKpiWithWeightage.department_id;
                group.designation_id = groupKpiWithWeightage.designation_id;
                group.employee_type_id = groupKpiWithWeightage.employee_type_id;
                db.GroupKpis.Add(group);
                db.Kpis.Add(groupKpiWithWeightage.kpi);
                db.SubKpiWeightages.AddRange(groupKpiWithWeightage.subKpiWeightages);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Group Kpi Added Successfully");
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }*/

        /*[HttpGet]
        [Route("api/KPI/GetKpiGroupId")]
        public HttpResponseMessage GetKpiGroupId(int department_id, int designation_id, int employee_type_id, int employee_id)
        {
            try
            {
                var group = db.GroupKpis.Where(x => x.employee_type_id == employee_type_id && x.designation_id == designation_id && x.department_id == department_id && x.employee_id == employee_id).FirstOrDefault();
                if (group == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
                return Request.CreateResponse(HttpStatusCode.OK, group.id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }*/

        /*[HttpGet]
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
        }*/

        /*[HttpPut]
        [Route("api/KPI/PutGeneralKpi")]
        public HttpResponseMessage PutGeneralKpi(List<KpiPutRequest> kpiPutRequests)
        {
            try
            {
                foreach (KpiPutRequest k in kpiPutRequests)
                {
                    var kpi = db.Kpis.Where(x => x.id == k.id).FirstOrDefault();
                    kpi.name = k.name;
                    var kpiWeightage = db.KpiWeightages.Where(x => x.kpi_id == k.id).FirstOrDefault();
                    kpiWeightage.weightage = k.kpiWeightage.weightage;
                    if(k.subKpiWeightages != null)
                    {
                        foreach (var item in k.subKpiWeightages)
                        {
                            var subKpiWeightage = db.SubKpiWeightages.Where(x => x.id == item.id && x.deleted == false).FirstOrDefault();
                            if (subKpiWeightage != null)
                            {
                                subKpiWeightage.weightage = item.weightage;
                            }
                            else
                            {
                                item.deleted = false;
                                db.SubKpiWeightages.Add(item);
                            }
                        }
                    }
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "General kpi Updated Successfully");
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }*/

        /*[HttpPut]
        [Route("api/KPI/PutGroupKpi")]
        public HttpResponseMessage PutGroupKpi(List<KpiPutRequest> kpiPutRequests)
        {
            try
            {
                foreach (KpiPutRequest k in kpiPutRequests)
                {
                    var kpi = db.Kpis.Where(x => x.id == k.id).FirstOrDefault();
                    kpi.name = k.name;
                    var kpiWeightage = db.KpiWeightages.Where(x => x.kpi_id == k.id && x.group_kpi_id == k.kpiWeightage.group_kpi_id).FirstOrDefault();
                    kpiWeightage.weightage = k.kpiWeightage.weightage;
                    if (k.subKpiWeightages != null)
                    {
                        foreach (var item in k.subKpiWeightages)
                        {
                            var subKpiWeightage = db.SubKpiWeightages.Where(x => x.id == item.id && x.deleted == false).FirstOrDefault();
                            if (subKpiWeightage != null)
                            {
                                subKpiWeightage.weightage = item.weightage;
                            }
                            else
                            {
                                item.deleted = false;
                                db.SubKpiWeightages.Add(item);
                            }
                        }
                    }
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Group kpi Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }*/

        /*[HttpPut]
        [Route("api/KPI/PutEmployeeKpi")]
        public HttpResponseMessage PutEmployeeKpi(List<KpiPutRequest> employeeKpis)
        {
            try
            {
                foreach (var item in employeeKpis)
                {
                    var record = db.KpiWeightages
                        .Where(x => x.group_kpi_id == item.kpiWeightage.group_kpi_id && x.session_id == item.kpiWeightage.session_id && x.kpi_id == item.id)
                        .FirstOrDefault();
                    record.kpi_id = item.id;
                    record.weightage = item.kpiWeightage.weightage;
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Employee kpi Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }*/

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