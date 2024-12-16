using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface ICoursesService
    {
        Task<IEnumerable<CoursesModel>> GetAllCourses();
        Task<CoursesModel?> SelectcourseByID(int courses_ID);
        Task<bool> Insertcourse(CoursesModel courseModel);
        Task<bool> Updatecourse(CoursesModel courseModel);
        Task<bool> Deletecourse(int courses_ID);
    }
}
