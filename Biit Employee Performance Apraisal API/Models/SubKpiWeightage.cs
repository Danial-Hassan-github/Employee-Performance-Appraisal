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
    using System; using Newtonsoft.Json;
    using System.Collections.Generic;
    
    public partial class SubKpiWeightage
    {
        public Nullable<int> kpi_id { get; set; }
        public int sub_kpi_id { get; set; }
        public int session_id { get; set; }
        public int weightage { get; set; }
    
        [JsonIgnore] public virtual Kpi Kpi { get; set; }
        [JsonIgnore] public virtual Session Session { get; set; }
        [JsonIgnore] public virtual SubKpi SubKpi { get; set; }
    }
}
