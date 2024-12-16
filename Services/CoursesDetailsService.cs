using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class CoursesDetailsService : ICoursesDetailsService
    {
        private readonly IDbConnection _db;

        public CoursesDetailsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<CoursesDetailsModel>> GetAllCourseDetails()
        {
            var sql = @"SELECT * FROM CoursesDetails";
            return await _db.QueryAsync<CoursesDetailsModel>(sql);
        }
        
        public async Task<IEnumerable<CoursesDetails2Model>> GetAllAvailableCourseDetails(int students_ID, int coursesdetails_semesters_ID, int coursesdetails_departments_ID)
        {
            var sql = @"SELECT 
                            cd.coursesdetails_ID,
                            c.courses_Name,
                            c.courses_Credit,
                            p.professors_Name,
                            r.room_Name,
                            d.days_Name,
                            p1.periods_Name,
                            CASE 
                                WHEN sc.coursesdetails_ID IS NOT NULL THEN 1 
                                ELSE 0 
                            END AS Is_Added
                        FROM 
                            CoursesDetails cd
                        LEFT JOIN 
                            Courses c ON cd.coursesdetails_courses_ID = c.courses_ID
                        LEFT JOIN 
                            Professors p ON cd.coursesdetails_professors_ID = p.professors_ID
                        LEFT JOIN 
                            Rooms r ON cd.CoursesDetails_rooms_ID = r.rooms_ID
                        LEFT JOIN 
                            Periods p1 ON cd.coursesdetails_periods_ID = p1.periods_ID
                        LEFT JOIN 
                            Days d ON cd.CoursesDetails_days_ID = d.days_ID
                        LEFT JOIN 
                            StudentCourses sc ON cd.coursesdetails_ID = sc.coursesdetails_ID 
                                               AND sc.students_ID = @students_ID -- Match only for the current student
                        WHERE 
                            cd.coursesdetails_semesters_ID = @coursesdetails_semesters_ID
                            AND cd.coursesdetails_departments_ID = @coursesdetails_departments_ID
                            AND cd.coursesdetails_IsEnd = 0;";
            return await _db.QueryAsync<CoursesDetails2Model>(sql, new { students_ID , coursesdetails_semesters_ID , coursesdetails_departments_ID });
        }
        
        public async Task<IEnumerable<CoursesDetails2Model>> GetAllCourseDetailsByProfessorsID(int coursesdetails_professors_ID)
        {
            var sql = @"SELECT 
                            cd.coursesdetails_ID,
                            c.courses_Name,
                            c.courses_Credit,
                            p.professors_Name,
                            r.room_Name,
                            d.days_Name,
                            p1.periods_Name
                        FROM 
                            CoursesDetails cd
                        LEFT JOIN 
                            Courses c ON cd.coursesdetails_courses_ID = c.courses_ID
                        LEFT JOIN 
                            Professors p ON cd.coursesdetails_professors_ID = p.professors_ID
                        LEFT JOIN 
                            Rooms r ON cd.CoursesDetails_rooms_ID = r.rooms_ID
                        LEFT JOIN 
                            Periods p1 ON cd.coursesdetails_periods_ID = p1.periods_ID
                        LEFT JOIN 
                            Days d ON cd.CoursesDetails_days_ID = d.days_ID
                        WHERE 
                            cd.coursesdetails_professors_ID = @coursesdetails_professors_ID
                            AND cd.coursesdetails_IsEnd = 0;";
            return await _db.QueryAsync<CoursesDetails2Model>(sql, new { coursesdetails_professors_ID });
        }

        public async Task<CoursesDetailsModel?> GetCourseDetailById(int id)
        {
            var sql = @"SELECT cd.coursesdetails_ID
                              ,cd.coursesdetails_courses_ID
                              ,cd.coursesdetails_semesters_ID
                              ,cd.coursesdetails_departments_ID
                              ,cd.CoursesDetails_years_ID
                              ,cd.CoursesDetails_days_ID
                              ,cd.CoursesDetails_rooms_ID
                              ,cd.coursesdetails_periods_ID
                              ,cd.coursesdetails_IsEnd
                              ,cd.coursesdetails_professors_ID
                              ,c.courses_Name
                        FROM CoursesDetails cd
                        LEFT JOIN Courses c ON cd.coursesdetails_courses_ID = c.courses_ID
                        WHERE cd.coursesdetails_ID = @id";
            return await _db.QuerySingleOrDefaultAsync<CoursesDetailsModel>(sql, new { id });
        }
        public async Task<bool> InsertCourseDetail(CoursesDetailsModel courseDetail)
        {
            var sql = @"
            INSERT INTO CoursesDetails 
            (coursesdetails_ID,coursesdetails_courses_ID, coursesdetails_semesters_ID, coursesdetails_departments_ID, 
             CoursesDetails_years_ID, CoursesDetails_days_ID, CoursesDetails_rooms_ID, 
             coursesdetails_periods_ID, coursesdetails_IsEnd, coursesdetails_professors_ID) 
            VALUES 
            (@coursesdetails_ID,@coursesdetails_courses_ID, @coursesdetails_semesters_ID, @coursesdetails_departments_ID, 
             @CoursesDetails_years_ID, @CoursesDetails_days_ID, @CoursesDetails_rooms_ID, 
             @coursesdetails_periods_ID, @coursesdetails_IsEnd, @coursesdetails_professors_ID);";

            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, courseDetail);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateCourseDetail(CoursesDetailsModel courseDetail)
        {
            var sql = @"
            UPDATE CoursesDetails SET 
            coursesdetails_courses_ID = @coursesdetails_courses_ID, 
            coursesdetails_semesters_ID = @coursesdetails_semesters_ID, 
            coursesdetails_departments_ID = @coursesdetails_departments_ID, 
            CoursesDetails_years_ID = @CoursesDetails_years_ID, 
            CoursesDetails_days_ID = @CoursesDetails_days_ID, 
            CoursesDetails_rooms_ID = @CoursesDetails_rooms_ID, 
            coursesdetails_periods_ID = @coursesdetails_periods_ID, 
            coursesdetails_IsEnd = @coursesdetails_IsEnd, 
            coursesdetails_professors_ID = @coursesdetails_professors_ID
            WHERE coursesdetails_ID = @coursesdetails_ID";

            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, courseDetail);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteCourseDetail(int coursesdetails_ID)
        {
            var sql = @"DELETE FROM CoursesDetails WHERE coursesdetails_ID = @coursesdetails_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { coursesdetails_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetMaxCoursesDetailsID()
        {
            var sql = @"SELECT MAX(coursesdetails_ID) FROM CoursesDetails;";
            var maxTeacherID = await _db.ExecuteScalarAsync<int?>(sql);
            return maxTeacherID ?? 222222;
        }

        public async Task<IEnumerable<StudentsModel>> GetAllStudentsByCourseID(int coursesdetails_ID)
        {
            var sql = @"SELECT s.students_ID
                              ,s.students_Name
                              ,s.students_PhoneNumber
                              ,d.departments_Name
                              ,s.students_SemesterIN
                        FROM StudentCourses sc
                        LEFT JOIN CoursesDetails cd ON sc.coursesdetails_ID = cd.coursesdetails_ID
                        LEFT JOIN Students s ON sc.students_ID = s.students_ID
                        LEFT JOIN Departments d ON cd.coursesdetails_departments_ID = d.departments_ID
                        WHERE cd.coursesdetails_ID = @coursesdetails_ID";
            return await _db.QueryAsync<StudentsModel>(sql, new { coursesdetails_ID });
        }
        public async Task<IEnumerable<MarkStudentsModel>> GetAllMarksStudentsByCourseID(int coursesexamsdetails_ID)
        {
            var sql = @"SELECT 
                            s.students_ID,
                            s.students_Name,
                            ISNULL(cem.coursesexamsdetails_MarkOfStudent, 0) AS MarksObtained
                        FROM 
                            StudentCourses sc
                        LEFT JOIN 
                            CoursesDetails cd ON sc.coursesdetails_ID = cd.coursesdetails_ID
                        LEFT JOIN 
                            CoursesExamsDetails ced ON cd.coursesdetails_ID = ced.coursesexamsdetails_coursesdetails_ID
                        LEFT JOIN 
                            CoursesExamsMarks cem ON ced.coursesexamsdetails_ID = cem.coursesexamsmarks_coursesexamsdetails_ID
                            AND cem.coursesexamsdetails_students_ID = sc.students_ID
                        LEFT JOIN 
                            Students s ON sc.students_ID = s.students_ID
                        WHERE 
                            ced.coursesexamsdetails_ID = @coursesexamsdetails_ID;";
            return await _db.QueryAsync<MarkStudentsModel>(sql, new { coursesexamsdetails_ID });
        }
        
        public async Task<IEnumerable<CoursesDetailsModel>> GetAllCoursesByProfessorsID(int coursesdetails_professors_ID)
        {
            var sql = @"SELECT s.students_ID
                              ,s.students_Name
                              ,s.students_PhoneNumber
                              ,s.students_GPA
                              ,s.students_departments_ID
                              ,s.students_SemesterIN
                        FROM StudentCourses sc
                        LEFT JOIN CoursesDetails cd ON sc.coursesdetails_ID = cd.coursesdetails_ID
                        LEFT JOIN Students s ON sc.students_ID = s.students_ID
                        WHERE cd.coursesdetails_ID = @coursesdetails_ID";
            return await _db.QueryAsync<CoursesDetailsModel>(sql, new { coursesdetails_professors_ID });
        }
        
        public async Task<IEnumerable<CoursesDetailsModel>> GetAllCoursesByStudentID(int students_ID)
        {
            var sql = @"SELECT cd.coursesdetails_ID
                              ,cd.coursesdetails_courses_ID
                              ,cd.coursesdetails_semesters_ID
                              ,cd.coursesdetails_departments_ID
                              ,cd.CoursesDetails_years_ID
                              ,cd.CoursesDetails_days_ID
                              ,cd.CoursesDetails_rooms_ID
                              ,cd.coursesdetails_periods_ID
                              ,cd.coursesdetails_professors_ID FROM StudentCourses sc
                        LEFT JOIN CoursesDetails cd ON sc.coursesdetails_ID = cd.coursesdetails_ID
                        WHERE sc.students_ID = @students_ID";
            return await _db.QueryAsync<CoursesDetailsModel>(sql, new { students_ID });
        }

        public async Task<IEnumerable<CourseGradeModel>> GetStudentGradesAsync(int studentId)
        {
            var sql = @"
            SELECT 
                sc.coursesdetails_ID AS CoursesDetailsId,
                c.courses_Name AS CourseName,
                ce.coursesexamsdetails_Name AS ExamName,
                ce.coursesexamsdetails_Date AS ExamDate,
                ce.coursesexamsdetails_TotalMarks AS ExamTotalMarks,
                ISNULL(cem.coursesexamsdetails_MarkOfStudent, 0) AS MarksObtained
            FROM 
                StudentCourses sc
            INNER JOIN 
                CoursesDetails cd ON sc.coursesdetails_ID = cd.coursesdetails_ID
            INNER JOIN 
                Courses c ON cd.coursesdetails_courses_ID = c.courses_ID
            LEFT JOIN 
                CoursesExamsDetails ce ON cd.coursesdetails_ID = ce.coursesexamsdetails_coursesdetails_ID
            LEFT JOIN 
                CoursesExamsMarks cem ON ce.coursesexamsdetails_ID = cem.coursesexamsmarks_coursesexamsdetails_ID
                AND cem.coursesexamsdetails_students_ID = @StudentId
            WHERE 
                sc.students_ID = @StudentId";

            var examResults = await _db.QueryAsync<ExamResult>(sql, new { StudentId = studentId });

            var groupedResults = examResults
                .GroupBy(x => new { x.CoursesDetailsId, x.CourseName })
                .Select(g => new CourseGradeModel
                {
                    CourseId = g.Key.CoursesDetailsId,
                    CourseName = g.Key.CourseName,
                    TotalMarksObtained = g.Sum(e => e.MarksObtained),
                    TotalCourseMarks = g.Sum(e => e.ExamTotalMarks),
                    Grade = CalculateGradeSafe(
                        g.Sum(e => e.MarksObtained),
                        g.Sum(e => e.ExamTotalMarks)
                    ),
                    ExamGrades = g.Select(e => new ExamGradeModel
                    {
                        ExamName = e.ExamName,
                        ExamDate = e.ExamDate,
                        MarksObtained = e.MarksObtained,
                        ExamTotalMarks = e.ExamTotalMarks
                    }).ToList()
                }).ToList();

            return groupedResults;
        }

        private string CalculateGradeSafe(decimal marksObtained, decimal totalMarks)
        {
            if (totalMarks == 0)
            {
                return "N/A";
            }
            var percentage = (marksObtained / totalMarks) * 100;

            if (percentage >= 90) return "A";
            if (percentage >= 80) return "B";
            if (percentage >= 70) return "C";
            if (percentage >= 60) return "D";
            return "F";
        }

        public async Task<IEnumerable<LectureGridModel>> GetStudentLectureGridAsync(int studentId)
        {
            var sql = @"
        SELECT 
            c.courses_Name AS CourseName,
            r.room_Name AS RoomName,
            d.days_Name AS DayName,
            p.periods_Name AS PeriodName,
            pr.professors_Name AS ProfessorName
        FROM 
            CoursesDetails cd
        INNER JOIN 
            Courses c ON cd.coursesdetails_courses_ID = c.courses_ID
        INNER JOIN 
            Rooms r ON cd.CoursesDetails_rooms_ID = r.rooms_ID
        INNER JOIN 
            Days d ON cd.CoursesDetails_days_ID = d.days_ID
        INNER JOIN 
            Periods p ON cd.coursesdetails_periods_ID = p.periods_ID
        INNER JOIN 
            Professors pr ON cd.coursesdetails_professors_ID = pr.professors_ID
        INNER JOIN 
            StudentCourses sc ON cd.coursesdetails_ID = sc.coursesdetails_ID
        WHERE 
            sc.students_ID = @StudentId";

            return await _db.QueryAsync<LectureGridModel>(sql, new { StudentId = studentId });
        }
    }
}
