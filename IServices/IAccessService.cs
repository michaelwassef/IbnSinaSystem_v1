using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IAccessService
    {
        Task<AuthenticationResultModel> AuthenticateUserAsync(LoginsModel access);
    }
}
