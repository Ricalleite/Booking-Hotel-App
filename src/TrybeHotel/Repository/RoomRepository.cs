using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 7. Refatore o endpoint GET /room
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            List<RoomDto> rooms = (from hotel in _context.Hotels
                                  where hotel.HotelId == HotelId
                                  join room in _context.Rooms on hotel.HotelId equals room.HotelId
                                  join city in _context.Cities on hotel.CityId equals city.CityId
                                  select new RoomDto
                                  {
                                    RoomId = room.RoomId,
                                    Name = room.Name,
                                    Capacity = room.Capacity,
                                    Image = room.Image,
                                    Hotel = new HotelDto
                                    {
                                        HotelId = hotel.HotelId,
                                        Name = hotel.Name,
                                        Address = hotel.Address,
                                        CityId = hotel.CityId,
                                        CityName = city.Name,
                                        State = city.State
                                    }
                                 }).ToList();
            return rooms;
        }

        // 8. Refatore o endpoint POST /room
        public RoomDto AddRoom(Room room) 
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            var hotelData = from h in _context.Hotels
                            join r in _context.Rooms on h.HotelId equals r.HotelId
                            where h.HotelId == room.HotelId
                            select new HotelDto
                            {
                                HotelId = h.HotelId,
                                Name = h.Name,
                                Address = h.Address,
                                CityId = h.CityId,
                                CityName = (from city in _context.Cities
                                            where city.CityId == h.CityId
                                            select city.Name).First(),
                                State = (from city in _context.Cities
                                         where city.CityId == h.CityId
                                         select city.State).First(),
                           };

            return new RoomDto
            {
                RoomId = room.RoomId, 
                Name = room.Name, 
                Capacity = room.Capacity, 
                Image = room.Image,
                Hotel = hotelData.First()
            };
        }

        public void DeleteRoom(int RoomId) {
           throw new NotImplementedException();
        }
    }
}