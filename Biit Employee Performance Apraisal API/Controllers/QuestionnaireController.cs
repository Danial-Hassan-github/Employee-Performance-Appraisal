﻿using Biit_Employee_Performance_Apraisal_API.Models;
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
    public class QuestionnaireController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        EvaluationQuestionaireService questionaireService = new EvaluationQuestionaireService();

        [HttpGet]
        [Route("api/Questionnaire/GetQuestionnaireTypes")]
        public HttpResponseMessage GetQuestionnaireTypes()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.QuestionaireTypes);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Questionnaire/GetOptionsWeightages")]
        public HttpResponseMessage GetOptionsWeightages()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.OptionsWeightages.ToList());
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Questionnaire/GetQuestionnaireByTypeID")]
        public HttpResponseMessage GetQuestionnaireByTypeID(int questionnaireTypeId) 
        {

            try
            {
                var result = db.Questionaires.Where(question => question.type_id == questionnaireTypeId && question.deleted == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Questionnaire/GetQuestionnaireByType")]
        public HttpResponseMessage GetQuestionnaireByType(string questionnaireType)
        {

            try
            {
                int type_id = questionaireService.getQuestionTypeID(questionnaireType);
                var result = db.Questionaires.Where(question => question.type_id == type_id && question.deleted == false).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Questionnaire/GetStudentQuestions")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/Questionnaire/GetConfidentialQuestions")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/Questionnaire/GetPeerQuestions")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        [Route("api/Questionnaire/GetSupervisorQuestions")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/Questionnaire/GetSeniorTeacherQuestions")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage PostQuestion([FromBody] Questionaire question)
        {
            var q = questionaireService.AddQuestion(question);
            if (q != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, question);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, questionaireService.message);
        }

        [HttpPut]
        public HttpResponseMessage PutQuestion([FromBody] Questionaire question)
        {
            var q = questionaireService.UpdateQuestion(question);
            if (q != null)
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
                return Request.CreateResponse(HttpStatusCode.OK, qs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
