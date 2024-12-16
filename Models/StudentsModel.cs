namespace IbnSinaSystem.Models
{
    public class StudentsModel
    {
        public int students_ID { get; set; }
        public string students_Name { get; set; }
        public string? students_PhoneNumber { get; set; }
        public DateTime students_BirthDay { get; set; }
        public decimal students_GPA { get; set; }
        public int students_departments_ID { get; set; }
        public string? departments_Name { get; set; }
        public int students_SemesterIN { get; set; }
        public int students_TotalCreidts { get; set; }
        public string students_username { get; set; }
        public string students_Password { get; set; }
    }
    
    public class MarkStudentsModel
    {
        public int students_ID { get; set; }
        public string students_Name { get; set; }
        public decimal MarksObtained { get; set; }
    }
}
