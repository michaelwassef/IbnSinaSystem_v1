using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class CoursesExamsDetailsService : ICoursesExamsDetailsService
    {
        private readonly IDbConnection _db;
        public CoursesExamsDetailsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        // ------------------ CoursesExamsDetails Methods ------------------
        public async Task<IEnumerable<CoursesExamsDetailsModel>> GetAllCoursesExamsDetails()
        {
            string query = "SELECT * FROM CoursesExamsDetails";
            return await _db.QueryAsync<CoursesExamsDetailsModel>(query);
        }
        public async Task<IEnumerable<CoursesExamsDetailsModel>> GetAllCoursesExamsDetailsByCourse(int coursesdetails_ID)
        {
            string query = "SELECT * FROM CoursesExamsDetails WHERE coursesexamsdetails_coursesdetails_ID = @coursesdetails_ID";
            return await _db.QueryAsync<CoursesExamsDetailsModel>(query, new { coursesdetails_ID });
        }
        public async Task<int> GetTotalMarksByCourse(int courseId)
        {
            var sql = @"
                        SELECT COALESCE(SUM(coursesexamsdetails_TotalMarks), 0) 
                        FROM CoursesExamsDetails 
                        WHERE coursesexamsdetails_coursesdetails_ID = @courseId";

            return await _db.ExecuteScalarAsync<int>(sql, new { courseId });
        }
        public async Task<CoursesExamsDetailsModel?> GetCoursesExamsDetailsById(int coursesExamsDetailsId)
        {
            string query = "SELECT * FROM CoursesExamsDetails WHERE coursesexamsdetails_ID = @Id";
            return await _db.QueryFirstOrDefaultAsync<CoursesExamsDetailsModel>(query, new { Id = coursesExamsDetailsId });
        }
        public async Task<bool> InsertCoursesExamsDetails(CoursesExamsDetailsModel examDetails)
        {
            string query = @"
            INSERT INTO CoursesExamsDetails (coursesexamsdetails_coursesdetails_ID, coursesexamsdetails_Name, coursesexamsdetails_Date, coursesexamsdetails_TotalMarks)
            VALUES (@coursesexamsdetails_coursesdetails_ID, @coursesexamsdetails_Name, @coursesexamsdetails_Date, @coursesexamsdetails_TotalMarks)";
            int rowsAffected = await _db.ExecuteAsync(query, examDetails);
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateCoursesExamsDetails(CoursesExamsDetailsModel examDetails)
        {
            string query = @"
            UPDATE CoursesExamsDetails
            SET coursesexamsdetails_coursesdetails_ID = @coursesexamsdetails_coursesdetails_ID,
                coursesexamsdetails_Name = @coursesexamsdetails_Name,
                coursesexamsdetails_Date = @coursesexamsdetails_Date,
                coursesexamsdetails_TotalMarks = @coursesexamsdetails_TotalMarks
            WHERE coursesexamsdetails_ID = @coursesexamsdetails_ID";
            int rowsAffected = await _db.ExecuteAsync(query, examDetails);
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteCoursesExamsDetails(int coursesExamsDetailsId)
        {
            string query = "DELETE FROM CoursesExamsDetails WHERE coursesexamsdetails_ID = @Id";
            int rowsAffected = await _db.ExecuteAsync(query, new { Id = coursesExamsDetailsId });
            return rowsAffected > 0;
        }

        // ------------------ CoursesExamsMarks Methods ------------------
        public async Task<bool> AddOrUpdateGradeAsync(CoursesExamsMarksModel model)
        {
            var sql = @"
                IF EXISTS (
                    SELECT 1 
                    FROM CoursesExamsMarks 
                    WHERE coursesexamsmarks_coursesexamsdetails_ID = @coursesexamsmarks_coursesexamsdetails_ID AND coursesexamsdetails_students_ID = @coursesexamsdetails_students_ID
                )
                BEGIN
                    UPDATE CoursesExamsMarks 
                    SET coursesexamsdetails_MarkOfStudent = @coursesexamsdetails_MarkOfStudent 
                    WHERE coursesexamsmarks_coursesexamsdetails_ID = @coursesexamsmarks_coursesexamsdetails_ID AND coursesexamsdetails_students_ID = @coursesexamsdetails_students_ID
                END
                ELSE
                BEGIN
                    INSERT INTO CoursesExamsMarks (coursesexamsmarks_coursesexamsdetails_ID, coursesexamsdetails_students_ID, coursesexamsdetails_MarkOfStudent) 
                    VALUES (@coursesexamsmarks_coursesexamsdetails_ID, @coursesexamsdetails_students_ID, @coursesexamsdetails_MarkOfStudent)
                END";

            try
            {
                int affectedRows = await _db.ExecuteAsync(sql, new
                {
                    model.coursesexamsmarks_coursesexamsdetails_ID,
                    model.coursesexamsdetails_students_ID,
                    model.coursesexamsdetails_MarkOfStudent
                });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<CoursesExamsMarksModel>> GetAllCoursesExamsMarks()
        {
            string query = "SELECT * FROM CoursesExamsMarks";
            return await _db.QueryAsync<CoursesExamsMarksModel>(query);
        }
        public async Task<IEnumerable<CoursesExamsMarksModel>> GetMarksByStudentId(int studentId)
        {
            string query = "SELECT * FROM CoursesExamsMarks WHERE coursesexamsdetails_students_ID = @StudentId";
            return await _db.QueryAsync<CoursesExamsMarksModel>(query, new { StudentId = studentId });
        }
        public async Task<bool> InsertCoursesExamsMarks(CoursesExamsMarksModel marks)
        {
            string query = @"
            INSERT INTO CoursesExamsMarks (coursesexamsmarks_coursesexamsdetails_ID, coursesexamsdetails_students_ID, coursesexamsdetails_MarkOfStudent)
            VALUES (@coursesexamsmarks_coursesexamsdetails_ID, @coursesexamsdetails_students_ID, @coursesexamsdetails_MarkOfStudent)";
            int rowsAffected = await _db.ExecuteAsync(query, marks);
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateCoursesExamsMarks(CoursesExamsMarksModel marks)
        {
            string query = @"
            UPDATE CoursesExamsMarks
            SET coursesexamsmarks_coursesexamsdetails_ID = @coursesexamsmarks_coursesexamsdetails_ID,
                coursesexamsdetails_students_ID = @coursesexamsdetails_students_ID,
                coursesexamsdetails_MarkOfStudent = @coursesexamsdetails_MarkOfStudent
            WHERE coursesexamsmarks_ID = @coursesexamsmarks_ID";
            int rowsAffected = await _db.ExecuteAsync(query, marks);
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteCoursesExamsMarks(int coursesExamsMarksId)
        {
            string query = "DELETE FROM CoursesExamsMarks WHERE coursesexamsmarks_ID = @Id";
            int rowsAffected = await _db.ExecuteAsync(query, new { Id = coursesExamsMarksId });
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteCoursesExamsMarksByExam(int coursesexamsdetails_ID)
        {
            string query = "DELETE FROM CoursesExamsMarks WHERE coursesexamsmarks_coursesexamsdetails_ID = @coursesexamsdetails_ID";
            int rowsAffected = await _db.ExecuteAsync(query, new { coursesexamsdetails_ID });
            return rowsAffected > 0;
        }
    }
}
