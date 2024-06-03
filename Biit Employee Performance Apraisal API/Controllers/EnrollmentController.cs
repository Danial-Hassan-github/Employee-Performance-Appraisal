using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
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
            String pathStr = "";
            var request = HttpContext.Current.Request;
            var enrollmentFile = request.Files["enrollment"];
            var path = HttpContext.Current.Server.MapPath("~/Enrollment_excel_sheet/" + enrollmentFile.FileName.Trim());
            pathStr = path;
            enrollmentFile.SaveAs(path);
            OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path
                    + ";Extended Properties='Excel 12.0 Xml;HDR=NO'");
            try
            {
                oleDbConnection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", oleDbConnection);
                OleDbDataReader reader = command.ExecuteReader();
                List<Enrollment> enrollments = new List<Enrollment>();
                while (reader.Read())
                {
                    Enrollment enrollment = new Enrollment();
                    enrollment.session_id = Convert.ToInt32(reader[0].ToString());
                    enrollment.teacher_id = Convert.ToInt32(reader[1].ToString());
                    enrollment.student_id = Convert.ToInt32(reader[2].ToString());
                    enrollment.course_id = Convert.ToInt32(reader[3].ToString());
                    
                    enrollments.Add(enrollment);
                    // db.Enrollments.Add(enrollment);
                    // db.SaveChanges();
                }
                db.Enrollments.AddRange(enrollments);
                db.SaveChanges();
                oleDbConnection.Close();
                
                return Request.CreateResponse(HttpStatusCode.OK, "Data Submitted");
            }
            catch (Exception e)
            {
                oleDbConnection.Close();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
