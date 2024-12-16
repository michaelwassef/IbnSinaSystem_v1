using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IDaysService
    {
        Task<IEnumerable<DaysModel>> GetAllDays();
        Task<DaysModel?> SelectDayByID(int days_ID);
        Task<bool> InsertDay(DaysModel dayModel);
        Task<bool> UpdateDay(DaysModel dayModel);
        Task<bool> DeleteDay(int days_ID);
    }
}
