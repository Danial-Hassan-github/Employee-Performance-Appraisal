﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Biit_Employee_Performance_AppraisalEntities : DbContext
    {
        public Biit_Employee_Performance_AppraisalEntities()
            : base("name=Biit_Employee_Performance_AppraisalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CLASS_HELD_REPORT> CLASS_HELD_REPORT { get; set; }
        public virtual DbSet<COURSE> COURSEs { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEEs { get; set; }
        public virtual DbSet<ENROLLMENT> ENROLLMENTs { get; set; }
        public virtual DbSet<EVALUATOR> EVALUATORs { get; set; }
        public virtual DbSet<KPI> KPIs { get; set; }
        public virtual DbSet<PEER_EVALUATION> PEER_EVALUATION { get; set; }
        public virtual DbSet<QUESTIONAIRE> QUESTIONAIREs { get; set; }
        public virtual DbSet<SESSION> SESSIONs { get; set; }
        public virtual DbSet<STUDENT> STUDENTs { get; set; }
        public virtual DbSet<STUDENT_EVALUATION> STUDENT_EVALUATION { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TASK> TASKs { get; set; }
        public virtual DbSet<KPI_WEIGHTAGE> KPI_WEIGHTAGE { get; set; }
        public virtual DbSet<SUB_KPI> SUB_KPI { get; set; }
        public virtual DbSet<SUB_KPI_WEIGHTAGE> SUB_KPI_WEIGHTAGE { get; set; }
        public virtual DbSet<EMPLOYEE_TYPE> EMPLOYEE_TYPE { get; set; }
        public virtual DbSet<KPI_EMPLOYEE_SCORE> KPI_EMPLOYEE_SCORE { get; set; }
        public virtual DbSet<SUBKPI_EMPLOYEE_SCORE> SUBKPI_EMPLOYEE_SCORE { get; set; }
    }
}
