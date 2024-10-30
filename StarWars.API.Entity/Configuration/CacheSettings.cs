namespace StarWars.API.Entity.Configuration
{
    public class CacheSettings
    {
        public int StarshipsCacheMinutes { get; set; } = 60;
        public int ManufacturersCacheMinutes { get; set; } = 120;
    }
}
