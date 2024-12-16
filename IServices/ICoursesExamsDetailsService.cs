using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface ICoursesExamsDetailsService
    {
        Task<IEnumerable<CoursesExamsDetailsModel>> GetAllCoursesExamsDetails();
        Task<IEnumerable<CoursesExamsDetailsModel>> GetAllCoursesExamsDetailsByCourse(int coursesdetails_ID);
        Task<int> GetTotalMarksByCourse(int courseId);
        Task<CoursesExamsDetailsModel?> GetCoursesExamsDetailsById(int coursesExamsDetailsId);
        Task<bool> InsertCoursesExamsDetails(CoursesExamsDetailsModel examDetails);
        Task<bool> UpdateCoursesExamsDetails(CoursesExamsDetailsModel examDetails);
        Task<bool> DeleteCoursesExamsDetails(int coursesExamsDetailsId);

        Task<bool> AddOrUpdateGradeAsync(CoursesExamsMarksModel model);
        Task<IEnumerable<CoursesExamsMarksModel>> GetAllCoursesExamsMarks();
        Task<IEnumerable<CoursesExamsMarksModel>> GetMarksByStudentId(int studentId);
        Task<bool> InsertCoursesExamsMarks(CoursesExamsMarksModel marks);
        Task<bool> UpdateCoursesExamsMarks(CoursesExamsMarksModel marks);
        Task<bool> DeleteCoursesExamsMarks(int coursesExamsMarksId);
        Task<bool> DeleteCoursesExamsMarksByExam(int coursesexamsdetails_ID);
    }
}
