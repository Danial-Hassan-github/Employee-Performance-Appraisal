//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Biit_Employee_Performance_Apraisal_API
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class StudentEvaluation
    {
        public int student_id { get; set; }
        public int session_id { get; set; }
        public int teacher_id { get; set; }
        public int question_id { get; set; }
        public int score { get; set; }
    
        [JsonIgnore]
        public virtual Employee Employee { get; set; }
        [JsonIgnore]
        public virtual Questionaire Questionaire { get; set; }
        [JsonIgnore]
        public virtual Session Session { get; set; }
        [JsonIgnore]
        public virtual Student Student { get; set; }
    }
}