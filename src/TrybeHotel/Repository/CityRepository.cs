using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
           throw new NotImplementedException();
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();

            return new CityDto { CityId = city.CityId, Name = city.Name, State = city.State };
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();

            var updateCity = from c in _context.Cities
                              where c.CityId == city.CityId
                              select new CityDto
                              {
                                  CityId = c.CityId,
                                  Name = c.Name,
                                  State = c.State,
                              };

            return updateCity.First();
        }

    }
}