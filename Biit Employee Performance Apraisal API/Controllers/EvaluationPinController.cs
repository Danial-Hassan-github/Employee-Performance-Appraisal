using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EvaluationPinController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpPost]
        public HttpResponseMessage PostConfidentialEvaluationPin([FromBody] EvaluationPin evaluationPin)
        {
            try
            {
                var pin = db.EvaluationPins.Add(evaluationPin);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, pin);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }
    }
}
