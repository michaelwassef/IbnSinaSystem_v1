using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IYearsService
    {
        Task<IEnumerable<YearsModel>> GetAllYears();
        Task<YearsModel?> SelectYearByID(int years_ID);
        Task<bool> InsertYear(YearsModel yearModel);
        Task<bool> UpdateYear(YearsModel yearModel);
        Task<bool> DeleteYear(int years_ID);
    }
}
