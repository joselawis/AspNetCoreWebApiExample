using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesManager.WebAPI.DTO
{
    public class AuthenticationResponse
    {
        public string? PersonName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; }
    }
}
