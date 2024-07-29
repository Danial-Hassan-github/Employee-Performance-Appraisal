using Biit_Employee_Performance_Apraisal_API.Models;
using Biit_Employee_Performance_Apraisal_API.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Apraisal_API.Controllers
{
    public class EmployeeKpiPerformanceController : ApiController
    {
        Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        EmployeeScoreService service = new EmployeeScoreService();

        public GroupKpi getGroup(int employeeID)
        {
            var employee = db.Employees.FirstOrDefault(e => e.id == employeeID);

            // Get all GroupKpis and calculate match score based on criteria
            var groupKpiMatches = db.GroupKpis.Select(g => new
            {
                GroupKpi = g,
                MatchScore = (g.designation_id == employee.designation_id ? 1 : 0) +
                             (g.department_id == employee.department_id ? 1 : 0) +
                             (g.employee_type_id == employee.employee_type_id ? 1 : 0) +
                             (g.employee_id == employee.id ? 1 : 0)
            });

            // Find the best matching GroupKpi instance
            var bestGroupKpi = groupKpiMatches.OrderByDescending(g => g.MatchScore).FirstOrDefault()?.GroupKpi;

            return bestGroupKpi;
        }

        [HttpGet]
        public HttpResponseMessage GetKpiEmployeePerformance(int employeeID, int sessionID)
        {
            try
            {
                // var empKpiGroup = getGroup(employeeID);
                // int groupID = empKpiGroup.id;

                var result = from empScore in db.KpiEmployeeScores
                             where empScore.employee_id == employeeID && empScore.session_id == sessionID
                             from kpi in db.Kpis.Where(k => k.id == empScore.kpi_id).DefaultIfEmpty()
                             where kpi != null
                             from weightage in db.KpiWeightages
                                 .Where(w => w.kpi_id == empScore.kpi_id && w.session_id == empScore.session_id)
                                 .DefaultIfEmpty()
                             select new
                             {
                                 empScore.employee_id,
                                 empScore.kpi_id,
                                 kpi_title = kpi.name,
                                 empScore.score,
                                 weightage = weightage != null ? weightage.weightage : (double?)null
                             };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetKpiEmployeePerformanceMultiSession(int employeeID, int startingSessionID, int endingSessionID)
        {
            try
            {
                var result = from empScore in db.KpiEmployeeScores
                             join session in db.Sessions
                                 on empScore.session_id equals session.id
                             where empScore.employee_id == employeeID &&
                                   empScore.session_id >= startingSessionID &&
                                   empScore.session_id <= endingSessionID
                             join kpi in db.Kpis
                                 on empScore.kpi_id equals kpi.id into kpiGroup
                             from kpi in kpiGroup.DefaultIfEmpty()
                             join weightage in db.KpiWeightages
                                 on new { empScore.kpi_id, empScore.session_id } equals new { weightage.kpi_id, weightage.session_id } into weightageGroup
                             from weightage in weightageGroup.DefaultIfEmpty()
                             group new { empScore, session, kpi, weightage } by session into sessionGroup
                             select new
                             {
                                 session = sessionGroup.Key,
                                 scores = sessionGroup.Select(item => new
                                 {
                                     item.empScore.employee_id,
                                     item.empScore.kpi_id,
                                     kpi_title = item.kpi != null ? item.kpi.name : null,
                                     item.empScore.score,
                                     weightage = item.weightage != null ? item.weightage.weightage : (decimal?)null
                                 })
                             };

                return Request.CreateResponse(HttpStatusCode.OK, result.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeKpiPerformance/CompareSingleKpiEmployeePerformance")]
        public HttpResponseMessage CompareSingleKpiEmployeePerformance([FromUri] List<int> employeeIDs, int session_id, int kpi_id)
        {
            try
            {
                // List<int> sessionIds = db.Sessions.Where(x => x.title.Contains(year)).Select(y => y.id).ToList();
                var comparisonResult = new List<object>();

                foreach (var employeeId in employeeIDs)
                {
                    var employee = db.Employees.Where(x => x.id == employeeId).FirstOrDefault();
                    var kpiScores = new List<object>();

                    // foreach (var sessionId in sessionIds)
                    // {
                    var sessionKpiScores = (from empScore in db.KpiEmployeeScores
                                            where empScore.employee_id == employeeId && empScore.session_id == session_id && empScore.kpi_id == kpi_id
                                            join kpi in db.Kpis on empScore.kpi_id equals kpi.id into kpiJoin
                                            from kpi in kpiJoin.DefaultIfEmpty()
                                            join weightage in db.KpiWeightages on new { empScore.kpi_id, empScore.session_id } equals new { weightage.kpi_id, weightage.session_id } into weightageJoin
                                            from weightage in weightageJoin.DefaultIfEmpty()
                                            where kpi != null
                                            select new
                                            {
                                                employee_id = empScore.employee_id,
                                                kpi_id = empScore.kpi_id,
                                                kpi_title = kpi.name,
                                                session_id = empScore.session_id,
                                                session_title = db.Sessions.Where(s => s.id == empScore.session_id).Select(s => s.title).FirstOrDefault(),
                                                score = empScore.score,
                                                weightage = weightage != null ? weightage.weightage : (double?)null
                                            }).ToList();

                    kpiScores.AddRange(sessionKpiScores);
                    // }

                    var employeePerformance = new
                    {
                        employee = employee,
                        kpiScores = kpiScores
                    };

                    comparisonResult.Add(employeePerformance);
                }

                return Request.CreateResponse(HttpStatusCode.OK, comparisonResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeKpiPerformance/CompareMultiSessionSingleKpiPerformance")]
        public HttpResponseMessage CompareMultiSessionSingleKpiPerformance([FromUri] List<int> sessionIds, int employeeId, int kpi_id)
        {
            try
            {
                // List<int> sessionIds = db.Sessions.Where(x => x.title.Contains(year)).Select(y => y.id).ToList();
                var comparisonResult = new List<object>();

                foreach (var sessionId in sessionIds)
                {
                    var employee = db.Employees.Where(x => x.id == employeeId).FirstOrDefault();
                    var kpiScores = new List<object>();

                    // foreach (var sessionId in sessionIds)
                    // {
                    var sessionKpiScores = (from empScore in db.KpiEmployeeScores
                                            where empScore.employee_id == employeeId && empScore.session_id == sessionId && empScore.kpi_id == kpi_id
                                            join kpi in db.Kpis on empScore.kpi_id equals kpi.id into kpiJoin
                                            from kpi in kpiJoin.DefaultIfEmpty()
                                            join weightage in db.KpiWeightages on new { empScore.kpi_id, empScore.session_id } equals new { weightage.kpi_id, weightage.session_id } into weightageJoin
                                            from weightage in weightageJoin.DefaultIfEmpty()
                                            where kpi != null
                                            select new
                                            {
                                                employee_id = empScore.employee_id,
                                                kpi_id = empScore.kpi_id,
                                                kpi_title = kpi.name,
                                                session_id = empScore.session_id,
                                                session_title = db.Sessions.Where(s => s.id == empScore.session_id).Select(s => s.title).FirstOrDefault(),
                                                score = empScore.score,
                                                weightage = weightage != null ? weightage.weightage : (double?)null
                                            }).ToList();

                    kpiScores.AddRange(sessionKpiScores);
                    // }

                    var employeePerformance = new
                    {
                        employee = employee,
                        kpiScores = kpiScores
                    };

                    comparisonResult.Add(employeePerformance);
                }

                return Request.CreateResponse(HttpStatusCode.OK, comparisonResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeKpiPerformance/CompareMultiSessionKpiEmployeePerformance")]
        public HttpResponseMessage CompareMultiSessionKpiEmployeePerformance([FromUri] List<int> sessionIDs, int employeeId, int kpi_id)
        {
            try
            {
                // List<int> sessionIds = db.Sessions.Where(x => x.title.Contains(year)).Select(y => y.id).ToList();
                // var comparisonResult = new object();
                var kpiScores = new List<object>();
                Employee employee = null;
                foreach (var sessionId in sessionIDs)
                {
                    employee = db.Employees.Where(x => x.id == employeeId).FirstOrDefault();

                    // foreach (var sessionId in sessionIDs)
                    // {
                        var sessionKpiScores = (from empScore in db.KpiEmployeeScores
                     where empScore.employee_id == employeeId && empScore.session_id == sessionId && empScore.kpi_id == kpi_id
                     join kpi in db.Kpis on empScore.kpi_id equals kpi.id into kpiJoin
                     from kpi in kpiJoin.DefaultIfEmpty()
                     join weightage in db.KpiWeightages on new { empScore.kpi_id, empScore.session_id } equals new { weightage.kpi_id, weightage.session_id } into weightageJoin
                     from weightage in weightageJoin.DefaultIfEmpty()
                     where kpi != null
                     select new
                     {
                         employee_id = empScore.employee_id,
                         kpi_id = empScore.kpi_id,
                         kpi_title = kpi.name,
                         session_id = empScore.session_id,
                         session_title = db.Sessions.Where(s => s.id == empScore.session_id).Select(s => s.title).FirstOrDefault(),
                         score = empScore.score,
                         weightage = weightage != null ? weightage.weightage : (double?)null
                     }).ToList();

                    kpiScores.AddRange(sessionKpiScores);
                    // }

                    /*var employeePerformance = new
                    {
                        employee = employee,
                        kpiScores = kpiScores
                    };

                    comparisonResult.Add(kpiScores);*/
                }

                /*var employeePerformance = new
                {
                    employee = employee,
                    kpiScores = kpiScores
                };

                comparisonResult = employeePerformance;*/



                return Request.CreateResponse(HttpStatusCode.OK, kpiScores);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeKpiPerformance/CompareMultiSessionAllKpiEmployeePerformance")]
        public HttpResponseMessage CompareMultiSessionAllKpiEmployeePerformance([FromUri] List<int> sessionIDs, int employeeId)
        {
            try
            {
                // List<int> sessionIds = db.Sessions.Where(x => x.title.Contains(year)).Select(y => y.id).ToList();
                var comparisonResult = new List<object>();
                var kpiScores = new List<object>();
                Employee employee = null;

                var result = db.KpiEmployeeScores
                    .Where(emp => sessionIDs.Contains(emp.session_id) && emp.employee_id == employeeId)
                               .GroupBy(emp => emp.session_id)
                               .Select(g => new
                               {
                                   employee = db.Employees.Where(x => x.id == employeeId).FirstOrDefault(),
                                   kpiScores = g.Select(empScore => new
                                   {
                                       empScore.employee_id,
                                       empScore.kpi_id,
                                       kpi_title = empScore.Kpi.name,
                                       session_title = empScore.Session.title,
                                       empScore.score,
                                       weightage = 0
                                   }).ToList()
                               })
                               .ToList();

/*
                foreach (var sessionId in sessionIDs)
                {
                    employee = db.Employees.Where(x => x.id == employeeId).FirstOrDefault();

                    // foreach (var sessionId in sessionIDs)
                    // {
                    var sessionKpiScores = (from empScore in db.KpiEmployeeScores
                                            where empScore.employee_id == employeeId && empScore.session_id == sessionId
                                            join kpi in db.Kpis on empScore.kpi_id equals kpi.id into kpiJoin
                                            from kpi in kpiJoin.DefaultIfEmpty()
                                            join weightage in db.KpiWeightages on new { empScore.kpi_id, empScore.session_id } equals new { weightage.kpi_id, weightage.session_id } into weightageJoin
                                            from weightage in weightageJoin.DefaultIfEmpty()
                                            where kpi != null
                                            select new
                                            {
                                                employee_id = empScore.employee_id,
                                                kpi_id = empScore.kpi_id,
                                                kpi_title = kpi.name,
                                                session_id = empScore.session_id,
                                                session_title = db.Sessions.Where(s => s.id == empScore.session_id).Select(s => s.title).FirstOrDefault(),
                                                score = empScore.score,
                                                weightage = weightage != null ? weightage.weightage : (double?)null
                                            }).ToList();
                    *//*var k = new
                    {
                        kpiScores = sessionKpiScores
                    };*//*
                    kpiScores.Add(sessionKpiScores);
                    // }

                    *//*var employeePerformance = new
                    {
                        employee = employee,
                        kpiScores = kpiScores
                    };*//*

                    comparisonResult.Add(kpiScores);
                }*/

                // comparisonResult.Add(employeePerformance);



                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeKpiPerformance/CompareKpiEmployeePerformanceYearly")]
        public HttpResponseMessage CompareKpiEmployeePerformanceYearly([FromUri] List<int> employeeIDs, string year, int kpi_id)
        {
            try
            {
                List<int> sessionIds = db.Sessions.Where(x => x.title.Contains(year)).Select(y => y.id).ToList();
                var comparisonResult = new List<object>();

                foreach (var employeeId in employeeIDs)
                {
                    var employee = db.Employees.Where(x => x.id == employeeId).FirstOrDefault();
                    var kpiScores = new List<object>();

                    foreach (var sessionId in sessionIds)
                    {
                        var sessionKpiScores = (from empScore in db.KpiEmployeeScores
                                                where empScore.employee_id == employeeId && empScore.session_id == sessionId && empScore.kpi_id == kpi_id
                                                join kpi in db.Kpis on empScore.kpi_id equals kpi.id into kpiJoin
                                                from kpi in kpiJoin.DefaultIfEmpty()
                                                join weightage in db.KpiWeightages on new { empScore.kpi_id, empScore.session_id } equals new { weightage.kpi_id, weightage.session_id } into weightageJoin
                                                from weightage in weightageJoin.DefaultIfEmpty()
                                                where kpi != null
                                                select new
                                                {
                                                    employee_id = empScore.employee_id,
                                                    kpi_id = empScore.kpi_id,
                                                    kpi_title = kpi.name,
                                                    session_id = empScore.session_id,
                                                    session_title = db.Sessions.Where(s => s.id == empScore.session_id).Select(s => s.title).FirstOrDefault(),
                                                    score = empScore.score,
                                                    weightage = weightage != null ? weightage.weightage : (double?)null
                                                }).ToList();

                        kpiScores.AddRange(sessionKpiScores);
                    }

                    var employeePerformance = new
                    {
                        employee = employee,
                        kpiScores = kpiScores
                    };

                    comparisonResult.Add(employeePerformance);
                }

                return Request.CreateResponse(HttpStatusCode.OK, comparisonResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        public HttpResponseMessage AddFreeKpiEmployeeScore(KpiEmployeeScore employeeScore)
        {
            if (service.isEmployeeScoreExists(employeeScore))
            {
                service.UpdateEmployeeKpiScore(employeeScore);
            }
            else
            {
                service.AddEmployeeKpiScore(employeeScore);
            }
            var result = db.KpiEmployeeScores.Find(employeeScore.kpi_id, employeeScore.employee_id, employeeScore.session_id);
            return Request.CreateResponse(HttpStatusCode.OK,result);
        }
        [HttpPost]
        [Route("api/EmployeeKpiPerformance/CompareKpiEmployeePerformance")]
        public HttpResponseMessage CompareKpiEmployeePerformance([FromBody] EmployeeIdsWithSession employeeIdsWithSession)
        {
            try
            {
                var comparisonResult = new List<object>();

                foreach (var employeeId in employeeIdsWithSession.employeeIds)
                {
                    // var employeeKpiGroup = getGroup(employeeId);

                    var employeePerformance = new
                    {
                        employee = db.Employees.Where(x => x.id == employeeId).FirstOrDefault(),
                        kpiScores = (from empScore in db.KpiEmployeeScores
                                          where empScore.employee_id == employeeId && empScore.session_id == employeeIdsWithSession.session_id
                                          from kpi in db.Kpis.Where(k => k.id == empScore.kpi_id).DefaultIfEmpty()
                                          where kpi != null
                                          from weightage in db.KpiWeightages
                                              .Where(w => w.kpi_id == empScore.kpi_id && w.session_id == empScore.session_id)
                                              .DefaultIfEmpty()
                                          select new
                                          {
                                              employee_id = empScore.employee_id,
                                              kpi_id = empScore.kpi_id,
                                              kpi_title = kpi.name,
                                              score = empScore.score,
                                              weightage = weightage != null ? weightage.weightage : (double?)null
                                          }).ToList()
                    };

                    comparisonResult.Add(employeePerformance);
                }

                return Request.CreateResponse(HttpStatusCode.OK, comparisonResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /*[HttpPost]
        public HttpResponseMessage FreeKpiEmployeeScore(KpiEmployeeScore kpiEmployeeScore)
        {
            try
            {
                db.KpiEmployeeScores.Add(kpiEmployeeScore);
                return Request.CreateResponse(HttpStatusCode.OK, kpiEmployeeScore);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }*/
    }
}
