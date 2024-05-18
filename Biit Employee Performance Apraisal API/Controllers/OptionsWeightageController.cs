using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class OptionsWeightageController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();

        [HttpGet]
        [Route("api/OptionsWeightage/GetOptionsWeightages")]
        public HttpResponseMessage GetOptionsWeightages()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.OptionsWeightages.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutOptionsWeightage(List<OptionsWeightage> optionsWeightage)
        {
            try
            {
                var existingRecords = db.OptionsWeightages.ToList();

                foreach (var newRecord in optionsWeightage)
                {
                    var existingRecord = existingRecords
                        .FirstOrDefault(r => r.id == newRecord.id);

                    if (existingRecord != null)
                    {
                        existingRecord.weightage = newRecord.weightage;
                        existingRecord.name = newRecord.name;
                    }
                    /*else
                    {
                        db.OptionsWeightages.Add(newRecord);
                    }*/
                }

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, db.OptionsWeightages.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostOptionWeightage(OptionsWeightage optionsWeightage)
        {
            try
            {
                var result = db.OptionsWeightages.Add(optionsWeightage);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }
    }
}
