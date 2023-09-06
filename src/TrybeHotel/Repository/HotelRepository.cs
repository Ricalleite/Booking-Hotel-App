using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        //  5. Refatore o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            List<HotelDto> hotels = (from hotel in _context.Hotels
                                     join city in _context.Cities on hotel.CityId equals city.CityId
                                     select new HotelDto
                                    {
                                        HotelId = hotel.HotelId,
                                        Name = hotel.Name,
                                        Address = hotel.Address,
                                        CityId = hotel.CityId,
                                        CityName = city.Name,
                                        State = city.State,
                                    }).ToList();
            return hotels;
        }

        // 6. Refatore o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var hotelAdd = from h in _context.Hotels
                           join city in _context.Cities on h.CityId equals city.CityId
                           orderby h.HotelId
                           select new HotelDto
                           {
                            HotelId = h.HotelId,
                            Name = h.Name,
                            Address = h.Address,
                            CityId = city.CityId,
                            CityName = city.Name,
                            State = city.State
                            };
            return hotelAdd.Last();
        }
    }
}