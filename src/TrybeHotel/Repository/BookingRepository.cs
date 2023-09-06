using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId);
            
            if (room == null || user == null) 
            {
                return null;
            } 
            
//            Console.WriteLine("********");
//            Console.WriteLine(booking.GuestQuant);
//            Console.WriteLine(room.Capacity);
            if (booking.GuestQuant > room.Capacity)
            {
                throw new Exception();
            }

            var bookingToMade = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = user.UserId,  
                RoomId = booking.RoomId
            };

            _context.Bookings.Add(bookingToMade);
            _context.SaveChanges();

            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId);
            var city = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId);

            return new BookingResponse
            {
                BookingId = bookingToMade.BookingId,
                CheckIn = bookingToMade.CheckIn,
                CheckOut = bookingToMade.CheckOut,
                GuestQuant = bookingToMade.GuestQuant,
                Room = new RoomDto
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
                }
            };
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var bookings = _context.Bookings;
            var rooms = _context.Rooms;
            var cities = _context.Cities;
            var hotels = _context.Hotels;

            var bookingDone = from b in bookings
                              join r in rooms on b.RoomId equals r.RoomId
                              join h in hotels on r.HotelId equals h.HotelId
                              join c in cities on h.CityId equals c.CityId
                              where b.BookingId == bookingId
                              where b.User.Email == email
                              select new BookingResponse
                              {
                                BookingId = b.BookingId,
                                CheckIn = b.CheckIn,
                                CheckOut = b.CheckOut,
                                GuestQuant = b.GuestQuant,
                                Room = new RoomDto
                                {
                                    RoomId = r.RoomId,
                                    Name = r.Name,
                                    Capacity = r.Capacity,
                                    Image = r.Image,
                                    Hotel = new HotelDto
                                    {
                                        HotelId = h.HotelId,
                                        Name = h.Name,
                                        Address = h.Address,
                                        CityId = h.CityId,
                                        CityName = c.Name,
                                        State = c.State
                                    }
                                }
                              };
                              switch (bookingDone.FirstOrDefault())
                              {
                                case null:
                                return null;
                                default:
                                return bookingDone.First();
                              }
        }

        public Room GetRoomById(int RoomId)
        {
             throw new NotImplementedException();
        }

    }

}