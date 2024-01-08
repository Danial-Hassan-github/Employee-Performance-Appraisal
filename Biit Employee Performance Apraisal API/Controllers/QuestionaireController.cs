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
        EvaluationQuestionaireService questionaire = new EvaluationQuestionaireService();
        [HttpGet]
        [Route("api/Questionaire/GetStudentQuestions")]
        public HttpResponseMessage GetStudentQuestions()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.QUESTIONAIREs.Where(question => question.Type.Equals("student") && question.Deleted == false));
            }catch (Exception ex)
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
                return Request.CreateResponse(HttpStatusCode.OK, db.QUESTIONAIREs.Where(question => question.Type.Equals("peer") && question.Deleted == false));
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
                return Request.CreateResponse(HttpStatusCode.OK, db.QUESTIONAIREs.Where(question=>question.Type.Equals("confidential") && question.Deleted==false));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostQuestion([FromBody] QUESTIONAIRE question)
        {
            if (questionaire.AddQuestion(question))
            {
                return Request.CreateResponse(HttpStatusCode.OK, question);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, questionaire.message);
        }

        [HttpPut]
        public HttpResponseMessage PutQuestion([FromBody] QUESTIONAIRE question)
        {
            if (questionaire.UpdateQuestion(question))
            {
                return Request.CreateResponse(HttpStatusCode.OK, question);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, questionaire.message);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteQuestion(int id)
        {
            try
            {
                var qs=db.QUESTIONAIREs.Find(id);
                qs.Deleted=true;
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
