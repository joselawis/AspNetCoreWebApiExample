using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;

namespace CitiesManager.WebAPI.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}
