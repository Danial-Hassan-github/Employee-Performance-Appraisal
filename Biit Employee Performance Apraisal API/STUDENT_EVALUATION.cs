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
    
    public partial class STUDENT_EVALUATION
    {
        public int StudentID { get; set; }
        public int SessionID { get; set; }
        public int TeacherID { get; set; }
        public int QuestionID { get; set; }
        public int Score { get; set; }

        [JsonIgnore]
        public virtual EMPLOYEE EMPLOYEE { get; set; }
        public virtual QUESTIONAIRE QUESTIONAIRE { get; set; }
        [JsonIgnore]
        public virtual SESSION SESSION { get; set; }
        [JsonIgnore]
        public virtual STUDENT STUDENT { get; set; }
    }
}
