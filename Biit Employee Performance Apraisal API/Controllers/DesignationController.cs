using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class DesignationController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        public HttpResponseMessage GetDesignations()
        {
            try
            {
                var result = db.Designations.Where(x => !x.name.Equals("Director", StringComparison.OrdinalIgnoreCase)).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
