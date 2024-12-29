using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IDbConnection _db;

        public StudentsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<StudentsModel>> GetAllStudents()
        {
            var sql = @"SELECT s.*, d.departments_Name 
                FROM Students s
                LEFT JOIN Departments d ON s.students_departments_ID = d.departments_ID";
            return await _db.QueryAsync<StudentsModel>(sql);
        }

        public async Task<StudentsModel?> GetStudentById(int studentId)
        {
            var sql = @"SELECT s.*,d.departments_Name FROM Students s
                        left JOIN Departments d ON s.students_departments_ID = d.departments_ID
                         WHERE s.students_ID = @studentId";
            return await _db.QuerySingleOrDefaultAsync<StudentsModel>(sql, new { studentId });
        }

        public async Task<bool> InsertStudent(StudentsModel student)
        {
            var sql = @"INSERT INTO Students (students_ID,students_Name,students_PhoneNumber, students_BirthDay, students_GPA, students_departments_ID, students_SemesterIN, students_TotalCreidts, students_username, students_Password) 
                        VALUES (@students_ID,@students_Name, @students_PhoneNumber, @students_BirthDay, @students_GPA, @students_departments_ID, @students_SemesterIN, @students_TotalCreidts, @students_username, @students_Password)";
            var result = await _db.ExecuteAsync(sql, student);
            return result > 0;
        }

        public async Task<bool> UpdateStudent(StudentsModel student)
        {
            var sql = @"UPDATE Students 
                        SET students_Name = @students_Name, 
                            students_PhoneNumber = @students_PhoneNumber,
                            students_BirthDay = @students_BirthDay, 
                            students_GPA = @students_GPA, 
                            students_departments_ID = @students_departments_ID, 
                            students_SemesterIN = @students_SemesterIN, 
                            students_TotalCreidts = @students_TotalCreidts,
                            students_username = @students_username, 
                            students_Password = @students_Password
                        WHERE students_ID = @students_ID";
            var result = await _db.ExecuteAsync(sql, student);
            return result > 0;
        }

        public async Task<bool> DeleteStudent(int studentId)
        {
            var sql = @"DELETE FROM Students WHERE students_ID = @studentId";
            var result = await _db.ExecuteAsync(sql, new { studentId });
            return result > 0;
        }
        public async Task<int> GetMaxStudentsID()
        {
            var sql = @"SELECT MAX(students_ID) FROM Students;";
            var maxTeacherID = await _db.ExecuteScalarAsync<int?>(sql);
            return maxTeacherID ?? 555555;
        }

        public async Task<bool> AddCourseAsync(int studentId, int courseId)
        {
            var sql = @"INSERT INTO StudentCourses (students_ID, coursesdetails_ID)
                        VALUES (@StudentId, @CourseId)";
            var result = await _db.ExecuteAsync(sql, new { StudentId = studentId, CourseId = courseId });
            return result > 0;
        }

        public async Task<bool> RemoveCourseAsync(int studentId, int courseId)
        {
            var sql = @"DELETE FROM StudentCourses 
                        WHERE students_ID = @StudentId AND coursesdetails_ID = @CourseId";
            var result = await _db.ExecuteAsync(sql, new { StudentId = studentId, CourseId = courseId });
            return result > 0;
        }

        public async Task<bool> IsCourseAddedAsync(int studentId, int courseId)
        {
            var sql = @"SELECT 1 FROM StudentCourses 
                        WHERE students_ID = @StudentId AND coursesdetails_ID = @CourseId";
            var result = await _db.ExecuteScalarAsync<int?>(sql, new { StudentId = studentId, CourseId = courseId });
            return result.HasValue;
        }

        public async Task<bool> HasScheduleConflictAsync(int userId, int courseId)
        {
            var query = @"
                SELECT 1
                FROM StudentCourses sc
                LEFT JOIN CoursesDetails cd1 ON sc.coursesdetails_ID = cd1.coursesdetails_ID
                LEFT JOIN CoursesDetails cd2 ON cd1.coursesdetails_days_ID = cd2.coursesdetails_days_ID
                WHERE sc.students_ID = @UserId
                  AND cd1.coursesdetails_periods_ID = cd2.coursesdetails_periods_ID
                  AND cd2.coursesdetails_ID = @CourseId";

            var result = await _db.ExecuteScalarAsync<int?>(query, new { UserId = userId, CourseId = courseId });
            return result.HasValue;
        }

        public async Task<IEnumerable<ContactFormModel>> GetAllMessagesAsync()
        {
            var sql = @"SELECT * FROM ContactUs";
            return await _db.QueryAsync<ContactFormModel>(sql);
        }

        public async Task<bool> SaveContactFormAsync(ContactFormModel model)
        {
            var sql = @"
                INSERT INTO ContactUs (Name, Email, Subject, Message, SubmittedAt) 
                VALUES (@Name, @Email, @Subject, @Message, @SubmittedAt)";

            var result = await _db.ExecuteAsync(sql, new
            {
                model.Name,
                model.Email,
                model.Subject,
                model.Message,
                SubmittedAt = DateTime.Now
            });

            return result > 0;
        }

        public async Task<bool> CheckUsernameExistsInStudents(string username)
        {
            string query = "SELECT COUNT(1) FROM Students WHERE students_Username = @Username";
            int count = await _db.ExecuteScalarAsync<int>(query, new { Username = username });
            return count > 0;
        }

        public async Task<bool> CheckUsernameExistsInProfessors(string username)
        {
            string query = "SELECT COUNT(1) FROM Professors WHERE professors_Username = @Username";
            int count = await _db.ExecuteScalarAsync<int>(query, new { Username = username });
            return count > 0;
        }

        public async Task<bool> CheckUsernameExistsInAdmins(string username)
        {
            string query = "SELECT COUNT(1) FROM AdminUsers WHERE adminusers_Username = @Username";
            int count = await _db.ExecuteScalarAsync<int>(query, new { Username = username });
            return count > 0;
        }
    }
}
