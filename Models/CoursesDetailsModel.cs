namespace IbnSinaSystem.Models
{
    public class CoursesDetailsModel
    {
        public int coursesdetails_ID { get; set; }
        public int coursesdetails_courses_ID { get; set; }
        public int coursesdetails_semesters_ID { get; set; }
        public int coursesdetails_departments_ID { get; set; }
        public int CoursesDetails_years_ID { get; set; }
        public int CoursesDetails_days_ID { get; set; }
        public int CoursesDetails_rooms_ID { get; set; }
        public int coursesdetails_periods_ID { get; set; }
        public bool coursesdetails_IsEnd { get; set; }
        public int coursesdetails_professors_ID { get; set; }
        public string courses_Name { get; set; }
    }
    
    public class CoursesDetails2Model
    {
        public int coursesdetails_ID { get; set; }
        public string courses_Name { get; set; }
        public int courses_Credit { get; set; }
        public string professors_Name { get; set; }
        public string room_Name { get; set; }
        public string days_Name { get; set; }
        public string periods_Name { get; set; }
        public bool Is_Added { get; set; }
    }

    public class CourseGradeModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal TotalMarksObtained { get; set; }
        public decimal TotalCourseMarks { get; set; }
        public string Grade { get; set; }
        public List<ExamGradeModel> ExamGrades { get; set; }
    }

    public class ExamGradeModel
    {
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public decimal MarksObtained { get; set; }
        public decimal ExamTotalMarks { get; set; }
    }

    public class ExamResult
    {
        public int CoursesDetailsId { get; set; }
        public string CourseName { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public decimal ExamTotalMarks { get; set; }
        public decimal MarksObtained { get; set; }
    }

    public class LectureGridModel
    {
        public string CourseName { get; set; }
        public string RoomName { get; set; }
        public string DayName { get; set; }
        public string PeriodName { get; set; }
        public string ProfessorName { get; set; }
    }

}
