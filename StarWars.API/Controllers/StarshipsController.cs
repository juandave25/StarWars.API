using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarWars.API.Entity.Dto;
using StarWars.API.Services;

namespace StarWars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipsController : ControllerBase
    {
        private readonly IStarwarsApiService _swapiService;
        private readonly ILogger<StarshipsController> _logger;

        public StarshipsController(
            IStarwarsApiService swapiService,
            ILogger<StarshipsController> logger)
        {
            _swapiService = swapiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StarshipResponse>>> GetStarships(
            [FromQuery] string? manufacturer = null)
        {
            try
            {
                var starships = await _swapiService.GetStarshipsAsync();

                if (!string.IsNullOrEmpty(manufacturer))
                {
                    starships = starships.Where(s =>
                        s.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase));
                }

                return Ok(starships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting starships");
                return StatusCode(500, "An error occurred while fetching starships");
            }
        }

        [HttpGet("manufacturers")]
        public async Task<ActionResult<IEnumerable<string>>> GetManufacturers()
        {
            try
            {
                var manufacturers = await _swapiService.GetManufacturersAsync();
                return Ok(manufacturers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting manufacturers");
                return StatusCode(500, "An error occurred while fetching manufacturers");
            }
        }
    }
}
