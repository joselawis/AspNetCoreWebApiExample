using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public required string Password { get; set; } = string.Empty;
    }
}
