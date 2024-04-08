using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EvaluationScoresController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        public HttpResponseMessage GetQuestionsScoresByEvaluationId(int employeeID, int sessionID, int evluationType)
        {
            try
            {
                var questionnaireType = db.QuestionaireTypes.Where(x => x.id == evluationType).FirstOrDefault();
                var result = db.Questionaires.Join(db.StudentEvaluations, x => x.id, y => y.question_id, (x,y) => new { x,y }).Where(combined => combined.x.type_id == evluationType);
                if (questionnaireType != null)
                {
                    if (questionnaireType.name.ToLower().Equals("Student") || questionnaireType.name.ToLower().Equals("Confidential"))
                    {
                        
                    }else if (questionnaireType.name.ToLower().Equals("Senior") || questionnaireType.name.ToLower().Equals("Peer") || questionnaireType.name.ToLower().Equals("Supervisor"))
                    {
                        
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
