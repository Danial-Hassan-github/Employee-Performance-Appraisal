//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Biit_Employee_Performance_Apraisal_API.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class SubkpiEmployeeScore
    {
        public int subkpi_id { get; set; }
        public int employee_id { get; set; }
        public int session_id { get; set; }
        public Nullable<int> score { get; set; }
    
        [JsonIgnore] public virtual Employee Employee { get; set; }
        [JsonIgnore] public virtual Session Session { get; set; }
        [JsonIgnore] public virtual SubKpi SubKpi { get; set; }
    }
}
