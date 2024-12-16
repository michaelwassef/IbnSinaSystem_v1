using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IProfessorsService
    {
        Task<IEnumerable<ProfessorsModel>> GetAllProfessors();
        Task<ProfessorsModel?> GetProfessorById(int professorId);
        Task<bool> InsertProfessor(ProfessorsModel professor);
        Task<bool> UpdateProfessor(ProfessorsModel professor);
        Task<bool> DeleteProfessor(int professorId);
        Task<int> GetMaxprofessorsID();
    }
}
