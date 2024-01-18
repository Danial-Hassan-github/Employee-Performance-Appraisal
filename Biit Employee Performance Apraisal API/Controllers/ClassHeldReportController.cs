 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Biit_Employee_Performance_Apraisal_API;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class CLASS_HELD_REPORTController : ApiController
    {
        private Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            try
            {
                ClassHeldReport chr=new ClassHeldReport();
                var request = HttpContext.Current.Request;
                var chrFile = request.Files["chr"];
                var path = HttpContext.Current.Server.MapPath("~/CHR_Excel_Sheet/"+ chrFile.FileName.Trim());
                chrFile.SaveAs(path);
                OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path
                    + ";Extended Properties='Excel 12.0 Xml;HDR=NO'");
                oleDbConnection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", oleDbConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    chr.course = reader[0].ToString();
                    chr.teacher = reader[1].ToString();
                    chr.discipline = reader[2].ToString();
                    chr.venue = reader[3].ToString();
                     chr.status = reader[4].ToString().ToLower().Trim();
                    if (chr.late_in!=null)
                    {
                        chr.late_in = int.Parse(reader[5].ToString());
                    }
                    if (chr.left_early!=null)
                    {
                    chr.left_early = int.Parse(reader[6].ToString());
                    }
                    chr.remarks = reader[7].ToString();
                    db.ClassHeldReports.Add(chr);
                }
                db.SaveChanges();
                oleDbConnection.Close();
                return Request.CreateResponse(HttpStatusCode.OK,"Data Submitted");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}