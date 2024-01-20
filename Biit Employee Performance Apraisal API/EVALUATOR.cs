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
    
    public partial class Evaluator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Evaluator()
        {
            this.PeerEvaluations = new HashSet<PeerEvaluation>();
        }
    
        public int id { get; set; }
        public int session_id { get; set; }
        public int evaluatee_id { get; set; }
        public Nullable<bool> deleted { get; set; }

        [JsonIgnore]
        public virtual Employee Employee { get; set; }
        [JsonIgnore]
        public virtual Employee Employee1 { get; set; }
        [JsonIgnore]
        public virtual Session Session { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<PeerEvaluation> PeerEvaluations { get; set; }
    }
}
