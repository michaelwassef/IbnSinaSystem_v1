using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;


namespace IbnSinaSystem.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly IDbConnection _db;

        public CoursesService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }
        public async Task<IEnumerable<CoursesModel>> GetAllCourses()
        {
            var sql = @"SELECT * FROM Courses";
            return await _db.QueryAsync<CoursesModel>(sql);
        }

        public async Task<CoursesModel?> SelectcourseByID(int courses_ID)
        {
            var sql = @"SELECT * FROM Courses WHERE courses_ID = @courses_ID";
            var Results = await _db.QuerySingleOrDefaultAsync<CoursesModel>(sql, new { courses_ID });
            return Results;
        }

        public async Task<bool> Insertcourse(CoursesModel courseModel)
        {
            var sql = @"INSERT INTO Courses (courses_Name, courses_Credit) VALUES (@courses_Name, @courses_Credit);";

            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, courseModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Updatecourse(CoursesModel courseModel)
        {
            var sql = @"UPDATE Courses SET courses_Name = @courses_Name, courses_Credit = @courses_Credit WHERE courses_ID = @courses_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, courseModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Deletecourse(int courses_ID)
        {
            var sql = @"DELETE FROM Courses WHERE courses_ID = @courses_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { courses_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
