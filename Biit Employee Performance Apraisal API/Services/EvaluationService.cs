﻿using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class EvaluationService
    {
        int maxWeightage = 0;
        public EvaluationService()
        {
            maxWeightage = (int)db.OptionsWeightages
                .OrderByDescending(x => x.id)
                .Select(y => y.weightage)
                .First();
        }
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

        public int GetSumOfPeerEvaluations(int employeeID, int sessionID)
        {
            return db.PeerEvaluations
                .Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }

        public int GetSumOfStudentEvaluations(int employeeID, int sessionID)
        {
            return db.StudentEvaluations
                .Where(p => p.teacher_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }

        public int GetSumOfDegreeExitEvaluations(int employeeID, int sessionID)
        {
            return db.DegreeExitEvaluations
                .Where(p => p.supervisor_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }

        public int GetSumOfSeniorTeacherEvaluations(int employeeID, int sessionID)
        {
            return db.SeniorTeacherEvaluations
                .Where(p => p.junior_teacher_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }
        
        public int GetSumOfDirectorEvaluations(int employeeID, int sessionID)
        {
            return db.DirectorEvaluations
                .Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }

        public int GetSumOfConfidentialEvaluations(int employeeID, int sessionID)
        {
            return db.ConfidentialEvaluations
                .Where(p => p.teacher_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }

        public int GetSumOfSupervisorEvaluations(int employeeID, int sessionID)
        {
            return db.SupervisorEvaluations
                .Where(p => p.subordinate_id == employeeID && p.session_id == sessionID)
                .Count() * maxWeightage;
        }

        public int GetObtainedPeerEvaluationScore(int employeeID, int sessionID)
        {
            var result = db.PeerEvaluations
                .Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID)
                .Sum(x => x.score);
            return result;
        }

        public int GetObtainedDegreeExitEvaluationScore(int employeeID, int sessionID)
        {
            var result = db.DegreeExitEvaluations
                .Where(d => d.supervisor_id == employeeID && d.session_id == sessionID)
                .Sum(x => x.score);
            return result == null ? 0 : (int)result;
        }

        public int GetObtainedDirectorEvaluationScore(int employeeID, int sessionID)
        {
            var result = db.DirectorEvaluations
                .Where(p => p.evaluatee_id == employeeID && p.session_id == sessionID)
                .Sum(x => x.score);
            return result == null ? 0 : (int) result;
        }

        public int GetObtainedSupervisorEvaluationScore(int employeeID, int sessionID)
        {
            var result = db.SupervisorEvaluations
                .Where(p => p.subordinate_id == employeeID && p.session_id == sessionID)
                .Sum(x => x.score);
            return result;
        }

        public int GetObtainedStudentEvaluationScore(int employeeID, int sessionID)
        {
            var result = db.StudentEvaluations
                .Where(d => d.teacher_id == employeeID && d.session_id == sessionID)
                .Sum(x => x.score);
            return result == null ? 0 : (int)result;
        }

        public int GetObtainedSeniorTeacherEvaluationScore(int employeeID, int sessionID)
        {
            var result = db.SeniorTeacherEvaluations
                .Where(p => p.junior_teacher_id == employeeID && p.session_id == sessionID)
                .Sum(x => x.score);
            return result == null ? 0 : (int)result;
        }
        
        public int GetObtainedConfidentialEvaluationScore(int employeeID, int sessionID)
        {
            var result = db.ConfidentialEvaluations
                .Where(p => p.teacher_id == employeeID && p.session_id == sessionID)
                .Sum(x => x.score);
            return result == null ? 0 : (int)result;
        }
    }
}