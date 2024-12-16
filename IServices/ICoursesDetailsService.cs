using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface ICoursesDetailsService
    {
        Task<IEnumerable<CoursesDetailsModel>> GetAllCourseDetails();
        Task<IEnumerable<CoursesDetails2Model>> GetAllAvailableCourseDetails(int students_ID, int coursesdetails_semesters_ID, int coursesdetails_departments_ID);
        Task<IEnumerable<CoursesDetails2Model>> GetAllCourseDetailsByProfessorsID(int coursesdetails_professors_ID);

        Task<CoursesDetailsModel?> GetCourseDetailById(int id);
        Task<bool> InsertCourseDetail(CoursesDetailsModel courseDetail);
        Task<bool> UpdateCourseDetail(CoursesDetailsModel courseDetail);
        Task<bool> DeleteCourseDetail(int coursesdetails_ID);
        Task<int> GetMaxCoursesDetailsID();
        Task<IEnumerable<StudentsModel>> GetAllStudentsByCourseID(int coursesdetails_ID);
        Task<IEnumerable<MarkStudentsModel>> GetAllMarksStudentsByCourseID(int coursesexamsdetails_ID);
        Task<IEnumerable<CoursesDetailsModel>> GetAllCoursesByProfessorsID(int coursesdetails_professors_ID);
        Task<IEnumerable<CoursesDetailsModel>> GetAllCoursesByStudentID(int students_ID);
        Task<IEnumerable<CourseGradeModel>> GetStudentGradesAsync(int studentId);
        Task<IEnumerable<LectureGridModel>> GetStudentLectureGridAsync(int studentId);
    }
}
