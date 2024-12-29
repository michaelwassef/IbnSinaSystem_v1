using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IStudentsService
    {
        Task<IEnumerable<StudentsModel>> GetAllStudents();
        Task<StudentsModel?> GetStudentById(int studentId);
        Task<bool> InsertStudent(StudentsModel student);
        Task<bool> UpdateStudent(StudentsModel student);
        Task<bool> DeleteStudent(int studentId);
        Task<int> GetMaxStudentsID();

        Task<bool> AddCourseAsync(int studentId, int courseId);
        Task<bool> RemoveCourseAsync(int studentId, int courseId);
        Task<bool> IsCourseAddedAsync(int studentId, int courseId);
        Task<bool> HasScheduleConflictAsync(int userId, int courseId);

        Task<IEnumerable<ContactFormModel>> GetAllMessagesAsync();
        Task<bool> SaveContactFormAsync(ContactFormModel model);

        Task<bool> CheckUsernameExistsInStudents(string username);
        Task<bool> CheckUsernameExistsInProfessors(string username);
        Task<bool> CheckUsernameExistsInAdmins(string username);
    }
}
