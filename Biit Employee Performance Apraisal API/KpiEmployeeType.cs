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
    
    public partial class KpiEmployeeType
    {
        public int id { get; set; }
        public int kpi_id { get; set; }
        public int employee_type_id { get; set; }
    
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual Kpi Kpi { get; set; }
    }
}
