namespace TrybeHotel.Dto {

    //2. Refatore o endpoint POST /city aqui
    //4. Refatore o endpoint GET /city
    public class CityDto 
    {
        public int CityId { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; } 
    }
}