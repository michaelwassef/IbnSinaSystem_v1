namespace IbnSinaSystem.Models
{
    public class AdminUsersModel
    {
        public int adminusers_ID { get; set; }
        public string adminusers_UserName { get; set; }
        public string adminusers_Password { get; set; }
    } 
    
    public class DBAdminUsersModel
    {
        public string CourseName { get; set; }
        public int StudentsCount { get; set; }
    }
}
