using Dapper;
using IbnSinaSystem.Dapper;
using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using System.Data;

namespace IbnSinaSystem.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IDbConnection _db;

        public RoomsService(DapperContext dapperContext)
        {
            _db = dapperContext.CreateConnection();
        }

        public async Task<IEnumerable<RoomsModel>> GetAllRooms()
        {
            var sql = @"SELECT * FROM Rooms";
            return await _db.QueryAsync<RoomsModel>(sql);
        }

        public async Task<RoomsModel?> SelectRoomByID(int rooms_ID)
        {
            var sql = @"SELECT * FROM Rooms WHERE rooms_ID = @rooms_ID";
            var result = await _db.QuerySingleOrDefaultAsync<RoomsModel>(sql, new { rooms_ID });
            return result;
        }

        public async Task<bool> InsertRoom(RoomsModel roomModel)
        {
            var sql = @"INSERT INTO Rooms (room_Name, room_Capacity) VALUES (@room_Name, @room_Capacity);";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, roomModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRoom(RoomsModel roomModel)
        {
            var sql = @"UPDATE Rooms SET room_Name = @room_Name, room_Capacity = @room_Capacity WHERE rooms_ID = @rooms_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, roomModel);
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRoom(int rooms_ID)
        {
            var sql = @"DELETE FROM Rooms WHERE rooms_ID = @rooms_ID";
            try
            {
                var affectedRows = await _db.ExecuteAsync(sql, new { rooms_ID });
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
