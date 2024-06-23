using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class SubKpiController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        [HttpGet]
        [Route("api/SubKpi/GetSubKPIs")]
        public HttpResponseMessage GetSubKPIs(int sessionID)
        {
            try
            {
                var result = db.SubKpiWeightages
                    .Join(db.SubKpis,
                        subKpiWeightage => subKpiWeightage.sub_kpi_id,
                        subKpi => subKpi.id,
                        (subKpiWeightage, subKpi) => new { subKpiWeightage, subKpi })
                    .Where(combined => combined.subKpiWeightage.session_id == sessionID)
                    .Select(combined => new
                    {
                        combined.subKpi.id,
                        combined.subKpi.name,
                        combined.subKpiWeightage
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
        [Route("api/SubKpi/GetAvailableSubKpis")]
        public HttpResponseMessage GetAvailableSubKpis(int kpiID, int sessionID)
        {
            try
            {
                // Step 1: Get the department ID associated with the given KPI ID
                var departmentID = db.Kpis
                    .Where(k => k.id == kpiID)
                    .Select(k => k.department_id)
                    .FirstOrDefault();

                if (departmentID == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Department not found for the given KPI ID.");
                }

                // Step 2: Get all KPI IDs associated with that department ID
                var kpiIDs = db.Kpis
                    .Where(k => k.department_id == departmentID)
                    .Select(k => k.id)
                    .ToList();

                // Step 3: Get all subKpi IDs that are present in the SubKpiWeightages table for those KPI IDs, session ID, and deleted == false
                var subKpiWeightageIds = db.SubKpiWeightages
                    .Where(sw => sw.session_id == sessionID && kpiIDs.Contains(sw.kpi_id) && sw.deleted == false)
                    .Select(sw => sw.sub_kpi_id)
                    .ToList();

                // Step 4: Return a list of available subKpis that are not present in the SubKpiWeightages table with deleted == false
                var result = db.SubKpis
                    .Where(subKpi => !subKpiWeightageIds.Contains(subKpi.id))
                    .Select(subKpi => new
                    {
                        subKpi.id,
                        subKpi.name
                    })
                    .Distinct()
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/SubKpi/GetSubKPIsOfKpi")]
        public HttpResponseMessage GetSubKPIsOfKpi(int kpi_id, int sessionID)
        {
            try
            {
                var result = db.SubKpiWeightages
                    .Join(db.SubKpis, x => x.sub_kpi_id, y => y.id, (x, y) => new { x, y })
                    .Where(combined => combined.x.session_id == sessionID && combined.x.kpi_id == kpi_id && combined.x.deleted == false)
                    .Select(combined => new
                    {
                        combined.y.id,
                        combined.y.name,
                        subKpiWeightage = combined.x
                    })
                    .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
