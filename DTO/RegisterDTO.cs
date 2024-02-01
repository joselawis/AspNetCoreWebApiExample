using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name can't be blank")]
        public required string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote(
            action: "IsEmailNotRegistered",
            controller: "Account",
            ErrorMessage = "Email is already registered"
        )]
        public required string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone can't be blank")]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "Please enter a valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must be the same")]
        public required string ConfirmPassword { get; set; }
    }
}
