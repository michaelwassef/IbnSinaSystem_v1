namespace IbnSinaSystem.Models
{
    public class CoursesExamsDetailsModel
    {
        public int coursesexamsdetails_ID { get; set; }
        public int coursesexamsdetails_coursesdetails_ID { get; set; }
        public string coursesexamsdetails_Name { get; set; }
        public DateTime coursesexamsdetails_Date { get; set; }
        public int coursesexamsdetails_TotalMarks { get; set; }
    }
}
