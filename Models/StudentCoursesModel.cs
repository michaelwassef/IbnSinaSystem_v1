namespace IbnSinaSystem.Models
{
    public class StudentCoursesModel
    {
        public int students_ID { get; set; }
        public int coursesdetails_ID { get; set; }
        public StudentsModel studentsModel { get; set; }
        public CoursesDetailsModel coursesDetailsModel { get; set; }
    }
}
