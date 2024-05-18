using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class EvaluatorEvaluatees
    {
        public int evaluator_id {  get; set; }
        public int session_id { get; set; }
        public List<int> evaluatee_ids { get; set; }
    }
}