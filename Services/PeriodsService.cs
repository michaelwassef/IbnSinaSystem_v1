using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class PeriodsService : IPeriodsService
    {
        private readonly IDbConnection _db;

        public PeriodsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<PeriodsModel>> GetAllPeriods()
        {
            var sql = @"SELECT * FROM Periods";
            return await _db.QueryAsync<PeriodsModel>(sql);
        }

        public async Task<PeriodsModel?> SelectPeriodByID(int Periods_ID)
        {
            var sql = @"SELECT * FROM Periods WHERE periods_ID = @periods_ID";
            var result = await _db.QuerySingleOrDefaultAsync<PeriodsModel>(sql, new { Periods_ID });
            return result;
        }

        public async Task<bool> InsertPeriod(PeriodsModel PeriodModel)
        {
            var sql = @"INSERT INTO Periods (periods_Name) VALUES (@periods_Name);";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, PeriodModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePeriod(PeriodsModel PeriodModel)
        {
            var sql = @"UPDATE Periods SET periods_Name = @periods_Name WHERE periods_ID = @periods_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, PeriodModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePeriod(int Periods_ID)
        {
            var sql = @"DELETE FROM Periods WHERE periods_ID = @periods_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { Periods_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
