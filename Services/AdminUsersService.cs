﻿using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class AdminUsersService : IAdminUsersService
    {
        private readonly IDbConnection _db;

        public AdminUsersService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<AdminUsersModel>> GetAllAdminUsers()
        {
            string query = "SELECT * FROM AdminUsers";
            return await _db.QueryAsync<AdminUsersModel>(query);
        }

        public async Task<AdminUsersModel?> GetAdminUserById(int adminUserId)
        {
            string query = "SELECT * FROM AdminUsers WHERE adminusers_ID = @Id";
            return await _db.QueryFirstOrDefaultAsync<AdminUsersModel>(query, new { Id = adminUserId });
        }

        public async Task<bool> InsertAdminUser(AdminUsersModel adminUser)
        {
            string query = @"
            INSERT INTO AdminUsers (adminusers_UserName, adminusers_Password)
            VALUES (@adminusers_UserName, @adminusers_Password)";
            int rowsAffected = await _db.ExecuteAsync(query, adminUser);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAdminUser(AdminUsersModel adminUser)
        {
            string query = @"
            UPDATE AdminUsers
            SET adminusers_UserName = @adminusers_UserName,
                adminusers_Password = @adminusers_Password
            WHERE adminusers_ID = @adminusers_ID";
            int rowsAffected = await _db.ExecuteAsync(query, adminUser);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAdminUser(int adminUserId)
        {
            string query = "DELETE FROM AdminUsers WHERE adminusers_ID = @Id";
            int rowsAffected = await _db.ExecuteAsync(query, new { Id = adminUserId });
            return rowsAffected > 0;
        }

        public async Task<int> GetTotalStudentsAsync()
        {
            string query = "SELECT COUNT(*) FROM Students";
            return await _db.ExecuteScalarAsync<int>(query);
        }

        public async Task<int> GetTotalProfessorsAsync()
        {
            string query = "SELECT COUNT(*) FROM Professors";
            return await _db.ExecuteScalarAsync<int>(query);
        }

        public async Task<int> GetTotalCoursesAsync()
        {
            string query = "SELECT COUNT(*) FROM CoursesDetails";
            return await _db.ExecuteScalarAsync<int>(query);
        }

        public async Task<IEnumerable<DBAdminUsersModel>> GetStudentsCountByCourse()
        {
            string query = @"
                SELECT c.courses_Name AS CourseName, COUNT(sc.students_ID) AS StudentsCount
                FROM StudentCourses sc
                LEFT JOIN CoursesDetails cd ON sc.coursesdetails_ID = cd.coursesdetails_ID
                LEFT JOIN Courses c ON cd.coursesdetails_courses_ID = c.courses_ID
                GROUP BY c.courses_Name";

            return await _db.QueryAsync<DBAdminUsersModel>(query);
        }

    }
}