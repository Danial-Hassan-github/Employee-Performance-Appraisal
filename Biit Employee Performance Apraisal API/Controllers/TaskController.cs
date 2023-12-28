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
        [HttpGet]
        [Route("api/Task/GetTasks")]
        public HttpResponseMessage GetTasks()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.TASKs);
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
                return Request.CreateResponse(HttpStatusCode.OK, db.TASKs.Where(task=>task.Status==0));
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
                return Request.CreateResponse(HttpStatusCode.OK, db.TASKs.Where(task => task.Status == 1));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Task/GetTeacherTasks")]
        public HttpResponseMessage GetTeacherTasks(int teacherID)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.TASKs.Where(task => task.Status == 0 && task.AssignedToID==teacherID));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostTask([FromBody] TASK task)
        {
            if (taskService.AddTask(task))
            {
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, taskService.message);
        }

        [HttpPut]
        public HttpResponseMessage PutTask(int id, [FromBody] TASK task)
        {
            if (taskService.UpdateTask(task))
            {
                return Request.CreateResponse(HttpStatusCode.OK, task);
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
