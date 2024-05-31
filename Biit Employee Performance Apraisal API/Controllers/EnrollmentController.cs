using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EnrollmentController : ApiController
    {
        private Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            try
            {
                Enrollment enrollment = new Enrollment();
                var request = HttpContext.Current.Request;
                var enrollmentFile = request.Files["enrollment"];
                var path = HttpContext.Current.Server.MapPath("~/Enrollment_excel_sheet/" + enrollmentFile.FileName.Trim());
                enrollmentFile.SaveAs(path);
                OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path
                    + ";Extended Properties='Excel 12.0 Xml;HDR=NO'");
                oleDbConnection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", oleDbConnection);
                OleDbDataReader reader = command.ExecuteReader();
                List<ClassHeldReport> classHeldReports = new List<ClassHeldReport>();
                while (reader.Read())
                {
                    enrollment.session_id = Convert.ToInt32(reader[0].ToString());
                    enrollment.teacher_id = Convert.ToInt32(reader[1].ToString());
                    enrollment.student_id = Convert.ToInt32(reader[2].ToString());
                    enrollment.course_id = Convert.ToInt32(reader[3].ToString());
                    
                    db.Enrollments.Add(enrollment);
                    db.SaveChanges();
                }
                oleDbConnection.Close();
                return Request.CreateResponse(HttpStatusCode.OK, "Data Submitted");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
