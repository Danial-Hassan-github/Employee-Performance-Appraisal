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
    
    public partial class EMPLOYEE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYEE()
        {
            this.CLASS_HELD_REPORT = new HashSet<CLASS_HELD_REPORT>();
            this.ENROLLMENTs = new HashSet<ENROLLMENT>();
            this.EVALUATORs = new HashSet<EVALUATOR>();
            this.EVALUATORs1 = new HashSet<EVALUATOR>();
            this.KPI_EMPLOYEE_SCORE = new HashSet<KPI_EMPLOYEE_SCORE>();
            this.PEER_EVALUATION = new HashSet<PEER_EVALUATION>();
            this.STUDENT_EVALUATION = new HashSet<STUDENT_EVALUATION>();
            this.SUBKPI_EMPLOYEE_SCORE = new HashSet<SUBKPI_EMPLOYEE_SCORE>();
            this.TASKs = new HashSet<TASK>();
            this.TASKs1 = new HashSet<TASK>();
        }
    
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<int> EmployeeTypeID { get; set; }
        public string Department { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<CLASS_HELD_REPORT> CLASS_HELD_REPORT { get; set; }
        public virtual EMPLOYEE_TYPE EMPLOYEE_TYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<ENROLLMENT> ENROLLMENTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<EVALUATOR> EVALUATORs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<EVALUATOR> EVALUATORs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<KPI_EMPLOYEE_SCORE> KPI_EMPLOYEE_SCORE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<PEER_EVALUATION> PEER_EVALUATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<STUDENT_EVALUATION> STUDENT_EVALUATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<SUBKPI_EMPLOYEE_SCORE> SUBKPI_EMPLOYEE_SCORE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<TASK> TASKs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<TASK> TASKs1 { get; set; }
    }
}
