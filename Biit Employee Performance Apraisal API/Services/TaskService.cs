﻿using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class TaskService
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        SubKpiService subKpiService = new SubKpiService();
        public string message = string.Empty;

        public List<object> GetTasksWithEmployees()
        {
            // Assuming db is your DbContext instance
            var tasksWithEmployees = db.Tasks
                .Join(db.Employees,
                    task => task.assigned_to_id,
                    assignedToEmployee => assignedToEmployee.id,
                    (task, assignedToEmployee) => new { Task = task, AssignedToEmployee = assignedToEmployee })
                .Join(db.Employees,
                    taskAssigned => taskAssigned.Task.assigned_by_id,
                    assignedByEmployee => assignedByEmployee.id,
                    (taskAssigned, assignedByEmployee) => new
                    {
                        task = taskAssigned.Task,
                        assigned_to = taskAssigned.AssignedToEmployee,
                        assigned_by = assignedByEmployee
                    })
                .ToList();

            // Return the result
            return tasksWithEmployees.Cast<object>().ToList(); // Change object to appropriate type
        }

        public List<object> GetEmployeeTasks(int employeeID)
        {
            // Assuming db is your DbContext instance
            var tasksWithEmployees = db.Tasks
                .Where(x => x.assigned_to_id == employeeID && x.status == 0)
                .Join(db.Employees,
                    task => task.assigned_to_id,
                    assignedToEmployee => assignedToEmployee.id,
                    (task, assignedToEmployee) => new { Task = task, AssignedToEmployee = assignedToEmployee })
                .Join(db.Employees,
                    taskAssigned => taskAssigned.Task.assigned_by_id,
                    assignedByEmployee => assignedByEmployee.id,
                    (taskAssigned, assignedByEmployee) => new
                    {
                        task = taskAssigned.Task,
                        assigned_to = taskAssigned.AssignedToEmployee,
                        assigned_by = assignedByEmployee
                    })
                .ToList();

            // Return the result
            return tasksWithEmployees.Cast<object>().ToList(); // Change object to appropriate type
        }

        public bool AddTask(Task task)
        {
            if (ValidateTaskData(task))
            {
                try
                {
                    db.Tasks.Add(task);
                    int i = db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool AddRoleBasedTask(TaskWithRole taskWithRole)
        {
            List<Task> tasks = new List<Task>();
            List<int> assignedToIds = new List<int>();
            if (!ValidateTaskData(taskWithRole.Task))
            {
                return false;
            }
            try
            {
                Designation employeeDesignation = taskWithRole.Role.Designation;
                Department employeeDepartment = taskWithRole.Role.Department;
                EmployeeType employeeType = taskWithRole.Role.EmployeeType;
                if (employeeDesignation != null && employeeDepartment != null && employeeType != null)
                    assignedToIds = db.Employees.Where(x => x.designation_id == employeeDesignation.id && x.department_id == employeeDepartment.id && x.employee_type_id == employeeType.id).Select(y => y.id).ToList();
                else if (employeeDesignation != null && employeeDepartment != null)
                    assignedToIds = db.Employees.Where(x => x.designation_id == employeeDesignation.id && x.department_id == employeeDepartment.id).Select(y => y.id).ToList();
                else if (employeeDesignation != null && employeeType != null)
                    assignedToIds = db.Employees.Where(x => x.designation_id == employeeDesignation.id && x.employee_type_id == employeeType.id).Select(y => y.id).ToList();
                else if (employeeDepartment != null && employeeType != null)
                    assignedToIds = db.Employees.Where(x => x.department_id == employeeDepartment.id && x.employee_type_id == employeeType.id).Select(y => y.id).ToList();
                else if (employeeDesignation != null)
                    assignedToIds = db.Employees.Where(x => x.designation_id == employeeDesignation.id).Select(y => y.id).ToList();
                else if (employeeDepartment != null)
                    assignedToIds = db.Employees.Where(x => x.department_id == employeeDepartment.id).Select(y => y.id).ToList();
                else if (employeeType != null)
                    assignedToIds = db.Employees.Where(x => x.employee_type_id == employeeType.id).Select(y => y.id).ToList();
                else
                    return false;

                foreach (int id in assignedToIds)
                {
                    Task task = taskWithRole.Task;
                    Task newTask = new Task()
                    {
                        task_description = task.task_description,
                        assigned_by_id = task.assigned_by_id,
                        assigned_to_id = id,
                        due_date = task.due_date,
                        assigned_date = task.assigned_date,
                        weightage = task.weightage,
                        session_id = task.session_id
                    };
                    tasks.Add(newTask);
                }

                db.Tasks.AddRange(tasks);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }

        public bool DeleteTask(int id)
        {
            try
            {
                Task task = db.Tasks.Find(id);
                if(task != null)
                {
                    if (task.status == 0)
                    {
                        db.Tasks.Remove(task);
                        db.SaveChanges();
                    }
                    else
                    {
                        task.status = 2;
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex) 
            {
                message = ex.Message;
            }
            return false;
        }

        public bool UpdateTask(Task task)
        {
            if (ValidateTaskData(task))
            {
                try
                {
                    var t = db.Tasks.Find(task.id);
                    t.task_description = task.task_description;
                    t.assigned_by_id = task.assigned_by_id;
                    t.assigned_to_id = task.assigned_to_id;
                    t.weightage = task.weightage;
                    t.due_date = task.due_date;
                    if (task.score!=null)
                    {
                        t.score = task.score;
                        t.status = task.status;
                    }
                    db.SaveChanges();
                    setTaskScores(t);
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool setTaskScores(Task task)
        {
            try
            {
                var empTasks = db.Tasks.Where(t => t.session_id == task.session_id && t.assigned_to_id==task.assigned_to_id );
                int score= (int)empTasks.Sum(t => t.score);
                int total_score = (int)empTasks.Sum(t => t.weightage);
                KpiService kpiService = new KpiService();
                int sub_kpi_id = subKpiService.getSubKpiID("task");
                var sub_kpi_Weightage = db.SubKpiWeightages.Where(x => x.sub_kpi_id == sub_kpi_id && x.session_id==task.session_id).Select(y => y.weightage).FirstOrDefault();
                int empScore = Convert.ToInt32(((double)score/total_score) * sub_kpi_Weightage);

                var subKpiEmployeeScore = db.SubkpiEmployeeScores.Where(x => x.employee_id == task.assigned_to_id && x.session_id == task.session_id && x.subkpi_id==sub_kpi_id).FirstOrDefault();
                if (subKpiEmployeeScore!=null)
                {
                    subKpiEmployeeScore.score = empScore;
                }
                else
                {
                    subKpiEmployeeScore.subkpi_id = sub_kpi_id;
                    subKpiEmployeeScore.employee_id = task.assigned_to_id;
                    subKpiEmployeeScore.session_id = (int)task.session_id;
                    subKpiEmployeeScore.score = empScore;
                    db.SubkpiEmployeeScores.Add(subKpiEmployeeScore);
                }
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                message = e.Message;
                return false;
            }
        }

        public bool ValidateTaskData(Task task) 
        {
            if (task.task_description == null)
                message = "Please Enter Task Description";
            else if (task.weightage==0)
                message = "Please Enter Weightage";
            else if (task.due_date == null)
                message = "Please Select Due Date";

            if (message == string.Empty) 
                return true;

            return false;
        }
    }
}