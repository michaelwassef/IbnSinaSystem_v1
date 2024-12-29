using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IAdminUsersService
    {
        Task<IEnumerable<AdminUsersModel>> GetAllAdminUsers();
        Task<AdminUsersModel?> GetAdminUserById(int adminUserId);
        Task<bool> InsertAdminUser(AdminUsersModel adminUser);
        Task<bool> UpdateAdminUser(AdminUsersModel adminUser);
        Task<bool> DeleteAdminUser(int adminUserId);

        Task<int> GetTotalStudentsAsync();
        Task<int> GetTotalProfessorsAsync();
        Task<int> GetTotalCoursesAsync();
        Task<IEnumerable<DBAdminUsersModel>> GetStudentsCountByCourse();
    }
}
