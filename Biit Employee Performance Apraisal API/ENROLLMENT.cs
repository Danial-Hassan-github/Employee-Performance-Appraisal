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
    
    public partial class ENROLLMENT
    {
        public int SessionID { get; set; }
        public int TeacherID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
    
        public virtual COURSE COURSE { get; set; }
        public virtual EMPLOYEE EMPLOYEE { get; set; }
        public virtual SESSION SESSION { get; set; }
        public virtual STUDENT STUDENT { get; set; }
    }
}