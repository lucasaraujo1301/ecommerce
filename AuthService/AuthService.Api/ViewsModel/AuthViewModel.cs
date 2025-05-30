using System.ComponentModel.DataAnnotations;

namespace ECommerce.AuthService.Api.ViewsModel
{
    public class LoginRequestViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must have between {1} and {2}", MinimumLength = 6)]
        [
            RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[A-Z]).*$",
            ErrorMessage = "Password must be alphanumeric and contain at least one uppercase letter.")
        ]
        public required string Password { get; set; }
    }

    public class RegisterRequestViewModel : LoginRequestViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "The {0} must have between {1} and {2}", MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "The {0} must have between {1} and {2}", MinimumLength = 2)]
        public required string LastName { get; set;}
    }
}