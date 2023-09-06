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
            throw new NotImplementedException();
        }

        public Room GetRoomById(int RoomId)
        {
             throw new NotImplementedException();
        }

    }

}