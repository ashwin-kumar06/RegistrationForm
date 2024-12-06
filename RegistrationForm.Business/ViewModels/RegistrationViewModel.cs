using System.ComponentModel.DataAnnotations;

namespace RegistrationForm.Business.ViewModels
{
    public class RegistrationViewModel
    {
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Please enter your first name")]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        //[Required]
        public string? PhoneNumber { get; set; }

        //[Required]
        public DateTime DateOfBirth { get; set; }

        //[Required]
        public string? Gender { get; set; }

        //[Required]
        public string? StreetAddress { get; set; }

        //[Required]
        public string? City { get; set; }

        //[Required]
        public string? State { get; set; }

        //[Required]
        public string? Country { get; set; }

        //[Required]
        public string? PostalCode { get; set; }
    }
}