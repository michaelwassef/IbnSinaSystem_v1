using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class AccessService : IAccessService
    {
        private readonly IDbConnection _db;

        public AccessService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<AuthenticationResultModel> AuthenticateUserAsync(LoginsModel access)
        {
            try
            {
                var user = await AuthenticateFromTableAsync(
                    "SELECT students_ID AS Id, students_Username AS UserName, students_Password AS Password, 'Student' AS Role FROM Students WHERE students_Username = @Username",
                    access.UserName,
                    access.Password
                );

                if (user == null)
                {
                    user = await AuthenticateFromTableAsync(
                        "SELECT professors_ID AS Id, professors_UserName AS UserName, professors_Password AS Password, 'Professor' AS Role FROM Professors WHERE professors_UserName = @Username",
                        access.UserName,
                        access.Password
                    );
                }

                if (user == null)
                {
                    user = await AuthenticateFromTableAsync(
                        "SELECT adminusers_ID AS Id, adminusers_UserName AS UserName, adminusers_Password AS Password, 'Admin' AS Role FROM AdminUsers WHERE adminusers_UserName = @Username",
                        access.UserName,
                        access.Password
                    );
                }

                if (user == null)
                {
                    return new AuthenticationResultModel { IsAuthenticated = false };
                }

                return new AuthenticationResultModel
                {
                    IsAuthenticated = true,
                    userid = user.Value.Id,
                    UserName = user.Value.UserName,
                    Role = user.Value.Role
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<(int Id, string UserName, string Role)?> AuthenticateFromTableAsync(string sql, string username, string password)
        {
            try
            {
                var user = await _db.QuerySingleOrDefaultAsync<(int Id, string UserName, string Password, string Role)>(sql, new { Username = username });

                if (user == default || !PasswordHasher.VerifyPassword(password, user.Password))
                {
                    return null;
                }

                return (user.Id, user.UserName, user.Role);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
