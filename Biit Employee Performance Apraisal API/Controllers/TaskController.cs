﻿using Biit_Employee_Performance_Apraisal_API.Models;
using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class TaskController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db=new Biit_Employee_Performance_AppraisalEntities();
        TaskService taskService=new TaskService();
        KpiService kpiService=new KpiService();
        SubKpiService subKpiService=new SubKpiService();
        [HttpGet]
        [Route("api/Task/GetTasks")]
        public HttpResponseMessage GetTasks()
        {
            try
            {
                var tasksWithEmployees = taskService.GetTasksWithEmployees();
                return Request.CreateResponse(HttpStatusCode.OK, tasksWithEmployees);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Task/GetPendingTasks")]
        public HttpResponseMessage GetPendingTasks()
        {
            try
            {
                var pendingTasksWithEmployees = taskService.GetTasksWithEmployees().Where(item => ((dynamic)item).task.status == 0).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, pendingTasksWithEmployees);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Task/GetCompletedTasks")]
        public HttpResponseMessage GetCompletedTasks()
        {
            try
            {
                var completedTasksWithEmployees = taskService.GetTasksWithEmployees().Where(item => ((dynamic)item).task.status == 1).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, completedTasksWithEmployees);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Task/GetEmployeeTasks")]
        public HttpResponseMessage GetEmployeeTasks(int employeeID)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, taskService.GetEmployeeTasks(employeeID));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Task/PostTask")]
        public HttpResponseMessage PostTask([FromBody] Task task)
        {
            try
            {
                if (taskService.AddTask(task))
                    return Request.CreateResponse(HttpStatusCode.OK, taskService.GetTasksWithEmployees());
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, taskService.message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Task/PostRoleBasedTask")]
        public HttpResponseMessage PostRoleBasedTask([FromBody] TaskWithRole taskWithRole)
        {
            try
            {
                if (taskService.AddRoleBasedTask(taskWithRole))
                    return Request.CreateResponse(HttpStatusCode.OK, taskService.GetTasksWithEmployees());
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, taskService.message);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("api/Task/PutTask")]
        public HttpResponseMessage PutTask([FromBody] Task task)
        {
            if (taskService.UpdateTask(task))
            {
                int subKpi_id = subKpiService.getSubKpiID("task");
                var result = db.Tasks
                    .Join(db.SubkpiEmployeeScores, updatedTask => updatedTask.assigned_to_id, subKpi_score => subKpi_score.employee_id, (updatedTask, subKpi_score) => new { updatedTask, subKpi_score })
                    .Where(combined => combined.subKpi_score.session_id==task.session_id && combined.subKpi_score.subkpi_id==subKpi_id && combined.updatedTask.id==task.id)
                    .FirstOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, taskService.message);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteTask(int id)
        {
            var t = taskService.DeleteTask(id);
            if (t != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, t); ;
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, taskService.message);
        }
    }
}
