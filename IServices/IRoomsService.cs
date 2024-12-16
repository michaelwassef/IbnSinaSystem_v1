using IbnSinaSystem.Models;

namespace IbnSinaSystem.IServices
{
    public interface IRoomsService
    {
        Task<IEnumerable<RoomsModel>> GetAllRooms();
        Task<RoomsModel?> SelectRoomByID(int rooms_ID);
        Task<bool> InsertRoom(RoomsModel roomModel);
        Task<bool> UpdateRoom(RoomsModel roomModel);
        Task<bool> DeleteRoom(int rooms_ID);
    }
}
