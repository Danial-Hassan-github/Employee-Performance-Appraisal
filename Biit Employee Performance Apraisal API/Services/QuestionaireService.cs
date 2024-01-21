﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class EvaluationQuestionaireService
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        public string message=string.Empty;

        public bool AddQuestion(Questionaire question)
        {
            if (ValidateQuestionData(question))
            {
                try
                {
                    db.Questionaires.Add(question);
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

        public bool UpdateQuestion(Questionaire question)
        {
            if (ValidateQuestionData(question))
            {
                try
                {
                    var qs = db.Questionaires.Find(question.id);
                    qs.question = question.question;
                    qs.type_id = question.type_id;
                    qs.deleted = question.deleted;
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

        public int getQuestionTypeID(string type)
        {
            int id=db.QuestionaireTypes.Where(q => q.name == type).Select(x => x.id).FirstOrDefault();
            return id;
        }

        private bool ValidateQuestionData(Questionaire question)
        {
            if (question.question==null)
            {
                message = "Please Enter Question";
                return false;
            }
            return true;
        }
    }
}