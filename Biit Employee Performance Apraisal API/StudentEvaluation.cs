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
    using System;
    using System.Collections.Generic;
    
    public partial class StudentEvaluation
    {
        public int student_id { get; set; }
        public int session_id { get; set; }
        public int teacher_id { get; set; }
        public int question_id { get; set; }
        public int score { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Questionaire Questionaire { get; set; }
        public virtual Session Session { get; set; }
        public virtual Student Student { get; set; }
    }
}
