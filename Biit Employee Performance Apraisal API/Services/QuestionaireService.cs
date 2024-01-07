using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class EvaluationQuestionaireService
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        public string message=string.Empty;

        public bool AddQuestion(QUESTIONAIRE question)
        {
            if (ValidateQuestionData(question))
            {
                try
                {
                    db.QUESTIONAIREs.Add(question);
                    int i = db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool UpdateQuestion(QUESTIONAIRE question)
        {
            if (ValidateQuestionData(question))
            {
                try
                {
                    var qs = db.QUESTIONAIREs.Find(question.QuestionID);
                    qs.Question = question.Question;
                    qs.Type = question.Type;
                    qs.Deleted = question.Deleted;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        private bool ValidateQuestionData(QUESTIONAIRE question)
        {
            if (question.Question==null)
            {
                message = "Please Enter Question";
                return false;
            }
            return true;
        }
    }
}