﻿ using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Biit_Employee_Performance_Apraisal_API.Models;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class ClassHeldReportController : ApiController
    {
        private Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            String pathStr = "";
            try
            {
                var request = HttpContext.Current.Request;
                var chrFile = request.Files["chr"];
                var path = HttpContext.Current.Server.MapPath("~/CHR_Excel_Sheet/"+ chrFile.FileName.Trim());
                pathStr = path;
                chrFile.SaveAs(path);
                OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path
                    + ";Extended Properties='Excel 12.0 Xml;HDR=YES'");
                oleDbConnection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", oleDbConnection);
                OleDbDataReader reader = command.ExecuteReader();
                List<ClassHeldReport> classHeldReports=new List<ClassHeldReport>();
                while (reader.Read())
                {
                    ClassHeldReport chr = new ClassHeldReport();
                    chr.course = reader[0].ToString();
                    chr.teacher = reader[1].ToString();
                    chr.discipline = reader[2].ToString();
                    chr.venue = reader[3].ToString();
                    chr.status = reader[4].ToString().ToLower().Trim();
                    chr.session = reader[5].ToString();
                    if (chr.late_in!=null)
                    {
                        chr.late_in = int.Parse(reader[6].ToString());
                    }
                    if (chr.left_early!=null)
                    {
                    chr.left_early = int.Parse(reader[7].ToString());
                    }
                    chr.remarks = reader[8].ToString();
                    classHeldReports.Add(chr);
                    classHeldReports.Add(chr);
                    // db.SaveChanges();
                }
                db.ClassHeldReports.AddRange(classHeldReports);
                db.SaveChanges();
                /*db.ClassHeldReports.AddRange(classHeldReports);
                db.SaveChanges();*/
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