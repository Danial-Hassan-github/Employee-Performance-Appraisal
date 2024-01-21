﻿using Biit_Employee_Performance_Apraisal_API.Services;
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
        [HttpGet]
        [Route("api/Task/GetTasks")]
        public HttpResponseMessage GetTasks()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Tasks);
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
                return Request.CreateResponse(HttpStatusCode.OK, db.Tasks.Where(task=>task.status==0));
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
                return Request.CreateResponse(HttpStatusCode.OK, db.Tasks.Where(task => task.status == 1));
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
                return Request.CreateResponse(HttpStatusCode.OK, db.Tasks.Where(task => task.status == 0 && task.assigned_to_id==employeeID));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostTask([FromBody] Task task)
        {
            if (taskService.AddTask(task))
            {
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, taskService.message);
        }

        [HttpPut]
        [Route("api/Task/PutTask")]
        public HttpResponseMessage PutTask([FromBody] Task task)
        {
            if (taskService.UpdateTask(task))
            {
                int subKpi_id = kpiService.getSubKpiID("task");
                var result = db.Tasks.Join(db.SubkpiEmployeeScores, updatedTask => updatedTask.assigned_to_id, subKpi_score => subKpi_score.employee_id, (updatedTask, subKpi_score) => new { updatedTask, subKpi_score }).Where(combined => combined.subKpi_score.session_id==task.session_id && combined.subKpi_score.subkpi_id==subKpi_id && combined.updatedTask.id==task.id).FirstOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, taskService.message);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteTask(int id)
        {
            if (taskService.DeleteTask(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Succesfully"); ;
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, taskService.message);
        }
    }
}
