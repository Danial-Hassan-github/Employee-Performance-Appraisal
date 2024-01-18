using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biit_Employee_Performance_Apraisal_API.Services
{
    public class TaskService
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        public string message = string.Empty;

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
                    var t = db.Tasks.Find(task);
                    t.task_description = task.task_description;
                    t.assigned_by_id = task.assigned_by_id;
                    t.assigned_to_id = task.assigned_to_id;
                    t.weightage = task.weightage;
                    t.due_date = task.due_date;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool ValidateTaskData(Task task) 
        {
            if (task.task_description == null)
            {
                message = "Please Enter Task Description";
            }
            else if(task.assigned_to_id == 0)
            {
                message = "Please select person";
            }else if (task.weightage==0)
            {
                message = "Please Enter Weightage";
            }
            else if (task.due_date == null)
            {
                message = "Please Select Due Date";
            }

            if (message == string.Empty) 
                return true;

            return false;
        }
    }
}