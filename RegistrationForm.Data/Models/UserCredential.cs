namespace RegistrationForm.Data.Models
{
    public class UserCredential
    {
        public int UserCredentialId { get; set; }
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime LastLoginDate { get; set; }

        // Navigation property
        public UserDetail? User { get; set; }
    }
}