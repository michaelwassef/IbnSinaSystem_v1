using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IDbConnection _db;

        public DepartmentsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }
        public async Task<IEnumerable<DepartmentsModel>> GetAllDepartments()
        {
            var sql = @"SELECT * FROM Departments";
            return await _db.QueryAsync<DepartmentsModel>(sql);
        }

        public async Task<DepartmentsModel?> SelectdepartmentByID(int departments_ID)
        {
            var sql = @"SELECT * FROM Departments WHERE departments_ID = @departments_ID";
            var Results = await _db.QuerySingleOrDefaultAsync<DepartmentsModel>(sql, new { departments_ID });
            return Results;
        }

        public async Task<bool> Insertdepartment(DepartmentsModel departmentModel)
        {
            var sql = @"INSERT INTO Departments (departments_Name) VALUES (@departments_Name);";

            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, departmentModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Updatedepartment(DepartmentsModel departmentModel)
        {
            var sql = @"UPDATE Departments SET departments_Name = @departments_Name WHERE departments_ID = @departments_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, departmentModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Deletedepartment(int departments_ID)
        {
            var sql = @"DELETE FROM Departments WHERE departments_ID = @departments_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { departments_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
