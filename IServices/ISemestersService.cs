using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface ISemestersService
    {
        Task<IEnumerable<SemestersModel>> GetAllSemesters();
        Task<SemestersModel?> SelectSemesterByID(int semesters_ID);
        Task<bool> InsertSemester(SemestersModel semestersModel);
        Task<bool> UpdateSemester(SemestersModel semestersModel);
        Task<bool> DeleteSemester(int semesters_ID);
    }
}
