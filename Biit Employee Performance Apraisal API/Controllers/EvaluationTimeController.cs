using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EvaluationTimeController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        [HttpGet]
        public HttpResponseMessage IsEvaluationTime(int sessionID, string evaluationType)
        {
            try
            {
                var result = db.EvaluationTimes
                    .Where(x => x.session_id == sessionID && x.evaluation_type.ToLower().Trim().Equals(evaluationType.Trim().ToLower()))
                    .FirstOrDefault(); // Use FirstOrDefault to handle if no record is found

                if (result != null)
                {
                    DateTime currentTime = DateTime.Now;
                    DateTime startTime = (DateTime)result.start_time;
                    DateTime endTime = (DateTime)result.end_time;

                    if (currentTime >= startTime && currentTime <= endTime)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, false);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No evaluation time found for the session ID.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostEvaluationTime([FromBody] EvaluationTime evaluationTime)
        {
            try
            {
                var result = db.EvaluationTimes.Add(evaluationTime);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutEvaluationTime([FromBody] EvaluationTime evaluationTime)
        {
            try
            {
                var result = db.EvaluationTimes.Find(evaluationTime.id);
                result.start_time = evaluationTime.start_time;
                result.end_time = evaluationTime.end_time;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
