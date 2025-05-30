using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AuthService.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int UserId { get; set;}

        [Required]
        [ProtectedPersonalData]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set;}

        [Required]
        [MaxLength(255)]
        public required string FirstName { get; set;}

        [Required]
        [MaxLength(255)]
        public required string LastName { get; set;}

        [Required]
        [MinLength(6)]
        [MaxLength(12)]
        public required string Password { get; set;}

        public bool IsActice { get; set;} = true;

        public DateTime CreatedAt { get; set;} = DateTime.Now;

        public DateTime? UpdatedAt  { get; set;}

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}