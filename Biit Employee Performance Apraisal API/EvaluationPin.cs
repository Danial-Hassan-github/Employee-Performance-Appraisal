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
    
    public partial class EvaluationPin
    {
        public int id { get; set; }
        public string pin { get; set; }
        public int session_id { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
    
        public virtual Session Session { get; set; }
    }
}
