using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class ProfessorsService : IProfessorsService
    {
        private readonly IDbConnection _db;

        public ProfessorsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<ProfessorsModel>> GetAllProfessors()
        {
            var sql = @"SELECT * FROM Professors";
            return await _db.QueryAsync<ProfessorsModel>(sql);
        }

        public async Task<ProfessorsModel?> GetProfessorById(int professorId)
        {
            var sql = @"SELECT * FROM Professors WHERE professors_ID = @professorId";
            return await _db.QuerySingleOrDefaultAsync<ProfessorsModel>(sql, new { professorId });
        }

        public async Task<bool> InsertProfessor(ProfessorsModel professor)
        {
            var sql = @"INSERT INTO Professors (professors_ID,professors_Name, professors_PhoneNumber, professors_UserName, professors_Password) 
                        VALUES (@professors_ID, @professors_Name, @professors_PhoneNumber, @professors_UserName, @professors_Password)";
            var result = await _db.ExecuteAsync(sql, professor);
            return result > 0;
        }

        public async Task<bool> UpdateProfessor(ProfessorsModel professor)
        {
            var sql = @"UPDATE Professors 
                        SET professors_Name = @professors_Name, 
                            professors_PhoneNumber = @professors_PhoneNumber, 
                            professors_UserName = @professors_UserName, 
                            professors_Password = @professors_Password
                        WHERE professors_ID = @professors_ID";
            var result = await _db.ExecuteAsync(sql, professor);
            return result > 0;
        }

        public async Task<bool> DeleteProfessor(int professorId)
        {
            var sql = @"DELETE FROM Professors WHERE professors_ID = @professorId";
            var result = await _db.ExecuteAsync(sql, new { professorId });
            return result > 0;
        }
        public async Task<int> GetMaxprofessorsID()
        {
            var sql = @"SELECT MAX(professors_ID) FROM Professors;";
            var maxTeacherID = await _db.ExecuteScalarAsync<int?>(sql);
            return maxTeacherID ?? 222222;
        }

    }
}
