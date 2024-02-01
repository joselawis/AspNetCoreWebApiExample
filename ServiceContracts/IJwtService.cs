using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;

namespace CitiesManager.WebAPI.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}
