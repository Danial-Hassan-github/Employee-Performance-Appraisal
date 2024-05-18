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
        [Route("api/SubKpi/GetSubKPIsOfKpi")]
        public HttpResponseMessage GetSubKPIsOfKpi(int kpi_id, int sessionID)
        {
            try
            {
                var result = db.SubKpiWeightages.Join(db.SubKpis, x => x.sub_kpi_id, y => y.id, (x, y) => new { x, y }).Where(combined => combined.x.session_id == sessionID && combined.x.kpi_id == kpi_id).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
