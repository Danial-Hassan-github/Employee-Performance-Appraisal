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
    public class QuestionaireController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        EvaluationQuestionaireService questionaireService = new EvaluationQuestionaireService();
        [HttpGet]
        [Route("api/Questionaire/GetStudentQuestions")]
        public HttpResponseMessage GetStudentQuestions()
        {
            try
            {
                int type_id = questionaireService.getQuestionTypeID("student");
                var result = db.Questionaires.Where(question => question.type_id == type_id && question.deleted == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/Questionaire/GetConfidentialQuestions")]
        public HttpResponseMessage GetConfidentialQuestions()
        {
            try
            {
                int type_id = questionaireService.getQuestionTypeID("confidential");
                var result = db.Questionaires.Where(question => question.type_id == type_id && question.deleted == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/Questionaire/GetPeerQuestions")]
        public HttpResponseMessage GetPeerQuestions()
        {
            try
            {
                int type_id = questionaireService.getQuestionTypeID("peer");
                var result = db.Questionaires.Where(question => question.type_id == type_id && question.deleted == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }



        [HttpGet]
        [Route("api/Questionaire/GetSupervisorQuestions")]
        public HttpResponseMessage GetSupervisorQuestions()
        {
            try
            {
                int type_id = questionaireService.getQuestionTypeID("supervisor");
                var result = db.Questionaires.Where(question => question.type_id == type_id && question.deleted == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/Questionaire/GetSeniorTeacherQuestions")]
        public HttpResponseMessage GetSeniorTeacherQuestions()
        {
            try
            {
                int type_id = questionaireService.getQuestionTypeID("senior");
                var result = db.Questionaires.Where(question => question.type_id == type_id && question.deleted == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage PostQuestion([FromBody] Questionaire question)
        {
            if (questionaireService.AddQuestion(question))
            {
                return Request.CreateResponse(HttpStatusCode.OK, question);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, questionaireService.message);
        }

        [HttpPut]
        public HttpResponseMessage PutQuestion([FromBody] Questionaire question)
        {
            if (questionaireService.UpdateQuestion(question))
            {
                return Request.CreateResponse(HttpStatusCode.OK, question);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, questionaireService.message);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteQuestion(int id)
        {
            try
            {
                var qs=db.Questionaires.Find(id);
                qs.deleted=true;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
