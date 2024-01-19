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
    
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<EvaluationPin> EvaluationPins { get; set; }
        public virtual DbSet<Evaluator> Evaluators { get; set; }
        public virtual DbSet<Kpi> Kpis { get; set; }
        public virtual DbSet<KpiWeightage> KpiWeightages { get; set; }
        public virtual DbSet<PeerEvaluation> PeerEvaluations { get; set; }
        public virtual DbSet<Questionaire> Questionaires { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentEvaluation> StudentEvaluations { get; set; }
        public virtual DbSet<SubKpi> SubKpis { get; set; }
        public virtual DbSet<SubKpiWeightage> SubKpiWeightages { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<ClassHeldReport> ClassHeldReports { get; set; }
        public virtual DbSet<KpiEmployeeScore> KpiEmployeeScores { get; set; }
        public virtual DbSet<SubkpiEmployeeScore> SubkpiEmployeeScores { get; set; }
    }
}
