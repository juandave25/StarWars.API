using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.API.Services
{
    public interface IAuthService
    {
        public string GenerateJwtToken(string username);
    }
}
