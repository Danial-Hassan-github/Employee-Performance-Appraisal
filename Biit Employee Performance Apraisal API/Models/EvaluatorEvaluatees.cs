using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    public class EvaluatorEvaluatees
    {
        public Evaluator evaluator {  get; set; }
        public List<int> evaluatee_id { get; set; }
    }
}