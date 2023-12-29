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
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        SqlConnection connection = null;
        [HttpGet]
        public IEnumerable<KPI> GetKPIs()
        {   
            return db.KPIs;
        }

        [HttpPost]
        public HttpResponseMessage PostKPI([FromBody] KPI kPI)
        {
            db.KPIs.Add(kPI);
            int i=db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK,i);
        }

        [HttpPut]
        public void PutKPI(int id, [FromBody] KPI kPI)
        {
        }

        [HttpDelete]
        public void DeleteKPI(int id)
        {

        }
    }
}