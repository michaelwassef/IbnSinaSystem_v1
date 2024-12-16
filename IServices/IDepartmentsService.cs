using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IDepartmentsService
    {
        Task<IEnumerable<DepartmentsModel>> GetAllDepartments();
        Task<DepartmentsModel?> SelectdepartmentByID(int departments_ID);
        Task<bool> Insertdepartment(DepartmentsModel departmentModel);
        Task<bool> Updatedepartment(DepartmentsModel departmentModel);
        Task<bool> Deletedepartment(int departments_ID);
    }
}
