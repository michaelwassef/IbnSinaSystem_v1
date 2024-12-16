using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class SemestersService : ISemestersService
    {
        private readonly IDbConnection _db;

        public SemestersService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }
        public async Task<IEnumerable<SemestersModel>> GetAllSemesters()
        {
            var sql = @"SELECT * FROM Semesters";
            return await _db.QueryAsync<SemestersModel>(sql);
        }

        public async Task<SemestersModel?> SelectSemesterByID(int semesters_ID)
        {
            var sql = @"SELECT * FROM Semesters WHERE semesters_ID = @semesters_ID";
            var Results = await _db.QuerySingleOrDefaultAsync<SemestersModel>(sql, new { semesters_ID });
            return Results;
        }

        public async Task<bool> InsertSemester(SemestersModel semestersModel)
        {
            var sql = @"INSERT INTO Semesters (semesters_Name) VALUES (@semesters_Name);";

            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, semestersModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateSemester(SemestersModel semestersModel)
        {
            var sql = @"UPDATE Semesters SET semesters_Name = @semesters_Name WHERE semesters_ID = @semesters_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, semestersModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSemester(int semesters_ID)
        {
            var sql = @"DELETE FROM Semesters WHERE semesters_ID = @semesters_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { semesters_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
