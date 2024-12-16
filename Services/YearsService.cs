using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class YearsService : IYearsService
    {
        private readonly IDbConnection _db;

        public YearsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<YearsModel>> GetAllYears()
        {
            var sql = @"SELECT * FROM Years";
            return await _db.QueryAsync<YearsModel>(sql);
        }

        public async Task<YearsModel?> SelectYearByID(int years_ID)
        {
            var sql = @"SELECT * FROM Years WHERE years_ID = @years_ID";
            var result = await _db.QuerySingleOrDefaultAsync<YearsModel>(sql, new { years_ID });
            return result;
        }

        public async Task<bool> InsertYear(YearsModel yearModel)
        {
            var sql = @"INSERT INTO Years (years_Name) VALUES (@years_Name);";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, yearModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateYear(YearsModel yearModel)
        {
            var sql = @"UPDATE Years SET years_Name = @years_Name WHERE years_ID = @years_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, yearModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteYear(int years_ID)
        {
            var sql = @"DELETE FROM Years WHERE years_ID = @years_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { years_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
