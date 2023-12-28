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

        public bool AddTask(TASK task)
        {
            if (ValidateTaskData(task))
            {
                try
                {
                    db.TASKs.Add(task);
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
                TASK task = db.TASKs.Find(id);
                if(task != null)
                {
                    if (task.Status == 0)
                    {
                        db.TASKs.Remove(task);
                        db.SaveChanges();
                    }
                    else
                    {
                        task.Status = 2;
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

        public bool UpdateTask(TASK task)
        {
            if (ValidateTaskData(task))
            {
                try
                {
                    var t = db.TASKs.Find(task);
                    t.TaskDescription = task.TaskDescription;
                    t.AssignedByID = task.AssignedByID;
                    t.AssignedToID = task.AssignedToID;
                    t.Weightage = task.Weightage;
                    t.DueDate = task.DueDate;
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

        public bool ValidateTaskData(TASK task) 
        {
            if (task.TaskDescription == null)
            {
                message = "Please Enter Task Description";
            }
            else if(task.AssignedToID == 0)
            {
                message = "Please select person";
            }else if (task.Weightage==0)
            {
                message = "Please Enter Weightage";
            }
            else if (task.DueDate == null)
            {
                message = "Please Select Due Date";
            }

            if (message == string.Empty) 
                return true;

            return false;
        }
    }
}