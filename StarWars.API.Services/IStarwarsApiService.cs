using StarWars.API.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.API.Services
{
    public interface IStarwarsApiService
    {
        Task<IEnumerable<StarshipResponse>> GetStarshipsAsync();
        Task<IEnumerable<string>> GetManufacturersAsync();
    }
}
