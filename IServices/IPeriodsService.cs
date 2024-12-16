using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IPeriodsService
    {
        Task<IEnumerable<PeriodsModel>> GetAllPeriods();
        Task<PeriodsModel?> SelectPeriodByID(int Periods_ID);
        Task<bool> InsertPeriod(PeriodsModel PeriodModel);
        Task<bool> UpdatePeriod(PeriodsModel yearModel);
        Task<bool> DeletePeriod(int Periods_ID);

    }
}
