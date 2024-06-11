using Biit_Employee_Performance_Apraisal_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Biit_Employee_Performance_Appraisal_API.Controllers
{
    public class EmployeeCoursePerformanceController : ApiController
    {
        private Biit_Employee_Performance_AppraisalEntities db = new Biit_Employee_Performance_AppraisalEntities();
        [HttpGet]
        [Route("api/EmployeeCoursePerformance/GetEmployeeCoursesPerformance")]
        public HttpResponseMessage GetEmployeeCoursesPerformance(int teacherID, int sessionID)
        {
            try
            {
                var comparisonResult = new List<object>();
                var courses_ids = db.Courses.Join(db.Enrollments, course => course.id, enrollment => enrollment.course_id, (course, enrollment) => new { course, enrollment }).Where(combined => combined.enrollment.teacher_id == teacherID && combined.enrollment.session_id == sessionID).Select(c => c.course.id).Distinct().ToList();

                foreach (var id in courses_ids)
                {
                    var evaluationWithQuestions = GetEvaluationWithQuestions(teacherID, sessionID, id);
                    double average = CalculateEmployeePerformance(teacherID, sessionID, id);

                    var response = new
                    {
                        course = db.Courses.Where(x => x.id == id).FirstOrDefault(),
                        average = average,
                        employeeQuestionScores = evaluationWithQuestions
                    };
                    comparisonResult.Add(response);
                }

                return Request.CreateResponse(HttpStatusCode.OK, comparisonResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/EmployeeCoursePerformance/GetMultiEmployeeCoursePerformance")]
        public HttpResponseMessage GetMultiEmployeeCoursePerformance(MultiEmployeeCoursePerformanceRequest multiEmployeeCoursePerformanceRequest)
        {
            try
            {
                var comparisonResult = new List<object>();

                foreach (var employee_id in multiEmployeeCoursePerformanceRequest.employeeIds)
                {
                    var employeePerformance = new List<object>();

                    foreach (var course_id in multiEmployeeCoursePerformanceRequest.courseIds)
                    {
                        var evaluationWithQuestions = GetEvaluationWithQuestions(employee_id, multiEmployeeCoursePerformanceRequest.sessionId, course_id);
                        double average = CalculateEmployeePerformance(employee_id, multiEmployeeCoursePerformanceRequest.sessionId, course_id);

                        var course = db.Courses.Where(x => x.id == course_id).FirstOrDefault();

                        var coursePerformance = new
                        {
                            course = course,
                            average = average,
                            employeeQuestionScores = evaluationWithQuestions
                        };

                        employeePerformance.Add(coursePerformance);
                    }

                    var employee = db.Employees.Where(x => x.id == employee_id).FirstOrDefault();

                    var response = new
                    {
                        employee = employee,
                        performance = employeePerformance
                    };

                    comparisonResult.Add(response);
                }

                return Request.CreateResponse(HttpStatusCode.OK, comparisonResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        /*[HttpGet]
        public HttpResponseMessage GetEmployeeCoursePerformance(int employeeID, int sessionID, int courseID)
        {
            try
            {
                var evaluationWithQuestions = GetEvaluationWithQuestions(employeeID, sessionID, courseID);
                double average = CalculateEmployeePerformance(employeeID, sessionID, courseID);

                var response = new
                {
                    employee = db.Employees.Where(x => x.id == employeeID).First(),
                    average = average,
                    employeeQuestionScores = evaluationWithQuestions
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/EmployeeCoursePerformance/CompareEmployeeCoursePerformance")]
        public HttpResponseMessage CompareEmployeeCoursePerformance(int employeeID1, int employeeID2, int sessionID, int courseID)
        {
            try
            {
                var evaluationWithQuestions1 = GetEvaluationWithQuestions(employeeID1, sessionID, courseID);
                var evaluationWithQuestions2 = GetEvaluationWithQuestions(employeeID2, sessionID, courseID);

                double average1 = CalculateEmployeePerformance(employeeID1, sessionID, courseID);
                double average2 = CalculateEmployeePerformance(employeeID2, sessionID, courseID);

                var employee1Data = new
                {
                    employee = db.Employees.Where(x => x.id == employeeID1).First(),
                    average = average1,
                    employeeQuestionScores = evaluationWithQuestions1
                };

                var employee2Data = new
                {
                    employee = db.Employees.Where(x => x.id == employeeID2).First(),
                    average = average2,
                    employeeQuestionScores = evaluationWithQuestions2
                };

                var response = new List<object> { employee1Data, employee2Data };

                return Request.CreateResponse(HttpStatusCode.OK, response);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }*/

        private object GetEvaluationWithQuestions(int employeeID, int sessionID, int courseID)
        {
            var maxWeightage = db.OptionsWeightages.OrderByDescending(x => x.weightage).FirstOrDefault()?.weightage ?? 0;
            int evaluationTypeID = db.QuestionaireTypes.Where(x => x.name.Equals("Student")).First().id;

            var evaluationsWithQuestions = db.StudentEvaluations
                .Where(e => e.teacher_id == employeeID && e.session_id == sessionID && e.course_id == courseID)
                .GroupBy(e => e.question_id)
                .Select(g => new
                {
                    QuestionId = g.Key,
                    ObtainedScore = g.Sum(e => e.score),
                    TotalScore = g.Count() * maxWeightage
                })
                .Join(db.Questionaires.Where(q => q.type_id == evaluationTypeID),
                    eval => eval.QuestionId,
                    question => question.id,
                    (eval, question) => new
                    {
                        question = question,
                        average = ((double)eval.ObtainedScore / eval.TotalScore) * 100
                        // totalScore = eval.TotalScore
                    })
                .ToList();

            return evaluationsWithQuestions;
        }

        private double CalculateEmployeePerformance(int employeeID, int sessionID, int courseID)
        {
            var result = db.StudentEvaluations
                           .Where(x => x.teacher_id == employeeID && x.session_id == sessionID && x.course_id == courseID)
                           .ToList();

            int totalRecords = result.Count();
            if (totalRecords == 0)
            {
                return 0;
            }

            int totalScore = (int)(db.OptionsWeightages.OrderByDescending(x => x.id).First().weightage * totalRecords);
            int obtainedScore = result.Sum(x => x.score);

            if (totalScore == 0)
            {
                return 0;
            }

            double average = ((double)obtainedScore / totalScore) * 100;
            return average;
        }
    }
}
