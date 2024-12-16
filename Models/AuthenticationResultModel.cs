namespace IbnSinaSystem.Models
{
    public class LoginsModel
    {
        public string userid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? UserTypeId { get; set; }
        public bool KeepLoggedIn { get; set; }
        public bool? isuserdisabled { get; set; }
    }
    
    public class AccessModel
    {
        public string userid { get; set; }
        public int userstypecode { get; set; }
        public string username { get; set; }
        public string usernameinarabic { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        public int? UserTypeId { get; set; }
        public bool KeepLoggedIn { get; set; }
        public bool? isroot { get; set; }
        public bool? isuserdisabled { get; set; }
        public bool? isusertobelisted { get; set; }
    }

    public class AuthenticationResultModel
    {
        public bool IsAuthenticated { get; set; }
        public int userid { get; set; }
        public bool isuserdisabled { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
