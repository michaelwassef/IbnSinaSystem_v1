namespace IbnSinaSystem.Models
{
    public class CoursesExamsMarksModel
    {
        public int coursesexamsmarks_ID { get; set; }
        public int coursesexamsmarks_coursesexamsdetails_ID { get; set; }
        public int coursesexamsdetails_students_ID { get; set; }
        public int coursesexamsdetails_MarkOfStudent { get; set; }
    }
}
