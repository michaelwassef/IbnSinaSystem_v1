using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class DaysService : IDaysService
    {
        private readonly IDbConnection _db;

        public DaysService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<DaysModel>> GetAllDays()
        {
            var sql = @"SELECT * FROM Days";
            return await _db.QueryAsync<DaysModel>(sql);
        }

        public async Task<DaysModel?> SelectDayByID(int days_ID)
        {
            var sql = @"SELECT * FROM Days WHERE days_ID = @days_ID";
            var Results = await _db.QuerySingleOrDefaultAsync<DaysModel>(sql, new { days_ID });
            return Results;
        }

        public async Task<bool> InsertDay(DaysModel dayModel)
        {
            var sql = @"INSERT INTO Days (days_Name) VALUES (@days_Name);";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, dayModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDay(DaysModel dayModel)
        {
            var sql = @"UPDATE Days SET days_Name = @days_Name WHERE days_ID = @days_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, dayModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteDay(int days_ID)
        {
            var sql = @"DELETE FROM Days WHERE days_ID = @days_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { days_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
