using System.ComponentModel.DataAnnotations;

namespace RegistrationForm.Data.Models
{
    public class UserDetail
    {
        [Key]
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public UserCredential? Credential { get; set; }
        public UserAddress? Address { get; set; }
    }
}