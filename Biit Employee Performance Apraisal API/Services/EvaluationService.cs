using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class EvaluationService
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        public string GetEvaluationType(int questionID)
        {
            var evaluationTypeID = db.Questionaires
                .Where(q => q.id == questionID)
                .Select(type => type.type_id)
                .First();

            return db.QuestionaireTypes
                .Where(x => x.id == evaluationTypeID)
                .Select(y => y.name)
                .FirstOrDefault();
        }

        public int GetSumOfEvaluations(int employeeID, int sessionID)
        {
            int maxWeightage = (int)db.OptionsWeightages
                .OrderByDescending(x => x.id)
                .Select(y => y.weightage)
                .First();
            return db.PeerEvaluations
                .Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }

        public int GetObtainedPeerEvaluationScore(int employeeID, int sessionID)
        {
            return db.PeerEvaluations
                .Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID)
                .Sum(x => x.score);
        }
    }
}